namespace GetMeX.ViewModels.Utilities.Messages
{
    public class NewQueryMsg
    {
        public NewQueryMsg(string q)
        {
            Query = q;
        }

        public string Query { get; set; }
    }
}
