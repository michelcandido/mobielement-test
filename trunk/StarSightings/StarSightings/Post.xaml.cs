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
using Microsoft.Xna.Framework.Media;
using StarSightings.Events;
using System.Windows.Navigation;
using System.Threading;

namespace StarSightings
{
    public partial class Summary : PhoneApplicationPage
    {
        //private PostEventHandler postHandler;

        public Summary()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
        }

        private void OnCancelTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml?clear", UriKind.RelativeOrAbsolute));
        }

        
        /*
        private void OnPostTap(object sender, System.Windows.Input.GestureEventArgs e)
        {            
                      
            if (App.ViewModel.PostMode == 1)
            {                
                App.SSAPI.NewPost((bool)cbTest.IsChecked);                
            }
            else
            {
                App.SSAPI.NewPost2((bool)cbTest.IsChecked);                
            }

            if (this.cbFacebook.IsChecked == true && !string.IsNullOrEmpty(App.ViewModel.User.FBToken) && App.ViewModel.AccountType == Constants.ACCOUNT_TYPE_FACEBOOK)
            {                
                App.SSAPI.PostOnFacebook();                
            }

            //this.NavigationService.Navigate(new Uri("/MainPage.xaml?clear&screen=2", UriKind.RelativeOrAbsolute));
            App.ViewModel.KeywordType = Constants.KEYWORD_MY;
            NavigationService.Navigate(new Uri("/SearchResultPage.xaml?clear", UriKind.RelativeOrAbsolute));
        }
         * */

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (App.ViewModel.AccountType != Constants.ACCOUNT_TYPE_FACEBOOK)
            {
                this.cbFacebook.IsEnabled = false;
            }
        }

        private PostEventHandler postHandler;
        private void OnPostTap(object sender, System.Windows.Input.GestureEventArgs e)
        {            
            if (App.ViewModel.PostMode == 1)
            {
                postHandler = new PostEventHandler(PostCompleted);
                App.SSAPI.NewPostHandler += postHandler;
                App.SSAPI.NewPost2((bool)cbTest.IsChecked);
            }
            else
            {
                App.SSAPI.NewPost2((bool)cbTest.IsChecked);
            }

            if (this.cbFacebook.IsChecked == true && !string.IsNullOrEmpty(App.ViewModel.User.FBToken) && App.ViewModel.AccountType == Constants.ACCOUNT_TYPE_FACEBOOK)
            {
                App.SSAPI.PostOnFacebook();
            }
            
            App.ViewModel.KeywordType = Constants.KEYWORD_MY;
            NavigationService.Navigate(new Uri("/SearchResultPage.xaml?clear", UriKind.RelativeOrAbsolute));
        }
        
        public void PostCompleted(object sender, PostEventArgs e)
        {
            App.SSAPI.NewPostHandler -= postHandler;
            if (e.Successful && e.Items != null && e.Items.Count == 1)
            {
                App.SSAPI.NewPost(e.Items[0].PhotoId);
            }
            else
            {
                PostEventArgs pe = new PostEventArgs(false);
                App.SSAPI.OnNewPost(pe);
            }
            
        }
         

        protected void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            this.busyIndicator.IsRunning = false;
        }

        private void GoHome(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml?clear", UriKind.RelativeOrAbsolute));
        }
    }
}