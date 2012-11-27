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
            if (!App.Config.IsAppInit)
            {
                App.Config.InitAppCompletedHandler +=new InitAppHandler(InitAppCompleted);
                App.Config.InitApp();                
            }			
        }

        private void InitAppCompleted(Object sender, SSEventArgs e)
        {
            if (!e.Successful)
            {
                // problem with init, probably because of login, we need to show login page.
                if (App.ViewModel.AccountType != Constants.ACCOUNT_TYPE_DEVICE)
                {
                    this.NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.RelativeOrAbsolute));
                }
            }
            else
            {
                if (!App.ViewModel.IsDataLoaded)
                {
                    App.ViewModel.LoadData();
                }
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
            /*
            ItemViewModel selectedItemData = (sender as ListBox).SelectedItem as ItemViewModel;
            if (selectedItemData != null)
            {
                this.NavigationService.Navigate(new Uri(string.Format("/DetailsPage.xaml?selectedItemId={0}&selectedGroupId={1}", selectedItemData.PhotoId, (string)(sender as FrameworkElement).Tag), UriKind.RelativeOrAbsolute));
            }
            (sender as ListBox).SelectedIndex = -1;
             * */
        }

        private void DoTest(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //this.NavigationService.Navigate(new Uri("/SignupPage.xaml", UriKind.RelativeOrAbsolute));
            //App.SSAPI.Logout();
            App.SSAPI.Keyword(Constants.KEYWORD_PLACE, "coffee");
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

        private void GoToCameraMode(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/CameraMode.xaml", UriKind.RelativeOrAbsolute));
        }        

        private LoginEventHandler myLoginEventHandler;
        private void GoToLogin(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (!App.ViewModel.NeedLogin)
            {
                myLoginEventHandler = new LoginEventHandler(LogoutCompleted);
                App.SSAPI.LoginHandler += myLoginEventHandler;
                App.SSAPI.Logout();
            }
            else
            {
                this.NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        public void LogoutCompleted(object sender, LoginEventArgs e)
        {
            App.SSAPI.LoginHandler -= myLoginEventHandler;
            if (e.Successful)
            {                                
                App.ViewModel.AccountType = Constants.ACCOUNT_TYPE_DEVICE;
                Utils.AddOrUpdateIsolatedStorageSettings("AccountType", App.ViewModel.AccountType);
                Utils.RemoveIsolatedStorageSettings("User");
                if (App.ViewModel.AccountType == Constants.ACCOUNT_TYPE_DEVICE)
                    App.ViewModel.NeedLogin = true;
                else
                    App.ViewModel.NeedLogin = false;
                App.Config.Login();
            }
            else
            {
                MessageBox.Show("Cannot logout, please try again.");
            }
        }

        private void Grid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ItemViewModel selectedItemData = (sender as Grid).DataContext as ItemViewModel;
            if (selectedItemData != null)
            {
                this.NavigationService.Navigate(new Uri(string.Format("/DetailsPage.xaml?selectedItemId={0}&selectedGroupId={1}", selectedItemData.PhotoId, (string)(sender as FrameworkElement).Tag), UriKind.RelativeOrAbsolute));
            }
            
        }        

        private void GoToSearch(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.RelativeOrAbsolute));
        }

       
    }
}
