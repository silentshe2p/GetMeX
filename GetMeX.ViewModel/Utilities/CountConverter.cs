using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace GetMeX.ViewModels.Utilities
{
    public sealed class CountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ListViewItem lvi = value as ListViewItem;
            int count = 0;

            if (lvi != null)
            {
                ListView lv = ItemsControl.ItemsControlFromItemContainer(lvi) as ListView;
                count = lv.ItemContainerGenerator.IndexFromContainer(lvi) + 1;
            }
            return count;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}