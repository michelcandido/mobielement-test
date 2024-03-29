﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Primitives;

namespace StarSightings
{
    public partial class Filter : PhoneApplicationPage
    {
        //private string[] categoryFilterNames = new string[] { "All", "Celebrities", "Musicians", "Politicians", "Models", "Athletes" };
        //private string[] mapFilterNames = new string[] { "Near Me", "Near Map Center", "Expand" };
        //private string[] followFilterNames = new string[] { "New", "All", "Photographers", "Friends" };
        private int searchGroupId;
        public Filter()
        {
            InitializeComponent();
            this.selectorCategory.DataSource = new ListLoopingDataSource<string>() { Items = Constants.categoryFilterNames, SelectedItem = "All" };
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (NavigationContext.QueryString.ContainsKey("selectedGroupId"))
            {
                string selectedGroupId = NavigationContext.QueryString["selectedGroupId"];
                int itemId = 0;
                int.TryParse(selectedGroupId, out itemId);

                switch (itemId)
                {
                    case 0:
                        this.selectorCategory.DataSource = new ListLoopingDataSource<string>() { Items = Constants.categoryFilterNames, SelectedItem = Constants.categoryFilterNames[App.ViewModel.SearchTypePopular] };
                        break;
                    case 1:
                        this.selectorCategory.DataSource = new ListLoopingDataSource<string>() { Items = Constants.categoryFilterNames, SelectedItem = Constants.categoryFilterNames[App.ViewModel.SearchTypeLatest] };
                        break;
                    case 2:
                        this.selectorCategory.DataSource = new ListLoopingDataSource<string>() { Items = Constants.mapFilterNames, SelectedItem = Constants.mapFilterNames[App.ViewModel.SearchTypeNearest] };
                        break;
                    case 3:
                        this.selectorCategory.DataSource = new ListLoopingDataSource<string>() { Items = Constants.followFilterNames, SelectedItem = Constants.followFilterNames[App.ViewModel.SearchTypeFollowing] };
                        break;
                    case 4:
                        this.selectorCategory.DataSource = new ListLoopingDataSource<string>() { Items = Constants.categoryFilterNames, SelectedItem = Constants.categoryFilterNames[App.ViewModel.SearchTypeKeyword] };
                        break;
                }
                searchGroupId = itemId;
            }
        }

        // abstract the reusable code in a base class
        // this will allow us to concentrate on the specifics when implementing deriving looping data source classes
        public abstract class LoopingDataSourceBase : ILoopingSelectorDataSource
        {
            private object selectedItem;

            #region ILoopingSelectorDataSource Members

            public abstract object GetNext(object relativeTo);

            public abstract object GetPrevious(object relativeTo);

            public object SelectedItem
            {
                get
                {
                    return this.selectedItem;
                }
                set
                {
                    // this will use the Equals method if it is overridden for the data source item class
                    if (!object.Equals(this.selectedItem, value))
                    {
                        // save the previously selected item so that we can use it 
                        // to construct the event arguments for the SelectionChanged event
                        object previousSelectedItem = this.selectedItem;
                        this.selectedItem = value;
                        // fire the SelectionChanged event
                        this.OnSelectionChanged(previousSelectedItem, this.selectedItem);
                    }
                }
            }

            public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

            protected virtual void OnSelectionChanged(object oldSelectedItem, object newSelectedItem)
            {
                EventHandler<SelectionChangedEventArgs> handler = this.SelectionChanged;
                if (handler != null)
                {
                    handler(this, new SelectionChangedEventArgs(new object[] { oldSelectedItem }, new object[] { newSelectedItem }));
                }
            }

            #endregion
        }

        public class ListLoopingDataSource<T> : LoopingDataSourceBase
        {
            private LinkedList<T> linkedList;
            private List<LinkedListNode<T>> sortedList;
            private IComparer<T> comparer;
            private NodeComparer nodeComparer;

            public ListLoopingDataSource()
            {
            }

            public IEnumerable<T> Items
            {
                get
                {
                    return this.linkedList;
                }
                set
                {
                    this.SetItemCollection(value);
                }
            }

