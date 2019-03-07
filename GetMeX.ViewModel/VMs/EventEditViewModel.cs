using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GetMeX.DAL;
using GetMeX.Models;
using GetMeX.ViewModels.Commands;
using GetMeX.ViewModels.Utilities;
using GetMeX.ViewModels.Utilities.Messages;

namespace GetMeX.ViewModels.VMs
{
    public class EventEditViewModel : INotifyPropertyChanged, IViewModel
    {
        private GXEventService _dbs;
        public string ActionName { get; set; }
        public string Account { get; set; }
        public string ErrorMsg { get; set; }
        public bool EventValidated { get; set; }

        private bool _saveOnline;
        public bool SaveOnline
        {
            get { return _saveOnline; }
            set
            {
                _saveOnline = value;
                OnPropertyChanged();
            }
        }

        private GXEvent _event;
        public GXEvent Event
        {
            get { return _event; }
            set
            {
                _event = value;
                OnPropertyChanged();
            }
        }

        public EventEditViewModel()
        {
            _dbs = new GXEventService();
            Event = CreateDefaultEvent();
            SaveOnline = false;
            ActionName = "Create event";
            Account = "Local";
            ErrorMsg = null;
            EventValidated = false;

            // Init messenger and command
            Messenger.Base.Register<EditEventMsg>(this, OnEventToEditReceived);
            ValidateCommand = new RelayCommand(
                (object q) => ValidateEvent(q),
                (object q) => { return true; }
            );
            DoWorkCommand = AsyncCommand.Create(DoWork);
        }

        public RelayCommand ValidateCommand { get; private set; }

        public IAsyncCommand DoWorkCommand { get; private set; }

        public Task DoWork() // Save current event
        {
            Messenger.Base.Send(new SaveEventMsg {
                SaveOnline = SaveOnline,
                Event = Event
            });
            return Task.CompletedTask;
        }

        private void OnEventToEditReceived(EditEventMsg m)
        {
            if (m.Event != null)
            {
                Event = m.Event;
                ActionName = "Edit event";
                if (!m.Gmail.IsNullOrEmpty())
                {
                    Account = m.Gmail;
                    SaveOnline = true;
                }
            }
        }

        private GXEvent CreateDefaultEvent()
        {
            return new GXEvent
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                ColorId = 1
            };
        }

        private void ValidateEvent(object q)
        {
            if (Event.Summary.IsNullOrEmpty())
            {
                ErrorMsg = "Event summary can't be empty";
            }
            else if (Event.StartDate > Event.EndDate)
            {
                ErrorMsg = "Start datetime can't be latter than end datetime";
            }
            else
            {
                EventValidated = true;
                return;
            }
            EventValidated = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
