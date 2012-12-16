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
    public class PlaceSuggestionEventArgs : EventArgs
    {
        private readonly bool successful = false;
        private ObservableCollection<ItemViewModel> items;

        public PlaceSuggestionEventArgs(bool successful)
        {
            this.successful = successful;
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
