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
    public partial class Details : PhoneApplicationPage
    {
        public Details()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
            pictureToShow.Source = App.ViewModel.SelectedImage;
        }              

        private void OnBackClick(object sender, EventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void OnNextClick(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Post.xaml", UriKind.RelativeOrAbsolute));
        }       

        private void Name_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {            
            this.NavigationService.Navigate(new Uri("/WhoDidUSee.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Place_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Place.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Location_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Location.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Time_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //this.NavigationService.Navigate(new Uri("/WhoDidUSee.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Event_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Event.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Story_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Story.xaml", UriKind.RelativeOrAbsolute));
        }
       
    }
}