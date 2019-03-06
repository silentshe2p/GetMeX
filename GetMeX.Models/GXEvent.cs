namespace GetMeX.Models
{
    using System;
    
    public partial class GXEvent
    {
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
