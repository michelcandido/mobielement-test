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

namespace StarSightings
{
    public partial class Summary : PhoneApplicationPage
    {
        //private PostEventHandler postHandler;

        public Summary()
        {
            InitializeComponent();
        }

        private void OnCancelTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/CameraMode.xaml", UriKind.RelativeOrAbsolute));
        }
        
        private void OnPostTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
//            System.Windows.MessageBox.Show((sender as ListBox).SelectedIndex.ToString());
            /*if (!string.IsNullOrEmpty(App.ViewModel.CelebNameList.ElementAt[0]))
            {
                postHandler = new PostEventHandler(PostCompleted);
                App.SSAPI.PostHandler += postHandler;
                App.SSAPI.NewComment(this.tbComment.Text.Trim());
            }*/
        }
        /*
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
       */
    }
}