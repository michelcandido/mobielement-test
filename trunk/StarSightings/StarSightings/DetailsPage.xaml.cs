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
using Microsoft.Phone.Tasks;
using System;
using StarSightings.Events;

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
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
            {
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

        private void Wiki_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();

            webBrowserTask.Uri = new Uri("http://en.m.wikipedia.org/wiki/" + ((string)((sender as Grid).Tag)).Trim(), UriKind.Absolute);

            webBrowserTask.Show();
        }

        private void Source_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri(((string)((sender as ListBoxItem).Tag)).Trim(), UriKind.Absolute);
            webBrowserTask.Show();
        }

        private void htmlTextBlock_NavigationRequested(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();

            webBrowserTask.Uri = new Uri(((e.Content as Hyperlink).CommandParameter as string), UriKind.Absolute);

            webBrowserTask.Show();
        }

        private AlertEventHandler followAlertEventHandler;
        private void Follow_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            followAlertEventHandler = new AlertEventHandler(FollowCompleted);
            App.SSAPI.AlertHandler += followAlertEventHandler;
            App.SSAPI.Alert(Constants.ALERT_SET, Constants.ALERT_TYPE_CELEBRITY,((string)((sender as Grid).Tag)).Trim());
        }

        public void FollowCompleted(object sender, AlertEventArgs e)
        {
            App.SSAPI.AlertHandler -= followAlertEventHandler;
            if (e.Successful)
            {
                App.ViewModel.Alerts = e.Alerts;
                Utils.AddOrUpdateIsolatedStorageSettings("Alerts", App.ViewModel.Alerts);
                MessageBox.Show("Your request has been set.");
                App.ViewModel.SearchFollowing(true, 0, null);
            }
            else
            {
                MessageBox.Show("Your Alert request cannot be fullfilled, please try again.");
            }
        }
    }
}