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
        private string[] categoryFilterNames = new string[] { "All", "Celebrities", "Musicians", "Politicians", "Models", "Athletes" };
        private string[] mapFilterNames = new string[] { "Near Me", "Near Map Center", "Expand" };
        private string[] followFilterNames = new string[] { "New", "All", "Photographers", "Friends" };
        private int searchGroupId;
        public Filter()
        {
            InitializeComponent();                                    
            this.selectorCategory.DataSource = new ListLoopingDataSource<string>() { Items = categoryFilterNames, SelectedItem = "All" };
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
                        this.selectorCategory.DataSource = new ListLoopingDataSource<string>() { Items = categoryFilterNames, SelectedItem = categoryFilterNames[App.ViewModel.SearchTypePopular] };
                        break;
                    case 1:
                        this.selectorCategory.DataSource = new ListLoopingDataSource<string>() { Items = categoryFilterNames, SelectedItem = categoryFilterNames[App.ViewModel.SearchTypeLatest] };
                        break;
                    case 2:
                        this.selectorCategory.DataSource = new ListLoopingDataSource<string>() { Items = mapFilterNames, SelectedItem = mapFilterNames[App.ViewModel.SearchTypeNearest] };
                        break;
                    case 3:
                        this.selectorCategory.DataSource = new ListLoopingDataSource<string>() { Items = followFilterNames, SelectedItem = followFilterNames[App.ViewModel.SearchTypeFollowing] };
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
            switch (searchGroupId)
            {
                case 0:
                    App.ViewModel.SearchTypePopular = Array.IndexOf(this.categoryFilterNames, (string)this.selectorCategory.DataSource.SelectedItem);
                    App.ViewModel.SearchPopular(true, 0, null);                    
                    break;
                case 1:
                    App.ViewModel.SearchTypeLatest = Array.IndexOf(this.categoryFilterNames, (string)this.selectorCategory.DataSource.SelectedItem);
                    App.ViewModel.SearchLatest(true, 0, null);
                    break;
                case 2:
                    App.ViewModel.SearchTypeNearest = Array.IndexOf(this.mapFilterNames, (string)this.selectorCategory.DataSource.SelectedItem);
                    break;
                case 3:
                    App.ViewModel.SearchTypeFollowing = Array.IndexOf(this.followFilterNames, (string)this.selectorCategory.DataSource.SelectedItem);
                    break;
            }
            this.NavigationService.GoBack();
        }
    }
}