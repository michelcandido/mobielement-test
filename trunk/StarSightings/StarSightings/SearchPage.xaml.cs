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
    public partial class SearchPage : PhoneApplicationPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = (sender as ListBox).SelectedIndex;
            (sender as ListBox).SelectedItem = null;
            this.NavigationService.Navigate(new Uri(string.Format("/SearchInputPage.xaml?page={0}", index), UriKind.RelativeOrAbsolute));
        }

        private void GoHome(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml?clear", UriKind.RelativeOrAbsolute));
        }

        
    }
}