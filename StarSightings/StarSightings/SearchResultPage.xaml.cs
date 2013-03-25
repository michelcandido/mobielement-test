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
using System.Windows.Navigation;
using StarSightings.Events;
using StarSightings.ViewModels;
using System.Text;
using System.ComponentModel;

namespace StarSightings
{
    public partial class SearchResultPage : PhoneApplicationPage
    {
        private PivotItem followingPivotItem;
        private BackgroundWorker bw_clearBackEntry = new BackgroundWorker();

        public SearchResultPage()
        {
            InitializeComponent();

            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(SearchResultPage_Loaded);
            bw_clearBackEntry.DoWork += new DoWorkEventHandler(clearBackEntry);
            followingPivotItem = (this.pivotControl.Items.Single(p => ((PivotItem)p).Name == "FollowingPivotItem")) as PivotItem;
        }

        void SearchResultPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
            {
                if (App.ViewModel.KeywordType != Constants.KEYWORD_MY)
                    //this.pivotControl.Title = App.ViewModel.SearchKeywords.ToUpper();
                    this.Title.Text = App.ViewModel.SearchKeywords.ToUpper();
                else
                    //this.pivotControl.Title = App.ViewModel.User.UserName.ToUpper();
                    this.Title.Text = App.ViewModel.User.UserName.ToUpper();

                if (App.ViewModel.KeywordType != Constants.KEYWORD_USER && App.ViewModel.KeywordType != Constants.KEYWORD_MY && followingPivotItem != null)
                {
                    this.pivotControl.Items.Remove(followingPivotItem);
                }

                if (App.ViewModel.KeywordType == Constants.KEYWORD_MY)
                {
                    this.Follow_Sightings.Visibility = Visibility.Collapsed;
                }

                if (App.ViewModel.KeywordType == Constants.KEYWORD_MY || App.ViewModel.KeywordType == Constants.KEYWORD_USER)
                {                    
                    this.By.Visibility = Visibility.Visible;
                }
                else
                {
                    this.By.Visibility = Visibility.Collapsed;
                }
            }

            if (App.ViewModel.KeywordType == Constants.KEYWORD_MY)
            {
                this.CelebsList.ItemsSource = App.ViewModel.MyFollowingCelebsSummaryList.View;
                this.UsersList.ItemsSource = App.ViewModel.MyFollowingUsersSummaryList.View;
            }
            else
            {
            }

            if (this.CelebsList.Items.Count == 0)
            {
                this.NoCeleb.Visibility = Visibility.Visible;
                this.MoreCeleb.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.NoCeleb.Visibility = Visibility.Collapsed;
                this.MoreCeleb.Visibility = Visibility.Visible;
            }

            if (this.UsersList.Items.Count == 0)
            {
                this.NoUser.Visibility = Visibility.Visible;
                this.MoreUser.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.NoUser.Visibility = Visibility.Collapsed;
                this.MoreUser.Visibility = Visibility.Visible;
            }

            if (e.NavigationMode == NavigationMode.New && NavigationContext.QueryString.ContainsKey("clear"))
            {
                bw_clearBackEntry.RunWorkerAsync();
            }
        }

