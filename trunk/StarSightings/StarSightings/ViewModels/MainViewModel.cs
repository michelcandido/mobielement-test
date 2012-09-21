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
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }
        public ObservableCollection<ItemViewModel> PopularItems { get; private set; }
        public ObservableCollection<ItemViewModel> PopularSummaryItems { get; private set; }
        public ObservableCollection<ItemViewModel> LatestItems { get; private set; }
        public ObservableCollection<ItemViewModel> LatestSummaryItems { get; private set; }
        public ObservableCollection<ItemViewModel> NearestItems { get; private set; }
        public ObservableCollection<ItemViewModel> NearestSummaryItems { get; private set; }
        public ObservableCollection<ItemViewModel> FollowingItems { get; private set; }
        public ObservableCollection<ItemViewModel> FollowingSummaryItems { get; private set; }

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
        }

        private void UpdateSummaryItems(ObservableCollection<ItemViewModel> source, ObservableCollection<ItemViewModel> target, int start, int end)
        {            
            IEnumerable<ItemViewModel> query = source.Where<ItemViewModel>(item => item.ID >= start && item.ID <= end);
            foreach (ItemViewModel item in query)
            {
                target.Add(item);                
            }            
        }

        public ItemViewModel GetItemById(int id)
        {
            return Items.FirstOrDefault(item => item.ID == id);       
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
}
