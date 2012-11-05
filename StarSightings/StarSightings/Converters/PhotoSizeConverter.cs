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
    public class PhotoSizeConverter : IValueConverter
    {        

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string rights = (string)value;
            if (rights == "1" || rights == "3")
                return 480;
            else
                return 160;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
