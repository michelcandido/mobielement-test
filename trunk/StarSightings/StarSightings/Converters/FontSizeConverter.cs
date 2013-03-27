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
    public class FontSizeConverter : IValueConverter
    {        

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || parameter == null)
                return 17;
            string name = (string)value;
            string sizeCase = (string)parameter;
            if (name.Contains(","))
            {
                if (sizeCase == "Summary")
                    return 21;
                else if (sizeCase == "List")
                    return 19;
                else
                    return 17;
            }
            else
            {
                if (sizeCase == "Summary")
                    return 25;
                else if (sizeCase == "List")
                    return 25;
                else if (sizeCase == "Following")
                    return 39;
                else
                    return 23;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
