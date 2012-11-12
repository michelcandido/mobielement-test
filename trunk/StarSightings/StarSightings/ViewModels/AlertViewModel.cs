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
using System.ComponentModel;

namespace StarSightings.ViewModels
{
    public class AlertViewModel : INotifyPropertyChanged
    {
        
        private string type;
        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                if (value != type)
                {
                    type = value;
                    NotifyPropertyChanged("Type");
                }
            }
        }

        private string name;
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (value != name)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private Boolean active;
        public Boolean Active
        {
            get
            {
                return this.active;
            }
            set
            {
                if (value != active)
                {
                    active = value;
                    NotifyPropertyChanged("Active");
                }
            }
        }

        private string geoLat;
        public string GeoLat
        {
            get
            {
                return this.geoLat;
            }
            set
            {
                if (value != this.geoLat)
                {
                    this.geoLat = value;
                    NotifyPropertyChanged("GeoLat");
                }
            }
        }

        private string geoLng;
        public string GeoLng
        {
            get
            {
                return this.geoLng;
            }
            set
            {
                if (value != this.geoLng)
                {
                    this.geoLng = value;
                    NotifyPropertyChanged("GeoLng");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
