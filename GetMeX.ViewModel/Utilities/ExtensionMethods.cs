using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Globalization;
using Google.Apis.Calendar.v3.Data;
using NodaTime;
using GetMeX.Models;

namespace GetMeX.ViewModels.Utilities
{
    public static class ExtensionMethods
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> lst)
        {
            if (lst == null)
                return true;

            return !lst.Any();
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> lst)
        {
            var result = new ObservableCollection<T>();
            if (lst != null)
            {
                foreach (var item in lst)
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public static List<GXEvent> ToGXEvents(this IEnumerable<Event> events, int accId)
        {
            var result = new List<GXEvent>();

            foreach (var e in events)
            {
                var startDateTime = (e.Start.DateTimeRaw != null) ? DateTimeOffset.Parse(e.Start.DateTimeRaw)
                                                                                                : (DateTimeOffset?)null;
                var endDateTime = (e.End.DateTimeRaw != null) ? DateTimeOffset.Parse(e.End.DateTimeRaw)
                                                                                            : (DateTimeOffset?)null;
                var gxEvent = new GXEvent
                {
                    AID = accId,
                    GID = e.Id,
                    Location = e.Location,
                    StartDate = startDateTime.HasValue ? startDateTime.Value.Date 
                                        : DateTime.ParseExact(e.Start.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    StartDateTime = startDateTime,
                    EndDate = endDateTime.HasValue ? endDateTime.Value.Date 
                                        : DateTime.ParseExact(e.End.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    EndDateTime = endDateTime,
                    Summary = e.Summary,
                    Description = e.Description,
                    ColorId = (e.ColorId != null) ? byte.Parse(e.ColorId) : (byte)1
                };
                result.Add(gxEvent);
            }
            return result;
        }

        public static Event ToEvent(this GXEvent e)
        {
            DateTimeZone tz = DateTimeZoneProviders.Tzdb.GetSystemDefault();
            return new Event
            {
                Id = null,
                Location = e.Location,
                Start = new EventDateTime
                {
                    Date = e.StartDate.ToString(),
                    DateTime = e.StartDateTime.HasValue ? e.StartDateTime.Value.DateTime : (DateTime?)null,
                    TimeZone = tz.ToString()
                },
                End = new EventDateTime
                {
                    Date = e.EndDate.ToString(),
                    DateTime = e.EndDateTime.HasValue ? e.EndDateTime.Value.DateTime : (DateTime?)null,
                    TimeZone = tz.ToString()
                },
                Summary = e.Summary,
                Description = e.Description,
                ColorId = e.ColorId.ToString()
            };
        }
    }
}
