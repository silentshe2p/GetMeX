namespace GetMeX.Models
{
    public class OnlineImageResult
    {
        public OnlineImageResult(string fullDescription, string description, string link)
        {
            FullDesc = fullDescription;
            Desc = description;
            Link = link;
        }

        public string FullDesc { get; set; }
        public string Desc { get; set; }
        public string Link { get; set; }
    }
}
