namespace GetMeX.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Account
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Account()
        {
            this.GXEvents = new HashSet<GXEvent>();
        }
    
        public int AID { get; set; }
        public string Gmail { get; set; }
        public DateTimeOffset LastSync { get; set; }
        public int Finished { get; set; }
        public int Missed { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GXEvent> GXEvents { get; set; }
    }
}
