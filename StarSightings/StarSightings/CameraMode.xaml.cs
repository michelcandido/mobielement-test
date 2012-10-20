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
using System.IO;
using Microsoft.Phone;
using Microsoft.Xna.Framework.Media;
using System.Windows.Resources;
using System.Windows.Media.Imaging;

namespace StarSightings
{
    public partial class CameraMode : PhoneApplicationPage
    {
        // Variables
        private int savedCounter = 0;
        PhotoChooserTask photoChooserTask;
        CameraCaptureTask cameraCaptureTask;
        MediaLibrary library = new MediaLibrary();

        public CameraMode()
        {
            InitializeComponent();
            cameraCaptureTask = new CameraCaptureTask();
            cameraCaptureTask.Completed += new EventHandler<PhotoResult>(cameraCaptureTask_Completed);

            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
        }

        private void TakePictureButton_Click(object sender, RoutedEventArgs e)
        {
            // this.NavigationService.Navigate(new Uri("/CameraPage.xaml", UriKind.RelativeOrAbsolute));
            try
            {
                cameraCaptureTask.Show();
            }
            catch (System.InvalidOperationException ex)
            {
                MessageBox.Show("Error opening camera.");
            }
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
                MessageBox.Show("Error opening picture selection.");
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
            //this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));

            ContentPanel.Children.Clear();

            Image selectedImage = new Image();
            selectedImage.Height = 800;
            selectedImage.Width = 480;
            selectedImage.HorizontalAlignment = HorizontalAlignment.Center;
            selectedImage.Stretch = Stretch.Uniform;
            selectedImage.VerticalAlignment = VerticalAlignment.Center;
            
            BitmapImage image = new BitmapImage();
            image.SetSource(e.ChosenPhoto);
            selectedImage.Source = image;
            ContentPanel.Children.Add(selectedImage);

            Button DoneButton = new Button();
            DoneButton.Height = 72;
            DoneButton.Width = 160;
            DoneButton.Margin = new Thickness(50,600,0,0);
            DoneButton.VerticalAlignment = VerticalAlignment.Top;
            DoneButton.HorizontalAlignment = HorizontalAlignment.Left;
            //DoneButton.Foreground = new SolidColorBrush(Colors.Green);
            DoneButton.Content = "Done";
            DoneButton.Click += new RoutedEventHandler(onDoneButtonClick);

            Button SAButton = new Button();
            SAButton.Height = 72;
            SAButton.Width = 220;
            SAButton.Margin = new Thickness(226,600,0,0);
            SAButton.VerticalAlignment = VerticalAlignment.Top;
            SAButton.HorizontalAlignment = HorizontalAlignment.Left;
            //SAButton.Foreground = new SolidColorBrush(Colors.Green);
            SAButton.Content = "Select Another";
            SAButton.Click += new RoutedEventHandler(onSAButtonClick);

            ContentPanel.Children.Add(SAButton);
            ContentPanel.Children.Add(DoneButton);
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

        void onSAButtonClick(object sender, RoutedEventArgs e)
        {
            SelectPictureButton_Click(sender, e);
        }

        void onDoneButtonClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MySightings.xaml", UriKind.RelativeOrAbsolute));
        }

        void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                savedCounter++;
                string fileName = "StarSightings_" + savedCounter + ".jpg";

                // Save the image to the camera roll album.
                Picture pic = library.SavePictureToCameraRoll(fileName, e.ChosenPhoto);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void noPictureButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MySightings.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}