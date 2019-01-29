using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GetMeX.Models;

namespace GetMeX.ViewModels.Services
{
    public class GetGoogleService
    {
        private static string searchEndpoint = "https://www.google.com/search?q={0}&start={1}";
        private static string langParam = "&lr=lang_{0}";
        private string queryUri { get; set; }
        public GetGoogleService(string query, string lang, int start=0)
        {
            var lp = (lang == "auto") ? "" : string.Format(langParam, lang);
            queryUri = string.Format(searchEndpoint, query, start) + lp;
        }

        public VideoResult VideoResultConverter(string link, string thumbnail) {
            var converters = new VideoConverter();
            var converter = converters.SearchKnownSite(link);
            if (thumbnail == null || converter == null)
            {
                return null;
            }
            return new VideoResult(converter(link), thumbnail);
        }

        public async Task<List<SearchResult>> GetGoogleSearches()
        {
            var results = new List<SearchResult>();
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(queryUri);
                // Capture until next tag in case span inside span
                var resultsPattern = "<h3 class=[^>]*><a href=[^>]*((?=http)[^&;\"\\s]+)[\\s\\S]*?<span class=\"st\">[\\s\\S]*?</span>(?:<[^>]*>)";
                var headerPattern = @"<h3 class=[\s\S]*?/h3>";
                //var linkPattern = @"<cite[\s\S]*?/cite>"; /* shortened link ver */
                var descPattern = "<span class=\"st\">[\\s\\S]*?/span>(?:<[^>]*>)";
                var imgPattern = "<img src=[\"]*((?=http)[^\"\\s]+)[\\s\\S]*?>";
                var bracketPattern = @"<[\s\S]*?>";
                Regex resultsRegex = new Regex(resultsPattern);
                foreach (Match m in resultsRegex.Matches(response))
                {
                    Match headerRaw = Regex.Match(m.Value, headerPattern);
                    //Match linkRaw = Regex.Match(m.Value, linkPattern); /* shortened link ver */
                    Match descRaw = Regex.Match(m.Value, descPattern);
                    Match imgLink = Regex.Match(m.Value, imgPattern);
                    var header = Regex.Replace(headerRaw.Value, bracketPattern, string.Empty);
                    //var link = Regex.Replace(linkRaw.Value, bracketPattern, string.Empty); /* shortened link ver */
                    var link = m.Groups[1].Value; /* full link ver - group ((?=http)[^&;\"\\s]+) */
                    var desc = Regex.Replace(descRaw.Value, bracketPattern, string.Empty);
                    var thumbnail = imgLink.Groups[1].Value;
                    results.Add(new SearchResult(WebUtility.HtmlDecode(header),
                                                                    WebUtility.UrlDecode(link),
                                                                    WebUtility.HtmlDecode(desc),
                                                                    VideoResultConverter(WebUtility.UrlDecode(link), thumbnail)));
                }
            }
            return results;
        } 
    }
}
