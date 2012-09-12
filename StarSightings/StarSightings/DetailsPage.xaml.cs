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
    public partial class DetailsPage : PhoneApplicationPage
    {
        public DetailsPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string selectedItemId = NavigationContext.QueryString["selectedItemId"];
            int itemId = -1;            
            ItemViewModel item = null;
            if (int.TryParse(selectedItemId, out itemId))
            {
                item = App.ViewModel.GetItemById(itemId);
            }
            this.slideView.SelectedItem = item;
        }
    }
}