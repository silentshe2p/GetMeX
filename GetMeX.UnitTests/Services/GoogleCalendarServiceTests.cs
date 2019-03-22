using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GetMeX.Models;
using GetMeX.ViewModels.Utilities;
using System.IO;

namespace GetMeX.ViewModels.Services.Tests
{
    [TestClass]
    public class GoogleCalendarServiceTests
    {
        [TestMethod]
        public async Task CalendarService_TestAddGetDelete()
        {
            // Path for credential and token
            const string credPath = @"auth\credentials.json";
            const string tokenPath = @"auth\token.json";
            var currentDir = Directory.GetCurrentDirectory();

            AppDomain.CurrentDomain.SetData("GoogleCalendarCredentialPath", Path.Combine(currentDir, credPath));
            AppDomain.CurrentDomain.SetData("GoogleCalendarTokenPath", Path.Combine(currentDir, tokenPath));

            // Actual test
            var service = new GoogleCalendarService();
            var eventId = await CalendarService_SucessfullyAddEvent(service);
            if (eventId.IsNullOrEmpty())
            {
                Assert.Fail("Empty google event id retrieved");
            }
            await CalendarService_SucessfullyRetrieveAddedEvent(service, eventId);
            await CalendarService_SucessfullyDeleteAddedEvent(service, eventId);

            // Clean up used token
            Directory.Delete(Path.Combine(currentDir, tokenPath), true);
        }

        private GXEvent CreateDummyEvent()
        {
            return new GXEvent
            {
                Summary = "dummy event",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(1)
            };
        }

        public async Task<string> CalendarService_SucessfullyAddEvent(GoogleCalendarService service)
        {
            var dummyEvent = CreateDummyEvent().ToEvent();
            string dummyId = "";
            try
            {
                dummyId = await service.AddEvent(dummyEvent);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to add event: " + ex.Message);
            }
            return dummyId;
        }

        public async Task CalendarService_SucessfullyRetrieveAddedEvent(GoogleCalendarService service, string eventId)
        {
            try
            {
                var dummyRetrieved = await service.GetEvent(eventId);
                if (dummyRetrieved == null || dummyRetrieved.Summary != "dummy event")
                {
                    Assert.Fail("Incorrect dummy event retrieved");
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to get event: " + ex.Message);
            }
        }

        public async Task CalendarService_SucessfullyDeleteAddedEvent(GoogleCalendarService service, string eventId)
        {
            try
            {
                await service.DeleteEvent(eventId);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to delete event: " + ex.Message);
            }
        }
    }
}
