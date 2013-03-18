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
using Facebook;
using StarSightings.Events;

namespace StarSightings
{
    public partial class FacebookLoginPage : PhoneApplicationPage
    {        

        /// <summary>
        /// Extended permissions is a comma separated list of permissions to ask the user.
        /// </summary>
        /// <remarks>
        /// For extensive list of available extended permissions refer to 
        /// https://developers.facebook.com/docs/reference/api/permissions/
        /// </remarks>
        private const string ExtendedPermissions = "email, read_friendlists, user_subscriptions";//user_about_me,read_stream,publish_stream";

        private readonly FacebookClient _fb = new FacebookClient();

        private string fbToken;

        public FacebookLoginPage()
        {
            InitializeComponent();
        }

        private void webBrowser1_Loaded(object sender, RoutedEventArgs e)
        {
            var loginUrl = GetFacebookLoginUrl(Constants.APP_ID_FACEBOOK, ExtendedPermissions);
            webBrowser1.Navigate(loginUrl);
        }

        private Uri GetFacebookLoginUrl(string appId, string extendedPermissions)
        {
            var parameters = new Dictionary<string, object>();
            parameters["client_id"] = appId;
            parameters["redirect_uri"] = "https://www.facebook.com/connect/login_success.html";
            parameters["response_type"] = "token";
            parameters["display"] = "touch";

            // add the 'scope' only if we have extendedPermissions.
            if (!string.IsNullOrEmpty(extendedPermissions))
            {
                // A comma-delimited list of permissions
                parameters["scope"] = extendedPermissions;
            }

            return _fb.GetLoginUrl(parameters);
        }

        private void webBrowser1_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            FacebookOAuthResult oauthResult;
            if (!_fb.TryParseOAuthCallbackUrl(e.Uri, out oauthResult))
            {
                return;
            }

            if (oauthResult.IsSuccess)
            {
                var accessToken = oauthResult.AccessToken;
                fbToken = accessToken;
                LoginSucceded(accessToken);
            }
            else
            {
                // user cancelled
                MessageBox.Show(oauthResult.ErrorDescription);
            }
        }

        private void LoginSucceded(string accessToken)
        {
            var fb = new FacebookClient(accessToken);

            fb.GetCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
                    return;
                }

                var result = (IDictionary<string, object>)e.GetResultData();
                var id = (string)result["id"];

                //LoginSS(accessToken);
                //var url = string.Format("/Pages/FacebookInfoPage.xaml?access_token={0}&id={1}", accessToken, id);

                //Dispatcher.BeginInvoke(() => NavigationService.Navigate(new Uri(url, UriKind.Relative)));
            };

            LoginSS(accessToken);
            //fb.GetAsync("me?fields=id");
        }

        private LoginEventHandler myLoginEventHandler;
        private void LoginSS(string accessToken)
        {
            App.ViewModel.AccountType = Constants.ACCOUNT_TYPE_FACEBOOK;
            //string query = string.Format("fb_token={0}&device_id={1}&auto_register=1",accessToken, App.ViewModel.DeviceId);
            string query = string.Format("fb_token={0}&auto_register=1", accessToken);
            myLoginEventHandler = new LoginEventHandler(LoginCompleted);
            App.SSAPI.LoginHandler += myLoginEventHandler;
            App.SSAPI.Login(App.ViewModel.AccountType, query);
        }

        public void LoginCompleted(object sender, LoginEventArgs e)
        {
            App.SSAPI.LoginHandler -= myLoginEventHandler;
            if (fbToken != null)
                App.ViewModel.User.FBToken = fbToken;
            if (e.Successful)
            {
                App.ViewModel.User = e.User;                
                Utils.AddOrUpdateIsolatedStorageSettings("User", App.ViewModel.User);
                Utils.AddOrUpdateIsolatedStorageSettings("AccountType", App.ViewModel.AccountType);
                /*
                if (App.ViewModel.AccountType == Constants.ACCOUNT_TYPE_DEVICE)
                    App.ViewModel.NeedLogin = true;
                else*/

                App.Config.loginSuccess = true;
                App.ViewModel.IsDataLoaded = false;
                App.ViewModel.NeedLogin = false;
                
                App.Config.UpdateAlerts();                
                /*
                App.ViewModel.KeywordType = Constants.KEYWORD_MY;
                App.ViewModel.SearchKeywordSearch(true, 0, null);
                */
                /*
                NavigationService.RemoveBackEntry();
                this.NavigationService.GoBack();                
                 * */
                GoHome(this, null);
            }
            else
            {
                App.ViewModel.NeedLogin = true;
                //MessageBox.Show("Cannot login, please try again.");
                NavigationService.RemoveBackEntry();
                this.NavigationService.GoBack();
            }
        }

        private void GoHome(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml?clear", UriKind.RelativeOrAbsolute));
        }
    }
}