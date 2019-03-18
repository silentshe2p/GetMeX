using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GetMeX.DAL;
using GetMeX.Models;
using GetMeX.ViewModels.Commands;
using GetMeX.ViewModels.Utilities;
using GetMeX.ViewModels.Utilities.Messages;

namespace GetMeX.ViewModels.VMs
{
    public class EventTreeViewModel : INotifyPropertyChanged, IViewModel
    {
        const int quarter = 4;
        private GXEventService _dbs;
        private int _yearRange;
        public int CurrentYear { get; private set; }

        // Structure: Year<Quarter<Event in the quarter>>
        private EventPeriod<EventPeriod<GXEvent>>[] _events;
        public EventPeriod<EventPeriod<GXEvent>>[] Events
        {
            get { return _events; }
            set
            {
                _events = value;
                OnPropertyChanged();
            }
        }

        public EventTreeViewModel(RelayCommand editCmd)
        {
            _dbs = new GXEventService();
            InitEvents();

            Messenger.Base.Register<UpdateTreeViewMsg>(this, OnUpdateTreeViewReceived);
            LoadEventsCommand = new RelayCommand(
                (object q) => LoadEvents(q),
                (object q) => CanLoadEvents(q)
            );
            EditEventCommand = editCmd;
            DoWorkCommand = AsyncCommand.Create(DoWork);
        }

        public IAsyncCommand DoWorkCommand { get; private set; }
        public Task DoWork()
        {
            throw new NotImplementedException();
        }

        public RelayCommand EditEventCommand { get; private set; }

        private void InitEvents()
        {
            _yearRange = (int)AppDomain.CurrentDomain.GetData("EventViewableYearRange");
            CurrentYear = DateTime.Today.Year;
            Events = new EventPeriod<EventPeriod<GXEvent>>[_yearRange];
            Events[0] = new EventPeriod<EventPeriod<GXEvent>>(CurrentYear, FetchEvents(CurrentYear));

            for (int i = 1; i < _yearRange; i++)
            {
                Events[i] = new EventPeriod<EventPeriod<GXEvent>>(CurrentYear+i, new EventPeriod<GXEvent>[quarter]);
            }
        }

        private EventPeriod<GXEvent>[] FetchEvents(int year)
        {
            var result = new EventPeriod<GXEvent>[quarter];
            var yearEvents = _dbs.GetEvents(e => e.StartDate.Year == year);
            for (int i = 1; i <= quarter; i++)
            {
                Quarter qt;
                if (Enum.TryParse("Q" + i, out qt))
                {
                    var quarterEvents = yearEvents.Where(e => (e.StartDate.Month >= 12 / quarter * (i - 1) + 1) && 
                                                                                            (e.StartDate.Month <= 12 / quarter * i))
                                                                        .OrderBy(e => e.StartDate).ToArray();
                    result[i-1] = new EventPeriod<GXEvent>(qt, quarterEvents);
                }
            }
            return result;
        }

        public RelayCommand LoadEventsCommand { get; private set; }
        private bool CanLoadEvents(object year)
        {
            if (year != null && year is int)
            {
                var idxToLoad = (int)year - CurrentYear;
                if (idxToLoad > 0 && idxToLoad < quarter)
                {
                    return Events[idxToLoad].Events?[0] == null;
                }
            }
            return false;
        }

        private void LoadEvents(object year)
        {
            // CanLoadEvents() took care of validating year
            Events[(int)year - CurrentYear].Events = FetchEvents((int)year);
        }

        private void OnUpdateTreeViewReceived(UpdateTreeViewMsg m)
        {
            var modifiedEventId = m.ModifiedEvent.EID;
            var yearIdx = m.ModifiedEvent.StartDate.Year - CurrentYear;
            var quarterIdx = m.ModifiedEvent.StartDate.Month / 4;
            var quarterEvents = Events[yearIdx].Events[quarterIdx].Events;
            if (yearIdx >= 0 && yearIdx < _yearRange)
            {
                switch (m.Action)
                {
                    case EventModifyAction.Update:
                        Events[yearIdx].Events[quarterIdx].Events =
                            quarterEvents.Select(e => (e.EID == modifiedEventId) ? m.ModifiedEvent : e)
                                                    .ToArray();
                        break;
                    case EventModifyAction.Delete:
                        Events[yearIdx].Events[quarterIdx].Events = 
                            quarterEvents.Where(e => e.EID != modifiedEventId).ToArray();
                        break;
                    default:
                        break;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
