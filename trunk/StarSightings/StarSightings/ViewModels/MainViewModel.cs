﻿using System;
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
using System.Net;

namespace StarSightings
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
            this.PopularItems = new ObservableCollection<ItemViewModel>();
            //this.PopularSummaryItems = new ObservableCollection<ItemViewModel>();
            this.LatestItems = new ObservableCollection<ItemViewModel>();
            //this.LatestSummaryItems = new ObservableCollection<ItemViewModel>();
            this.NearestItems = new ObservableCollection<ItemViewModel>();
            //this.NearestSummaryItems = new ObservableCollection<ItemViewModel>();
            this.FollowingItems = new ObservableCollection<ItemViewModel>();
            //this.FollowingSummaryItems = new ObservableCollection<ItemViewModel>();
            this.KeywordSearchItems = new ObservableCollection<ItemViewModel>();

            this.SelectedImage = new BitmapImage();
            this.CelebNameList = new ObservableCollection<String>();
            this.LocationList = new ObservableCollection<String>();
            this.PlaceList = new ObservableCollection<String>();
            this.EventList = new ObservableCollection<String>();

            this.User = new UserViewModel();
            this.Alerts = new ObservableCollection<AlertViewModel>();

            SearchTypePopular = 0;
            SearchTypeLatest = 0;
            SearchTypeNearest = 0;
            SearchTypeFollowing = 0;
            SearchTypeKeyword = 0;

            App.SSAPI.SearchHandler += new SearchEventHandler(SearchCompleted);
            App.SSAPI.CommentHandler += new CommentEventHandler(CommentCompleted);

            App.GeoWatcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            App.GeoWatcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
            
       
            PopularItemsSummaryList = new CollectionViewSource();
            LatestItemsSummaryList = new CollectionViewSource();
            NearestItemsSummaryList = new CollectionViewSource();
            FollowingItemsSummaryList = new CollectionViewSource();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        private ObservableCollection<ItemViewModel> items;
        private ObservableCollection<ItemViewModel> popularItems;
        //private ObservableCollection<ItemViewModel> popularSummaryItems;
        private ObservableCollection<ItemViewModel> latestItems;
        //private ObservableCollection<ItemViewModel> latestSummaryItems;
        private ObservableCollection<ItemViewModel> nearestItems;
        //private ObservableCollection<ItemViewModel> nearestSummaryItems;
        private ObservableCollection<ItemViewModel> followingItems;
        //private ObservableCollection<ItemViewModel> followingSummaryItems;
        private ObservableCollection<ItemViewModel> keywordSearchItems;
        private List<String> placeSuggestionItems; //TODO: move

       

        public ObservableCollection<ItemViewModel> Items { get { return items; } private set{if (value != items) {items = value; NotifyPropertyChanged("Items");}} }
        public ObservableCollection<ItemViewModel> PopularItems { get { return popularItems; } private set { if (value != popularItems) { popularItems = value; NotifyPropertyChanged("PopularItems"); } } }
        //public ObservableCollection<ItemViewModel> PopularSummaryItems { get { return popularSummaryItems; } private set { if (value != popularSummaryItems) { popularSummaryItems = value; NotifyPropertyChanged("PopularSummaryItems"); } } }
        public ObservableCollection<ItemViewModel> LatestItems { get { return latestItems; } private set { if (value != latestItems) { latestItems = value; NotifyPropertyChanged("LatestItems"); } } }
        //public ObservableCollection<ItemViewModel> LatestSummaryItems { get { return latestSummaryItems; } private set { if (value != latestSummaryItems) { latestSummaryItems = value; NotifyPropertyChanged("LatestSummaryItems"); } } }
        public ObservableCollection<ItemViewModel> NearestItems { get { return nearestItems; } private set { if (value != nearestItems) { nearestItems = value; NotifyPropertyChanged("NearestItems"); } } }
        //public ObservableCollection<ItemViewModel> NearestSummaryItems { get { return nearestSummaryItems; } private set { if (value != nearestSummaryItems) { nearestSummaryItems = value; NotifyPropertyChanged("NearestSummaryItems"); } } }
        public ObservableCollection<ItemViewModel> FollowingItems { get { return followingItems; } private set { if (value != followingItems) { followingItems = value; NotifyPropertyChanged("FollowingItems"); } } }
        //public ObservableCollection<ItemViewModel> FollowingSummaryItems { get { return followingSummaryItems; } private set { if (value != followingSummaryItems) { followingSummaryItems = value; NotifyPropertyChanged("FollowingSummaryItems"); } } }
        public ObservableCollection<ItemViewModel> KeywordSearchItems { get { return keywordSearchItems; } private set { if (value != keywordSearchItems) { keywordSearchItems = value; NotifyPropertyChanged("KeywordSearchItems"); } } }
        //TODO: move
        public List<String> PlaceSuggestionItems { get { return placeSuggestionItems; } private set { if (value != placeSuggestionItems) { placeSuggestionItems = value; } } }

        public CollectionViewSource popularItemsSummaryList;
        public CollectionViewSource latestItemsSummaryList;
        public CollectionViewSource nearestItemsSummaryList;
        public CollectionViewSource followingItemsSummaryList;

        //The image selected
        public BitmapImage selectedImage;
        public String celebName;
        public ObservableCollection<String> celebNameList;
        public ObservableCollection<String> locationList;
        public ObservableCollection<String> placeList;
        public ObservableCollection<String> eventList;

        public String picStory;
        public String storyLocation;
        public String storyPlace;
        public String storyEvent;

        public CollectionViewSource PopularItemsSummaryList { get { return popularItemsSummaryList; } private set { if (value != popularItemsSummaryList) { popularItemsSummaryList = value; NotifyPropertyChanged("PopularItemsSummaryList"); } } }
        public CollectionViewSource LatestItemsSummaryList { get { return latestItemsSummaryList; } private set { if (value != latestItemsSummaryList) { latestItemsSummaryList = value; NotifyPropertyChanged("LatestItemsSummaryList"); } } }
        public CollectionViewSource NearestItemsSummaryList { get { return nearestItemsSummaryList; } private set { if (value != nearestItemsSummaryList) { nearestItemsSummaryList = value; NotifyPropertyChanged("NearestItemsSummaryList"); } } }
        public CollectionViewSource FollowingItemsSummaryList { get { return followingItemsSummaryList; } private set { if (value != followingItemsSummaryList) { followingItemsSummaryList = value; NotifyPropertyChanged("FollowingItemsSummaryList"); } } }

        public BitmapImage SelectedImage { get { return selectedImage; } set { if (value != selectedImage) { selectedImage = value; NotifyPropertyChanged("SelectedImage"); } } }
        public String CelebName { get { return celebName; } set { if (value != celebName) { celebName = value; NotifyPropertyChanged("CelebName"); } } }
        public ObservableCollection<String> CelebNameList { get { return celebNameList; } set { if (value != celebNameList) { celebNameList = value; NotifyPropertyChanged("CelebNameList"); } } }
        public ObservableCollection<String> LocationList { get { return locationList; } set { if (value != locationList) { locationList = value; NotifyPropertyChanged("LocationList"); } } }
        public ObservableCollection<String> PlaceList { get { return placeList; } set { if (value != placeList) { placeList = value; NotifyPropertyChanged("PlaceList"); } } }
        public ObservableCollection<String> EventList { get { return eventList; } set { if (value != eventList) { eventList = value; NotifyPropertyChanged("EventList"); } } }
        public String PicStory { get { return picStory; } set { if (value != picStory) { picStory = value; NotifyPropertyChanged("PicStory"); } } }
        public String StoryLocation { get { return storyLocation; } set { if (value != storyLocation) { storyLocation = value; NotifyPropertyChanged("StoryLocation"); } } }
        public String StoryPlace { get { return storyPlace; } set { if (value != storyPlace) { storyPlace = value; NotifyPropertyChanged("StoryPlace"); } } }
        public String StoryEvent { get { return storyEvent; } set { if (value != storyEvent) { storyEvent = value; NotifyPropertyChanged("StoryEvent"); } } }

        private int searchTypePopular;
        private int searchTypeLatest;
        private int searchTypeNearest;
        private int searchTypeFollowing;
        private int searchTypeKeyword;

        private string keywordType;
        private string searchKeywords;
        private bool showSearchPivotItem = false;               

        private ItemViewModel selectedItem;
        public ItemViewModel SelectedItem
        {
            get
            {
                return this.selectedItem;
            }
            set
            {
                if (value != selectedItem)
                {
                    selectedItem = value;
                    NotifyPropertyChanged("SelectedItem");
                }
            }
        }       
        
        public bool ShowSearchPivotItem
        {
            get
            {
                return this.showSearchPivotItem;
            }
            set
            {
                this.showSearchPivotItem = value;
            }
        }

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

        public int SearchTypeKeyword
        {
            get
            {
                return this.searchTypeKeyword;
            }
            set
            {
                this.searchTypeKeyword = value;
            }
        }

        public string KeywordType
        {
            get
            {
                return this.keywordType;
            }
            set
            {
                this.keywordType = value;
            }
        }

        public string SearchKeywords
        {
            get
            {
                return this.searchKeywords;
            }
            set
            {
                this.searchKeywords = value;
            }
        }

        private ObservableCollection<AlertViewModel> alerts;
        public ObservableCollection<AlertViewModel> Alerts { get { return alerts; } set { if (value != alerts) { alerts = value; NotifyPropertyChanged("Alerts"); } } }

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

        private int accountType = Constants.ACCOUNT_TYPE_DEVICE;
        public int AccountType
        {
            get
            {
                return this.accountType;
            }
            set
            {
                if (value != accountType)
                {
                    accountType = value;
                    NotifyPropertyChanged("AccountType");
                }
            }
        }

        private bool needLogin = false;
        public bool NeedLogin
        {
            get
            {
                return this.needLogin;
            }
            set
            {
                if (value != needLogin)
                {
                    needLogin = value;
                    NotifyPropertyChanged("NeedLogin");
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

        private GeoCoordinate myLocation;
        public GeoCoordinate MyLocation
        {
            get
            {
                return this.myLocation;
            }
            set
            {
                if (value != myLocation)
                {
                    myLocation = value;
                    NotifyPropertyChanged("MyLocation");
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
                //this.FollowingItems.Add(item);
            }

            //UpdateSummaryItems(this.PopularItems, this.PopularSummaryItems, 1, 2);
            //UpdateSummaryItems(this.LatestItems, this.LatestSummaryItems, 3, 4);
            //UpdateSummaryItems(this.NearestItems, this.NearestSummaryItems, 5, 6);            
            //UpdateSummaryItems(this.FollowingItems, this.FollowingSummaryItems, 7, 8);

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
            SearchNearest(true, 0, null);
            //SearchFollowing(true, 0, null);
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

        private bool isUpdatingFollowing = false;
        public void SearchFollowing(bool fresh, int start, SearchParams param)
        {
            if (isUpdatingFollowing)
                return;
            if (param == null)
                param = new SearchParams();
            param.start = start;
            param.search_types = SearchType.CATEGORY_FILTER_NAMES[App.ViewModel.SearchTypeFollowing];
            param.search_user_name = App.ViewModel.User.UserName;
            IEnumerable<string> query = App.ViewModel.Alerts.Where(alert => alert.Type == "celebrity").Select(alert => alert.Name);
            foreach (string name in query)
            {
                param.search_cat_name += HttpUtility.UrlEncode(name) + ";";
            }
            param.logic = "or";            
            SearchToken token = new SearchToken();
            token.searchGroup = Constants.SEARCH_FOLLOWING;
            token.isFresh = fresh;
            token.start = start;
            isUpdatingFollowing = true;
            App.SSAPI.DoSearch(param, token);
        }

        private bool isUpdatingKeywordSearch = false;
        public void SearchKeywordSearch(bool fresh, int start, SearchParams param)
        {
            if (isUpdatingKeywordSearch)
                return;
            if (param == null)
                param = new SearchParams();
            param.start = start;
            param.search_types = SearchType.CATEGORY_FILTER_NAMES[App.ViewModel.SearchTypeKeyword];
            if (App.ViewModel.KeywordType == Constants.KEYWORD_NAME)
            {
                param.search_cat_name = HttpUtility.UrlEncode(App.ViewModel.SearchKeywords);
            }
            else if (App.ViewModel.KeywordType == Constants.KEYWORD_EVENT)
            {
                param.search_event_name = HttpUtility.UrlEncode(App.ViewModel.SearchKeywords);
            }
            else if (App.ViewModel.KeywordType == Constants.KEYWORD_LOCATION)
            {
                param.search_location_name = HttpUtility.UrlEncode(App.ViewModel.SearchKeywords);
            }
            else if (App.ViewModel.KeywordType == Constants.KEYWORD_PLACE)
            {
                param.search_place_name = HttpUtility.UrlEncode(App.ViewModel.SearchKeywords);
            }
           
            SearchToken token = new SearchToken();
            token.searchGroup = Constants.SEARCH_KEYWORDSEARCH;
            token.isFresh = fresh;
            token.start = start;
            isUpdatingKeywordSearch = true;
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
                        //UpdateSummaryItems(App.ViewModel.PopularItems, App.ViewModel.PopularSummaryItems, 0, 3);
                        
                        this.PopularItemsSummaryList.Source = App.ViewModel.PopularItems;
                        this.PopularItemsSummaryList.Filter += (s,a) => a.Accepted = App.ViewModel.PopularItems.IndexOf((ItemViewModel)a.Item) < Constants.SUMMARY_COUNT;
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
                        //UpdateSummaryItems(App.ViewModel.LatestItems, App.ViewModel.LatestSummaryItems, 0, 3);
                        this.LatestItemsSummaryList.Source = App.ViewModel.LatestItems;
                        this.LatestItemsSummaryList.Filter += (s, a) => a.Accepted = App.ViewModel.LatestItems.IndexOf((ItemViewModel)a.Item) < Constants.SUMMARY_COUNT;
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
                        //UpdateSummaryItems(App.ViewModel.NearestItems, App.ViewModel.NearestSummaryItems, 0, 3);
                        this.NearestItemsSummaryList.Source = App.ViewModel.NearestItems;
                        this.NearestItemsSummaryList.Filter += (s, a) => a.Accepted = App.ViewModel.NearestItems.IndexOf((ItemViewModel)a.Item) < Constants.SUMMARY_COUNT;
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
                else if (e.SearchToken.searchGroup == Constants.SEARCH_FOLLOWING)
                {
                    if (e.SearchToken.isFresh)
                    {
                        int count = App.ViewModel.FollowingItems.Count;
                        foreach (ItemViewModel item in e.Items)
                        {
                            if (item.PhotoId == id)
                                continue;
                            App.ViewModel.FollowingItems.Add(item);
                            id = item.PhotoId;
                        }
                        for (int i = 0; i < count; i++)
                        {
                            App.ViewModel.FollowingItems.RemoveAt(0);
                        }
                        //UpdateSummaryItems(App.ViewModel.LatestItems, App.ViewModel.LatestSummaryItems, 0, 3);
                        this.FollowingItemsSummaryList.Source = App.ViewModel.FollowingItems;
                        this.FollowingItemsSummaryList.Filter += (s, a) => a.Accepted = App.ViewModel.FollowingItems.IndexOf((ItemViewModel)a.Item) < Constants.SUMMARY_COUNT;
                    }
                    else
                    {
                        foreach (ItemViewModel item in e.Items)
                        {
                            if (item.PhotoId == id)
                                continue;
                            App.ViewModel.FollowingItems.Add(item);
                            id = item.PhotoId;
                        }
                    }
                    isUpdatingFollowing = false;
                }
                else if (e.SearchToken.searchGroup == Constants.SEARCH_KEYWORDSEARCH)
                {
                    if (e.SearchToken.isFresh)
                    {
                        //App.ViewModel.PopularItems.Clear();
                        int count = App.ViewModel.KeywordSearchItems.Count;
                        foreach (ItemViewModel item in e.Items)
                        {
                            if (item.PhotoId == id)
                                continue;
                            App.ViewModel.KeywordSearchItems.Add(item);
                            id = item.PhotoId;
                        }
                        for (int i = 0; i < count; i++)
                        {
                            App.ViewModel.KeywordSearchItems.RemoveAt(0);
                        }
                        //UpdateSummaryItems(App.ViewModel.PopularItems, App.ViewModel.PopularSummaryItems, 0, 3);

                        //this.PopularItemsSummaryList.Source = App.ViewModel.PopularItems;
                        //this.PopularItemsSummaryList.Filter += (s, a) => a.Accepted = App.ViewModel.PopularItems.IndexOf((ItemViewModel)a.Item) < Constants.SUMMARY_COUNT;
                    }
                    else
                    {
                        foreach (ItemViewModel item in e.Items)
                        {
                            if (item.PhotoId == id)
                                continue;
                            App.ViewModel.KeywordSearchItems.Add(item);
                            id = item.PhotoId;
                        }
                    }
                    isUpdatingKeywordSearch = false;
                }

                OnSearchCompleted(e);
            }
        }

        public event SearchCompletedCallback SearchDataReadyHandler;
        public event SearchCompletedCallback PopularItemsLoadReday;
        public event SearchCompletedCallback LatestItemsLoadReday;
        public event SearchCompletedCallback NearestItemsLoadReday;
        public event SearchCompletedCallback FollowingItemsLoadReday;

        protected virtual void OnSearchCompleted(SearchEventArgs e)
        {
            if (SearchDataReadyHandler != null)
            {
                SearchDataReadyHandler(this, e);
            }
            if (PopularItemsLoadReday != null && e.SearchToken.searchGroup == Constants.SEARCH_POPULAR)
            {
                PopularItemsLoadReday(this, e);
            }
            if (LatestItemsLoadReday != null && e.SearchToken.searchGroup == Constants.SEARCH_LATEST)
            {
                LatestItemsLoadReday(this, e);
            }            
            if (NearestItemsLoadReday != null && e.SearchToken.searchGroup == Constants.SEARCH_NEAREST)
            {
                NearestItemsLoadReday(this, e);
            }
            if (FollowingItemsLoadReday != null && e.SearchToken.searchGroup == Constants.SEARCH_FOLLOWING)
            {
                FollowingItemsLoadReday(this, e);
            }
        }

        public void CommentCompleted(object sender, CommentEventArgs e)
        {            
            if (e.Successful)
            {                
                /*
                int count = App.ViewModel.SelectedItem.Comments.Count;
                for (int i = 0; i < count; i++)
                {
                    App.ViewModel.SelectedItem.Comments.RemoveAt(0);
                }
                foreach (CommentViewModel item in e.Item.Comments)
                {
                    App.ViewModel.SelectedItem.Comments.Add(item);                    
                }
                */
                App.ViewModel.SelectedItem.CommentsCnt = e.Item.CommentsCnt;
                App.ViewModel.SelectedItem.Comments = e.Item.Comments;
                App.ViewModel.SelectedItem.CommentsSummaryList = e.Item.CommentsSummaryList;
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
            MyLocation = new GeoCoordinate(param.search_lat, param.search_lng);
            
            if (App.Config.IsAppInit)
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
                case Constants.SEARCH_KEYWORDSEARCH:
                    return KeywordSearchItems;
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