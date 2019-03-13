using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GetMeX.DAL;
using GetMeX.Models;

namespace GetMeX.ViewModels.VMs
{
    class EventWideViewViewModel : INotifyPropertyChanged, IViewModel
    {
        const int quarter = 4;
        private GXEventService _dbs;
        public int CurrentYear { get; private set; }
        // Structure: Year<Quarter<Event in the quarter>> 
        public EventPeriod<EventPeriod<GXEvent>>[] Events { get; private set; }

        public EventWideViewViewModel()
        {
            _dbs = new GXEventService();
            InitEvents();
            DoWorkCommand = AsyncCommand.Create(DoWork);
        }

        public IAsyncCommand DoWorkCommand { get; private set; }
        public Task DoWork()
        {
            throw new NotImplementedException();
        }

        private void InitEvents()
        {
            var range = (int)AppDomain.CurrentDomain.GetData("EventViewableYearRange");
            CurrentYear = DateTime.Today.Year;
            Events = new EventPeriod<EventPeriod<GXEvent>>[range];
            Events[0] = new EventPeriod<EventPeriod<GXEvent>>(CurrentYear, FetchEvents(CurrentYear));
            for (int i = 1; i < range; i++)
            {
                Events[i] = new EventPeriod<EventPeriod<GXEvent>>(CurrentYear+1, null);
            }
        }

        public EventPeriod<GXEvent>[] FetchEvents(int year)
        {
            var result = new EventPeriod<GXEvent>[quarter];
            var yearEvents = _dbs.GetEvents(e => e.StartDate.Year == year);
            for (int i = 1; i <= quarter; i++)
            {
                Quarter qt;
                if (Enum.TryParse("Q" + i, out qt))
                {
                    var quarterEvents = yearEvents.Where(e => e.StartDate.Month <= i * quarter)
                                                                        .OrderBy(e => e.StartDate).ToArray();
                    result[i-1] = new EventPeriod<GXEvent>(qt, quarterEvents);
                }
            }
            return result;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
