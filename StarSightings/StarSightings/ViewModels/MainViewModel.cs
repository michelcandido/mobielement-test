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
using System.Net;
using Microsoft.Phone.BackgroundTransfer;
using Microsoft.Phone.Net.NetworkInformation;


namespace StarSightings
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
            
            this.PopularItems = new ObservableCollection<ItemViewModel>();
            this.PopularItems.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(PopularItems_CollectionChanged);
            
            this.LatestItems = new ObservableCollection<ItemViewModel>();
            this.LatestItems.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(LatestItems_CollectionChanged);
            
            this.NearestItems = new ObservableCollection<ItemViewModel>();
            this.NearestItems.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(NearestItems_CollectionChanged);
            
            this.FollowingItems = new ObservableCollection<ItemViewModel>();
            this.FollowingItems.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(FollowingItems_CollectionChanged);
            
            this.MySightingsItems = new ObservableCollection<ItemViewModel>();
            this.MySightingsItems.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(MySightingsItems_CollectionChanged);
            
            this.KeywordSearchItems = new ObservableCollection<ItemViewModel>();
            this.KeywordSearchItems.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(KeywordSearchItems_CollectionChanged);

            this.MyFollowingCelebs = new ObservableCollection<UserViewModel>();
            this.MyFollowingCelebs.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(MyFollowingCelebs_CollectionChanged);
            
            this.MyFollowingUsers = new ObservableCollection<UserViewModel>();
            this.MyFollowingUsers.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(MyFollowingUsers_CollectionChanged);


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
            App.SSAPI.NewPostHandler += new PostEventHandler(PostCompleted);

            App.GeoWatcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            App.GeoWatcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);            
            
       
            PopularItemsSummaryList = new CollectionViewSource();
            //PopularItemsSummaryList.View.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(PopularItemsSummaryList_View_CollectionChanged);
            
            LatestItemsSummaryList = new CollectionViewSource();
            //LatestItemsSummaryList.View.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(LatestItemsSummaryList_View_CollectionChanged);
            
            NearestItemsSummaryList = new CollectionViewSource();
            //NearestItemsSummaryList.View.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(NearestItemsSummaryList_View_CollectionChanged);
            
            FollowingItemsSummaryList = new CollectionViewSource();
            //FollowingItemsSummaryList.View.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(FollowingItemsSummaryList_View_CollectionChanged);
            
            MySightingsItemsSummaryList = new CollectionViewSource();
            //MySightingsItemsSummaryList.View.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(MySightingsItemsSummaryList_View_CollectionChanged);
            
            MyFollowingCelebsSummaryList = new CollectionViewSource();
            //MyFollowingCelebsSummaryList.View.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(MyFollowingCelebsSummaryList_View_CollectionChanged);
            
            MyFollowingUsersSummaryList = new CollectionViewSource();
            //MyFollowingUsersSummaryList.View.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(MyFollowingUsersSummaryList_View_CollectionChanged);
        }

        /*
        void MyFollowingUsersSummaryList_View_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("MyFollowingUsersSummaryList");
        }

        void MyFollowingCelebsSummaryList_View_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("MyFollowingCelebsSummaryList");
        }

        void MySightingsItemsSummaryList_View_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("MySightingsItemsSummaryList");
        }

        void FollowingItemsSummaryList_View_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("FollowingItemsSummaryList");
        }

        void NearestItemsSummaryList_View_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("NearestItemsSummaryList");
        }

        void LatestItemsSummaryList_View_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("LatestItemsSummaryList");
        }

        void PopularItemsSummaryList_View_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("PopularItemsSummaryList");
        }
        */
        void KeywordSearchItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("KeywordSearchItems");
        }

        void NearestItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("NearestItems");
        }

        void LatestItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("LatestItems");
        }

        void PopularItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("PopularItems");
        }

        void MySightingsItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("MySightingsItems");
        }

        void MyFollowingUsers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("MyFollowingUsers");
        }

        void MyFollowingCelebs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("MyFollowingCelebs");
        }

        void FollowingItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("FollowingItems");
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
        private ObservableCollection<ItemViewModel> mySightingsItems;
        private ObservableCollection<UserViewModel> myFollowingCelebs;
        private ObservableCollection<UserViewModel> myFollowingUsers;

       

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
        public ObservableCollection<ItemViewModel> MySightingsItems { get { return mySightingsItems; } private set { if (value != mySightingsItems) { mySightingsItems = value; NotifyPropertyChanged("MySightingsItems"); } } }
        public ObservableCollection<UserViewModel> MyFollowingCelebs { get { return myFollowingCelebs; } private set { if (value != myFollowingCelebs) { myFollowingCelebs = value; NotifyPropertyChanged("MyFollowingCelebs"); } } }
        public ObservableCollection<UserViewModel> MyFollowingUsers { get { return myFollowingUsers; } private set { if (value != myFollowingUsers) { myFollowingUsers = value; NotifyPropertyChanged("MyFollowingUsers"); } } }
        

        private CollectionViewSource popularItemsSummaryList;
        private CollectionViewSource latestItemsSummaryList;
        private CollectionViewSource nearestItemsSummaryList;
        private CollectionViewSource followingItemsSummaryList;
        private CollectionViewSource mySightingsItemsSummaryList;
        private CollectionViewSource myFollowingCelebsSummaryList;
        private CollectionViewSource myFollowingUsersSummaryList;

        //The image selected
        private BitmapImage selectedImage;
        private WriteableBitmap writeableSelectedBitmap;
        private String celebName;
        private ObservableCollection<String> celebNameList;
        private ObservableCollection<String> locationList;
        private ObservableCollection<String> placeList;
        private ObservableCollection<String> eventList;

        private String picStory = string.Empty;
        private String storyLocation = string.Empty;
        private String storyPlace = string.Empty;
        private String storyEvent = string.Empty;
        private String cameraInfo = string.Empty;
        private DateTime storyTime;
        private double storyLat = 0.0;
        private double storyLng = 0.0;

        private int postMode;

        public CollectionViewSource PopularItemsSummaryList { get { return popularItemsSummaryList; } private set { if (value != popularItemsSummaryList) { popularItemsSummaryList = value; NotifyPropertyChanged("PopularItemsSummaryList"); } } }
        public CollectionViewSource LatestItemsSummaryList { get { return latestItemsSummaryList; } private set { if (value != latestItemsSummaryList) { latestItemsSummaryList = value; NotifyPropertyChanged("LatestItemsSummaryList"); } } }
        public CollectionViewSource NearestItemsSummaryList { get { return nearestItemsSummaryList; } private set { if (value != nearestItemsSummaryList) { nearestItemsSummaryList = value; NotifyPropertyChanged("NearestItemsSummaryList"); } } }
        public CollectionViewSource FollowingItemsSummaryList { get { return followingItemsSummaryList; } private set { if (value != followingItemsSummaryList) { followingItemsSummaryList = value; NotifyPropertyChanged("FollowingItemsSummaryList"); } } }
        public CollectionViewSource MySightingsItemsSummaryList { get { return mySightingsItemsSummaryList; } private set { if (value != mySightingsItemsSummaryList) { mySightingsItemsSummaryList = value; NotifyPropertyChanged("MySightingsItemsSummaryList"); } } }
        public CollectionViewSource MyFollowingCelebsSummaryList { get { return myFollowingCelebsSummaryList; } private set { if (value != myFollowingCelebsSummaryList) { myFollowingCelebsSummaryList = value; NotifyPropertyChanged("MyFollowingCelebsSummaryList"); } } }
        public CollectionViewSource MyFollowingUsersSummaryList { get { return myFollowingUsersSummaryList; } private set { if (value != myFollowingUsersSummaryList) { myFollowingUsersSummaryList = value; NotifyPropertyChanged("MyFollowingUsersSummaryList"); } } }

        public BitmapImage SelectedImage { get { return selectedImage; } set { if (value != selectedImage) { selectedImage = value; NotifyPropertyChanged("SelectedImage"); } } }
        public WriteableBitmap WriteableSelectedBitmap { get { return writeableSelectedBitmap; } set { if (value != writeableSelectedBitmap) { writeableSelectedBitmap = value; NotifyPropertyChanged("WriteableSelectedBitmap"); } } }
        public String CelebName { get { return celebName; } set { if (value != celebName) { celebName = value; NotifyPropertyChanged("CelebName"); } } }
        public ObservableCollection<String> CelebNameList { get { return celebNameList; } set { if (value != celebNameList) { celebNameList = value; NotifyPropertyChanged("CelebNameList"); } } }
        public ObservableCollection<String> LocationList { get { return locationList; } set { if (value != locationList) { locationList = value; NotifyPropertyChanged("LocationList"); } } }
        public ObservableCollection<String> PlaceList { get { return placeList; } set { if (value != placeList) { placeList = value; NotifyPropertyChanged("PlaceList"); } } }
        public ObservableCollection<String> EventList { get { return eventList; } set { if (value != eventList) { eventList = value; NotifyPropertyChanged("EventList"); } } }
        public String PicStory { get { return picStory; } set { if (value != picStory) { picStory = value; NotifyPropertyChanged("PicStory"); } } }
        public String StoryLocation { get { return storyLocation; } set { if (value != storyLocation) { storyLocation = value; NotifyPropertyChanged("StoryLocation"); } } }
        public String StoryPlace { get { return storyPlace; } set { if (value != storyPlace) { storyPlace = value; NotifyPropertyChanged("StoryPlace"); } } }
        public String StoryEvent { get { return storyEvent; } set { if (value != storyEvent) { storyEvent = value; NotifyPropertyChanged("StoryEvent"); } } }
        public String CameraInfo { get { return cameraInfo; } set { if (value != cameraInfo) { cameraInfo = value; NotifyPropertyChanged("CameraInfo"); } } }
        public DateTime StoryTime { get { return storyTime; } set { if (value != storyTime) { storyTime = value; NotifyPropertyChanged("StoryTime"); } } }
        public double StoryLat { get { return storyLat; } set { if (value != storyLat) { storyLat = value; NotifyPropertyChanged("StoryLat"); } } }
        public double StoryLng { get { return storyLng; } set { if (value != storyLng) { storyLng = value; NotifyPropertyChanged("StoryLng"); } } }
        public int PostMode { get { return postMode; } set { if (value != postMode) { postMode = value; NotifyPropertyChanged("PostMode"); } } }

        private int searchTypePopular;
        private int searchTypeLatest;
        private int searchTypeNearest;
        private int searchTypeFollowing;
        private int searchTypeKeyword;

        private string keywordType;
        private string searchKeywords;
        private bool showSearchPivotItem = false;


        private IEnumerable<BackgroundTransferRequest> transferRequests;
        public IEnumerable<BackgroundTransferRequest> TransferRequests { get { return transferRequests; } set { if (value != transferRequests) { transferRequests = value; NotifyPropertyChanged("TransferRequests"); } } }

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
                if (value != searchTypePopular)
                {
                    this.searchTypePopular = value;
                    NotifyPropertyChanged("SearchTypePopular");
                }
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
                if (value != searchTypeLatest)
                {
                    this.searchTypeLatest = value;
                    NotifyPropertyChanged("SearchTypeLatest");
                }
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
                if (value != searchTypeNearest)
                {
                    this.searchTypeNearest = value;
                    NotifyPropertyChanged("SearchTypeNearest");
                }
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
                if (value != searchTypeFollowing)
                {
                    this.searchTypeFollowing = value;
                    NotifyPropertyChanged("SearchTypeFollowing");
                }
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
                if (value != searchTypeKeyword)
                {
                    this.searchTypeKeyword = value;
                    NotifyPropertyChanged("SearchTypeKeyword");
                }
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
                if (value != keywordType)
                {
                    keywordType = value;
                    NotifyPropertyChanged("KeywordType");
                }
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
                if (value != searchKeywords)
                {
                    searchKeywords = value;
                    NotifyPropertyChanged("SearchKeywords");
                }
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

        private bool needLogin = true;
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

        private bool isUploading = false;
        public bool IsUploading
        {
            get
            {
                return this.isUploading;
            }
            set
            {
                if (value != isUploading)
                {
                    isUploading = value;
                    NotifyPropertyChanged("IsUploading");
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
            this.IsDataLoaded = true;
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("No Internet connection. StarSightings needs Internet access to function properly.");
                    this.IsDataLoaded = false;
                    return;
                });
            }
            else
            {
                DownloadData();
            }
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
            
            BackgroundWorker bw0 = new BackgroundWorker();
            bw0.DoWork += (s, e) =>
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    SearchPopular(true, 0, null);
                });
            };
            bw0.RunWorkerAsync();

            BackgroundWorker bw1 = new BackgroundWorker();
            bw1.DoWork += (s, e) =>
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                { 
                   SearchLatest(true, 0, null); 
                });
            };
            bw1.RunWorkerAsync();

            BackgroundWorker bw2 = new BackgroundWorker();
            bw2.DoWork += (s, e) => 
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    SearchNearest(true, 0, null);
                });
            };
            bw2.RunWorkerAsync();

            BackgroundWorker bw3 = new BackgroundWorker();
            bw3.DoWork += (s, e) =>
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    KeywordType = Constants.KEYWORD_MY;
                    SearchKeywordSearch(true, 0, null);
                });
            };
            bw3.RunWorkerAsync();
                       
            /*
            SearchPopular(true,0, null);
            SearchLatest(true,0, null);
            SearchNearest(true, 0, null);

            KeywordType = Constants.KEYWORD_MY;
            SearchKeywordSearch(true, 0, null);
            //SearchFollowing(true, 0, null);
            */
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
            //param.search_user_name = App.ViewModel.User.UserName;
            IEnumerable<string> query1 = App.ViewModel.Alerts.Where(alert => alert.Type == "celebrity").Select(alert => alert.Name);
            foreach (string name in query1)
            {
                param.search_cat_name += HttpUtility.UrlEncode(name) + ";";                
            }

            IEnumerable<string> query2 = App.ViewModel.Alerts.Where(alert => alert.Type == "photographer").Select(alert => alert.Name);
            foreach (string name in query2)
            {
                param.search_user_name += HttpUtility.UrlEncode(name) + ";";
            }

            SearchToken token = new SearchToken();
            token.searchGroup = Constants.SEARCH_FOLLOWING;
            token.isFresh = fresh;
            token.start = start;

            if (query1.Count() != 0 || query2.Count() != 0)
            {
                param.logic = "or";                
                isUpdatingFollowing = true;
                App.SSAPI.DoSearch(param, token);                
            }
            else
            {
                App.ViewModel.FollowingItems.Clear();

                SearchEventArgs se = new SearchEventArgs(true);
                se.SearchToken = token;
                se.Items = App.ViewModel.FollowingItems;
                OnSearchCompleted(se);
            }
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
            else if (App.ViewModel.KeywordType == Constants.KEYWORD_MY)
            {
                param.search_user_name = App.ViewModel.User.UserName;                
            }
            else if (App.ViewModel.KeywordType == Constants.KEYWORD_USER)
            {
                param.search_user_name = HttpUtility.UrlEncode(App.ViewModel.SearchKeywords);
            }
            param.order_by = "time";
            param.order_dir = "desc";
           
            SearchToken token = new SearchToken();
            token.searchGroup = Constants.SEARCH_KEYWORDSEARCH;
            token.isFresh = fresh;
            token.start = start;
            isUpdatingKeywordSearch = true;
            //App.ViewModel.KeywordSearchItems.Clear();            
            App.SSAPI.DoSearch(param, token);
            
        }

        public void SearchCompleted(object sender, SearchEventArgs e)
        {
            if (e.Successful)
            {
                string id = string.Empty;
                if (e.SearchToken.searchGroup == Constants.SEARCH_POPULAR)
                {
                    if (e.SearchToken.isFresh)
                    {
                        App.ViewModel.PopularItems.Clear();
                        //int count = App.ViewModel.PopularItems.Count;                        
                        foreach (ItemViewModel item in e.Items)
                        {
                            if (item.PhotoId == id)
                                continue;
                            App.ViewModel.PopularItems.Add(item);
                            id = item.PhotoId;                            
                        }
                        /*
                        for (int i = 0; i < count; i++)
                        {
                            App.ViewModel.PopularItems.RemoveAt(0);
                        }
                         * */
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
                        App.ViewModel.LatestItems.Clear();
                        //int count = App.ViewModel.LatestItems.Count;
                        foreach (ItemViewModel item in e.Items)
                        {
                            if (item.PhotoId == id)
                                continue;
                            App.ViewModel.LatestItems.Add(item);
                            id = item.PhotoId;                            
                        }
                        /*
                        for (int i = 0; i < count; i++)
                        {
                            App.ViewModel.LatestItems.RemoveAt(0);
                        }
                         * */
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
                        App.ViewModel.NearestItems.Clear();
                        //int count = App.ViewModel.NearestItems.Count;
                        foreach (ItemViewModel item in e.Items)
                        {
                            if (item.PhotoId == id)
                                continue;
                            App.ViewModel.NearestItems.Add(item);
                            id = item.PhotoId;
                        }
                        /*
                        for (int i = 0; i < count; i++)
                        {
                            App.ViewModel.NearestItems.RemoveAt(0);
                        }
                         * */
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
                        App.ViewModel.FollowingItems.Clear();                       
                        
                        //int count = App.ViewModel.FollowingItems.Count;
                        foreach (ItemViewModel item in e.Items)
                        {
                            if (item.PhotoId == id)
                                continue;
                            App.ViewModel.FollowingItems.Add(item);
                            id = item.PhotoId;
                        }
                        /*
                        for (int i = 0; i < count; i++)
                        {
                            App.ViewModel.FollowingItems.RemoveAt(0);
                        }
                         * */
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
                        //int count = 0;
                        if (KeywordType == Constants.KEYWORD_MY)
                        {
                            App.ViewModel.MySightingsItems.Clear();
                            //count = App.ViewModel.MySightingsItems.Count;
                        }
                        else
                        {
                            App.ViewModel.KeywordSearchItems.Clear();
                            //count = App.ViewModel.KeywordSearchItems.Count;
                        }

                        foreach (ItemViewModel item in e.Items)
                        {
                            if (item.PhotoId == id)
                                continue;

                            if (KeywordType == Constants.KEYWORD_MY)
                                App.ViewModel.MySightingsItems.Add(item);
                            else
                                App.ViewModel.KeywordSearchItems.Add(item);
                            
                            id = item.PhotoId;
                        }
                        /*
                        for (int i = 0; i < count; i++)
                        {
                            if (KeywordType == Constants.KEYWORD_MY)
                                App.ViewModel.MySightingsItems.RemoveAt(0);
                            else
                                App.ViewModel.KeywordSearchItems.RemoveAt(0);
                        }
                         * */
                        //UpdateSummaryItems(App.ViewModel.PopularItems, App.ViewModel.PopularSummaryItems, 0, 3);

                        if (KeywordType == Constants.KEYWORD_MY)
                        {
                            this.MySightingsItemsSummaryList.Source = App.ViewModel.MySightingsItems;
                            this.MySightingsItemsSummaryList.Filter += (s, a) => a.Accepted = App.ViewModel.MySightingsItems.IndexOf((ItemViewModel)a.Item) < Constants.SUMMARY_COUNT;
                        }
                    }
                    else
                    {
                        foreach (ItemViewModel item in e.Items)
                        {
                            if (item.PhotoId == id)
                                continue;

                            if (KeywordType == Constants.KEYWORD_MY)
                                App.ViewModel.MySightingsItems.Add(item);
                            else
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
        public event SearchCompletedCallback MySightingsItemsLoadReday;

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
            if (MySightingsItemsLoadReday != null && e.SearchToken.searchGroup == Constants.SEARCH_KEYWORDSEARCH && KeywordType == Constants.KEYWORD_MY)
            {
                MySightingsItemsLoadReday(this, e);
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

                App.ViewModel.SelectedItem.Votes = e.Item.Votes;
            }            
        }

        public void PostCompleted(object sender, PostEventArgs e)
        {            
            if (e.Successful)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    // Showing the exact error message is useful for debugging. In a finalized application, 
                    // output a friendly and applicable string to the user instead. 
                    MessageBox.Show("Your post has been submitted successfully.");
                });
                App.ViewModel.SearchLatest(true, 0, null);
                App.ViewModel.KeywordType = Constants.KEYWORD_MY;
                App.ViewModel.SearchKeywordSearch(true, 0, null);
                App.ViewModel.StoryTime = new DateTime(0);
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    // Showing the exact error message is useful for debugging. In a finalized application, 
                    // output a friendly and applicable string to the user instead. 
                    MessageBox.Show("Errors in your submission, please try again.");
                });

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
                    if (KeywordType == Constants.KEYWORD_MY)
                        return MySightingsItems;
                    else
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

        /*
         * Update the list of people I am following, celebrity or photographer
         * */
        public void UpdateMyFollowings()
        {
            IEnumerable<string> query = App.ViewModel.Alerts.Where(alert => alert.Type == "celebrity").Select(alert => alert.Name);
            App.ViewModel.MyFollowingCelebs.Clear();            
            foreach (string name in query)
            {                
                UserViewModel u = new UserViewModel();
                u.UserName = name;
                u.UserType = Constants.KEYWORD_NAME;//.ALERT_TYPE_CELEBRITY;
                App.ViewModel.MyFollowingCelebs.Add(u);                
            }
            
            this.MyFollowingCelebsSummaryList.Source = App.ViewModel.MyFollowingCelebs;
            this.MyFollowingCelebsSummaryList.Filter += (s, a) => a.Accepted = App.ViewModel.MyFollowingCelebs.IndexOf((UserViewModel)a.Item) < Constants.SUMMARY_COUNT;

            IEnumerable<UserViewModel> oq = App.ViewModel.MyFollowingCelebs.OrderBy(u => u.UserName);
            ObservableCollection<UserViewModel> list = new ObservableCollection<UserViewModel>();
            foreach (UserViewModel u in oq)
            {
                list.Add(u);
            }
            App.ViewModel.MyFollowingCelebs = list;

            query = App.ViewModel.Alerts.Where(alert => alert.Type == "photographer").Select(alert => alert.Name);
            App.ViewModel.MyFollowingUsers.Clear();            
            foreach (string name in query)
            {
                UserViewModel u = new UserViewModel();
                u.UserName = name;
                u.UserType = Constants.KEYWORD_USER;// ALERT_TYPE_PHOTOGRAPHER;
                App.ViewModel.MyFollowingUsers.Add(u);                
            }            

            this.MyFollowingUsersSummaryList.Source = App.ViewModel.MyFollowingUsers;
            this.MyFollowingUsersSummaryList.Filter += (s, a) => a.Accepted = App.ViewModel.MyFollowingUsers.IndexOf((UserViewModel)a.Item) < Constants.SUMMARY_COUNT;

            oq = App.ViewModel.MyFollowingUsers.OrderBy(u => u.UserName);
            list = new ObservableCollection<UserViewModel>();
            foreach (UserViewModel u in oq)
            {
                list.Add(u);
            }
            App.ViewModel.MyFollowingUsers = list;
        }
    }
    public delegate void SearchCompletedCallback(object sender, SearchEventArgs e);
}
