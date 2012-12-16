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
    public partial class WhoDidUSee : PhoneApplicationPage
    {
        private bool firstTime = true;

        public WhoDidUSee()
        {
            InitializeComponent();
        }

        private void onTextChange(object sender, TextChangedEventArgs e)
        {
            if (nameBox.Text == "")
            {
                nextButton.IsEnabled = false;
            }
            else
            {
                nextButton.IsEnabled = true;
                App.ViewModel.CelebName = nameBox.Text;
            }

        }

        private void onBackTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Scoop.xaml", UriKind.RelativeOrAbsolute));
        }

        private void onNextTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/AddWho.xaml", UriKind.RelativeOrAbsolute));
        }

        private void OnGetFocus(object sender, RoutedEventArgs e)
        {
            if (firstTime)
            {
                nameBox.Text = "";
            }
            firstTime = false;
        }


    }
}