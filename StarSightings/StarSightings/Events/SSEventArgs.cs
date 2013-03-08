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
    public class SSEventArgs : EventArgs
    {
        private readonly bool successful = false;

        public SSEventArgs(bool successful)
        {
            this.successful = successful;
        }
        
        public bool Successful
        {
            get { return successful; }
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
    }
}
