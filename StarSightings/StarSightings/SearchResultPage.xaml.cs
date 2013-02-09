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

namespace StarSightings
{
    public partial class SearchResultPage : PhoneApplicationPage
    {
        public SearchResultPage()
        {
            InitializeComponent();

            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(SearchResultPage_Loaded);
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
                    this.pivotControl.Title = App.ViewModel.SearchKeywords;
                else
                    this.pivotControl.Title = App.ViewModel.User.UserName;
            }
            
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
    }
}