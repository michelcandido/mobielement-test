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
using System.Collections.ObjectModel;

namespace StarSightings.Events
{
    public class LoginEventArgs : EventArgs
    {
        private readonly bool successful = false;

        public LoginEventArgs(bool successful)
        {
            this.successful = successful;
        }
        
        public bool Successful
        {
            get { return successful; }
        }
        
        private UserViewModel user;
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

        private int accountType;
        public int AccountType
        {
            get
            {
                return this.accountType;
            }
            set
            {
                this.accountType = value;
            }
        }
        /*
        private ObservableCollection<AlertViewModel> alerts;
        public ObservableCollection<AlertViewModel> Alerts
        {
            get
            {
                return this.alerts;
            }
            set
            {
                this.alerts = value;
            }
        }
         * */
    }
}
