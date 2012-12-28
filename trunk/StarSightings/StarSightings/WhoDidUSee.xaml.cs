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
using Microsoft.Phone.Shell;

namespace StarSightings
{
    public partial class WhoDidUSee : PhoneApplicationPage
    {
        //private bool firstTime = true;
        private ApplicationBarIconButton btnBack, btnNext;

        public WhoDidUSee()
        {
            InitializeComponent();
            DataContext = App.ViewModel;

            btnBack = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            btnNext = (ApplicationBarIconButton)ApplicationBar.Buttons[1];

            onTextChange(this, null);
        }

        private void onTextChange(object sender, TextChangedEventArgs e)
        {
            if (tbName.Text == "")
            {
                btnNext.IsEnabled = false;                
            }
            else
            {
                btnNext.IsEnabled = true;                
            }

        }        

        private void OnBackClick(object sender, EventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void OnNextClick(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/AddWho.xaml", UriKind.RelativeOrAbsolute));
        }


    }
}