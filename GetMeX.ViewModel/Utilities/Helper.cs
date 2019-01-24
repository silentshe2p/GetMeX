using System.Collections.Generic;
using System.Linq;

namespace GetMeX.ViewModels.Utilities
{
    public class Helper
    {
        public int ListSum(List<int> lst)
        {
            var it = lst.Aggregate<int>((a, b) => a + b);
            return it;
        }
    }
}
