using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GetMeX.ViewModels.Utilities
{
    public sealed class PeriodConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(System.Windows.Media.Brush))
            {
                return (value is int) ? "Orange" : "Green";
            }
            else if (targetType == typeof(Visibility))
            {
                // Show total overview for only quarter which is not an int
                return (value is int) ? Visibility.Collapsed : Visibility.Visible;
            }
            else if (targetType == typeof(Double))
            {
                // Smaller font size for quarter
                return (value is int) ? 20 : 16;
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
