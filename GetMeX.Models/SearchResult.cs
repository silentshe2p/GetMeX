using System.Collections.Generic;

namespace GetMeX.Models
{
    public class SearchResult
    {
        public SearchResult(string header, string link, string description)
        {
            Header = header;
            Link = link;
            Description = description;
        }

        public string Header { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public int Index { get; set; }
        public List<OnlineImageResult> Images { get; set; }
    }
}
