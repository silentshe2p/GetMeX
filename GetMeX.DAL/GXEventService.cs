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
            try
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
            catch (DataException)
            {
                throw new DataException("Failed to add new account");
            }
        }

        public async Task UpdateLastSync(int accId)
        {
            var accountToUpdate = _db.Accounts.Find(accId);
            try
            {
                if (accountToUpdate != null)
                {
                    accountToUpdate.LastSync = DateTime.Now;
                    await _db.SaveChangesAsync();
                }
            }
            catch (DataException)
            {
                throw new DataException("Failed to update last sync");
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
            var now = DateTime.Now;
            return (limit > 0) ? _db.GXEvents.Where(e => e.StartDate >= now).Take(limit).ToList() 
                                        : _db.GXEvents.Where(e => e.StartDate >= now).ToList();
        }

        public List<GXEvent> GetEvents(string searchQuery, int limit = 0)
        {
            IQueryable<GXEvent> query;
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
            return (limit > 0) ? query.Take(limit).ToList() : query.ToList();
        }

        public async Task AddEvents(List<GXEvent> events, int accId)
        {
            try
            {
                var account = _db.Accounts.Find(accId);
                foreach (var ev in events)
                {
                    account.GXEvents.Add(ev);
                    _db.GXEvents.Add(ev);
                }
                await _db.SaveChangesAsync();
            }
            catch (DataException)
            {
                throw new DataException("Failed to save events");
            }
        }

        public async Task EditEvent(int id, GXEvent newEvent)
        {
            var eventToEdit = _db.GXEvents.Find(id);
            try
            {
                if (eventToEdit != null)
                {
                    eventToEdit = newEvent;
                    await _db.SaveChangesAsync();
                }
            }
            catch (DataException)
            {
                throw new DataException("Failed to edit event");
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
