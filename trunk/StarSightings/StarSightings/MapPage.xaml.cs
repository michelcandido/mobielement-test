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
    public partial class MapPage : PhoneApplicationPage
    {
        public MapPage()
        {
            InitializeComponent();
        }

        private void GoToList(object sender, EventArgs e)
        {
            //this.NavigationService.Navigate(new Uri("/ListPage.xaml", UriKind.RelativeOrAbsolute));
            this.NavigationService.GoBack();
        }

        private void GoToFilter(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/FilterPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}