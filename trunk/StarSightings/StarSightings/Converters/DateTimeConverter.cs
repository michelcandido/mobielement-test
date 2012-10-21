using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;

namespace StarSightings.Converters
{
    public class DateTimeConverter : IValueConverter
    {        

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double time = 0;
            Double.TryParse((string)value, out time);

            string result;
            DateTime dt = Utils.ConvertFromUnixTimestamp(time);
            return dt.ToString("MMMM dd, yyyy h:m tt");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
