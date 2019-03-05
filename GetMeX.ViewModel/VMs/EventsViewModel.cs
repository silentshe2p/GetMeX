using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            RefreshEvents(viewableEventsNum);
            DoWorkCommand = AsyncCommand.Create(DoWork);
        }

        public IAsyncCommand DoWorkCommand { get; private set; }

        public async Task DoWork() // Add new gmail account and sync with db
        {
            var service = new GoogleCalendarService();
            var accounts = _dbs.GetAvailableAccounts();
            var events = await service.GetEvents();

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
                    newEvents = await fromLastSync.ToGXEvents(targetAccountId);
                }
                // New email
                else
                {
                    targetAccountId = accounts.Max(a => a.AccId) + 1;
                    await _dbs.AddAccount(email);
                    newEvents = await events.Items.ToGXEvents(targetAccountId);
                }

                // Add new events to db
                await _dbs.AddEvents(newEvents, targetAccountId);
                await _dbs.UpdateLastSync(targetAccountId);

                if (Events.Count < viewableEventsNum)
                {
                    RefreshEvents(viewableEventsNum);
                }
            }
        }

        private void RefreshEvents(int limit)
        {
            Events = _dbs.GetEvents(limit).ToObservableCollection();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));    
        }
    }
}
