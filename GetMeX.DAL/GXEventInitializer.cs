using System;
using System.Data.Entity;
using GetMeX.Models;

namespace GetMeX.DAL
{
    class GXEventInitializer : IDatabaseInitializer<GXEventsEntities>
    {
        public void InitializeDatabase(GXEventsEntities context)
        {
            var account = new Account
            {
                Gmail = "_local",
                LastSync = DateTimeOffset.Now,
                Finished = 0,
                Missed = 0
            };
            context.Accounts.Add(account);
            context.SaveChanges();
        }
    }
}
