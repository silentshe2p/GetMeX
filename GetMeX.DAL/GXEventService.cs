using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using GetMeX.Models;

namespace GetMeX.DAL
{
    public class GXEventService
    {
        private GXEventsEntities _db;

        public GXEventService()
        {
            _db = new GXEventsEntities();
        }

        ~GXEventService()
        {
            if (_db != null)
            {
                _db.Dispose();
            }
        }

        //////////////////// Account Entity ////////////////////
        public async Task AddAccount(string gmail)
        {
            var account = new Account {
                Gmail = gmail,
                LastSync = DateTime.Now,
                Finished = 0,
                Missed = 0
            };
            _db.Accounts.Add(account);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateLastSync(int accId)
        {
            var accountToUpdate = _db.Accounts.Find(accId);
            if (accountToUpdate != null)
            {
                accountToUpdate.LastSync = DateTime.Now;
                await _db.SaveChangesAsync();
            }
        }

        public List<AccountDetail> GetAvailableAccounts()
        {
            return _db.Accounts.Select(a => new AccountDetail
            {
                AccId = a.AID,
                Email = a.Gmail,
                LastSync = a.LastSync
            }).ToList();
        }
        ////////////////////////////////////////////////////////////

        //////////////////// GXEvent Entity ////////////////////
        public List<GXEvent> GetEvents(int limit = 0)
        {
            var query = GetAllNoDuplicate(_db.GXEvents)
                                    .Where(e => e.StartDate >= DateTime.Now)
                                    .OrderBy(e => e.StartDate);

            return (limit > 0) ? query.Take(limit).ToList()
                                        : query.ToList();
        }

        public List<GXEvent> GetEvents(string searchQuery, int limit = 0)
        {
            var query = GetAllNoDuplicate(_db.GXEvents).Where(e => e.StartDate >= DateTime.Now);
            DateTime dt;
            if (DateTime.TryParse(searchQuery, out dt))
            {
                query = _db.GXEvents.Where(e => e.StartDate == dt);
            }
            else if (searchQuery.Contains("@gmail.com"))
            {
                var events = _db.GXEvents.Include(e => e.Account);
                query = events.Where(e => e.Account.Gmail == searchQuery);
            }
            else
            {
                query = _db.GXEvents.Where(e => e.Summary.Contains(searchQuery) 
                                                    || e.Location.Contains(searchQuery) 
                                                    || e.Description.Contains(searchQuery));
            }

            return (limit > 0) ? query.OrderBy(e => e.StartDate).Take(limit).ToList() 
                                        : query.OrderBy(e => e.StartDate).ToList();
        }

        private IQueryable<GXEvent> GetAllNoDuplicate(DbSet<GXEvent> context)
        {
            var localEvents = context.Where(e => e.GID == null);
            var eventsBothLocalAndOnline = context.Where(e => e.GID != null) // online or (local but saved online) event
                                                                            .GroupBy(e => e.GID)
                                                                            .Select(g => g.OrderBy(e => e.AID).FirstOrDefault()); // select local one (with smaller AID)
            return localEvents.Union(eventsBothLocalAndOnline);
        }

        public async Task AddEvents(List<GXEvent> events, int accId)
        {
            var account = _db.Accounts.Find(accId);
            foreach (var ev in events)
            {
                account.GXEvents.Add(ev);
                _db.GXEvents.Add(ev);
            }
            await _db.SaveChangesAsync();
        }

        public async Task EditEvent(int id, GXEvent newEvent)
        {
            var eventToEdit = _db.GXEvents.Find(id);
            if (eventToEdit != null)
            {
                eventToEdit = newEvent;
                await _db.SaveChangesAsync();
            }
        }

        public void DeleteEvent(int id)
        {
            var eventToDelete = new GXEvent() { EID = id };
            _db.Entry(eventToDelete).State = EntityState.Deleted;
        }
        ////////////////////////////////////////////////////////////
    }
}
