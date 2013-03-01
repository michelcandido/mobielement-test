using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace StarSightings
{
    public partial class WebPage : PhoneApplicationPage
    {
        public WebPage()
        {
            InitializeComponent();
        }

        private void GoHome(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml?clear", UriKind.RelativeOrAbsolute));
        }        

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
            {
                if (NavigationContext.QueryString.ContainsKey("url"))
                {
                    string url = NavigationContext.QueryString["url"];
                    Uri uri = new Uri(url, UriKind.Absolute);
#if (DEBUG)
                    string auth = Constants.BASE_AUTH_USERNAME + ":" + Constants.BASE_AUTH_PASSWORD;
                    string authString = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(auth));
                    string hdr = "Authorization: Basic " + authString;
                    webBrowser1.Navigate(uri,null,hdr);
#else
                    webBrowser1.Navigate(uri);
#endif

                    //webBrowser1.Navigate(new Uri("http://www.google.com", UriKind.Absolute));
                }
            }
        }
    }
}