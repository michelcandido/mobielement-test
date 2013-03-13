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
        
        private void OnPostTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            /*
            String toReturn = "cat=";
            toReturn += App.SSAPI.getCatList();
            toReturn += "&time=" + Utils.ConvertToUnixTimestamp(App.ViewModel.StoryTime);
            if (App.ViewModel.StoryLat != 0.0)
                toReturn += "&geo_lat=" + App.ViewModel.StoryLat;
            if (App.ViewModel.StoryLng != 0.0)
                toReturn += "&geo_lng=" + App.ViewModel.StoryLng;
            toReturn += "&location=" + HttpUtility.UrlEncode(App.ViewModel.StoryLocation);
            if (!string.IsNullOrEmpty(App.ViewModel.StoryPlace) && !string.IsNullOrWhiteSpace(App.ViewModel.StoryPlace))
                toReturn += "&place=" + HttpUtility.UrlEncode(App.ViewModel.StoryPlace);
            toReturn += "&token=" + App.ViewModel.User.Token;

            string baseUri = Constants.SERVER_NAME + Constants.URL_POST_NEW;
            Uri requestUri = Utils.BuildUriWithAppendedParams(baseUri, toReturn);            
            
            AsyncHttpPostHelper asyncHttpPostHelper = new AsyncHttpPostHelper(null, requestUri);
            asyncHttpPostHelper.BeginSend(OnHttpPostCompleteDelegate);
             * */

            /*
            Dictionary<string, string> nvc = new Dictionary<string,string>();

            nvc.Add("cat", App.SSAPI.getCatList());
            nvc.Add("time", Utils.ConvertToUnixTimestamp(App.ViewModel.StoryTime).ToString());
            

            if (App.ViewModel.StoryLat != 0.0)
                nvc.Add("geo_lat", App.ViewModel.StoryLat.ToString());
            if (App.ViewModel.StoryLng != 0.0)
                nvc.Add("geo_lng",App.ViewModel.StoryLng.ToString());
            nvc.Add("location",HttpUtility.UrlEncode(App.ViewModel.StoryLocation).Replace("+", "%20"));
            
            if (!string.IsNullOrEmpty(App.ViewModel.StoryPlace) && !string.IsNullOrWhiteSpace(App.ViewModel.StoryPlace))
                nvc.Add("place",HttpUtility.UrlEncode(App.ViewModel.StoryPlace));
            if (!string.IsNullOrEmpty(App.ViewModel.StoryEvent) && !string.IsNullOrWhiteSpace(App.ViewModel.StoryEvent))
                nvc.Add("event", HttpUtility.UrlEncode(App.ViewModel.StoryEvent));
            if (!string.IsNullOrEmpty(App.ViewModel.PicStory) && !string.IsNullOrWhiteSpace(App.ViewModel.PicStory))
                nvc.Add("descr", HttpUtility.UrlEncode(App.ViewModel.PicStory));
            nvc.Add("token" , App.ViewModel.User.Token);
            */
            
            //postHandler = new PostEventHandler(PostCompleted);
            //App.SSAPI.NewPostHandler += postHandler;

            if (App.ViewModel.PostMode == 1)
            {                
                App.SSAPI.NewPost((bool)cbTest.IsChecked);                
            }
            else
            {
                App.SSAPI.NewPost2((bool)cbTest.IsChecked);                
            }

            if (this.cbFacebook.IsChecked==true && !string.IsNullOrEmpty(App.ViewModel.User.FBToken))
            {                
                App.SSAPI.PostOnFacebook();                
            }

            this.NavigationService.Navigate(new Uri("/MainPage.xaml?clear&screen=2", UriKind.RelativeOrAbsolute));
        }

        //private PostEventHandler postHandler;
        /*
        public void PostCompleted(object sender, PostEventArgs e)
        {
            App.SSAPI.NewPostHandler -= postHandler;
            if (e.Successful)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    // Showing the exact error message is useful for debugging. In a finalized application, 
                    // output a friendly and applicable string to the user instead. 
                    MessageBox.Show("Your post has been submitted successfully.");
                });
                App.ViewModel.SearchLatest(true, 0, null);   
                App.ViewModel.KeywordType = Constants.KEYWORD_MY;
                App.ViewModel.SearchKeywordSearch(true, 0, null);
                App.ViewModel.StoryTime = new DateTime(0);
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    // Showing the exact error message is useful for debugging. In a finalized application, 
                    // output a friendly and applicable string to the user instead. 
                    MessageBox.Show("Errors in your submission, please try again.");
                });         

            }            
        }
         * */

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