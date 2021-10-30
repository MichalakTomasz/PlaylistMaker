using System;
using System.Globalization;
using System.Windows.Data;

namespace PlaylistMaker.Commons.Converters
{
    public class TimeSpanToFormatedMinutesString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (TimeSpan)value;
            if (val == default)
                return val;
            var fp = $"{val.Hours}:{val.Minutes:00}:{val.Seconds:00}";
            return fp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
