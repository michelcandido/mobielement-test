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
using Microsoft.Phone.Tasks;
using System;
using StarSightings.Events;
using StarSightings.ViewModels;
using Microsoft.Phone.Tasks;
using System.Device.Location;
using System.Windows.Data;

namespace StarSightings
{
    public partial class DetailsPage : PhoneApplicationPage
    {
        public DetailsPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
            //this.items = new ObservableCollection<ItemViewModel>();
            this.Loaded += new RoutedEventHandler(DetailsPage_Loaded);            
        }        
        
        private ObservableCollection<ItemViewModel> items;
        void DetailsPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            //this.slideView.ItemsSource = this.items;
        }

        private string selectedGroupId;
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
            {
                if (NavigationContext.QueryString.ContainsKey("selectedItemId") && NavigationContext.QueryString.ContainsKey("selectedGroupId"))
                {
                    selectedGroupId = NavigationContext.QueryString["selectedGroupId"];
                    int groupId = 0;
                    int.TryParse(selectedGroupId, out groupId);
                    this.items = App.ViewModel.GetItemSouce(groupId);
                    this.slideView.ItemsSource = this.items;

                    string selectedItemId = NavigationContext.QueryString["selectedItemId"];
                    ItemViewModel item = null;
                    item = App.ViewModel.GetItemById(selectedItemId, groupId);
                    this.slideView.SelectedItem = item;
                }
            }
        }

        private void Wiki_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();

            webBrowserTask.Uri = new Uri("http://en.m.wikipedia.org/wiki/" + ((string)((sender as Grid).Tag)).Trim(), UriKind.Absolute);

            webBrowserTask.Show();
        }

        private void Source_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri(((string)((sender as ListBoxItem).Tag)).Trim(), UriKind.Absolute);
            webBrowserTask.Show();
        }

        private void htmlTextBlock_NavigationRequested(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();

            try
            {
                webBrowserTask.Uri = new Uri(((e.Content as Hyperlink).CommandParameter as string), UriKind.Absolute);
                webBrowserTask.Show();
            }
            catch (System.Exception)
            {
                
                //throw;
            }

            
        }

        private AlertEventHandler followAlertEventHandler;
        private bool isFollowing;
        private void Follow_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string name = ((string)((sender as Grid).Tag)).Trim();
            isFollowing = App.ViewModel.MyFollowingCelebs.Where(user => user.UserName == name).Count() != 0;
            followAlertEventHandler = new AlertEventHandler(FollowCompleted);
            App.SSAPI.AlertHandler += followAlertEventHandler;
            if (isFollowing)
                App.SSAPI.Alert(Constants.ALERT_REMOVE, Constants.ALERT_TYPE_CELEBRITY, name);
            else
                App.SSAPI.Alert(Constants.ALERT_SET, Constants.ALERT_TYPE_CELEBRITY, name);
        }

        public void FollowCompleted(object sender, AlertEventArgs e)
        {
            App.SSAPI.AlertHandler -= followAlertEventHandler;
            if (e.Successful)
            {
                // force update the binding
                (this.slideView.SelectedItem as ItemViewModel).Celebs = (string[])((this.slideView.SelectedItem as ItemViewModel).Celebs.Clone());

                App.ViewModel.Alerts = e.Alerts;
                Utils.AddOrUpdateIsolatedStorageSettings("Alerts", App.ViewModel.Alerts);
                if (isFollowing)
                    MessageBox.Show("Removed from your following list.");
                else
                    MessageBox.Show("Added to your following list.");
                App.ViewModel.UpdateMyFollowings();
                App.ViewModel.SearchFollowing(true, 0, null);
            }
            else
            {
                MessageBox.Show("Your Alert request cannot be fullfilled, please try again.");
            }
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ViewModel.SelectedItem = this.slideView.SelectedItem as ItemViewModel;
            //App.ViewModel.CommentList = (this.slideView.SelectedItem as ItemViewModel).Comments;
            this.NavigationService.Navigate(new Uri("/VoteCommentPage.xaml?pivotItem=comment", UriKind.RelativeOrAbsolute));
        }

        private void CommentButton_Click(object sender, System.EventArgs e)
        {
            gotoNewComment();
        }

        private void Comment_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            gotoNewComment();
        }

        private void gotoNewComment()
        {
            App.ViewModel.SelectedItem = this.slideView.SelectedItem as ItemViewModel;
            this.NavigationService.Navigate(new Uri("/CommentInputPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Vote_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            gotoVote();
        }

        private void VoteButton_Click(object sender, System.EventArgs e)
        {
            gotoVote();
        }

        private void gotoVote()
        {
            App.ViewModel.SelectedItem = this.slideView.SelectedItem as ItemViewModel;
            //App.ViewModel.CommentList = (this.slideView.SelectedItem as ItemViewModel).Comments;
            this.NavigationService.Navigate(new Uri("/VoteCommentPage.xaml?pivotItem=vote", UriKind.RelativeOrAbsolute));
        }

        private void MapButton_Click(object sender, System.EventArgs e)
        {

            gotoMap();
        }

        private void Map_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            gotoMap();
        }

        private void gotoMap()
        {
            App.ViewModel.SelectedItem = this.slideView.SelectedItem as ItemViewModel;
            BingMapsTask mapsTask = new BingMapsTask();
            mapsTask.Center = App.ViewModel.SelectedItem.GeoLocation;
            mapsTask.ZoomLevel = 12;
            mapsTask.Show();
            
        }

        private void CelebName_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ViewModel.KeywordType = Constants.KEYWORD_NAME;
            App.ViewModel.SearchKeywords = ((string)((sender as Grid).Tag)).Trim();
            App.ViewModel.SearchKeywordSearch(true, 0, null);
            this.NavigationService.Navigate(new Uri("/SearchResultPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Pic_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ViewModel.SelectedItem = this.slideView.SelectedItem as ItemViewModel;
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            if (!string.IsNullOrEmpty(App.ViewModel.SelectedItem.SourceUrl))
            {
                webBrowserTask.Uri = new Uri(App.ViewModel.SelectedItem.SourceUrl, UriKind.Absolute);
                webBrowserTask.Show();
            }
        }
    }
}