        private void clearBackEntry(object sender, DoWorkEventArgs ea)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                while (NavigationService.CanGoBack)
                {
                    NavigationService.RemoveBackEntry();
                }
            });
        }

        /// <summary>
        /// Navigates to filter page.
        /// </summary>       
        private void GoToFilter(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri(string.Format("/FilterPage.xaml?selectedGroupId={0}", 4), UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Navigates to map page.
        /// </summary>
        private void GoToMap(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri(string.Format("/MapPage.xaml?selectedGroupId={0}", 4), UriKind.RelativeOrAbsolute));
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemViewModel selectedItemData = (sender as ListBox).SelectedItem as ItemViewModel;
            if (selectedItemData != null)
            {
                this.NavigationService.Navigate(new Uri(string.Format("/DetailsPage.xaml?selectedItemId={0}", selectedItemData.ID), UriKind.RelativeOrAbsolute));
            }
        }

        private void GoToDetails(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(((NavigationEventArgs)e).Uri);
        }

        private void GoToSearch(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void GoToPost(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/CameraMode.xaml", UriKind.RelativeOrAbsolute));
        }

        private void GoToFollowed(object sender, System.Windows.Input.GestureEventArgs e)
        {
            UserViewModel selectedItemData = (sender as StackPanel).DataContext as UserViewModel;

            if (selectedItemData != null)
            {
                App.ViewModel.KeywordType = selectedItemData.UserType; //Constants.KEYWORD_NAME;
                App.ViewModel.SearchKeywords = selectedItemData.UserName;
                App.ViewModel.SearchKeywordSearch(true, 0, null);

                this.NavigationService.Navigate(new Uri("/SearchResultPage.xaml?refresh", UriKind.RelativeOrAbsolute));                
            }

        }  

        private AlertEventHandler followAlertEventHandler;
        private void DeleteFollowed(object sender, System.Windows.Input.GestureEventArgs e)
        {
            UserViewModel selectedItemData = (sender as Image).DataContext as UserViewModel;

            if (selectedItemData != null)
            {
                if (App.ViewModel.KeywordType == Constants.KEYWORD_MY)
                {
                    /*
                    if (selectedItemData.UserType == Constants.KEYWORD_NAME)
                    {
                        App.ViewModel.MyFollowingCelebs.Remove(selectedItemData);
                        App.ViewModel.UpdateMyFollowingCelebsSummaryList();
                        this.CelebsList.ItemsSource = App.ViewModel.MyFollowingCelebsSummaryList.View;
                    }
                    else if (selectedItemData.UserType == Constants.KEYWORD_USER)
                    {
                        App.ViewModel.MyFollowingUsers.Remove(selectedItemData);
                        App.ViewModel.UpdateMyFollowingUsersSummaryList();
                        this.UsersList.ItemsSource = App.ViewModel.MyFollowingUsersSummaryList.View;
                    }
                    */
                    ((sender as Image).Parent as Grid).Visibility = Visibility.Collapsed;

                    followAlertEventHandler = new AlertEventHandler(DeleteFollowCompleted);
                    App.SSAPI.AlertHandler += followAlertEventHandler;
                    App.SSAPI.Alert(Constants.ALERT_REMOVE, selectedItemData.UserType + "/", selectedItemData.UserName);
                }
                else
                {
                }
            }
        }

        public void DeleteFollowCompleted(object sender, AlertEventArgs e)
        {
            App.SSAPI.AlertHandler -= followAlertEventHandler;
            if (e.Successful)
            {
                App.ViewModel.Alerts = e.Alerts;
                Utils.AddOrUpdateIsolatedStorageSettings("Alerts", App.ViewModel.Alerts);
                MessageBox.Show("Removed from your following list.");
                App.ViewModel.UpdateMyFollowings();
                this.CelebsList.ItemsSource = App.ViewModel.MyFollowingCelebsSummaryList.View;
                this.UsersList.ItemsSource = App.ViewModel.MyFollowingUsersSummaryList.View;
                App.ViewModel.SearchFollowing(true, 0, null);
            }
            else
            {
                MessageBox.Show("Cannot remove it from your following list, please try again.");
            }
        }

        private void GoToMoreFollowing(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(string.Format("/FollowingPage.xaml?pivotItemId={0}", (string)(sender as FrameworkElement).Tag), UriKind.RelativeOrAbsolute));
        }

        private bool isFollowing;
        private void Follow_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            isFollowing = App.ViewModel.Alerts.Where(alert => alert.Type == App.ViewModel.KeywordType).Where(alert => alert.Name.Equals(App.ViewModel.SearchKeywords, StringComparison.OrdinalIgnoreCase)).Count() != 0;
            followAlertEventHandler = new AlertEventHandler(FollowCompleted);
            App.SSAPI.AlertHandler += followAlertEventHandler;
            if (isFollowing)
                App.SSAPI.Alert(Constants.ALERT_REMOVE, App.ViewModel.KeywordType + "/", App.ViewModel.SearchKeywords);
            else
                App.SSAPI.Alert(Constants.ALERT_SET, App.ViewModel.KeywordType + "/", App.ViewModel.SearchKeywords);
            /*
            string name = ((string)((sender as Grid).Tag)).Trim();
            isFollowing = App.ViewModel.MyFollowingCelebs.Where(user => user.UserName == name).Count() != 0;
            followAlertEventHandler = new AlertEventHandler(FollowCompleted);
            App.SSAPI.AlertHandler += followAlertEventHandler;
            if (isFollowing)
                App.SSAPI.Alert(Constants.ALERT_REMOVE, Constants.ALERT_TYPE_CELEBRITY, name);
            else
                App.SSAPI.Alert(Constants.ALERT_SET, Constants.ALERT_TYPE_CELEBRITY, name);
             * */
        }
        public void FollowCompleted(object sender, AlertEventArgs e)
        {
            App.SSAPI.AlertHandler -= followAlertEventHandler;
            if (e.Successful)
            {
                // force update the binding
                //(this.slideView.SelectedItem as ItemViewModel).Celebs = (string[])((this.slideView.SelectedItem as ItemViewModel).Celebs.Clone());
                                               
                App.ViewModel.Alerts = e.Alerts;
                Utils.AddOrUpdateIsolatedStorageSettings("Alerts", App.ViewModel.Alerts);
                if (isFollowing)
                    MessageBox.Show("Removed from your following list.");
                else
                    MessageBox.Show("Added to your following list.");

                
                string s = App.ViewModel.SearchKeywords;
                App.ViewModel.SearchKeywords = string.Empty;
                App.ViewModel.SearchKeywords = s;
                
                //App.ViewModel.SearchKeywords = new StringBuilder(App.ViewModel.SearchKeywords).ToString();

                App.ViewModel.UpdateMyFollowings();
                App.ViewModel.SearchFollowing(true, 0, null);
            }
            else
            {
                MessageBox.Show("Your request cannot be fullfilled, please try again.");
            }
        }

        private void GoHome(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml?clear", UriKind.RelativeOrAbsolute));
        }



        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {

            base.OnBackKeyPress(e);
            if (NavigationContext.QueryString.ContainsKey("clear"))
            {
                e.Cancel = true;
                GoHome(this, null);
            }

        }

  
    }
}