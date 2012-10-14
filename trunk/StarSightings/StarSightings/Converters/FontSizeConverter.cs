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
                return 20;
            string name = (string)value;
            string sizeCase = (string)parameter;
            if (name.Contains(",")) 
            {
                if (sizeCase == "Summary")
                    return 20;
                else if (sizeCase == "List")
                    return 17.333;
                else
                    return 0;
            }
            else
                return 22.667;                
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
