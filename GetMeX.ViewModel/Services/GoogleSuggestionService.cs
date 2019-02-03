using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using GetMeX.Models;

namespace GetMeX.ViewModels.Services
{
    public class GoogleSuggestionService
    {
        private const string suggestionEndpoint = "http://www.google.com/complete/search?output=toolbar&q={0}";
        private const string langParam = "&hl={0}";
        private string queryUri { get; set; }

        public GoogleSuggestionService(string query, string lang)
        {
            var lp = (lang == "auto") ? "" : string.Format(langParam, lang);
            queryUri = string.Format(suggestionEndpoint, query) + lp;
        }

        public async Task<List<GoogleSuggestion>> GetSuggestions()
        {
            List<GoogleSuggestion> result = null;
            using (var client = new HttpClient())
            {
               var response = await client.GetStringAsync(queryUri);
                XDocument doc = XDocument.Parse(response);
                var suggestions = from suggestion in doc.Descendants("CompleteSuggestion")
                                                select new GoogleSuggestion
                                                {
                                                    Suggestion = suggestion.Element("suggestion").Attribute("data").Value
                                                };
                result = suggestions.ToList();
            }
            return result;
        }
    }
}
