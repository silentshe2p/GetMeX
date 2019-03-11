using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using GetMeX.Models;
using GetMeX.DAL;
using GetMeX.ViewModels.Commands;
using GetMeX.ViewModels.Services;
using GetMeX.ViewModels.Utilities;

namespace GetMeX.ViewModels.VMs
{
    public class EventsViewModel : INotifyPropertyChanged, IViewModel
    {
        const int viewableYearRange = 2;
        const int viewableEventsNum = 20;

        private GXEventService _dbs;
        private List<AccountDetail> _accounts;

        public bool LoggedIn { get; set; }

        private string _lastFilterQuery;
        private string _filterQuery;
        public string FilterQuery
        {
            get { return _filterQuery; }
            set
            {
                _filterQuery = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<GXEvent> _cachedEvents;

        private ObservableCollection<GXEvent> _events;
        public ObservableCollection<GXEvent> Events
        {
            get { return _events; }
            set
            {
                _events = value;
                OnPropertyChanged();
            }
        }

        private ViewService editViewService { get; set; }
        private ViewService wideViewService { get; set; }

        public EventsViewModel(Window[] views)
        {
            _dbs = new GXEventService();
            // Summary of account avail in db
            _accounts = _dbs.GetAvailableAccounts();
            // User logged in if there is another account beside default _local account
            LoggedIn = _accounts.Count > 1;

            _cachedEvents = null;
            _lastFilterQuery = "";
            FilterQuery = "";
            RefreshEvents(viewableEventsNum);

            // Init commands and messenger
            Messenger.Base.Register<ModifyEventMsg>(this, OnModifyEventReceived);
            EditEventCommand = new RelayCommand(
                (object q) => EditEvent(q),
                (object q) => { return true; }
            );
            DoWorkCommand = AsyncCommand.Create(DoWork);
            FilterCommand = AsyncCommand.Create(Filter);
            SwitchAccountCommand = AsyncCommand.Create(SwitchAccount);
            WideViewCommand = AsyncCommand.Create(WideView);

            // Views for edit and wide view functions
            editViewService = new ViewService(views[0]);
            wideViewService = new ViewService(views[1]);
        }

        public IAsyncCommand FilterCommand { get; private set; }

        public Task Filter()
        {
            if (!FilterQuery.IsNullOrEmpty())
            {
                try
                {
                    if (FilterQuery != _lastFilterQuery)
                    {
                        if (_lastFilterQuery.IsNullOrEmpty())
                        {
                            _cachedEvents = Events;
                        }
                        _lastFilterQuery = FilterQuery;
                        char[] toTrim = { ' ', '"', '\'' };
                        var query = FilterQuery.Trim(toTrim);
                        Events = _dbs.GetEvents(query).ToObservableCollection();
                    }
                }
                catch (Newtonsoft.Json.JsonException)
                {
                    throw new FormatException("Incorrect oauth token file format. Restore or delete it and try again");
                }
                catch (DataException)
                {
                    throw new DataException("Failed to modify database. Verify the connection and try again");
                }
            }
            else if (_cachedEvents != null)
            {
                Events = _cachedEvents;
            }

            return Task.CompletedTask;
        }

        public IAsyncCommand DoWorkCommand { get; private set; }

        public async Task DoWork() // Add new gmail account and sync with db
        {
            var service = new GoogleCalendarService();
            var timeMax = DateTime.Now.AddYears(viewableYearRange);
            _accounts = _dbs.GetAvailableAccounts();

            try
            {
                var events = await service.GetEvents(timeMax);
                if (events != null && !events.Items.IsNullOrEmpty())
                {
                    var email = events.Items[0].Creator.Email;
                    var foundAcc = _accounts.Find(a => a.Email == email);
                    List<GXEvent> newEvents = null;
                    int targetAccountId = 0;

                    // Email exists within db, add new events from last sync timestamp
                    if (foundAcc != null)
                    {
                        targetAccountId = foundAcc.AccId;
                        var fromLastSync = events.Items.Where(e => e.Updated >= foundAcc.LastSync);
                        newEvents = fromLastSync.ToGXEvents(targetAccountId);
                    }
                    // New email
                    else
                    {
                        targetAccountId = _accounts.Max(a => a.AccId) + 1;
                        await _dbs.AddAccount(email);
                        newEvents = events.Items.ToGXEvents(targetAccountId);
                    }

                    // Add new events to db
                    await _dbs.AddEvents(newEvents, targetAccountId);
                    await _dbs.UpdateLastSync(targetAccountId);
                }
            }
            catch (Newtonsoft.Json.JsonException)
            {
                throw new FormatException("Incorrect oauth token file format. Restore or delete it and try again");
            }
            catch (DataException)
            {
                throw new DataException("Failed to modify database. Verify the connection and try again");
            }
            finally
            {
                if (Events.Count < viewableEventsNum)
                {
                    RefreshEvents(viewableEventsNum);
                }
            }
        }

        public IAsyncCommand SwitchAccountCommand { get; private set; }

        public Task SwitchAccount() // or "Add account" if not previously logged in
        {
            if (LoggedIn)
            {
                // Delete current saved oauth token
                var tokenPath = AppDomain.CurrentDomain.GetData("GoogleCalendarTokenPath").ToString();
                File.Delete(tokenPath);
                LoggedIn = false;
            }
            // Authenticate and retrieve events
            RefreshEvents(viewableEventsNum);
            return Task.CompletedTask;
        }

        public RelayCommand EditEventCommand { get; private set; }

        private void EditEvent(object eventToEdit)
        {
            var eventToSend = (eventToEdit != null) ? (GXEvent)eventToEdit : null;
            Messenger.Base.Send(eventToSend);
            editViewService.ShowDialog();
        }

        // Save received event (already validated by edit view model) to db
        // If SaveEventMsg.SaveChangeOnline true, then also save event to / delete event from google calendar 
        private async void OnModifyEventReceived(ModifyEventMsg m)
        {
            var eventToModify = m.Event;
            var service = new GoogleCalendarService();

            try
            {
                // Modify local db and possiblely google calendar based on action
                switch (m.Action)
                {
                    case EventModifyAction.Add:
                        string googleCalendarEventId = null;
                        var targetAccountId = eventToModify.AID;
                        // Apply changes online first so any failure will prevent changes from being saved locally
                        if (LoggedIn && m.SaveChangeOnline)
                        {
                            googleCalendarEventId = await service.AddEvent(eventToModify.ToEvent());
                        }
                        // Event was successfully persisted on Google Calendar, update account and id
                        if (!googleCalendarEventId.IsNullOrEmpty())
                        {
                            _accounts = _dbs.GetAvailableAccounts();
                            var lastSyncAccount = _accounts.Where(a => a.AccId > 1)
                                                                                .OrderByDescending(a => a.LastSync)
                                                                                .FirstOrDefault();
                            if (lastSyncAccount == null)
                            {
                                throw new DataException("Couldn't find target Google Calendar account");
                            }
                            targetAccountId = lastSyncAccount.AccId;
                            eventToModify.GID = googleCalendarEventId;
                        }
                        // Save changes locally
                        await _dbs.AddEvent(eventToModify, targetAccountId);
                        // Switch view to edit just created event
                        Messenger.Base.Send(eventToModify);
                        break;
                    case EventModifyAction.Update:
                        if (LoggedIn && m.SaveChangeOnline)
                        {
                            await service.UpdateEvent(eventToModify.ToEvent());
                        }
                        await _dbs.UpdateEvent(eventToModify, eventToModify.EID);
                        break;
                    case EventModifyAction.Delete:
                        if (LoggedIn && m.SaveChangeOnline)
                        {
                            await service.DeleteEvent(eventToModify.GID);
                        }
                        _dbs.DeleteEvent(eventToModify.EID);
                        break;
                    default:
                        SendModifyEventStatus(success: false, msg: "Unknown action received");
                        return;
                }
            }
            catch (DataException ex)
            {
                SendModifyEventStatus(success: false, msg: ex.Message);
                return;
            }
            catch (Google.GoogleApiException ex)
            {
                SendModifyEventStatus(success: false, msg: "Unable to apply changes online. If error persists, consider modifying event locally");
                return;
            }
            SendModifyEventStatus(success: true, deleted: m.Action == EventModifyAction.Delete);
            RefreshEvents(viewableEventsNum);
        }

        public IAsyncCommand WideViewCommand { get; private set; }

        public async Task WideView()
        {

        }

        private void RefreshEvents(int limit)
        {
            try
            {
                Events = _dbs.GetEvents(limit).ToObservableCollection();
                LoggedIn = true;
            }
            catch (DataException)
            {
                throw new DataException("Failed to retrieve events from database. Verify the connection and try again");
            }
        }

        private void SendModifyEventStatus(bool success = true, bool deleted = false, string msg = null)
        {
            Messenger.Base.Send(new ModifyEventStatusMsg
            {
                Success = success,
                Deleted = deleted,
                Error = msg
            });
        }

        public void CloseChildView(bool parentClosing)
        {
            editViewService.CloseView(parentClosing);
            wideViewService.CloseView(parentClosing);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));    
        }
    }
}
