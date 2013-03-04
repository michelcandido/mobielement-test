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
    public class FilterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int type = (int)value;
            string para = (string)parameter;
            if (para == "category")
            {
                return Constants.categoryFilterNames[type];
            }
            else if (para == "map")
            {
                return Constants.mapFilterNames[type];
            }
            else if (para == "follow")
            {
                return Constants.followFilterNames[type];
            }
            else
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
