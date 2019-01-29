using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetMeX.Models
{
    public class VideoResult
    {
        public VideoResult(string link, string thumbnail)
        {
            Link = link;
            Thumbnail = thumbnail;
        }

        public string Link { get; set; }
        public string Thumbnail { get; set; }
    }
}
