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
using Telerik.Windows.Controls.PhoneTextBox;
using Microsoft.Phone.Tasks;

namespace StarSightings
{
    public partial class SSLoginPage : PhoneApplicationPage
    {
        private LoginEventHandler myLoginEventHandler;
        private RegisterEventHandler myRegisterEventHandler;
        public SSLoginPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel.User;
            this.Loaded += new RoutedEventHandler(SSLoginPage_Loaded);
            //App.SSAPI.RegisterHandler += new RegisterEventHandler(RegisterCompleted);
        }

        void SSLoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void LoginPasswordGotFocus(object sender, RoutedEventArgs e)
        {
            tbLoginPasswordWatermark.Opacity = 0;
            pbLoginPassword.Opacity = 100;
        }

        private void LoginPasswordLostFocus(object sender, RoutedEventArgs e)
        {
            var passwordEmpty = string.IsNullOrEmpty(pbLoginPassword.Password);
            tbLoginPasswordWatermark.Opacity = passwordEmpty ? 100 : 0;
            pbLoginPassword.Opacity = passwordEmpty ? 0 : 100;
        }

        private void UsernameLostFocus(object sender, RoutedEventArgs e)
        {
            ValidateUserName();
        }

        private void PasswordGotFocus(object sender, RoutedEventArgs e)
        {
            tbPasswordWatermark.Opacity = 0;
            pbPassword.Opacity = 100;
        }

        private void PasswordLostFocus(object sender, RoutedEventArgs e)
        {
            ValidatePassword();
        }

        private void VerifyPasswordGotFocus(object sender, RoutedEventArgs e)
        {
            tbVerifyPasswordWatermark.Opacity = 0;
            pbVerifyPassword.Opacity = 100;
        }

        private void VerifyPasswordLostFocus(object sender, RoutedEventArgs e)
        {
            ValidateVerifyPassword();
        }

        private void EmailLostFocus(object sender, RoutedEventArgs e)
        {
            ValidateEmail();
        }

        

        private bool ValidateUserName()
        {
            if (string.IsNullOrEmpty(tbUserName.Text) || tbUserName.Text.Length < 3)
            {
                tbUserName.ChangeValidationState(ValidationState.Invalid, "Please enter a valid user name!");
                return false;
            }
            else
            {
                tbUserName.ChangeValidationState(ValidationState.Valid, "");
                return true;
            }
        }

        private bool ValidatePassword()
        {
            var passwordEmpty = string.IsNullOrEmpty(pbPassword.Password);
            //tbPasswordWatermark.Opacity = passwordEmpty ? 100 : 0;
            //pbPassword.Opacity = passwordEmpty ? 0 : 100;
            if (passwordEmpty || pbPassword.Password.Length < 6)
            {
                tbPasswordWatermark.Opacity = 100;
                pbPassword.Opacity = 0;
                tbPasswordWatermark.ChangeValidationState(ValidationState.Invalid, "Please enter a valid password!");
                return false;
            }
            else
            {
                tbPasswordWatermark.Opacity = 0;
                pbPassword.Opacity = 100;
                tbPasswordWatermark.ChangeValidationState(ValidationState.Valid, "");
                return true;
            }
        }

        private bool ValidateVerifyPassword()
        {
            var passwordEmpty = string.IsNullOrEmpty(pbVerifyPassword.Password);
            //tbVerifyPasswordWatermark.Opacity = passwordEmpty ? 100 : 0;
            //pbVerifyPassword.Opacity = passwordEmpty ? 0 : 100;
            if (passwordEmpty || pbVerifyPassword.Password.Length < 6 || !pbVerifyPassword.Password.Equals(pbPassword.Password))
            {
                tbVerifyPasswordWatermark.Opacity = 100;
                pbVerifyPassword.Opacity = 0;
                tbVerifyPasswordWatermark.ChangeValidationState(ValidationState.Invalid, "Please enter a valid password!");
                return false;
            }
            else
            {
                tbVerifyPasswordWatermark.Opacity = 0;
                pbVerifyPassword.Opacity = 100;
                tbVerifyPasswordWatermark.ChangeValidationState(ValidationState.Valid, "");
                return true;
            }
        }

        private bool ValidateEmail()
        {
            tbEmail.ChangeValidationState(ValidationState.Validating, "Validating");
            bool isValid = Utils.IsValidEmail(tbEmail.Text);
            if (isValid)
            {
                tbEmail.ChangeValidationState(ValidationState.Valid, "");
                return true;
            }
            else
            {
                tbEmail.ChangeValidationState(ValidationState.Invalid, "Please enter a valid email!");
                return false;
            }
        }

