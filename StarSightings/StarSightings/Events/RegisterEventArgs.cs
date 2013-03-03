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
using StarSightings.ViewModels;

namespace StarSightings.Events
{
    public class RegisterEventArgs : EventArgs
    {
        private readonly bool successful = false;
        private string deviceId;
        private UserViewModel user;

        public RegisterEventArgs(bool successful)
        {
            this.successful = successful;
        }

        private string errorCode;
        public string ErrorCode
        {
            get
            {
                return this.errorCode;
            }
            set
            {
                this.errorCode = value;
            }
        }

        public string DeviceId
        {
            get
            {
                return this.deviceId;
            }
            set
            {
                this.deviceId = value;
            }
        }

        public UserViewModel User
        {
            get
            {
                return this.user;
            }
            set
            {
                this.user = value;
            }
        }

        public bool Successful
        {
            get { return successful; }
        }
    }
}
