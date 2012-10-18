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
using System.Collections.ObjectModel;

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
}