        private void tbUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
                this.pbPassword.Focus();
        }

        private void pbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
                this.pbVerifyPassword.Focus();
        }

        private void pbVerifyPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
                this.tbEmail.Focus();
        }

        private void tbEmail_KeyDown(object sender, KeyEventArgs e)
        {/*
            if (e.Key.Equals(Key.Enter))
                this.btnSignup.Focus();
          * */
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            App.ViewModel.AccountType = Constants.ACCOUNT_TYPE_SS;
            switch (this.pivotControl.SelectedIndex)
            {
                case 0:
                    string query = string.Format("username={0}&password={1}", tbLoginUserName.Text, pbLoginPassword.Password);
                    myLoginEventHandler = new LoginEventHandler(LoginCompleted);
                    App.SSAPI.LoginHandler += myLoginEventHandler;
                    App.SSAPI.Login(App.ViewModel.AccountType, query);
                    break;
                case 1:
                    if (ValidateUserName() && ValidatePassword() && ValidateVerifyPassword() && ValidateEmail())
                    {
                        myRegisterEventHandler = new RegisterEventHandler(RegisterCompleted);
                        App.SSAPI.RegisterHandler += myRegisterEventHandler;
                        App.SSAPI.RegisterUser();
                    }
                    break;
            }
            
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            if (this.NavigationService.CanGoBack)
                this.NavigationService.GoBack();
        }

        public void LoginCompleted(object sender, LoginEventArgs e)
        {
            App.SSAPI.LoginHandler -= myLoginEventHandler;
            if (e.Successful)
            {
                App.ViewModel.User = e.User;
                Utils.AddOrUpdateIsolatedStorageSettings("User", App.ViewModel.User);
                Utils.AddOrUpdateIsolatedStorageSettings("AccountType", App.ViewModel.AccountType);                
                App.ViewModel.NeedLogin = false;

                App.Config.UpdateAlerts();
                /*
                App.ViewModel.KeywordType = Constants.KEYWORD_MY;
                App.ViewModel.SearchKeywordSearch(true, 0, null);                
                */
                NavigationService.RemoveBackEntry();
                this.NavigationService.GoBack();
            }
            else
            {
                App.ViewModel.NeedLogin = true;
                if (e.ErrorCode == Constants.ERROR_LOGIN_USERNAME)
                    MessageBox.Show("Cannot login: username doesn't exist.");
                else if (e.ErrorCode == Constants.ERROR_LOGIN_PASSWORD)
                    MessageBox.Show("Cannot login: password doesn't match.");
            }
        }

        public void RegisterCompleted(object sender, RegisterEventArgs e)
        {
            App.SSAPI.RegisterHandler -= myRegisterEventHandler;
            if (e.Successful)
            {
                App.ViewModel.User = e.User;
                Utils.AddOrUpdateIsolatedStorageSettings("User", e.User);
                App.ViewModel.NeedLogin = false;
                Utils.AddOrUpdateIsolatedStorageSettings("AccountType", App.ViewModel.AccountType);
                
                App.Config.UpdateAlerts();
                /*
                App.ViewModel.KeywordType = Constants.KEYWORD_MY;
                App.ViewModel.SearchKeywordSearch(true, 0, null);
                */
                NavigationService.RemoveBackEntry();
                this.NavigationService.GoBack();
            }
            else
            {
                App.ViewModel.NeedLogin = true;
                /*
                if (e.ErrorCode == Constants.ERROR_REGISTER_USERNAME)
                    MessageBox.Show("Cannot register: username has been used by others.");
                else if (e.ErrorCode == Constants.ERROR_REGISTER_EMAIL_INVALID)
                    MessageBox.Show("Cannot login: invalid email address.");
                else if (e.ErrorCode == Constants.ERROR_REGISTER_EMAIL_UNAVAILABLE)
                    MessageBox.Show("Cannot login: email address has been registered by others.");
                 * */
            }
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();

            webBrowserTask.Uri = new Uri(Constants.SERVER_NAME + "/forum/ucp.php?mode=sendpassword", UriKind.Absolute);

            webBrowserTask.Show();
            
        }

        private void GoHome(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml?clear", UriKind.RelativeOrAbsolute));
        }
    }
}