namespace GetMeX.Models
{
    using System.Collections.Generic;
    
    public partial class Recurrence
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Recurrence()
        {
            GXEvents = new HashSet<GXEvent>();
        }
    
        public int RID { get; set; }
        public int AID { get; set; }
        public short Daynum { get; set; }
    
        public virtual Account Account { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GXEvent> GXEvents { get; set; }
    }
}
