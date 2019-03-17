using GetMeX.Models;

namespace GetMeX.ViewModels.Utilities.Messages
{
    public class UpdateTreeViewMsg
    {
        public GXEvent ModifiedEvent { get; set; }

        public EventModifyAction Action { get; set; }
    }
}
