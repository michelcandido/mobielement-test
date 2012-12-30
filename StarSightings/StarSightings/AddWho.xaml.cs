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
    public partial class AddWho : PhoneApplicationPage
    {
        private ApplicationBarIconButton btnNext;

        public AddWho()
        {            
            InitializeComponent();
            DataContext = App.ViewModel;

            btnNext = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
        }               

        private void OnBackClick(object sender, EventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void OnNextClick(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Location.xaml", UriKind.RelativeOrAbsolute));
        }

        private void OnAddTap(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbName.Text) && !App.ViewModel.CelebNameList.Contains(tbName.Text))
            {
                App.ViewModel.CelebNameList.Add(tbName.Text);
                btnNext.IsEnabled = true;  
            }
        }
        

        private void OnTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ViewModel.CelebNameList.RemoveAt((sender as ListBox).SelectedIndex);
            if (App.ViewModel.CelebNameList.Count <= 0)
                btnNext.IsEnabled = false;  
        }

        

       
    }
}