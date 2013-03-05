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
            DataContext = App.ViewModel;
            //App.ViewModel.SelectedImage = null;
            
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
                ContentPanelChooser.Visibility = Visibility.Collapsed;//Visibility.Visible;
                ContentPanelScoop.Visibility = Visibility.Visible;//.Collapsed;
                ApplicationTitle.Visibility = Visibility.Visible;

                //this.ApplicationBar.IsVisible = true;
                
                getPicDateTime(info);
                App.ViewModel.CameraInfo = string.Format("APP {0};PHOTO {1};TIME {2};COORD {3}", System.Reflection.Assembly.GetExecutingAssembly().FullName.Split('=')[1].Split(',')[0], "library", (App.ViewModel.StoryTime.CompareTo(new DateTime(0)) <= 0) ? "user" : "exif", "asset");
                //this.NavigationService.Navigate(new Uri("/Scoop.xaml", UriKind.RelativeOrAbsolute));
            }
            
        }

        private Stream capturedImage;
        int _angle;
        void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {

                App.ViewModel.StoryTime = DateTime.UtcNow;
                App.ViewModel.StoryLat = App.ViewModel.MyLocation.Latitude;
                App.ViewModel.StoryLng = App.ViewModel.MyLocation.Longitude;

                string fileName = "StarSightings_" + App.ViewModel.StoryTime.ToString() + ".jpg";

                Picture pic = library.SavePictureToCameraRoll(fileName, e.ChosenPhoto);

                e.ChosenPhoto.Position = 0;
                JpegInfo info = ExifReader.ReadJpeg(e.ChosenPhoto, e.OriginalFileName);

                switch (info.Orientation)
                {
                    case ExifOrientation.TopLeft:
                    case ExifOrientation.Undefined:
                        _angle = 0;
                        break;
                    case ExifOrientation.TopRight:
                        _angle = 90;
                        break;
                    case ExifOrientation.BottomRight:
                        _angle = 180;
                        break;
                    case ExifOrientation.BottomLeft:
                        _angle = 270;
                        break;
                }

                if (_angle > 0d)
                {
                    capturedImage = RotateStream(e.ChosenPhoto, _angle);
                }
                else
                {
                    capturedImage = e.ChosenPhoto;
                }

                BitmapImage image = new BitmapImage();
                image.SetSource(capturedImage);//(e.ChosenPhoto);


                App.ViewModel.SelectedImage = image;
                App.ViewModel.WriteableSelectedBitmap = new WriteableBitmap(image);                
                                                             
                pictureToShow1.Source = App.ViewModel.SelectedImage;
                pictureToShow2.Source = App.ViewModel.SelectedImage;
                ContentPanel.Visibility = Visibility.Collapsed;
                ContentPanelChooser.Visibility = Visibility.Collapsed;//.Visible;
                ContentPanelScoop.Visibility = Visibility.Visible;//.Collapsed;
                ApplicationTitle.Visibility = Visibility.Visible;

                App.ViewModel.CameraInfo = string.Format("APP {0};PHOTO {1};TIME {2};COORD {3}", System.Reflection.Assembly.GetExecutingAssembly().FullName.Split('=')[1].Split(',')[0], "app", "asset", "app");
                //this.ApplicationBar.IsVisible = true;
                
                //this.NavigationService.Navigate(new Uri("/Scoop.xaml", UriKind.RelativeOrAbsolute));                                
            }


        }

        private Stream RotateStream(Stream stream, int angle)
        {
            stream.Position = 0;
            if (angle % 90 != 0 || angle < 0) throw new ArgumentException();
            if (angle % 360 == 0) return stream;

            BitmapImage bitmap = new BitmapImage();
            bitmap.SetSource(stream);
            WriteableBitmap wbSource = new WriteableBitmap(bitmap);

            WriteableBitmap wbTarget = null;
            if (angle % 180 == 0)
            {
                wbTarget = new WriteableBitmap(wbSource.PixelWidth, wbSource.PixelHeight);
            }
            else
            {
                wbTarget = new WriteableBitmap(wbSource.PixelHeight, wbSource.PixelWidth);
            }

            for (int x = 0; x < wbSource.PixelWidth; x++)
            {
                for (int y = 0; y < wbSource.PixelHeight; y++)
                {
                    switch (angle % 360)
                    {
                        case 90:
                            wbTarget.Pixels[(wbSource.PixelHeight - y - 1) + x * wbTarget.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                        case 180:
                            wbTarget.Pixels[(wbSource.PixelWidth - x - 1) + (wbSource.PixelHeight - y - 1) * wbSource.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                        case 270:
                            wbTarget.Pixels[y + (wbSource.PixelWidth - x - 1) * wbTarget.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                    }
                }
            }
            MemoryStream targetStream = new MemoryStream();
            wbTarget.SaveJpeg(targetStream, wbTarget.PixelWidth, wbTarget.PixelHeight, 0, 100);
            return targetStream;
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
            App.ViewModel.StoryLat = 47.6097;
            App.ViewModel.StoryLng = -122.1879;
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

            App.ViewModel.CameraInfo = string.Format("APP {0};PHOTO {1};TIME {2};COORD {3}", System.Reflection.Assembly.GetExecutingAssembly().FullName.Split('=')[1].Split(',')[0], "none", "user", "nil");
            //this.NavigationService.Navigate(new Uri("/Scoop.xaml", UriKind.RelativeOrAbsolute));
            ContentPanel.Visibility = Visibility.Collapsed;
            ContentPanelChooser.Visibility = Visibility.Collapsed;//Visibility.Visible;
            ContentPanelScoop.Visibility = Visibility.Visible;//.Collapsed;
            ApplicationTitle.Visibility = Visibility.Visible;
        }        

        private void OnDetailsTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            DateTime zero = new DateTime(0);
            if (App.ViewModel.StoryTime.CompareTo(zero) <= 0)
                this.NavigationService.Navigate(new Uri("/DatePickerPage.xaml", UriKind.RelativeOrAbsolute));
            else
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
            //this.ApplicationBar.IsVisible = false;
        }        

        private void GoHome(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml?clear", UriKind.RelativeOrAbsolute));
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            if (this.ApplicationTitle.Visibility == Visibility.Visible)
            { // in scoop
                MessageBoxResult result = MessageBox.Show("This photo is saved on your camera roll.");
                if (result == MessageBoxResult.OK)
                {
                    ContentPanel.Visibility = Visibility.Visible;
                    ContentPanelChooser.Visibility = Visibility.Collapsed;
                    ContentPanelScoop.Visibility = Visibility.Collapsed;
                    ApplicationTitle.Visibility = Visibility.Collapsed;

                    //this.ApplicationBar.IsVisible = false;
                }
            }
            else
            {
                GoHome(sender, null);
            }
        }        
    }
}