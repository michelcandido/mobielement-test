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
using System.Collections;
using System.Collections.ObjectModel;
using Microsoft.Phone.Net.NetworkInformation;
using Telerik.Windows.Controls;
using StarSightings.Events;
using System.Windows.Navigation;

namespace StarSightings
{
    public partial class ListViewControl : ListViewBase
    {
        private ObservableCollection<ItemViewModel> source;
        private MainViewModel context;
        private int currentStartIndex = 0;
        private bool requestIssued = false;

        public ListViewControl()
        {
            InitializeComponent();
            
            App.ViewModel.SearchDataReadyHandler += new SearchCompletedCallback(SearchDataDelivered);
        }

        protected override void OnDisplayed()
        {
            this.source = App.ViewModel.GetItemSouce(this.SearchGroup);
            currentStartIndex = this.source.Count;

            base.OnDisplayed();
            
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                RadMessageBox.Show("No Internet connection. This application needs Internet access to function properly.");
                return;
            }

            if (this.source.Count == 0)
            {
                //FiveHundredPxAPI.BeginGetPhotosByCategory("fresh", this.loadedPagesCount, this.PhotoDataDelivered);
            }
        }

        protected override RadDataBoundListBox ListBox
        {
            get
            {
                return this.listBox;
            }
        }

        protected override System.Collections.IEnumerable CreateItemsSource()
        {
            return this.source;
        }
        /*
        private void PhotoDataDelivered(PhotoQueryContext context)
        {
            this.loadedPagesCount++;

            foreach (Photo p in context.Photos)
            {
                this.source.Insert(0, p);
            }

            this.Dispatcher.BeginInvoke(() =>
            {
                this.listBox.StopPullToRefreshLoading(true);

                if (this.loadedPagesCount >= 11)
                {
                    this.listBox.IsPullToRefreshEnabled = false;
                }
            });
        }
         * */

        private void OnListBox_RefreshRequested(object sender, EventArgs e)
        {
            //FiveHundredPxAPI.BeginGetPhotosByCategory("fresh", this.loadedPagesCount, this.PhotoDataDelivered);
            //currentStartIndex = 0;
            switch (this.SearchGroup)
            {
                case Constants.SEARCH_POPULAR:
                    App.ViewModel.SearchPopular(true,0, null);
                    break;
                case Constants.SEARCH_LATEST:
                    App.ViewModel.SearchLatest(true,0, null);
                    break;
                case Constants.SEARCH_NEAREST:
                    App.ViewModel.SearchNearest(true, 0, null);
                    break;
                case Constants.SEARCH_FOLLOWING:
                    break;
            }
        }

        private void OnListBox_DataRequested(object sender, EventArgs e)
        {
            if (this.requestIssued)
            {
                return;
            }
            //currentStartIndex += Constants.LIMIT;
            switch (this.SearchGroup)
            {
                case Constants.SEARCH_POPULAR:
                    App.ViewModel.SearchPopular(false, currentStartIndex, null);
                    break;
                case Constants.SEARCH_LATEST:
                    App.ViewModel.SearchLatest(false, currentStartIndex, null);
                    break;
                case Constants.SEARCH_NEAREST:
                    App.ViewModel.SearchNearest(false, currentStartIndex, null);
                    break;
                case Constants.SEARCH_FOLLOWING:
                    break;
            }
            this.requestIssued = true;
        }

        public void SearchDataDelivered(object sender, SearchEventArgs e)
        {
            if (e.Successful)
            {
                currentStartIndex += e.Items.Count;
            }
            this.Dispatcher.BeginInvoke(() =>
            {
                this.listBox.StopPullToRefreshLoading(true);
                this.requestIssued = false;
                /*
                if (this.loadedPagesCount >= 11)
                {
                    this.listBox.IsPullToRefreshEnabled = false;
                }
                 * */
            });
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemViewModel selectedItemData = (sender as RadDataBoundListBox).SelectedItem as ItemViewModel;
            if (selectedItemData != null)
            {
                NavigateToDetails(new Uri(string.Format("/DetailsPage.xaml?selectedItemId={0}&selectedGroupId={1}", selectedItemData.PhotoId, this.SearchGroup), UriKind.RelativeOrAbsolute));
            }
            (sender as RadDataBoundListBox).SelectedItem = null;
        }

        

    }
}
