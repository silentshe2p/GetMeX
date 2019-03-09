using System;
using System.Globalization;
using System.Windows.Data;

namespace GetMeX.ViewModels.Utilities
{
    public sealed class StatusToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (ModifyEventStatusMsg)value;
            if (status.Error != null)
            {
                return status.Error;
            }
            else if (status.Success && status.Deleted)
            {
                return "Event was successfully deleted";
            }
            else
            {
                return "Event was successfully modified";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
