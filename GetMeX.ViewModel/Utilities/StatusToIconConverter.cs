using System;
using System.Globalization;
using System.Windows.Data;

namespace GetMeX.ViewModels.Utilities
{
    public sealed class StatusToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Check" : "Warning";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
