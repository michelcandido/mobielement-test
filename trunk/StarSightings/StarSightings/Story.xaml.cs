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
        public Story()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
        }        

        private void OnBackClick(object sender, EventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void OnNextClick(object sender, EventArgs e)
        {
            App.ViewModel.PicStory = storyBox.Text;
            this.NavigationService.Navigate(new Uri("/SightingDetail.xaml", UriKind.RelativeOrAbsolute));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            storyBox.Focus();
        }
    }
}