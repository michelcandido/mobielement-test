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

namespace StarSightings.Events
{
    public class RegisterEventArgs : EventArgs
    {
        private readonly bool successful = false;
        private string deviceId;

        public RegisterEventArgs(bool successful)
        {
            this.successful = successful;
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

        public bool Successful
        {
            get { return successful; }
        }
    }
}
