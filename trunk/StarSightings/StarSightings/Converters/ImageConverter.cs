#define TESTSERVER
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
using System.Globalization;
using System.Windows.Media.Imaging;
using System.IO;

namespace StarSightings.Converters
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            BitmapImage bmp = new BitmapImage();
            if (value == null)
                return bmp;
            System.Net.HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(value.ToString());
            //request.CookieContainer = App.CookieJar;
            request.Method = "GET";
#if (TESTSERVER)            
                string auth = Constants.BASE_AUTH_USERNAME + ":" + Constants.BASE_AUTH_PASSWORD;
                string authString = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(auth));
                request.Headers["Authorization"] = "Basic " + authString;            
#endif
            request.BeginGetResponse((s) =>
            {
                HttpWebRequest requestResult = (HttpWebRequest)s.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(s);
                Stream content = response.GetResponseStream();
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    bmp.SetSource(content);
                });
            }, request);

            return bmp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        } 
    }
}
