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
using Microsoft.Phone.Tasks;
using StarSightings.Events;

namespace StarSightings
{
    public partial class VoteCommentPage : PhoneApplicationPage
    {
        public VoteCommentPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
        }

        private void htmlTextBlock_NavigationRequested(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();

            try
            {
                webBrowserTask.Uri = new Uri(((e.Content as Hyperlink).CommandParameter as string), UriKind.Absolute);
                webBrowserTask.Show();
            }
            catch (System.Exception)
            {

                //throw;
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
            {
                if (NavigationContext.QueryString.ContainsKey("pivotItem"))
                {
                    string pivotItem = NavigationContext.QueryString["pivotItem"];
                    if (pivotItem == "comment")
                        this.pivotControl.SelectedIndex = 1;
                    else if (pivotItem == "vote")
                        this.pivotControl.SelectedIndex = 0;
                }
            }            
        }

        private void CommentButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/CommentInputPage.xaml", UriKind.RelativeOrAbsolute));
        }
        
        private void Vote_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //string[] input = ((sender as Grid).Tag as string).Split(new Char[] { '#' });
            UIElement marker = (sender as Grid).Children.First(c => (c as Grid).Name == "mark");
            TextBlock value = (((sender as Grid).Children.First() as Grid).Children.First(c => (c as FrameworkElement).Name == "value")) as TextBlock;
            marker.Visibility = marker.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                //App.SSAPI.SetVote(input[0], input[1].Equals("true", StringComparison.CurrentCultureIgnoreCase) ? 0 : 1);
                voteHandler = new CommentEventHandler(VoteCompleted);
                App.SSAPI.CommentHandler += voteHandler;
                markers.Enqueue(marker);
                App.SSAPI.SetVote(value.Text, marker.Visibility == Visibility.Visible ? 1 : 0);
            });            
        }

        private CommentEventHandler voteHandler;
        private Queue<UIElement> markers = new Queue<UIElement>();
        public void VoteCompleted(object sender, CommentEventArgs e)
        {
            App.SSAPI.CommentHandler -= voteHandler;
            if (e.Successful)
            {
                markers.Dequeue();
            }
            else
            {
                UIElement marker = markers.Dequeue();
                marker.Visibility = marker.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                MessageBox.Show("You can only vote at most 3 times.");
            }
        }
    }
}