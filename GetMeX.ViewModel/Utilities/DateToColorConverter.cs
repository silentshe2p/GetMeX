using System;
using System.Globalization;
using System.Windows.Data;

namespace GetMeX.ViewModels.Utilities
{
    public sealed class DateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                return (DateTime)value < DateTime.Now ? "AliceBlue" : "Beige";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
