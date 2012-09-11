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
    public partial class ListPage : PhoneApplicationPage
    {
        public ListPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(ListPage_Loaded);
        }

        void ListPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        /// <summary>
        /// Navigates to filter page.
        /// </summary>       
        private void GoToFilter(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/FilterPage.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Navigates to map page.
        /// </summary>
        private void GoToMap(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MapPage.xaml", UriKind.RelativeOrAbsolute));
            //this.NavigationService.RemoveBackEntry();
        }
    }
}