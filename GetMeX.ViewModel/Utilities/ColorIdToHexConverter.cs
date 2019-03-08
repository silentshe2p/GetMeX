using System;
using System.Globalization;
using System.Windows.Data;
using GetMeX.Models;

namespace GetMeX.ViewModels.Utilities
{
    public sealed class ColorIdToHexConverter : IValueConverter
    {
        private ColorIdHex _colorConverter = new ColorIdHex();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _colorConverter.ColorIdToHex((byte)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _colorConverter.ColorToId((string)value);
        }
    }
}