            private void SetItemCollection(IEnumerable<T> collection)
            {
                this.linkedList = new LinkedList<T>(collection);

                this.sortedList = new List<LinkedListNode<T>>(this.linkedList.Count);
                // initialize the linked list with items from the collections
                LinkedListNode<T> currentNode = this.linkedList.First;
                while (currentNode != null)
                {
                    this.sortedList.Add(currentNode);
                    currentNode = currentNode.Next;
                }

                IComparer<T> comparer = this.comparer;
                if (comparer == null)
                {
                    // if no comparer is set use the default one if available
                    if (typeof(IComparable<T>).IsAssignableFrom(typeof(T)))
                    {
                        comparer = Comparer<T>.Default;
                    }
                    else
                    {
                        throw new InvalidOperationException("There is no default comparer for this type of item. You must set one.");
                    }
                }

                this.nodeComparer = new NodeComparer(comparer);
                this.sortedList.Sort(this.nodeComparer);
            }

            public IComparer<T> Comparer
            {
                get
                {
                    return this.comparer;
                }
                set
                {
                    this.comparer = value;
                }
            }

            public override object GetNext(object relativeTo)
            {
                // find the index of the node using binary search in the sorted list
                int index = this.sortedList.BinarySearch(new LinkedListNode<T>((T)relativeTo), this.nodeComparer);
                if (index < 0)
                {
                    return default(T);
                }

                // get the actual node from the linked list using the index
                LinkedListNode<T> node = this.sortedList[index].Next;
                if (node == null)
                {
                    // if there is no next node get the first one
                    node = this.linkedList.First;
                }
                return node.Value;
            }

            public override object GetPrevious(object relativeTo)
            {
                int index = this.sortedList.BinarySearch(new LinkedListNode<T>((T)relativeTo), this.nodeComparer);
                if (index < 0)
                {
                    return default(T);
                }
                LinkedListNode<T> node = this.sortedList[index].Previous;
                if (node == null)
                {
                    // if there is no previous node get the last one
                    node = this.linkedList.Last;
                }
                return node.Value;
            }
           
            private class NodeComparer : IComparer<LinkedListNode<T>>
            {
                private IComparer<T> comparer;

                public NodeComparer(IComparer<T> comparer)
                {
                    this.comparer = comparer;
                }

                #region IComparer<LinkedListNode<T>> Members

                public int Compare(LinkedListNode<T> x, LinkedListNode<T> y)
                {
                    return this.comparer.Compare(x.Value, y.Value);
                }

                #endregion
            }

        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void CheckButton_Click(object sender, EventArgs e)
        {
            int idx;
            switch (searchGroupId)
            {
                case 0:
                    idx = Array.IndexOf(Constants.categoryFilterNames, (string)this.selectorCategory.DataSource.SelectedItem);
                    if (App.ViewModel.SearchTypePopular != idx)
                    {
                        App.ViewModel.SearchTypePopular = idx;
                        App.ViewModel.SearchPopular(true, 0, null);
                    }
                    break;
                case 1:
                    idx = Array.IndexOf(Constants.categoryFilterNames, (string)this.selectorCategory.DataSource.SelectedItem);
                    if (App.ViewModel.SearchTypeLatest != idx)
                    {
                        App.ViewModel.SearchTypeLatest = idx;
                        App.ViewModel.SearchLatest(true, 0, null);
                    }
                    break;
                case 2:
                    idx = Array.IndexOf(Constants.mapFilterNames, (string)this.selectorCategory.DataSource.SelectedItem);
                    if (App.ViewModel.SearchTypeNearest != idx)
                    {
                        App.ViewModel.SearchTypeNearest = idx;
                        switch (idx)
                        {
                            case 0:
                                App.ViewModel.SearchNearest(true, 0, null);
                                break;
                            case 1:
                                SearchParams param = new SearchParams();
                                if (App.ViewModel.MapCenter != null)
                                {
                                    param.search_lat = App.ViewModel.MapCenter.Latitude;
                                    param.search_lng = App.ViewModel.MapCenter.Longitude;
                                    App.ViewModel.SearchNearest(true, 0, param);
                                }
                                else
                                {
                                    App.ViewModel.SearchNearest(true, 0, null);
                                }
                                break;
                        }
                    }
                    break;
                case 3:
                    App.ViewModel.SearchTypeFollowing = Array.IndexOf(Constants.followFilterNames, (string)this.selectorCategory.DataSource.SelectedItem);                    
                    break;
                case 4:
                    idx = Array.IndexOf(Constants.categoryFilterNames, (string)this.selectorCategory.DataSource.SelectedItem);
                    if (App.ViewModel.SearchTypeKeyword != idx)
                    {
                        App.ViewModel.SearchTypeKeyword = idx;
                        App.ViewModel.SearchKeywordSearch(true, 0, null);
                    }
                    break;
            }
            this.NavigationService.GoBack();
        }

        private void GoHome(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml?clear", UriKind.RelativeOrAbsolute));
        }        
    }
}