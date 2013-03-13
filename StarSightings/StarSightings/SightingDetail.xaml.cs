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
using System.Collections.ObjectModel;

namespace StarSightings
{
    public partial class Details : PhoneApplicationPage
    {
        private ObservableCollection<String> celebNameList;
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
            if (App.ViewModel.CelebNameList.Count > 1)
                this.NavigationService.Navigate(new Uri("/AddWho.xaml?edit", UriKind.RelativeOrAbsolute));
            else
                this.NavigationService.Navigate(new Uri("/WhoDidUSee.xaml?edit", UriKind.RelativeOrAbsolute));
        }

        private void Place_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Place.xaml?edit", UriKind.RelativeOrAbsolute));
        }

        private void Location_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Location.xaml?edit", UriKind.RelativeOrAbsolute));
        }

        private void Time_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/DatePickerPage.xaml?edit", UriKind.RelativeOrAbsolute));
        }

        private void Event_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Event.xaml?edit", UriKind.RelativeOrAbsolute));
        }

        private void Story_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Story.xaml?edit", UriKind.RelativeOrAbsolute));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.tbTime.Text = App.ViewModel.StoryTime.ToLocalTime().ToString();
            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.Back)
            {
                celebNameList = new ObservableCollection<String>();
                foreach (string c in App.ViewModel.CelebNameList)
                {
                    celebNameList.Add(c);
                }
                App.ViewModel.CelebNameList = celebNameList;                
            }
        }

        private void GoHome(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml?clear", UriKind.RelativeOrAbsolute));
        }
       
    }
}