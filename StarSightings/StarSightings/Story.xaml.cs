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
    public partial class Story : PhoneApplicationPage
    {
        private bool firstTime = true;

        public Story()
        {
            InitializeComponent();
        }

        private void OnNextTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ViewModel.PicStory = storyBox.Text;
            this.NavigationService.Navigate(new Uri("/SightingDetail.xaml", UriKind.RelativeOrAbsolute));
        }

        private void OnBackTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Event.xaml", UriKind.RelativeOrAbsolute));
        }

        private void OnGetFocus(object sender, RoutedEventArgs e)
        {
            if (firstTime)
            {
                storyBox.Text = "";
            }
            firstTime = false;
        }
    }
}