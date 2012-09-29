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
using Microsoft.Phone.Controls.Maps;
using System.Device.Location;

namespace StarSightings
{
    public partial class MapPage : PhoneApplicationPage
    {
        public MapPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.items = new ObservableCollection<ItemViewModel>();
            this.Loaded += new RoutedEventHandler(MapPage_Loaded);
        }

        void MapPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            this.MapPins.ItemsSource = this.items;
        }

        private string selectedGroupId;
        private ObservableCollection<ItemViewModel> items;
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (NavigationContext.QueryString.ContainsKey("selectedGroupId"))
            {
                selectedGroupId = NavigationContext.QueryString["selectedGroupId"];
                int itemId = 0;
                int.TryParse(selectedGroupId, out itemId);
                switch (itemId)
                {
                    case 0:
                        this.GroupTitle.Text = "popular";
                        items = App.ViewModel.PopularItems;
                        break;
                    case 1:
                        this.GroupTitle.Text = "latest";
                        items = App.ViewModel.LatestItems;
                        break;
                    case 2:
                        this.GroupTitle.Text = "nearest";
                        items = App.ViewModel.NearestItems;
                        break;
                    case 3:
                        this.GroupTitle.Text = "following";
                        items = App.ViewModel.FollowingItems;
                        break;
                }

                GeoCoordinate[] locations = new GeoCoordinate[items.Count];
                int i = 0;
                foreach (ItemViewModel item in items)
                {                    
                    Double lat = 0.0, lng = 0.0;
                    Double.TryParse(item.GeoLat, out lat);
                    Double.TryParse(item.GeoLng, out lng);
                    item.GeoLocation = new GeoCoordinate(lat, lng);
                    locations[i++] = item.GeoLocation;                                        
                }
                LocationRect.CreateLocationRect(locations);
            }
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            this.Map.Children.Clear();
        }

        private void GoToList(object sender, EventArgs e)
        {
            //this.NavigationService.Navigate(new Uri("/ListPage.xaml", UriKind.RelativeOrAbsolute));
            this.NavigationService.GoBack();
        }

        private void GoToFilter(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri(string.Format("/FilterPage.xaml?selectedGroupId={0}", this.selectedGroupId), UriKind.RelativeOrAbsolute));                        
        }

        private void Map_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {            
            
            pinContent.Visibility = System.Windows.Visibility.Collapsed; 
        }

        System.Windows.Controls.Border pinContent; 
        private void Pin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (pinContent != null && pinContent.Visibility == System.Windows.Visibility.Visible)
                pinContent.Visibility = System.Windows.Visibility.Collapsed; 

            pinContent = (sender as Pushpin).Content as Border;
            pinContent.Visibility = System.Windows.Visibility.Visible;
            /*
            var _ppmodel = sender as Pushpin;
            ContextMenu contextMenu =
                ContextMenuService.GetContextMenu(_ppmodel);
            contextMenu.DataContext = items.Where
                (c => (c.GeoLocation
                    == _ppmodel.Location)).FirstOrDefault();
            if (contextMenu.Parent == null)
            {
                contextMenu.IsOpen = true;
            } 
             * */

            //MessageBox.Show("Please wait while your prosition is determined....");
            //this.PinContent.Visibility = System.Windows.Visibility.Visible;

            //stop the event from going to the parent map control
            e.Handled = true;
        }
    }
}