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

namespace StarSightings.Events
{
    public class SearchEventArgs : EventArgs
    {
        private readonly bool successful = false;
        private ObservableCollection<ItemViewModel> items;
        private SearchToken searchToken;
        
        public SearchEventArgs(bool successful)
        {
            this.successful = successful;
        }
       
        public SearchToken SearchToken
        {
            get
            {
                return this.searchToken;
            }
            set
            {
                this.searchToken = value;
            }
        }

        public ObservableCollection<ItemViewModel> Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        public bool Successful
        {
            get { return successful; }
        }

        
    }
}
