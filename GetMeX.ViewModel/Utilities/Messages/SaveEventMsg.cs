using GetMeX.Models;

namespace GetMeX.ViewModels.Utilities.Messages
{
    public class SaveEventMsg
    {
        public bool SaveOnline { get; set; }

        public GXEvent Event { get; set; }
    }
}
