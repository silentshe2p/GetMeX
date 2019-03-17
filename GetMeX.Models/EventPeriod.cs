using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Markup;

namespace GetMeX.Models
{
    public class EventPeriod<T> : INotifyPropertyChanged
    {
        public EventPeriod(object period, T[] events)
        {
            Period = period;
            Events = events;
        }

        private object _period;
        public object Period
        {
            get { return _period; }
            set
            {
                _period = value;
                OnPropertyChanged();
            }
        }

        private T[] _events;
        public T[] Events
        {
            get { return _events; }
            set
            {
                _events = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum Quarter { Q1, Q2, Q3, Q4 }

    public class EventPeriodGeneric : MarkupExtension
    {
        private readonly Type _type;

        public EventPeriodGeneric(Type type)
        {
            _type = type;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return typeof(EventPeriod<>).MakeGenericType(_type);
        }
    }
}
