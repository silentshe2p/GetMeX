using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GetMeX.Models;

namespace GetMeX.ViewModels.Services
{
    public class ImageRetrieverService
    {
        private static string defaultOrder = "default";
        private static string commonPattern = "{0}['\"//]*([^,;'\"]+)"; // TODO: escaped " case
        private static ImageSiteStructure _iss = new ImageSiteStructure();
        private int _resultLimit;
        private int _descLimit = 20;
        private int _linkMinLength = 8; // http://_

        public ImageRetrieverService(int limit=2)
        {
            _resultLimit = limit;
        }

        public void SetLimit(int limit)
        {
            _resultLimit = limit;
        }

        public async Task<List<OnlineImageResult>> RetrieveImages(string url)
        {
            var results = new List<OnlineImageResult>();
            var structure = _iss.GetSiteStructure(url);
            if (structure == null)
            {
                return results;
            }
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(url);
                var descPattern = string.Format(commonPattern, structure[(int)StructureElement.desc]);
                var linkPattern = string.Format(commonPattern, structure[(int)StructureElement.link]);
                var merged = (structure[(int)StructureElement.order] == defaultOrder) 
                                        ? descPattern + "[^<(]*?" + linkPattern
                                        : linkPattern + "[^<(]*?" + descPattern;
                var descIndex = (structure[(int)StructureElement.order] == defaultOrder) ? 1 : 2;
                var linkIndex = 2 / descIndex;
                Regex mergedRegex = new Regex(merged);
                var resultCount = 0;
                foreach (Match m in mergedRegex.Matches(response))
                {
                    if (resultCount >= _resultLimit)
                    {
                        break;
                    }
                    var fullDesc = Regex.Unescape(WebUtility.HtmlDecode(m.Groups[descIndex].Value));
                    var desc = "";
                    if (fullDesc.Length > _descLimit)
                    {
                        desc = fullDesc.Substring(0, _descLimit) + "...";
                    }
                    var link = Regex.Unescape(m.Groups[linkIndex].Value);
                    if (fullDesc != null && link != null && link.Length > _linkMinLength)
                    {
                        results.Add(new OnlineImageResult(fullDesc, desc, link));
                        resultCount++;
                    }
                }
            }
            return results;
        }
    }
}
