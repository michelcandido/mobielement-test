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
using System.Collections.ObjectModel;
using StarSightings.ViewModels;

namespace StarSightings.Events
{
    public class AlertEventArgs : EventArgs
    {
        private readonly bool successful = false;
        private ObservableCollection<AlertViewModel> alerts;
        
        
        public AlertEventArgs(bool successful)
        {
            this.successful = successful;
        }
       
        
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

        public bool Successful
        {
            get { return successful; }
        }

        
    }
}
