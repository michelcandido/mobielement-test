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
using System.Windows.Navigation;
using StarSightings.ViewModels;
using System.Windows.Controls.Primitives;

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
                    this.NavigationService.Navigate(new Uri("/SSLoginPage.xaml", UriKind.RelativeOrAbsolute));
                }
            }
            else
            {
                if (!App.ViewModel.IsDataLoaded)
                {
                    App.ViewModel.PopularItemsLoadReday += new SearchCompletedCallback(ViewModel_ItemsLoadReday);
                    App.ViewModel.LatestItemsLoadReday += new SearchCompletedCallback(ViewModel_ItemsLoadReday);
                    App.ViewModel.NearestItemsLoadReday += new SearchCompletedCallback(ViewModel_ItemsLoadReday);
                    //App.ViewModel.FollowingItemsLoadReday +=new SearchCompletedCallback(ViewModel_ItemsLoadReday);
                    App.ViewModel.MySightingsItemsLoadReday += new SearchCompletedCallback(ViewModel_ItemsLoadReday);

                    this.busyIndicator_popular.IsRunning = true;
                    this.busyIndicator_latest.IsRunning = true;
                    this.busyIndicator_nearest.IsRunning = true;
                    //this.busyIndicator_following.IsRunning = true;
                    this.busyIndicator_my.IsRunning = true;

                    App.ViewModel.LoadData();                    
                }
            }
        }

        private void ViewModel_ItemsLoadReday(object sender, SearchEventArgs e)
        {
            if (e.SearchToken.searchGroup == Constants.SEARCH_POPULAR)
            {
                this.busyIndicator_popular.IsRunning = false;
            }
            if (e.SearchToken.searchGroup == Constants.SEARCH_LATEST)
            {
                this.busyIndicator_latest.IsRunning = false;
            }
            if (e.SearchToken.searchGroup == Constants.SEARCH_NEAREST)
            {
                this.busyIndicator_nearest.IsRunning = false;
            }
            if (e.SearchToken.searchGroup == Constants.SEARCH_FOLLOWING)
            {
                this.busyIndicator_following.IsRunning = false;
            }
            if (e.SearchToken.searchGroup == Constants.SEARCH_KEYWORDSEARCH && App.ViewModel.KeywordType == Constants.KEYWORD_MY)
            {
                this.busyIndicator_my.IsRunning = false;
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
            //App.SSAPI.Alert(Constants.ALERT_SET, Constants.ALERT_TYPE_LOCATION, "Seattle");
            //App.SSAPI.Alert(Constants.ALERT_SET, Constants.ALERT_TYPE_EVENT, "Basketball game");
            //App.SSAPI.Alert(Constants.ALERT_SET, Constants.ALERT_TYPE_PLACE, "101 Coffee Shop");
            //App.SSAPI.Alert(Constants.ALERT_SET, Constants.ALERT_TYPE_PHOTOGRAPHER, "JamieBolton");
            App.SSAPI.Alert(Constants.ALERT_SET, Constants.ALERT_TYPE_PHOTOGRAPHER, "Joe Giordano 71686");
            
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
                MessageBoxResult result =
                    MessageBox.Show("Are you sure you want to logout of your StarSightings account?",
                    "Logout", MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    myLoginEventHandler = new LoginEventHandler(LogoutCompleted);
                    App.SSAPI.LoginHandler += myLoginEventHandler;
                    App.SSAPI.Logout();
                }
                
            }
            else
            {
                this.NavigationService.Navigate(new Uri("/LoginOptionsPage.xaml", UriKind.RelativeOrAbsolute));
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
            string selectedGroupId = (string)(sender as FrameworkElement).Tag;
            if (selectedGroupId == Constants.SEARCH_KEYWORDSEARCH.ToString())
                App.ViewModel.KeywordType = Constants.KEYWORD_MY;
            if (selectedItemData != null)
            {                
                this.NavigationService.Navigate(new Uri(string.Format("/DetailsPage.xaml?selectedItemId={0}&selectedGroupId={1}", selectedItemData.PhotoId, selectedGroupId), UriKind.RelativeOrAbsolute));
            }
            
        }

        private void GoToFollowed(object sender, System.Windows.Input.GestureEventArgs e)
        {
            UserViewModel selectedItemData = (sender as StackPanel).DataContext as UserViewModel;
            
            if (selectedItemData != null)
            {
                App.ViewModel.KeywordType = selectedItemData.UserType; //Constants.KEYWORD_NAME;
                App.ViewModel.SearchKeywords = selectedItemData.UserName;
                App.ViewModel.SearchKeywordSearch(true, 0, null);
                
                this.NavigationService.Navigate(new Uri("/SearchResultPage.xaml", UriKind.RelativeOrAbsolute));
            }

        }  

        private void GoToSearch(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.RelativeOrAbsolute));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New && NavigationContext.QueryString.ContainsKey("clear"))
            {
                while (NavigationService.CanGoBack)
                {
                    NavigationService.RemoveBackEntry();
                }
            }
            /*
            if (e.NavigationMode == NavigationMode.New && NavigationContext.QueryString.ContainsKey("screen"))
            {
                string screenId = NavigationContext.QueryString["screen"];
                int sId = 0;
                if (int.TryParse(screenId, out sId))
                {
                    this.panoramaControl.SelectedIndex = sId;                    
                }
            }
             * */
        }

        private void Post_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/CameraMode.xaml", UriKind.RelativeOrAbsolute));
        }

        private void GoToMyList(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ViewModel.KeywordType = Constants.KEYWORD_MY;
            NavigationService.Navigate(new Uri("/SearchResultPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void AddFollowing(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private AlertEventHandler followAlertEventHandler;
        private void DeleteFollowed(object sender, System.Windows.Input.GestureEventArgs e)
        {
            UserViewModel selectedItemData = (sender as TextBlock).DataContext as UserViewModel;

            if (selectedItemData != null)
            {
                followAlertEventHandler = new AlertEventHandler(DeleteFollowCompleted);
                App.SSAPI.AlertHandler += followAlertEventHandler;
                App.SSAPI.Alert(Constants.ALERT_REMOVE, selectedItemData.UserType, selectedItemData.UserName);
                
            }
        }

        public void DeleteFollowCompleted(object sender, AlertEventArgs e)
        {
            App.SSAPI.AlertHandler -= followAlertEventHandler;
            if (e.Successful)
            {
                App.ViewModel.Alerts = e.Alerts;
                Utils.AddOrUpdateIsolatedStorageSettings("Alerts", App.ViewModel.Alerts);
                MessageBox.Show("Your request has been set.");
                App.ViewModel.UpdateMyFollowings();
                App.ViewModel.SearchFollowing(true, 0, null);
            }
            else
            {
                MessageBox.Show("Your Alert request cannot be fullfilled, please try again.");
            }
        }

        private void GoToMoreFollowing(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(string.Format("/FollowingPage.xaml?pivotItemId={0}", (string)(sender as FrameworkElement).Tag), UriKind.RelativeOrAbsolute));
        }

        private void ListBox_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {

        }        
    }
}
