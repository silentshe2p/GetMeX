namespace GetMeX.Models
{
    public class GoogleSuggestion
    {
        public string Suggestion { get; set; }

        public override string ToString()
        {
            return Suggestion;
        }
    }
}
