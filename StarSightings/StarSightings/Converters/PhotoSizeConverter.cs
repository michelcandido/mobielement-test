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
            /*
            string filename = (string)parameter;
            filename = filename.Substring(filename.IndexOf("thumb"));
            int start = filename.IndexOf('.') + 1;
            int end = filename.IndexOf('x');
            string size_string = filename.Substring(start, end - start);
            int size_int = 160;
            Int32.TryParse(size_string, out size_int);
             * */
            if (rights == "1" || rights == "3")
                //return Math.Min(480,size_int*2);
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
