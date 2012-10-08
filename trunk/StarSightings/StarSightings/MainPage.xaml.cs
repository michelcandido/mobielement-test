﻿using System;
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
            if (!App.Config.IsAppInit)
            {
                App.Config.InitApp();
                this.tbUserName.DataContext = App.ViewModel.User;
            }

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
                this.NavigationService.Navigate(new Uri(string.Format("/DetailsPage.xaml?selectedItemId={0}&selectedGroupId={1}", selectedItemData.PhotoId, (string)(sender as FrameworkElement).Tag), UriKind.RelativeOrAbsolute));
            }
            (sender as ListBox).SelectedIndex = -1;
        }

        private void DoTest(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/SignupPage.xaml", UriKind.RelativeOrAbsolute));
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
