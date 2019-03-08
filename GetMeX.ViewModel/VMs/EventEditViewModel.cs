using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
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
        private GXEvent _originalEvent;

        public List<string> ColorList { get; private set; }

        private bool _eventModified;
        public bool EventModified
        {
            get { return _eventModified; }
            set
            {
                _eventModified = value;
                OnPropertyChanged();
            }
        }

        private string _error;
        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged();
            }
        }

        private string _actionName;
        public string ActionName
        {
            get { return _actionName; }
            set
            {
                _actionName = value;
                OnPropertyChanged();
            }
        }

        private string _account;
        public string Account
        {
            get { return _account; }
            set
            {
                _account = value;
                OnPropertyChanged();
            }
        }

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
            ColorIdHex colors = new ColorIdHex();
            ColorList = colors.GetColors();
            InitModel();

            // Init messenger and command
            Messenger.Base.Register<GXEvent>(this, OnEventToEditReceived);
            CheckEventModifiedCommand = new RelayCommand(
                (object q) => EventModified = true,
                (object q) => { return true; }
            );
            CloseWindowCommand = new RelayCommand(
                (object q) => { ((Window)q).Close(); },
                (object q) => { return true; }
            );
            DoWorkCommand = AsyncCommand.Create(DoWork);
        }
        
        public RelayCommand CheckEventModifiedCommand { get; private set; }

        public RelayCommand CloseWindowCommand { get; private set; }

        public IAsyncCommand DoWorkCommand { get; private set; }

        public Task DoWork() // Save current event
        {
            if (EventValidated())
            {
                Messenger.Base.Send(new SaveEventMsg
                {
                    SaveOnline = SaveOnline,
                    Event = Event
                });
                _originalEvent = Event;
            }
            EventModified = false;
            return Task.CompletedTask;
        }

        private void OnEventToEditReceived(GXEvent ev)
        {
            if (ev != null)
            {
                _originalEvent = ev;
                Event = new GXEvent(ev);
                ActionName = "Edit event";
                if (ev.Account.Gmail.Contains("@gmail.com"))
                {
                    Account = ev.Account.Gmail;
                    SaveOnline = true;
                }
            }
        }

        public void InitModel()
        {
            _originalEvent = null;
            Event = new GXEvent
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                ColorId = 1
            };
            SaveOnline = false;
            ActionName = "Create event";
            Account = "Local";
            Error = null;
            EventModified = false;
        }

        private bool EventValidated()
        {
            if (Event.Summary.IsNullOrEmpty())
            {
                Error = "Event summary can't be empty";
            }
            else if (Event.StartDate > Event.EndDate)
            {
                Error = "Start datetime can't be latter than end datetime";
            }
            else if (Event.Equals(_originalEvent))
            {
                Error = "No changes detected for event";
            }
            else
            {
                Error = null;
            }
            return Error == null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
