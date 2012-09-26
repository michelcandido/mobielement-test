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

            App.SSAPI.Search += new SearchEventHandler(SearchCompleted);            
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

        private string _sampleProperty = "Sample Runtime Property Value";        

        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
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
                this.PopularItems.Add(item);
                this.LatestItems.Add(item);
                this.NearestItems.Add(item);                
                this.FollowingItems.Add(item);
            }

            UpdateSummaryItems(this.PopularItems, this.PopularSummaryItems, 1, 2);
            UpdateSummaryItems(this.LatestItems, this.LatestSummaryItems, 3, 4);
            UpdateSummaryItems(this.NearestItems, this.NearestSummaryItems, 5, 6);            
            UpdateSummaryItems(this.FollowingItems, this.FollowingSummaryItems, 7, 8);

            DownloadData();
        }

        private void UpdateSummaryItems(ObservableCollection<ItemViewModel> source, ObservableCollection<ItemViewModel> target, int start, int end)
        {
            target.Clear();       
            for (int i = start; i <= end; i++)
            {
                target.Add(source.ElementAt(i));            
            }            
        }

        public void DownloadData()
        {
            SearchPopular(true,0);
            SearchLatest(true,0);
        }

        public void SearchPopular(bool fresh, int start)
        {
            SearchParams param = new SearchParams();
            param.start = start;      
            SearchToken token = new SearchToken();
            token.searchGroup = Constants.SEARCH_POPULAR;
            token.isFresh = fresh;
            token.start = start;
            App.SSAPI.DoSearch(param, token);
        }

        public void SearchLatest(bool fresh, int start)
        {
            SearchParams param = new SearchParams();
            param.start = start;
            param.order_by = "time";
            param.order_dir = "asc";
            SearchToken token = new SearchToken();
            token.searchGroup = Constants.SEARCH_LATEST;
            token.isFresh = fresh;
            token.start = start;
            App.SSAPI.DoSearch(param, token);
        }

        public void SearchCompleted(object sender, SearchEventArgs e)
        {
            if (e.Successful)
            {
                if (e.SearchToken.searchGroup == Constants.SEARCH_POPULAR)
                {
                    if (e.SearchToken.isFresh)
                    {
                        App.ViewModel.PopularItems.Clear();
                        foreach (ItemViewModel item in e.Items)
                        {
                            App.ViewModel.PopularItems.Add(item);
                        }
                        UpdateSummaryItems(App.ViewModel.PopularItems, App.ViewModel.PopularSummaryItems, 0, 1);
                    }
                    else
                    {
                        int i = 0;
                        foreach (ItemViewModel item in e.Items)
                        {
                            App.ViewModel.PopularItems.Insert(e.SearchToken.start + i++, item);
                        }
                    }

                }
                else if (e.SearchToken.searchGroup == Constants.SEARCH_LATEST)
                {
                    if (e.SearchToken.isFresh)
                    {
                        App.ViewModel.LatestItems.Clear();
                        foreach (ItemViewModel item in e.Items)
                        {
                            App.ViewModel.LatestItems.Add(item);
                        }
                        UpdateSummaryItems(App.ViewModel.LatestItems, App.ViewModel.LatestSummaryItems, 0, 1);
                    }
                    else
                    {
                        int i = 0;
                        foreach (ItemViewModel item in e.Items)
                        {
                            App.ViewModel.LatestItems.Insert(e.SearchToken.start + i++, item);
                        }
                    }
                }
                OnSearchCompleted(new EventArgs());
            }
        }

        public event SearchCompletedCallback SearchDataReadyHandler;        
        protected virtual void OnSearchCompleted(EventArgs e)
        {
            if (SearchDataReadyHandler != null)
            {
                SearchDataReadyHandler(this, e);
            }
        }

        public ItemViewModel GetItemById(int id)
        {
            return Items.FirstOrDefault(item => item.ID == id);       
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
    public delegate void SearchCompletedCallback(object sender, EventArgs e);
}
