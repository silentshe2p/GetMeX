using GetMeX.Models;

namespace GetMeX.ViewModels.Utilities
{
    public class ModifyEventMsg
    {
        public EventModifyAction Action { get; set; }

        public bool SaveChangeOnline { get; set; }

        public GXEvent Event { get; set; }
    }
}
