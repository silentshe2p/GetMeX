using System;
using System.Globalization;
using System.Windows.Data;

namespace GetMeX.ViewModels.Utilities
{
    public sealed class HeightToMarginConverter : IValueConverter
    {
        // For EventEditWindow only, use parameter if generalizing for other windows
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dialogHeight = (double)value;
            var marginTop = dialogHeight / 14 /*14 rows total */ * 11 /*button row @ 11th*/;
            // Margin format: left top right bottom
            return string.Format("0 {0} 0 0", marginTop);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
