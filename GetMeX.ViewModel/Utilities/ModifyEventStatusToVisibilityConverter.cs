using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GetMeX.ViewModels.Utilities
{
    public sealed class ModifyEventStatusToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (ModifyEventStatusMsg)value;
            return (status.Success || status.Error != null) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
