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
    public class VoteViewModel : INotifyPropertyChanged
    {
        
        private string voteValue;
        public string VoteValue
        {
            get
            {
                return this.voteValue;
            }
            set
            {
                if (value != voteValue)
                {
                    voteValue = value;
                    NotifyPropertyChanged("VoteValue");
                }
            }
        }

        private string count;
        public string Count
        {
            get
            {
                return this.count;
            }
            set
            {
                if (value != count)
                {
                    count = value;
                    NotifyPropertyChanged("Count");
                }
            }
        }

        private Boolean selected;
        public Boolean Selected
        {
            get
            {
                return this.selected;
            }
            set
            {
                if (value != selected)
                {
                    selected = value;
                    NotifyPropertyChanged("Selected");
                }
            }
        }

        private string imageFilename;
        public string ImageFilename
        {
            get
            {
                return this.imageFilename;
            }
            set
            {
                if (value != imageFilename)
                {
                    imageFilename = value;
                    NotifyPropertyChanged("ImageFilename");
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
