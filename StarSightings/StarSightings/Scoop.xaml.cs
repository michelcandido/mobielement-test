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
    public partial class Scoop : PhoneApplicationPage
    {
        public Scoop()
        {
            InitializeComponent();
            pictureToShow.Source = App.ViewModel.SelectedImage;
            DataContext = App.ViewModel;
        }

        private void OnDetailsTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            DateTime zero = new DateTime(0);
            if (App.ViewModel.StoryTime.CompareTo(zero) <= 0)
                this.NavigationService.Navigate(new Uri("/DatePickerPage.xaml", UriKind.RelativeOrAbsolute));
            else
                this.NavigationService.Navigate(new Uri("/WhoDidUSee.xaml", UriKind.RelativeOrAbsolute));
        }

        private void GoHome(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml?clear", UriKind.RelativeOrAbsolute));
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            MessageBoxResult result = MessageBox.Show("This photo is saved on your camera roll.");
            if (result == MessageBoxResult.OK)
                this.NavigationService.Navigate(new Uri("/CameraMode.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}