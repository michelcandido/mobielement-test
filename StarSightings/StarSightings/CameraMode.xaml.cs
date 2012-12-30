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
            
            if (e.TaskResult == TaskResult.OK)
            {
                BitmapImage image = new BitmapImage();
                image.SetSource(e.ChosenPhoto);
                App.ViewModel.SelectedImage = image;

                pictureToShow1.Source = App.ViewModel.SelectedImage;
                pictureToShow2.Source = App.ViewModel.SelectedImage;
                ContentPanel.Visibility = Visibility.Collapsed;
                ContentPanelChooser.Visibility = Visibility.Visible;
                ContentPanelScoop.Visibility = Visibility.Collapsed;
                this.ApplicationBar.IsVisible = true;
            }
            
        }

        
        void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
 
                String timestamp = DateTime.Now.ToString();
                string fileName = "StarSightings_" + timestamp + ".jpg";

                // Save the image to the camera roll album.
                Picture pic = library.SavePictureToCameraRoll(fileName, e.ChosenPhoto);

                BitmapImage image = new BitmapImage();
                image.SetSource(e.ChosenPhoto);
                App.ViewModel.SelectedImage = image;

                pictureToShow1.Source = App.ViewModel.SelectedImage;
                pictureToShow2.Source = App.ViewModel.SelectedImage;
                ContentPanel.Visibility = Visibility.Collapsed;
                ContentPanelChooser.Visibility = Visibility.Collapsed;
                ContentPanelScoop.Visibility = Visibility.Visible;
                ApplicationTitle.Visibility = Visibility.Visible;
            }


        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();// .Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void noPictureButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/WhoDidUSee.xaml", UriKind.RelativeOrAbsolute));
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
            SelectPictureButton_Click(sender, null);
        }

    }
}