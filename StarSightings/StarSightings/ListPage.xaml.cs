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
using System.Device.Location;
using System.Windows.Navigation;

namespace StarSightings
{
    public partial class ListPage : PhoneApplicationPage
    {
        bool isNewPageInstance = false;
        GeoCoordinateWatcher watcher;
        PivotItem searchPivotItem;

        public ListPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(ListPage_Loaded);
            isNewPageInstance = true;
            searchPivotItem = (this.pivotControl.Items.Single(p => ((PivotItem)p).Name == "SearchPivotItem")) as PivotItem;
        }

        void ListPage_Loaded(object sender, RoutedEventArgs e)
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
                if (NavigationContext.QueryString.ContainsKey("pivotItemId"))
                {
                    string pivotItemId = NavigationContext.QueryString["pivotItemId"];
                    int itemId = 0;
                    if (int.TryParse(pivotItemId, out itemId))
                    {
                        this.pivotControl.SelectedIndex = itemId;
                        if (!App.ViewModel.ShowSearchPivotItem && searchPivotItem != null)
                            this.pivotControl.Items.Remove(searchPivotItem);
                        
                    }
                }                
            }
            else
            {
                if (App.ViewModel.ShowSearchPivotItem && searchPivotItem != null && !this.pivotControl.Items.Contains(searchPivotItem))
                    this.pivotControl.Items.Add(searchPivotItem);
            }
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            // If this is a back navigation, the page will be discarded, so there
            // is no need to save state.
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
            {
                // Save the ViewModel variable in the page's State dictionary.
                State["CurrentPivot"] = this.pivotControl.SelectedIndex;
            }
        }

        /// <summary>
        /// Navigates to filter page.
        /// </summary>       
        private void GoToFilter(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri(string.Format("/FilterPage.xaml?selectedGroupId={0}", this.pivotControl.SelectedIndex), UriKind.RelativeOrAbsolute));            
        }

        /// <summary>
        /// Navigates to map page.
        /// </summary>
        private void GoToMap(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri(string.Format("/MapPage.xaml?selectedGroupId={0}", this.pivotControl.SelectedIndex), UriKind.RelativeOrAbsolute));             
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

        private void GoHome(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml?clear", UriKind.RelativeOrAbsolute));
        }        
    }
}