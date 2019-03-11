using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GetMeX.ViewModels.Utilities
{
    public class ModifyEventStatusMsg : INotifyPropertyChanged
    {
        public bool Success { get; set; }

        public bool Deleted { get; set; }

        private string _err;
        public string Error
        {
            get { return _err; }
            set
            {
                _err = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
