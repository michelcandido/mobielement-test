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
using Microsoft.Phone.Tasks;

namespace StarSightings
{
    public partial class SelectedPicturePage : PhoneApplicationPage
    {
        PhotoChooserTask photoChooserTask;

        public SelectedPicturePage()
        {
            InitializeComponent();
        }

        private void OnDoneButtonTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void OnSAButtonTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
            photoChooserTask.ShowCamera = false;
            try
            {
                photoChooserTask.Show();
            }
            catch (System.InvalidOperationException ex)
            {
            }
        }

        private void photoChooserTask_Completed(object sender, PhotoResult e)
        {
        }
    }
}