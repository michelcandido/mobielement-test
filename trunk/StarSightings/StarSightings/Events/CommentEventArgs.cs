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
    public class CommentEventArgs : EventArgs
    {
        private readonly bool successful = false;
        private ItemViewModel item;
        private ObservableCollection<ItemViewModel> items;
        private string errorCode;

        public CommentEventArgs(bool successful)
        {
            this.successful = successful;
        }

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

        public ItemViewModel Item
        {
            get
            {
                return this.item;
            }
            set
            {
                this.item = value;
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
