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
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
        }

        private LoginEventHandler myLoginEventHandler;
        private void GoToLogin(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (!App.ViewModel.NeedLogin)
            {
                MessageBoxResult result =
                    MessageBox.Show("Are you sure you want to logout of your StarSightings account?",
                    "Logout", MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    myLoginEventHandler = new LoginEventHandler(LogoutCompleted);
                    App.SSAPI.LoginHandler += myLoginEventHandler;
                    App.SSAPI.Logout();
                }

            }
            else
            {
                this.NavigationService.Navigate(new Uri("/LoginOptionsPage.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        public void LogoutCompleted(object sender, LoginEventArgs e)
        {
            App.SSAPI.LoginHandler -= myLoginEventHandler;
            if (e.Successful)
            {
                App.ViewModel.AccountType = Constants.ACCOUNT_TYPE_DEVICE;
                App.ViewModel.User = null;
                Utils.AddOrUpdateIsolatedStorageSettings("AccountType", App.ViewModel.AccountType);
                Utils.RemoveIsolatedStorageSettings("User");

                App.ViewModel.NeedLogin = true;
                App.ViewModel.IsDataLoaded = false;
                App.Config.Login();
                GoHome(this, null);
            }
            else
            {
                App.ViewModel.NeedLogin = false;
                MessageBox.Show("Cannot logout, please try again.");
            }
        }

        private void GoHome(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml?clear", UriKind.RelativeOrAbsolute));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
            {
                switch (App.ViewModel.AccountType)
                {
                    case Constants.ACCOUNT_TYPE_DEVICE:
                        this.UserType.Text = "You are logged in as a guest";
                        break;
                    case Constants.ACCOUNT_TYPE_FACEBOOK:
                        this.UserType.Text = "You are logged in via Facebook";
                        break;
                    case Constants.ACCOUNT_TYPE_SS:
                        this.UserType.Text = "Username";
                        break;
                }      
            }
        }
    }
}