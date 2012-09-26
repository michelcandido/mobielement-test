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

namespace StarSightings
{
    public partial class ListViewControl : ListViewBase
    {
        private ObservableCollection<ItemViewModel> source;
        private MainViewModel context;
        private int loadedPagesCount = 1;

        public ListViewControl()
        {
            InitializeComponent();
            
            App.ViewModel.SearchDataReadyHandler += new SearchCompletedCallback(SearchDataDelivered);
        }

        protected override void OnDisplayed()
        {
            this.source = App.ViewModel.GetItemSouce(this.SearchGroup);
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

        private void ListBox_RefreshRequested(object sender, EventArgs e)
        {
            //FiveHundredPxAPI.BeginGetPhotosByCategory("fresh", this.loadedPagesCount, this.PhotoDataDelivered);
            switch (this.SearchGroup)
            {
                case Constants.SEARCH_POPULAR:
                    App.ViewModel.SearchPopular(true);
                    break;
                case Constants.SEARCH_LATEST:
                    App.ViewModel.SearchLatest(true);
                    break;
                case Constants.SEARCH_NEAREST:
                    break;
                case Constants.SEARCH_FOLLOWING:
                    break;
            }
        }

        public void SearchDataDelivered(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                this.listBox.StopPullToRefreshLoading(true);
                /*
                if (this.loadedPagesCount >= 11)
                {
                    this.listBox.IsPullToRefreshEnabled = false;
                }
                 * */
            });
        }
    }
}
