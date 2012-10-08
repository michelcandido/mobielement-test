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

        public void InitApp()
        {
            // register device for the first time launch
            if (Utils.GetIsolatedStorageSettings("DeviceId") == null)
            {
                App.SSAPI.Register += new RegisterEventHandler(RegisterDeviceCompleted);
                App.SSAPI.RegisterDevice();
            }
            else
            {
                RegisterEventArgs re = new RegisterEventArgs(true);
                re.DeviceId = (string)Utils.GetIsolatedStorageSettings("DeviceId");
                RegisterDeviceCompleted(null, re);
            }
            
            if (Utils.GetIsolatedStorageSettings("User") != null)
                App.ViewModel.User = (UserViewModel)Utils.GetIsolatedStorageSettings("User");
        }

        public void RegisterDeviceCompleted(object sender, RegisterEventArgs e)
        {
            if (e.Successful)
            {
                App.ViewModel.DeviceId = e.DeviceId;
                IsAppInit = true;
                Utils.AddOrUpdateIsolatedStorageSettings("DeviceId", App.ViewModel.DeviceId);
            }
        }
    }
}
