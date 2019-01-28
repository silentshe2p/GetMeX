using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GetMeX.ViewModels.Utilities
{
    public static class ExtensionMethods
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> lst)
        {
            var result = new ObservableCollection<T>();
            if (lst != null)
            {
                foreach (var item in lst)
                {
                    result.Add(item);
                }
            }
            return result;
        }
    }
}
