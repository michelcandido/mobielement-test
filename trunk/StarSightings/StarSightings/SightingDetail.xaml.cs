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

        private void OnNextTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Post.xaml", UriKind.RelativeOrAbsolute));
        }

        private void OnBackTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Story.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Name_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/WhoDidUSee.xaml", UriKind.RelativeOrAbsolute));
        }
        /*
        private void OnTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //            WebBrowserTask webBrowserTask = new WebBrowserTask();
            //System.Windows.MessageBox.Show((sender as ListBox).SelectedIndex.ToString());
            App.ViewModel.CelebNameList.RemoveAt((sender as ListBox).SelectedIndex);
            //            webBrowserTask.Uri = new Uri(((string)((sender as ListBoxItem).Tag)).Trim(), UriKind.Absolute);
            //            webBrowserTask.Show();
        }*/
    }
}