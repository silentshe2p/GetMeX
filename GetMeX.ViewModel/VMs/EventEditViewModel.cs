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

        private ModifyEventStatusMsg _status;
        public ModifyEventStatusMsg Status
        {
            get { return _status; }
            set
            {
                _status = value;
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

        private bool _saveChangeOnline;
        public bool SaveChangeOnline
        {
            get { return _saveChangeOnline; }
            set
            {
                _saveChangeOnline = value;
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
            Messenger.Base.Register<ModifyEventStatusMsg>(this, OnModifyStatusReceived);
            CheckEventModifiedCommand = new RelayCommand(
                (object q) => EventModified = !Event.Equals(_originalEvent),
                (object q) => { return true; }
            );
            CloseWindowCommand = new RelayCommand(
                (object q) => { ((Window)q).Close(); },
                (object q) => { return true; }
            );
            DoWorkCommand = AsyncCommand.Create(DoWork);
            DeleteEventCommand = AsyncCommand.Create(DeleteEvent);
        }
        
        public RelayCommand CheckEventModifiedCommand { get; private set; }

        public RelayCommand CloseWindowCommand { get; private set; }

        public IAsyncCommand DoWorkCommand { get; private set; }

        public Task DoWork() // Add/save current event
        {
            if (EventValidated())
            {
                var action = (Event.EID == 0) ? EventModifyAction.Add
                                                                : EventModifyAction.Update;
                SendModifyEventMsg(action);
            }
            return Task.CompletedTask;
        }

        public IAsyncCommand DeleteEventCommand { get; private set; }

        public Task DeleteEvent()
        {
            if (Event.Equals(_originalEvent))
            {
                SendModifyEventMsg(EventModifyAction.Delete);
            }
            return Task.CompletedTask;
        }

        private void SendModifyEventMsg(EventModifyAction action)
        {
            Messenger.Base.Send(new ModifyEventMsg
            {
                Action = action,
                SaveChangeOnline = SaveChangeOnline,
                Event = Event
            });
            _originalEvent = new GXEvent(Event);
        }

        private void OnEventToEditReceived(GXEvent ev)
        {
            if (ev != null)
            {
                Event = new GXEvent(ev);
                _originalEvent = new GXEvent(Event);
                ActionName = "Edit event";
                if (ev.Account != null && ev.Account.Gmail.Contains("@gmail.com"))
                {
                    Account = ev.Account.Gmail;
                    SaveChangeOnline = true;
                }
            }
        }

        private void OnModifyStatusReceived(ModifyEventStatusMsg m)
        {
            if (m.Deleted)
            {
                InitModel();
            }
            EventModified = false;
            Status = m;
        }

        public void InitModel()
        {
            Event = new GXEvent
            {
                AID = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                ColorId = 1
            };
            _originalEvent = new GXEvent(Event);
            SaveChangeOnline = false;
            ActionName = "Create event";
            Account = "Local";
            Status = new ModifyEventStatusMsg {
                Success = false,
                Deleted = false,
                Error = null
            };
            EventModified = false;
        }

        private bool EventValidated()
        {
            var status = new ModifyEventStatusMsg
            {
                Success = Status.Success,
                Deleted = Status.Deleted
            };

            if (Event.Summary.IsNullOrEmpty())
            {
                status.Error = "Event summary can't be empty";
            }
            else if (Event.StartDate > Event.EndDate)
            {
                status.Error = "Start datetime can't be latter than end datetime";
            }
            else if (Event.Equals(_originalEvent))
            {
                status.Error = "No changes detected for event";
            }
            else
            {
                status.Error = null;
            }
            Status = status;
            return Status.Error == null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
