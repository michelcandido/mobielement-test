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
using System.Collections;
using Telerik.Windows.Controls;
using System.ComponentModel;
using System.Threading;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Navigation;

namespace StarSightings
{
    public class ListViewBase : UserControl
    {
        private bool isDisplayed;
        private bool isLoaded;
        private IEnumerable itemsSource;

        public ListViewBase()
        {
            this.Loaded += this.OnLoaded;
            this.Unloaded += this.OnUnloaded;
        }

        private int searchGroup;
        [Category("Data")]
        [DefaultValue(null)]
        [Description("Search Type")]
        public int SearchGroup
        {
            get { return searchGroup; }
            set { searchGroup = value; }
        }

        protected virtual RadDataBoundListBox ListBox
        {
            get
            {
                return null;
            }
        }

        protected virtual bool EnableAsyncBalance
        {
            get
            {
                return true;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.isLoaded = true;
            if (DesignerProperties.IsInDesignTool)
            {
                return;
            }

            if (this.ListBox != null)
            {
                // enable async balance upon listbox loading
                this.ListBox.IsAsyncBalanceEnabled = this.EnableAsyncBalance;
            }

            this.isDisplayed = true;
            this.OnDisplayed();            
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.isLoaded = false;            
        }

        protected virtual void OnDisplayed()
        {
            if (this.ListBox == null)
            {
                return;
            }

            if (this.itemsSource == null)
            {
                ThreadPool.QueueUserWorkItem(this.LoadItemsSource);
            }
            else
            {
                this.DisplayData(this.itemsSource);
            }
        }

        protected virtual void OnHidden()
        {
            if (this.ListBox != null)
            {
                this.Dispatcher.BeginInvoke(() =>
                {
                    this.ListBox.ItemsSource = null;
                });
            }
        }

        private void LoadItemsSource(object state)
        {
            this.itemsSource = this.CreateItemsSource();
            if (this.isDisplayed)
            {
                this.DisplayData(this.itemsSource);
            }
        }

        /// <summary>
        /// Core items source creation method. Note that it is on a worker thread.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable CreateItemsSource()
        {
            return null;
        }

        

        protected virtual void DisplayData(IEnumerable items)
        {
            if (this.ListBox == null || !this.isDisplayed)
            {
                return;
            }

            this.Dispatcher.BeginInvoke(() =>
            {               
                this.ListBox.BeginAsyncBalance();
                this.ListBox.ItemsSource = items;
                this.OnItemsBound();
            });
        }

        /// <summary>
        /// Fired immediately after the source collection has been bound to the list box.
        /// </summary>
        protected virtual void OnItemsBound()
        {
        }

        [Category("Event")]
        [DefaultValue(null)]
        [Description("Event for page navigation")]
        public event EventHandler EventForPageNavigation;
        public void NavigateToDetails(Uri uri)
        {
            var e = new NavigationEventArgs(null, uri);
            if (EventForPageNavigation != null)
                EventForPageNavigation(this, e);
        }
    }
}
