namespace GetMeX.ViewModels.Utilities.Messages
{
    public class FetchResultsMsg
    {
        public FetchResultsMsg(string q, int s)
        {
            query = q;
            start = s;
        }

        public string query { get; set; }
        public int start { get; set; }
    }
}
