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
using StarSightings.Events;

namespace StarSightings
{
    public partial class LoginPage : PhoneApplicationPage
    {
        private LoginEventHandler myLoginEventHandler;
        public LoginPage()
        {
            InitializeComponent();
        }

        private void PasswordGotFocus(object sender, RoutedEventArgs e)
        {
            tbPasswordWatermark.Opacity = 0;
            pbPassword.Opacity = 100;
        }

        private void PasswordLostFocus(object sender, RoutedEventArgs e)
        {
            var passwordEmpty = string.IsNullOrEmpty(pbPassword.Password);
            tbPasswordWatermark.Opacity = passwordEmpty ? 100 : 0;
            pbPassword.Opacity = passwordEmpty ? 0 : 100;
        }

        private void btnSignup_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/SignupPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnLogin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ViewModel.AccountType = Constants.ACCOUNT_TYPE_SS;
            string query = "username=" + tbUserName.Text + "&password=" + pbPassword.Password;
            myLoginEventHandler = new LoginEventHandler(LoginCompleted);
            App.SSAPI.LoginHandler += myLoginEventHandler;
            App.SSAPI.Login(App.ViewModel.AccountType, query);
        }

        public void LoginCompleted(object sender, LoginEventArgs e)
        {
            App.SSAPI.LoginHandler -= myLoginEventHandler;
            if (e.Successful)
            {
                App.ViewModel.User = e.User;
                Utils.AddOrUpdateIsolatedStorageSettings("User", App.ViewModel.User);                
                Utils.AddOrUpdateIsolatedStorageSettings("AccountType", App.ViewModel.AccountType);
                if (App.ViewModel.AccountType == Constants.ACCOUNT_TYPE_DEVICE)
                    App.ViewModel.NeedLogin = true;
                else
                    App.ViewModel.NeedLogin = false;
                this.NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Cannot login, please try again.");
            }
        }
    }
}