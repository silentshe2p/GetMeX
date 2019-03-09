using System;
using System.Globalization;
using System.Windows.Data;

namespace GetMeX.ViewModels.Utilities
{
    public sealed class StatusToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Green" : "OrangeRed";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
