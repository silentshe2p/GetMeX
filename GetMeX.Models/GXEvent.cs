namespace GetMeX.Models
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public partial class GXEvent : INotifyPropertyChanged
    {
        public GXEvent() { }

        public GXEvent(GXEvent other)
        {
            EID = other.EID;
            AID = other.AID;
            GID = other.GID;
            Location = other.Location;
            Summary = other.Summary;
            Description = other.Description;
            StartDate = other.StartDate;
            StartDateTime = other.StartDateTime;
            EndDate = other.EndDate;
            EndDateTime = other.EndDateTime;
            ColorId = other.ColorId;
        }

        public override bool Equals(object obj)
        {
            var other = obj as GXEvent;
            if (other == null)
            {
                return false;
            }
            return _summary == other.Summary &&
                        _sd == other.StartDate &&
                        _ed == other.EndDate &&
                        _location == other.Location || (_location == "" && other.Location == null) && 
                        _desc == other.Description || (_desc == "" && other.Description == null);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int EID { get; set; }
        public int AID { get; set; }
        public string GID { get; set; }

        private string _location;
        public string Location
        {
            get { return _location; }
            set
            {
                _location = value;
                OnPropertyChanged();
            }
        }

        private string _summary;
        public string Summary
        {
            get { return _summary; }
            set
            {
                _summary = value;
                OnPropertyChanged();
            }
        }

        private string _desc;
        public string Description
        {
            get { return _desc; }
            set
            {
                _desc = value;
                OnPropertyChanged();
            }
        }

        private DateTime _sd;
        public DateTime StartDate
        {
            get { return _sd; }
            set
            {
                _sd = value;
                OnPropertyChanged();
            }
        }

        private DateTime _ed;
        public DateTime EndDate
        {
            get { return _ed; }
            set
            {
                _ed = value;
                OnPropertyChanged();
            }
        }

        public DateTimeOffset? StartDateTime { get; set; }
        public DateTimeOffset? EndDateTime { get; set; }
        public byte ColorId { get; set; }
    
        public virtual Account Account { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
