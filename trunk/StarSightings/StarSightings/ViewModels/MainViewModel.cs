using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Linq;
using StarSightings.Events;
using System.Device.Location;
using StarSightings.ViewModels;

namespace StarSightings
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
            this.PopularItems = new ObservableCollection<ItemViewModel>();
            this.PopularSummaryItems = new ObservableCollection<ItemViewModel>();
            this.LatestItems = new ObservableCollection<ItemViewModel>();
            this.LatestSummaryItems = new ObservableCollection<ItemViewModel>();
            this.NearestItems = new ObservableCollection<ItemViewModel>();
            this.NearestSummaryItems = new ObservableCollection<ItemViewModel>();
            this.FollowingItems = new ObservableCollection<ItemViewModel>();
            this.FollowingSummaryItems = new ObservableCollection<ItemViewModel>();
            this.User = new UserViewModel();

            SearchTypePopular = 0;
            SearchTypeLatest = 0;
            SearchTypeNearest = 0;
            SearchTypeFollowing = 0;

            App.SSAPI.Search += new SearchEventHandler(SearchCompleted);
            App.GeoWatcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            App.GeoWatcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);           
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        private ObservableCollection<ItemViewModel> items;
        private ObservableCollection<ItemViewModel> popularItems;
        private ObservableCollection<ItemViewModel> popularSummaryItems;
        private ObservableCollection<ItemViewModel> latestItems;
        private ObservableCollection<ItemViewModel> latestSummaryItems;
        private ObservableCollection<ItemViewModel> nearestItems;
        private ObservableCollection<ItemViewModel> nearestSummaryItems;
        private ObservableCollection<ItemViewModel> followingItems;
        private ObservableCollection<ItemViewModel> followingSummaryItems;

        public ObservableCollection<ItemViewModel> Items { get { return items; } private set{if (value != items) {items = value; NotifyPropertyChanged("Items");}} }
        public ObservableCollection<ItemViewModel> PopularItems { get { return popularItems; } private set { if (value != popularItems) { popularItems = value; NotifyPropertyChanged("PopularItems"); } } }
        public ObservableCollection<ItemViewModel> PopularSummaryItems { get { return popularSummaryItems; } private set { if (value != popularSummaryItems) { popularSummaryItems = value; NotifyPropertyChanged("PopularSummaryItems"); } } }
        public ObservableCollection<ItemViewModel> LatestItems { get { return latestItems; } private set { if (value != latestItems) { latestItems = value; NotifyPropertyChanged("LatestItems"); } } }
        public ObservableCollection<ItemViewModel> LatestSummaryItems { get { return latestSummaryItems; } private set { if (value != latestSummaryItems) { latestSummaryItems = value; NotifyPropertyChanged("LatestSummaryItems"); } } }
        public ObservableCollection<ItemViewModel> NearestItems { get { return nearestItems; } private set { if (value != nearestItems) { nearestItems = value; NotifyPropertyChanged("NearestItems"); } } }
        public ObservableCollection<ItemViewModel> NearestSummaryItems { get { return nearestSummaryItems; } private set { if (value != nearestSummaryItems) { nearestSummaryItems = value; NotifyPropertyChanged("NearestSummaryItems"); } } }
        public ObservableCollection<ItemViewModel> FollowingItems { get { return followingItems; } private set { if (value != followingItems) { followingItems = value; NotifyPropertyChanged("FollowingItems"); } } }
        public ObservableCollection<ItemViewModel> FollowingSummaryItems { get { return followingSummaryItems; } private set { if (value != followingSummaryItems) { followingSummaryItems = value; NotifyPropertyChanged("FollowingSummaryItems"); } } }

        private int searchTypePopular;
        private int searchTypeLatest;
        private int searchTypeNearest;
        private int searchTypeFollowing;

        public int SearchTypePopular
        {
            get
            {
                return this.searchTypePopular;
            }
            set
            {
                this.searchTypePopular = value;
            }
        }

        public int SearchTypeLatest
        {
            get
            {
                return this.searchTypeLatest;
            }
            set
            {
                this.searchTypeLatest = value;
            }
        }

        public int SearchTypeNearest
        {
            get
            {
                return this.searchTypeNearest;
            }
            set
            {
                this.searchTypeNearest = value;
            }
        }

        public int SearchTypeFollowing
        {
            get
            {
                return this.searchTypeFollowing;
            }
            set
            {
                this.searchTypeFollowing = value;
            }
        }

        private string deviceId = "Guest";
        public string DeviceId
        {
            get
            {
                return this.deviceId;
            }
            set
            {
                if (value != deviceId)
                {
                    deviceId = value;
                    NotifyPropertyChanged("DeviceId");
                }
            }
        }

        private GeoCoordinate mapCenter;
        public GeoCoordinate MapCenter
        {
            get
            {
                return this.mapCenter;
            }
            set
            {
                this.mapCenter = value;
            }
        }

        private UserViewModel user;
        public UserViewModel User
        {
            get
            {
                return this.user;
            }
            set
            {
                if (value != user)
                {
                    user = value;
                    NotifyPropertyChanged("User");
                }
            }
        }

        

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            // Sample data; replace with real data
            this.Items.Add(new ItemViewModel() { ID = 1, LineOne = "runtime one", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu", ImageSource = new Uri("Images/Frame.png", UriKind.RelativeOrAbsolute) });
            this.Items.Add(new ItemViewModel() { ID = 2, LineOne = "runtime two", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus", ImageSource = new Uri("Images/Frame.png", UriKind.RelativeOrAbsolute) });
            this.Items.Add(new ItemViewModel() { ID = 3, LineOne = "runtime three", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent", ImageSource = new Uri("Images/Frame.png", UriKind.RelativeOrAbsolute) });
            this.Items.Add(new ItemViewModel() { ID = 4, LineOne = "runtime four", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos", ImageSource = new Uri("Images/Frame.png", UriKind.RelativeOrAbsolute) });
            this.Items.Add(new ItemViewModel() { ID = 5, LineOne = "runtime five", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur", ImageSource = new Uri("Images/Frame.png", UriKind.RelativeOrAbsolute) });
            this.Items.Add(new ItemViewModel() { ID = 6, LineOne = "runtime six", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent", ImageSource = new Uri("Images/Frame.png", UriKind.RelativeOrAbsolute) });
            this.Items.Add(new ItemViewModel() { ID = 7, LineOne = "runtime seven", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat", ImageSource = new Uri("Images/Frame.png", UriKind.RelativeOrAbsolute) });
            this.Items.Add(new ItemViewModel() { ID = 8, LineOne = "runtime eight", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum", ImageSource = new Uri("Images/Frame.png", UriKind.RelativeOrAbsolute) });
            this.Items.Add(new ItemViewModel() { ID = 9, LineOne = "runtime nine", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu", ImageSource = new Uri("Images/Frame.png", UriKind.RelativeOrAbsolute) });
            this.Items.Add(new ItemViewModel() { ID = 10, LineOne = "runtime ten", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus", ImageSource = new Uri("Images/Frame.png", UriKind.RelativeOrAbsolute) });
            this.Items.Add(new ItemViewModel() { ID = 11, LineOne = "runtime eleven", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent", ImageSource = new Uri("Images/Frame.png", UriKind.RelativeOrAbsolute) });
            this.Items.Add(new ItemViewModel() { ID = 12, LineOne = "runtime twelve", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos", ImageSource = new Uri("Images/Frame.png", UriKind.RelativeOrAbsolute) });
            this.Items.Add(new ItemViewModel() { ID = 13, LineOne = "runtime thirteen", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur", ImageSource = new Uri("Images/Frame.png", UriKind.RelativeOrAbsolute) });
            this.Items.Add(new ItemViewModel() { ID = 14, LineOne = "runtime fourteen", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent", ImageSource = new Uri("Images/Frame.png", UriKind.RelativeOrAbsolute) });
            this.Items.Add(new ItemViewModel() { ID = 15, LineOne = "runtime fifteen", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat", ImageSource = new Uri("Images/Frame.png", UriKind.RelativeOrAbsolute) });
            this.Items.Add(new ItemViewModel() { ID = 16, LineOne = "runtime sixteen", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum", ImageSource = new Uri("Images/Frame.png", UriKind.RelativeOrAbsolute) });

            this.IsDataLoaded = true;

            foreach (ItemViewModel item in this.Items)
            {
                //this.PopularItems.Add(item);
                //this.LatestItems.Add(item);
                //this.NearestItems.Add(item);                
                this.FollowingItems.Add(item);
            }

            //UpdateSummaryItems(this.PopularItems, this.PopularSummaryItems, 1, 2);
            //UpdateSummaryItems(this.LatestItems, this.LatestSummaryItems, 3, 4);
            //UpdateSummaryItems(this.NearestItems, this.NearestSummaryItems, 5, 6);            
            UpdateSummaryItems(this.FollowingItems, this.FollowingSummaryItems, 7, 8);

            DownloadData();
        }

        private void UpdateSummaryItems(ObservableCollection<ItemViewModel> source, ObservableCollection<ItemViewModel> target, int start, int end)
        {
            target.Clear();       
            for (int i = start; i <= end && i < source.Count; i++)
            {                
                target.Add(source.ElementAt(i));            
            }            
        }

        public void DownloadData()
        {
            SearchPopular(true,0, null);
            SearchLatest(true,0, null);
        }

        private bool isUpdatingPopular = false;
        public void SearchPopular(bool fresh, int start, SearchParams param)
        {
            if (isUpdatingPopular)
                return;
            if (param == null)
                param = new SearchParams();
            param.start = start;
            param.search_types = SearchType.CATEGORY_FILTER_NAMES[App.ViewModel.SearchTypePopular];
            SearchToken token = new SearchToken();
            token.searchGroup = Constants.SEARCH_POPULAR;
            token.isFresh = fresh;
            token.start = start;
            isUpdatingPopular = true;
            App.SSAPI.DoSearch(param, token);
        }

        private bool isUpdatingLatest = false;
        public void SearchLatest(bool fresh, int start, SearchParams param)
        {
            if (isUpdatingLatest)
                return;
            if (param == null)
                param = new SearchParams();
            param.start = start;
            param.search_types = SearchType.CATEGORY_FILTER_NAMES[App.ViewModel.SearchTypeLatest];
            param.order_by = "time";
            param.order_dir = "desc";
            SearchToken token = new SearchToken();
            token.searchGroup = Constants.SEARCH_LATEST;
            token.isFresh = fresh;
            token.start = start;
            isUpdatingLatest = true;
            App.SSAPI.DoSearch(param, token);
        }

        private bool isUpdatingNearest = false;
        public void SearchNearest(bool fresh, int start, SearchParams param)
        {
            if (isUpdatingNearest)
                return;
            if (param == null)
            {
                param = new SearchParams();
                double geoLat = (double)Utils.GetIsolatedStorageSettings("GeoLat");
                double geoLng = (double)Utils.GetIsolatedStorageSettings("GeoLng");
                param.search_lat = geoLat == null ? 0 : geoLat;
                param.search_lng = geoLng == null ? 0 : geoLng;
            }
            param.start = start;
            param.order_by = "geo";
            param.order_dir = "desc";
            param.filter = "alltime";
            SearchToken token = new SearchToken();
            token.searchGroup = Constants.SEARCH_NEAREST;
            token.isFresh = fresh;
            token.start = start;
            isUpdatingNearest = true;
            App.SSAPI.DoSearch(param, token);
        }

        public void SearchCompleted(object sender, SearchEventArgs e)
        {
            if (e.Successful)
            {
                string id = "";
                if (e.SearchToken.searchGroup == Constants.SEARCH_POPULAR)
                {
                    if (e.SearchToken.isFresh)
                    {
                        //App.ViewModel.PopularItems.Clear();
                        int count = App.ViewModel.PopularItems.Count;                        
                        foreach (ItemViewModel item in e.Items)
                        {
                            if (item.PhotoId == id)
                                continue;
                            App.ViewModel.PopularItems.Add(item);
                            id = item.PhotoId;                            
                        }
                        for (int i = 0; i < count; i++)
                        {
                            App.ViewModel.PopularItems.RemoveAt(0);
                        }
                        UpdateSummaryItems(App.ViewModel.PopularItems, App.ViewModel.PopularSummaryItems, 0, 1);
                    }
                    else
                    {                        
                        foreach (ItemViewModel item in e.Items)
                        {
                            if (item.PhotoId == id)
                                continue;
                            App.ViewModel.PopularItems.Add(item);
                            id = item.PhotoId;
                        }
                    }
                    isUpdatingPopular = false;
                }
                else if (e.SearchToken.searchGroup == Constants.SEARCH_LATEST)
                {
                    if (e.SearchToken.isFresh)
                    {
                        int count = App.ViewModel.LatestItems.Count;
                        foreach (ItemViewModel item in e.Items)
                        {
                            if (item.PhotoId == id)
                                continue;
                            App.ViewModel.LatestItems.Add(item);
                            id = item.PhotoId;                            
                        }
                        for (int i = 0; i < count; i++)
                        {
                            App.ViewModel.LatestItems.RemoveAt(0);
                        }
                        UpdateSummaryItems(App.ViewModel.LatestItems, App.ViewModel.LatestSummaryItems, 0, 1);
                    }
                    else
                    {                        
                        foreach (ItemViewModel item in e.Items)
                        {
                            if (item.PhotoId == id)
                                continue;
                            App.ViewModel.LatestItems.Add(item);
                            id = item.PhotoId;
                        }
                    }
                    isUpdatingLatest = false;
                }
                else if (e.SearchToken.searchGroup == Constants.SEARCH_NEAREST)
                {
                    if (e.SearchToken.isFresh)
                    {
                        int count = App.ViewModel.NearestItems.Count;
                        foreach (ItemViewModel item in e.Items)
                        {
                            if (item.PhotoId == id)
                                continue;
                            App.ViewModel.NearestItems.Add(item);
                            id = item.PhotoId;
                        }
                        for (int i = 0; i < count; i++)
                        {
                            App.ViewModel.NearestItems.RemoveAt(0);
                        }
                        UpdateSummaryItems(App.ViewModel.NearestItems, App.ViewModel.NearestSummaryItems, 0, 1);
                    }
                    else
                    {
                        foreach (ItemViewModel item in e.Items)
                        {
                            if (item.PhotoId == id)
                                continue;
                            App.ViewModel.NearestItems.Add(item);
                            id = item.PhotoId;                            
                        }
                    }
                    isUpdatingNearest = false;
                }
                OnSearchCompleted(e);
            }
        }

        public event SearchCompletedCallback SearchDataReadyHandler;
        protected virtual void OnSearchCompleted(SearchEventArgs e)
        {
            if (SearchDataReadyHandler != null)
            {
                SearchDataReadyHandler(this, e);
            }
        }

        // Event handler for the GeoCoordinateWatcher.StatusChanged event.
        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    // The Location Service is disabled or unsupported.
                    // Check to see whether the user has disabled the Location Service.
                    if (App.GeoWatcher.Permission != GeoPositionPermission.Denied)
                    {
                        // The user has disabled the Location Service on their device.
                        //statusTextBlock.Text = "you have this application access to location.";
                    }
                    else
                    {
                        MessageBox.Show("location is not functioning on this device");
                    }
                    break;

                case GeoPositionStatus.Initializing:
                    // The Location Service is initializing.
                    // Disable the Start Location button.
                    //startLocationButton.IsEnabled = false;
                    break;

                case GeoPositionStatus.NoData:
                    // The Location Service is working, but it cannot get location data.
                    // Alert the user and enable the Stop Location button.
                    MessageBox.Show("location data is not available.");
                    //stopLocationButton.IsEnabled = true;
                    break;

                case GeoPositionStatus.Ready:
                    // The Location Service is working and is receiving location data.
                    // Show the current position and enable the Stop Location button.
                    //statusTextBlock.Text = "location data is available.";
                    //stopLocationButton.IsEnabled = true;
                    break;
            }
        }
        
        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            //latitudeTextBlock.Text = e.Position.Location.Latitude.ToString("0.000");
            //longitudeTextBlock.Text = e.Position.Location.Longitude.ToString("0.000");
            SearchParams param = new SearchParams();
            param.search_lat = e.Position.Location.Latitude;
            param.search_lng = e.Position.Location.Longitude;
            Utils.AddOrUpdateIsolatedStorageSettings("GeoLat", param.search_lat);
            Utils.AddOrUpdateIsolatedStorageSettings("GeoLng", param.search_lng);
            
            SearchNearest(true, 0, param);
        }


        public ItemViewModel GetItemById(string id, int searchGroup)
        {
            return GetItemSouce(searchGroup).FirstOrDefault(item => item.PhotoId == id);       
        }

        public ObservableCollection<ItemViewModel> GetItemSouce(int searchGroup)
        {
            switch (searchGroup)
            {
                case Constants.SEARCH_POPULAR:
                    return PopularItems;
                case Constants.SEARCH_LATEST:
                    return LatestItems;
                case Constants.SEARCH_NEAREST:
                    return NearestItems;
                case Constants.SEARCH_FOLLOWING:
                    return FollowingItems;
            }
            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    public delegate void SearchCompletedCallback(object sender, SearchEventArgs e);
}
