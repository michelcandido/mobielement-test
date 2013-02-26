using System;
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
using StarSightings.Events;

namespace StarSightings
{
    public partial class CommentInputPage : PhoneApplicationPage
    {
        private CommentEventHandler commentHandler;
        public CommentInputPage()
        {
            InitializeComponent();
        }        

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.tbComment.Text))
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    commentHandler = new CommentEventHandler(CommentCompleted);
                    App.SSAPI.CommentHandler += commentHandler;
                    App.SSAPI.NewComment(this.tbComment.Text.Trim());
                });                   
            }
        }

         public void CommentCompleted(object sender, CommentEventArgs e)
        {
            App.SSAPI.CommentHandler -= commentHandler;
            if (e.Successful)
            {                
                //App.ViewModel.SelectedItem = e.Item;
                this.NavigationService.GoBack();
            }
            else
            {
                if (!string.IsNullOrEmpty(e.ErrorCode))
                {
                    if (e.ErrorCode == Constants.ERROR_COMMENT_LIMIT)
                    {
                        MessageBox.Show("You has reached your comment limit.");
                    }
                    else if (e.ErrorCode == Constants.ERROR_COMMENT_DENIED)
                    {
                        MessageBox.Show("Your commenting request is denied.");
                    }
                    else
                    {
                        MessageBox.Show("Errors in commenting, please try again.");
                    }
                }
                else
                {
                    MessageBox.Show("Errors in commenting, please try again.");
                }
            }
        }

         private void OnPost(object sender, EventArgs e)
         {
             if (!string.IsNullOrEmpty(this.tbComment.Text))
             {
                 Deployment.Current.Dispatcher.BeginInvoke(() =>
                 {
                     commentHandler = new CommentEventHandler(CommentCompleted);
                     App.SSAPI.CommentHandler += commentHandler;
                     App.SSAPI.NewComment(this.tbComment.Text.Trim());
                 });
             }

         }

         private void OnCancel(object sender, EventArgs e)
         {
             this.NavigationService.GoBack();
         }         

         private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
         {
             tbComment.Focus();
         }

         private void GoHome(object sender, System.Windows.Input.GestureEventArgs e)
         {
             this.NavigationService.Navigate(new Uri("/MainPage.xaml?clear", UriKind.RelativeOrAbsolute));
         }
    }
}