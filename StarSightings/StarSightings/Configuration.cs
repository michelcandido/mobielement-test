using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using StarSightings.Events;
using StarSightings.ViewModels;
using System.Collections.ObjectModel;

namespace StarSightings
{
    public class Configuration
    {        
        public bool IsAppInit
        {
            get;
            private set;
        }

        public event InitAppHandler InitAppCompletedHandler;

        protected virtual void OnInit(SSEventArgs e)
        {
            if (InitAppCompletedHandler != null)
            {
                InitAppCompletedHandler(this, e);               
            }
        }

        public void InitApp()
        {
            // register device for the first time launch
            if (Utils.GetIsolatedStorageSettings("DeviceId") == null)
            {
                App.SSAPI.RegisterHandler += new RegisterEventHandler(RegisterDeviceCompleted);
                App.SSAPI.RegisterDevice();
            }
            else
            {
                RegisterEventArgs re = new RegisterEventArgs(true);
                re.DeviceId = (string)Utils.GetIsolatedStorageSettings("DeviceId");
                RegisterDeviceCompleted(null, re);
            }            
        }

        public void RegisterDeviceCompleted(object sender, RegisterEventArgs e)
        {
            if (e.Successful)
            {
                App.ViewModel.DeviceId = e.DeviceId;                
                Utils.AddOrUpdateIsolatedStorageSettings("DeviceId", App.ViewModel.DeviceId);
                Login();
            }
        }

        private bool loginSuccess;
        public void Login()
        {
            bool needLogin = false;
            if (Utils.GetIsolatedStorageSettings("User") != null)
            {
                App.ViewModel.User = (UserViewModel)Utils.GetIsolatedStorageSettings("User");
                if (Utils.ConvertToUnixTimestamp(DateTime.UtcNow) > App.ViewModel.User.TokenExpiration)
                    needLogin = true;
            }
            else
            {
                needLogin = true;
            }

            if (Utils.GetIsolatedStorageSettings("AccountType") != null)
            {
                App.ViewModel.AccountType = (int)Utils.GetIsolatedStorageSettings("AccountType");
            }
            else
            {
                App.ViewModel.AccountType = Constants.ACCOUNT_TYPE_DEVICE;
            }

            if (needLogin)
            {
                string query = string.Empty;
                switch (App.ViewModel.AccountType)
                {
                    case Constants.ACCOUNT_TYPE_DEVICE:
                        query = string.Format("device_id={0}",(string)Utils.GetIsolatedStorageSettings("DeviceId"));
                        break;
                    case Constants.ACCOUNT_TYPE_SS:
                        query = string.Format("username={0}&password={1}",App.ViewModel.User.UserName,App.ViewModel.User.Password);
                        break;
                    case Constants.ACCOUNT_TYPE_FACEBOOK:
                        query = string.Format("fb_token={0}",App.ViewModel.User.FBToken);
                        break;
                }
                myLoginEventHandler = new LoginEventHandler(LoginCompleted);
                App.SSAPI.LoginHandler += myLoginEventHandler;
                App.SSAPI.Login(App.ViewModel.AccountType, query);
            }
            else
            {                                
                if (App.ViewModel.AccountType == Constants.ACCOUNT_TYPE_DEVICE)
                    App.ViewModel.NeedLogin = true;
                else
                    App.ViewModel.NeedLogin = false;
                
                loginSuccess = true;
                UpdateAlerts();
            }
        }

        private LoginEventHandler myLoginEventHandler;
        public void LoginCompleted(object sender, LoginEventArgs e)
        {
            App.SSAPI.LoginHandler -= myLoginEventHandler;
            if (e.Successful)
            {
                App.ViewModel.User = e.User;
                Utils.AddOrUpdateIsolatedStorageSettings("User", App.ViewModel.User);
                if (App.ViewModel.AccountType == Constants.ACCOUNT_TYPE_DEVICE)
                    App.ViewModel.NeedLogin = true;
                else
                    App.ViewModel.NeedLogin = false;
            }
            else
            {
                if (e.ErrorCode == Constants.ERROR_LOGIN_USERNAME)
                    MessageBox.Show("Cannot login: username doesn't exist.");
                else if (e.ErrorCode == Constants.ERROR_LOGIN_PASSWORD)
                    MessageBox.Show("Cannot login: password doesn't match.");
            }
            
            loginSuccess = e.Successful;
            UpdateAlerts();
        }

        public void UpdateAlerts()
        {
            myAlertEventHandler = new AlertEventHandler(AlertCompleted);
            App.SSAPI.AlertHandler += myAlertEventHandler;
            App.SSAPI.Alert(Constants.ALERT_GET,null,null);
        }

        private AlertEventHandler myAlertEventHandler;
        public void AlertCompleted(object sender, AlertEventArgs e)
        {
            App.SSAPI.AlertHandler -= myAlertEventHandler;
            if (e.Successful)
            {
                App.ViewModel.Alerts = e.Alerts;
                Utils.AddOrUpdateIsolatedStorageSettings("Alerts", App.ViewModel.Alerts);
            }
            else
            {
                if (Utils.GetIsolatedStorageSettings("Alerts") != null)
                {
                    App.ViewModel.Alerts = (ObservableCollection<AlertViewModel>)Utils.GetIsolatedStorageSettings("Alerts");                    
                }
                
            }
            App.ViewModel.UpdateMyFollowings();
            App.ViewModel.SearchFollowing(true, 0, null);

            App.ViewModel.KeywordType = Constants.KEYWORD_MY;
            App.ViewModel.SearchKeywordSearch(true, 0, null);

            InitCompleted(loginSuccess);
        }

        private void InitCompleted(bool success)
        {
            IsAppInit = true;
            SSEventArgs se = new SSEventArgs(success);
            OnInit(se);
        }
    }

    public delegate void InitAppHandler(object sender, SSEventArgs e);
}
