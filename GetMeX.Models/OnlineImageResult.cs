namespace GetMeX.Models
{
    public class OnlineImageResult
    {
        public OnlineImageResult(string d, string l)
        {
            Desc = d;
            Link = l;
        }

        public string Desc { get; set; }
        public string Link { get; set; }
    }
}
