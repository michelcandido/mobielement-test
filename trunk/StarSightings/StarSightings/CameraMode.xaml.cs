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
    public partial class CameraMode : PhoneApplicationPage
    {
        PhotoChooserTask photoChooserTask;

        public CameraMode()
        {
            InitializeComponent();
            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
        }

        private void TakePictureButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/CameraPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void SelectPictureButton_Click(object sender, RoutedEventArgs e)
        {
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
            //var frame = Application.Current.RootVisual as PhoneApplicationFrame;
            //var startPage = frame.Content as PhoneApplicationPage;

            /*
            TaskEventArgs<PhotoResult> photo = e as TaskEventArgs<PhotoResult>;
            if (photo != null && photo.Result != null && photo.Result.ChosenPhoto != null)
            {
                BitmapImage image = new BitmapImage();
                image.SetSource(photo.Result.ChosenPhoto);

                SelectedPicturePage.
                imgPhoto.Source = image;
            }*/

            /*
            base.Completed(sender, e);
            TaskEventArgs<PhotoResult> photo = e as TaskEventArgs<PhotoResult>;
            if (photo != null && photo.Result != null && photo.Result.ChosenPhoto != null)
            {
                BitmapImage image = new BitmapImage();
                image.SetSource(photo.Result.ChosenPhoto);
                imgPhoto.Source = image;
            }*/
         /*   if (e.TaskResult == TaskResult.OK)
            {
                MessageBox.Show(e.ChosenPhoto.Length.ToString());

                //Code to display the photo on the page in an image control named myImage.
                //System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                //bmp.SetSource(e.ChosenPhoto);
                //myImage.Source = bmp;
            }*/
            //this.NavigationService.Navigate(new Uri("/SelectedPicturePage.xaml", UriKind.RelativeOrAbsolute));
        }
        /*
        public override void OnChooserReturn(object sender, EventArgs e)
        {
            base.OnChooserReturn(sender, e);
            TaskEventArgs<PhotoResult> photo = e as TaskEventArgs<PhotoResult>;
            if (photo != null && photo.Result != null && photo.Result.ChosenPhoto != null)
            {
                BitmapImage image = new BitmapImage();
                image.SetSource(photo.Result.ChosenPhoto);
                imgPhoto.Source = image;
            }
        }*/

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}