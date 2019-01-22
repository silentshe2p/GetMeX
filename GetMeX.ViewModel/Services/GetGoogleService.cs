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
        private int start { get; set; }
        public GetGoogleService(string q, string l, int s=0)
        {
            var lp = (l == "auto") ? "" : string.Format(langParam, l);
            queryUri = string.Format(searchEndpoint, q, s) + lp;
            start = s;
        }

        public async Task<List<SearchResult>> GetGoogleSearches()
        {
            var results = new List<SearchResult>();
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(queryUri);
                var resultsPattern = "<h3 class=[\\s\\S]*?<span class=\"st\">[\\s\\S]*?</span>";
                var headerPattern = @"<h3 class=[\s\S]*?/h3>";
                var linkPattern = @"<cite>[\s\S]*?</cite>";
                var descPattern = "<span class=\"st\">[\\s\\S]*?</span>";
                var bracketPattern = @"<[\s\S]*?>";
                Regex resultsRegex = new Regex(resultsPattern);
                foreach (Match m in resultsRegex.Matches(response))
                {
                    Match headerRaw = Regex.Match(m.Value, headerPattern);
                    Match linkRaw = Regex.Match(m.Value, linkPattern);
                    Match descRaw = Regex.Match(m.Value, descPattern);
                    var header = Regex.Replace(headerRaw.Value, bracketPattern, string.Empty);
                    var link = Regex.Replace(linkRaw.Value, bracketPattern, string.Empty);
                    var desc = Regex.Replace(descRaw.Value, bracketPattern, string.Empty);
                    results.Add(new SearchResult(WebUtility.HtmlDecode(header), link, WebUtility.HtmlDecode(desc)));
                }
            }
            return results;
        } 
    }
}
