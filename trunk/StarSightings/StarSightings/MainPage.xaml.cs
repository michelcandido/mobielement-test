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
using System.Text;
using StarSightings.Events;

namespace StarSightings
{
    public partial class MainPage : PhoneApplicationPage
    {        
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

			//Shows the rate reminder message, according to the settings of the RateReminder.
            (App.Current as App).rateReminder.Notify();
        }

		void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
			if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }		
        }

		/// <summary>
        /// Navigates to about page.
        /// </summary>
        private void GoToAbout(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/About.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Navigates to list page.
        /// </summary>
        private void GoToList(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(string.Format("/ListPage.xaml?pivotItemId={0}", (string)(sender as FrameworkElement).Tag), UriKind.RelativeOrAbsolute));
            //this.NavigationService.Navigate(new Uri("/ListPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemViewModel selectedItemData = (sender as ListBox).SelectedItem as ItemViewModel;
            if (selectedItemData != null)
            {
                this.NavigationService.Navigate(new Uri(string.Format("/DetailsPage.xaml?selectedItemId={0}", selectedItemData.ID), UriKind.RelativeOrAbsolute));
            }
        }

        private void DoTest(object sender, System.Windows.Input.GestureEventArgs e)
        {/*
            WebClient webClient = new WebClient();
            string auth = Constants.BASE_AUTH_USERNAME + ":" + Constants.BASE_AUTH_PASSWORD;
            string authString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(auth));

            webClient.Headers["Authorization"] = "Basic " + authString;
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(WebClient_DownloadStringCompleted);
            //webClient.DownloadStringAsync(new System.Uri("http://test.starsightings.com/index.php?mobile=1&start=0&limit=15&search_types="));            
            //webClient.DownloadStringAsync(new System.Uri("http://test.starsightings.com/index.php?page=device&mode=register&mobile=1&device_id=1_1212121212&device_token=att"));      
            UriBuilder baseUri = new UriBuilder("http://test.starsightings.com/index.php");
            string paras = "page=device&mode=unregister&mobile=1&device_id=1_1212121212";
            baseUri.Query = paras;
            webClient.DownloadStringAsync(baseUri.Uri);

            */
            App.SSAPI.Register += new RegisterEventHandler(RegisterDeviceCompleted);
            App.SSAPI.UnregisterDevice();
        }

        public void RegisterDeviceCompleted(object sender, RegisterEventArgs e)
        {
            //MessageBox.Show(e.Successful+":"+e.DeviceId);            
            App.ViewModel.DeviceId = "Guest";
        }

        private void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    // Showing the exact error message is useful for debugging. In a finalized application, 
                    // output a friendly and applicable string to the user instead. 
                    MessageBox.Show(e.Error.Message);
                });
            }
            else
            {
                // Save the feed into the State property in case the application is tombstoned. 
                this.State["feed"] = e.Result;

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    // Showing the exact error message is useful for debugging. In a finalized application, 
                    // output a friendly and applicable string to the user instead. 
                    MessageBox.Show(e.Result);
                });
                //UpdateFeedList(e.Result);
            }
        }
    }
}
