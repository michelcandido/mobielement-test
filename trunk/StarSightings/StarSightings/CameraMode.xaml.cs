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
using System.Windows.Navigation;
using ExifLib;

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
            App.ViewModel.SelectedImage = null;
            cameraCaptureTask = new CameraCaptureTask();
            cameraCaptureTask.Completed += new EventHandler<PhotoResult>(cameraCaptureTask_Completed);

            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
        }

        private void TakePictureButton_Click(object sender, RoutedEventArgs e)
        {
            App.ViewModel.PostMode = 1;
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
            App.ViewModel.PostMode = 1;
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
            
            if (e.TaskResult == TaskResult.OK)
            {
                JpegInfo info = ExifLib.ExifReader.ReadJpeg(e.ChosenPhoto, e.OriginalFileName);
                BitmapImage image = new BitmapImage();
                image.SetSource(e.ChosenPhoto);
                App.ViewModel.SelectedImage = image;
                App.ViewModel.WriteableSelectedBitmap = new WriteableBitmap(image);

                pictureToShow1.Source = App.ViewModel.SelectedImage;
                pictureToShow2.Source = App.ViewModel.SelectedImage;
                ContentPanel.Visibility = Visibility.Collapsed;
                ContentPanelChooser.Visibility = Visibility.Visible;
                ContentPanelScoop.Visibility = Visibility.Collapsed;
                this.ApplicationBar.IsVisible = true;
                
                getPicDateTime(info);
            }
            
        }

        
        void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {

                App.ViewModel.StoryTime = DateTime.Now;
                App.ViewModel.StoryLat = App.ViewModel.MyLocation.Latitude;
                App.ViewModel.StoryLng = App.ViewModel.MyLocation.Longitude;

                string fileName = "StarSightings_" + App.ViewModel.StoryTime.ToString() + ".jpg";

                Picture pic = library.SavePictureToCameraRoll(fileName, e.ChosenPhoto);
                
                BitmapImage image = new BitmapImage();                
                image.SetSource(e.ChosenPhoto);
                App.ViewModel.SelectedImage = image;
                App.ViewModel.WriteableSelectedBitmap = new WriteableBitmap(image);                
                
                /*
                e.ChosenPhoto.Position = 0;
                JpegInfo info = ExifReader.ReadJpeg(e.ChosenPhoto, e.OriginalFileName);

                switch (info.Orientation)
                {

                    case ExifOrientation.TopLeft:

                    case ExifOrientation.Undefined:
                        ImageRotate.Angle = 0d;
                        break;

                    case ExifOrientation.TopRight:
                        ImageRotate.Angle = 90d;
                        break;

                    case ExifOrientation.BottomRight:
                        ImageRotate.Angle = 180d;
                        break;

                    case ExifOrientation.BottomLeft:
                        ImageRotate.Angle = 270d;
                        break;
                }

                */
                // Save the image to the camera roll album.                
                
               
                pictureToShow1.Source = App.ViewModel.SelectedImage;
                pictureToShow2.Source = App.ViewModel.SelectedImage;
                ContentPanel.Visibility = Visibility.Collapsed;
                ContentPanelChooser.Visibility = Visibility.Visible;
                ContentPanelScoop.Visibility = Visibility.Collapsed;
                this.ApplicationBar.IsVisible = true;

                                
            }


        }

        private void getPicDateTime(JpegInfo info)
        {
            if (!string.IsNullOrEmpty(info.DateTime) && !string.IsNullOrWhiteSpace(info.DateTime))
            {
                string[] dt = info.DateTime.Split(new char[]{' '});
                string[] dates = dt[0].Split(new char[]{':'});
                string[] times = dt[1].Split(new char[]{':'});
                App.ViewModel.StoryTime = new DateTime(int.Parse(dates[0]), int.Parse(dates[1]), int.Parse(dates[2]),int.Parse(times[0]), int.Parse(times[1]), int.Parse(times[2]));                
            }
            App.ViewModel.StoryLat = info.GpsLatitude[0] + info.GpsLatitude[1] * 1 / 60.0 + info.GpsLatitude[2] * 1 / 3600.0;
            App.ViewModel.StoryLng = info.GpsLongitude[0] + info.GpsLongitude[1] * 1 / 60.0 + info.GpsLongitude[2] * 1 / 3600.0;

            if (info.GpsLatitudeRef.ToString() == "South")
                App.ViewModel.StoryLat = -App.ViewModel.StoryLat;
            if (info.GpsLongitudeRef.ToString() == "West")
                App.ViewModel.StoryLng = -App.ViewModel.StoryLng;
            if (info.GpsLatitudeRef.ToString() == "Unknown" || info.GpsLongitudeRef.ToString() == "Unknown")
            {
                App.ViewModel.StoryLat = 0;
                App.ViewModel.StoryLng = 0;
            }
            /*
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show("lat:" + info.GpsLatitude[0] + "///" + info.GpsLatitude[1] + "///" + info.GpsLatitude[2] + "///" + "lng:" + info.GpsLongitude[0] + "///" + info.GpsLongitude[1] + "///" + info.GpsLongitude[2] +"slat:"+App.ViewModel.StoryLat+"///"+"slng:"+App.ViewModel.StoryLng);
            });
             * */
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();// .Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void noPictureButton_Click(object sender, RoutedEventArgs e)
        {
            App.ViewModel.PostMode = 2;
            BitmapImage image = new BitmapImage(new Uri("/Images/no-photo.png", UriKind.RelativeOrAbsolute));            
            App.ViewModel.SelectedImage = image;
            //App.ViewModel.WriteableSelectedBitmap = new WriteableBitmap(image);
            this.NavigationService.Navigate(new Uri("/Scoop.xaml", UriKind.RelativeOrAbsolute));
            //this.NavigationService.Navigate(new Uri("/WhoDidUSee.xaml", UriKind.RelativeOrAbsolute));
        }        

        private void OnDetailsTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/WhoDidUSee.xaml", UriKind.RelativeOrAbsolute));
        }

        

        private void OnAcceptClick(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Scoop.xaml", UriKind.RelativeOrAbsolute));
        }
       

        private void OnCancelClick(object sender, EventArgs e)
        {
            ContentPanel.Visibility = Visibility.Visible;
            ContentPanelChooser.Visibility = Visibility.Collapsed;
            ContentPanelScoop.Visibility = Visibility.Collapsed;
            this.ApplicationBar.IsVisible = false;
        }

    }
}