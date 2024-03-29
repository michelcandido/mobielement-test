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
using StarSightings.Events;
using StarSightings.ViewModels;

namespace StarSightings
{
    public partial class FollowingPage : PhoneApplicationPage
    {
        public FollowingPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
            {
                if (NavigationContext.QueryString.ContainsKey("pivotItemId"))
                {
                    string pivotItemId = NavigationContext.QueryString["pivotItemId"];
                    int itemId = 0;
                    if (int.TryParse(pivotItemId, out itemId))
                    {
                        this.pivotControl.SelectedIndex = itemId;                        
                    }
                }
            }
            
        }

        private void AddFollowing(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //this.NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.RelativeOrAbsolute));
            this.NavigationService.Navigate(new Uri(string.Format("/SearchInputPage.xaml?page={0}", 0), UriKind.RelativeOrAbsolute));
        }

        private void AddFollowing2(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //this.NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.RelativeOrAbsolute));
            this.NavigationService.Navigate(new Uri(string.Format("/SearchInputPage.xaml?page={0}", 4), UriKind.RelativeOrAbsolute));
        }

        private AlertEventHandler followAlertEventHandler;
        private void DeleteFollowed(object sender, System.Windows.Input.GestureEventArgs e)
        {
            UserViewModel selectedItemData = (sender as Image).DataContext as UserViewModel;
            /*
            if (selectedItemData.UserType == Constants.KEYWORD_NAME)
            {
                App.ViewModel.MyFollowingCelebs.Remove(selectedItemData);
            }
            else if (selectedItemData.UserType == Constants.KEYWORD_USER)
            {
                App.ViewModel.MyFollowingUsers.Remove(selectedItemData);
            }
             * */

            ((sender as Image).Parent as Grid).Visibility = Visibility.Collapsed;

            if (selectedItemData != null)
            {
                followAlertEventHandler = new AlertEventHandler(DeleteFollowCompleted);
                App.SSAPI.AlertHandler += followAlertEventHandler;
                App.SSAPI.Alert(Constants.ALERT_REMOVE, selectedItemData.UserType +"/", selectedItemData.UserName);

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
                App.ViewModel.SearchFollowing(true, 0, null);
            }
            else
            {
                MessageBox.Show("Cannot remove it from your following list, please try again.");
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

        private void GoHome(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml?clear", UriKind.RelativeOrAbsolute));
        }  
    }
}