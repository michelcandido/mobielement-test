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
                commentHandler = new CommentEventHandler(CommentCompleted);
                App.SSAPI.CommentHandler += commentHandler;
                App.SSAPI.NewComment(this.tbComment.Text.Trim());
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
                MessageBox.Show("Failed in posting new comment.");
            }
        }
    }
}