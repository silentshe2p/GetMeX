﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace GetMeX.ViewModels.Services
{
    public class GoogleCalendarService
    {
        private TimeSpan _authTimeout = TimeSpan.FromMinutes(5);
        private static string ApplicationName = "GetMeX - Events";
        private static string[] ViewScopes = { CalendarService.Scope.CalendarReadonly };
        private static string[] EditScopes = { CalendarService.Scope.Calendar,
                                                                CalendarService.Scope.CalendarEvents,
                                                                CalendarService.Scope.CalendarReadonly };
        private UserCredential credential;
        private CalendarService calService;

        public GoogleCalendarService()
        {
            credential = null;
            calService = null;
        }

        private async Task Auth(string[] scopes)
        {
            var credentialPath = AppDomain.CurrentDomain.GetData("GoogleCalendarCredentialPath").ToString();
            var tokenPath = AppDomain.CurrentDomain.GetData("GoogleCalendarTokenPath").ToString();

            if (!File.Exists(credentialPath))
            {
                throw new FileNotFoundException("Client credential file not found");
            }

            using (var stream = new FileStream(credentialPath, FileMode.Open, FileAccess.Read))
            {
                // Stores the user's access and refresh tokens after the authorization flow completes
                var authTask = GoogleWebAuthorizationBroker.AuthorizeAsync(
                                            GoogleClientSecrets.Load(stream).Secrets,
                                            scopes,
                                            "user",
                                            CancellationToken.None,
                                            new FileDataStore(tokenPath, true));
                using (var cancelTokenSource = new CancellationTokenSource())
                {
                    var completed = await Task.WhenAny(authTask, Task.Delay(_authTimeout, cancelTokenSource.Token));
                    if (completed != authTask)
                    {
                        throw new TimeoutException("Google Calendar authorization timed out");
                    }
                    else
                    {
                        cancelTokenSource.Cancel();
                        credential = await authTask;
                    }
                }
            }

            // Google Calendar Api service
            calService = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
        }

        public async Task<Events> GetEvents(DateTime timeMax, string calendar="primary")
        {
            if (calService == null)
            {
                await Auth(EditScopes);
            }

            EventsResource.ListRequest req = calService.Events.List(calendar);
            req.TimeMin = DateTime.Now;
            req.TimeMax = timeMax;
            req.ShowDeleted = false;
            req.SingleEvents = true;
            req.OrderBy = EventsResource.ListRequest.OrderByEnum.Updated;
            return await req.ExecuteAsync();
        }

        public async Task<Event> GetEvent(string eventId, string calendar="primary")
        {
            if (calService == null)
            {
                await Auth(EditScopes);
            }

            EventsResource.GetRequest req = calService.Events.Get(calendar, eventId);
            return await req.ExecuteAsync();
        }

        public async Task<string> AddEvent(Event e, string calendar="primary")
        {
            if (calService == null)
            {
                await Auth(EditScopes);
            }

            EventsResource.InsertRequest req = calService.Events.Insert(e, calendar);
            var ev = await req.ExecuteAsync();
            return ev.Id;
        }

        public async Task DeleteEvent(string eventId, string calendar="primary")
        {
            if (calService == null)
            {
                await Auth(EditScopes);
            }

            EventsResource.DeleteRequest req = calService.Events.Delete(calendar, eventId);
            await req.ExecuteAsync();
        }

        public async Task UpdateEvent(Event e, string calendar = "primary")
        {
            if (calService == null)
            {
                await Auth(EditScopes);
            }

            EventsResource.UpdateRequest req = calService.Events.Update(e, calendar, e.Id);
            await req.ExecuteAsync();
        }
    }
}
