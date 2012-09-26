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

namespace StarSightings
{
    public partial class MapPage : PhoneApplicationPage
    {
        public MapPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MapPage_Loaded);
        }

        void MapPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private string selectedGroupId;
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            selectedGroupId = NavigationContext.QueryString["selectedGroupId"];
            int itemId = 0;
            int.TryParse(selectedGroupId, out itemId);
            switch (itemId)
            {
                case 0:
                    this.GroupTitle.Text = "popular";
                    break;
                case 1:
                    this.GroupTitle.Text = "latest";
                    break;
                case 2:
                    this.GroupTitle.Text = "nearest";
                    break;
                case 3:
                    this.GroupTitle.Text = "following";
                    break;
            }            
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
    }
}