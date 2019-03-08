namespace GetMeX.Models
{
    using System;
    
    public partial class GXEvent
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
            return Summary == other.Summary && 
                        StartDate == other.StartDate && 
                        EndDate == other.EndDate && 
                        Location == other.Location && 
                        Description == other.Description && 
                        ColorId == other.ColorId;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int EID { get; set; }
        public int AID { get; set; }
        public string GID { get; set; }
        public string Location { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTimeOffset? StartDateTime { get; set; }
        public DateTime EndDate { get; set; }
        public DateTimeOffset? EndDateTime { get; set; }
        public byte ColorId { get; set; }
    
        public virtual Account Account { get; set; }
    }
}
