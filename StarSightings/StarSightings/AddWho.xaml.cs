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
    public partial class AddWho : PhoneApplicationPage
    {
        private bool firstTime = true;

        public AddWho()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
            App.ViewModel.CelebNameList.Clear();
            App.ViewModel.CelebNameList.Add(App.ViewModel.CelebName);
        }

        private void OnAdd(object sender, System.Windows.Input.GestureEventArgs e)
        {

            //System.Windows.MessageBox.Show(App.ViewModel.CelebName);

            if (!App.ViewModel.CelebNameList.Contains(App.ViewModel.CelebName))
            {
                App.ViewModel.CelebNameList.Add(App.ViewModel.CelebName);
            }
        }


        private void onNextTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
           // App.ViewModel.SearchKeywords = App.ViewModel.CelebName;
            //App.ViewModel.SearchKeywordSearch(true, 0, null);
            
            this.NavigationService.Navigate(new Uri("/Location.xaml", UriKind.RelativeOrAbsolute));
        }

        private void onBackTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/WhoDidUSee.xaml", UriKind.RelativeOrAbsolute));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
            {
                
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
                App.ViewModel.CelebName = nameBox.Text;
        }

        private void OnTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
//            WebBrowserTask webBrowserTask = new WebBrowserTask();
            //System.Windows.MessageBox.Show((sender as ListBox).SelectedIndex.ToString());
            App.ViewModel.CelebNameList.RemoveAt((sender as ListBox).SelectedIndex);
//            webBrowserTask.Uri = new Uri(((string)((sender as ListBoxItem).Tag)).Trim(), UriKind.Absolute);
//            webBrowserTask.Show();
        }

        private void OnGetFocus(object sender, RoutedEventArgs e)
        {
            if (firstTime)
            {
                nameBox.Text = "";
            }
            firstTime = false;
        }
    }
}