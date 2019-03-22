using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GetMeX.Models;

namespace GetMeX.DAL.Tests
{
    [TestClass]
    public class EventsDbServiceTests
    {
        private EventTestContext _db;
        private GXEventService _service;

        [TestInitialize]
        public void DbInit()
        {
            _db = new EventTestContext();
            _service = new GXEventService(_db);
        }

        [TestMethod]
        public void EventDb_LocalAccountInitialized()
        {
            var accounts = _service.GetAvailableAccounts();
            if (accounts.Count < 1 || accounts[0].Email != "_local")
            {
                Assert.Fail("Failed to initialized db with local account");
            }
        }

        [TestMethod]
        public async Task EventDb_InsertWithoutReqCol_ThrowException()
        {
            var missingSummaryEv = new GXEvent
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(1)
            };
            try
            {
                await _service.AddEvent(missingSummaryEv, 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        [TestCleanup]
        public void DbCleanUp()
        {
            _db.Database.Delete();
        }
    }
}
