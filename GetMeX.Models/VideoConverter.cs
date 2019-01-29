using System;
using System.Collections.Generic;
using System.Linq;

namespace GetMeX.Models
{
    public class VideoConverter
    {
        private static Dictionary<string, Func<string, string>> ConverterDict = new Dictionary<string, Func<string, string>>()
        {
            ["youtube.com/watch?v="] = (string link) => YouTubeConverter(link)
        };

        public Func<string, string> SearchKnownSite(string link)
        {
            var keys = ConverterDict.Keys.ToList();
            foreach (var key in keys)
            {
                if (link.Contains(key))
                {
                    return ConverterDict[key];
                }
            }
            return null;
        }

        public static string YouTubeConverter(string link)
        {
            string embedLink = link.Replace("watch?v=", "embed/");
            return embedLink;
        }
    }
}
