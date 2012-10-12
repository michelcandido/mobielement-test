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
                string query = "";
                switch (App.ViewModel.AccountType)
                {
                    case Constants.ACCOUNT_TYPE_DEVICE:
                        query = "device_id=" + (string)Utils.GetIsolatedStorageSettings("DeviceId");
                        break;
                    case Constants.ACCOUNT_TYPE_SS:
                        query = "username=" + App.ViewModel.User.UserName + "&password=" + App.ViewModel.User.Password;
                        break;
                    case Constants.ACCOUNT_TYPE_FACEBOOK:
                        break;
                }
                myLoginEventHandler = new LoginEventHandler(LoginCompleted);
                App.SSAPI.LoginHandler += myLoginEventHandler;
                App.SSAPI.Login(App.ViewModel.AccountType, query);
            }
            else
            {
                IsAppInit = true;                
                if (App.ViewModel.AccountType == Constants.ACCOUNT_TYPE_DEVICE)
                    App.ViewModel.NeedLogin = true;
                else
                    App.ViewModel.NeedLogin = false;
                SSEventArgs se = new SSEventArgs(true);
                OnInit(se);
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

            IsAppInit = true;
            SSEventArgs se = e.Successful?new SSEventArgs(true):new SSEventArgs(false);
            OnInit(se);            
        }
    }

    public delegate void InitAppHandler(object sender, SSEventArgs e);
}
