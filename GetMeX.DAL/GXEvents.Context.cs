namespace GetMeX.DAL
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using GetMeX.Models;

    public partial class GXEventsEntities : DbContext
    {
        public GXEventsEntities()
            : base("name=GXEventsEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<GXEvent>().HasRequired(e => e.Account).WithMany(a => a.GXEvents).HasForeignKey<int>(e => e.AID).WillCascadeOnDelete(true);
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<GXEvent> GXEvents { get; set; }
    }
}
