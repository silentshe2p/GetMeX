namespace GetMeX.Models
{
    public class EventPeriod<T>
    {
        public EventPeriod(object period, T[] events)
        {
            Period = period;
            Events = events;
        }

        public object Period { get; set; }

        public T[] Events { get; set; }
    }

    public enum Quarter { Q1, Q2, Q3, Q4 }
}
