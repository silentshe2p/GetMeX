using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using GetMeX.Models;

namespace GetMeX.ViewModels.Utilities
{
    public sealed class SaveChangeOnlineVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentEvent = (GXEvent)value;
            return (currentEvent.EID == 0 || currentEvent.AID != 1) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
