using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GetMeX.Models;
using GetMeX.DAL;
using GetMeX.ViewModels.Services;
using GetMeX.ViewModels.Utilities;

namespace GetMeX.ViewModels.VMs
{
    public class EventsViewModel : INotifyPropertyChanged, IViewModel
    {
        const int viewableYearRange = 2;
        const int viewableEventsNum = 20;

        private GXEventService _dbs;

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

        public EventsViewModel()
        {
            _dbs = new GXEventService();
            _cachedEvents = null;
            _lastFilterQuery = "";
            FilterQuery = "";
            RefreshEvents(viewableEventsNum);
            DoWorkCommand = AsyncCommand.Create(DoWork);
            FilterCommand = AsyncCommand.Create(Filter);
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
            else
            {
                Events = _cachedEvents;
            }

            return Task.CompletedTask;
        }

        public IAsyncCommand DoWorkCommand { get; private set; }

        public async Task DoWork() // Add new gmail account and sync with db
        {
            var service = new GoogleCalendarService();
            var accounts = _dbs.GetAvailableAccounts();
            var timeMax = DateTime.Now.AddYears(viewableYearRange);

            try
            {
                var events = await service.GetEvents(timeMax);
                if (events != null && !events.Items.IsNullOrEmpty())
                {
                    var email = events.Items[0].Creator.Email;
                    var foundAcc = accounts.Find(a => a.Email == email);
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
                        targetAccountId = accounts.Max(a => a.AccId) + 1;
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

        private void RefreshEvents(int limit)
        {
            try
            {
                Events = _dbs.GetEvents(limit).ToObservableCollection();
            }
            catch (DataException)
            {
                throw new DataException("Failed to retrieve events from database. Verify the connection and try again");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));    
        }
    }
}
