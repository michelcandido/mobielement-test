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
            
            //ContentPanel.Children.Clear();
            BitmapImage image = new BitmapImage();
            image.SetSource(e.ChosenPhoto);
            App.ViewModel.SelectedImage = image;

            pictureToShow1.Source = App.ViewModel.SelectedImage;
            pictureToShow2.Source = App.ViewModel.SelectedImage;
            ContentPanel.Visibility = Visibility.Collapsed;
            ContentPanelChooser.Visibility = Visibility.Visible;
            ContentPanelScoop.Visibility = Visibility.Collapsed;
            /*
            Image selectedImage = new Image();
            selectedImage.Height = 800;
            selectedImage.Width = 480;
            selectedImage.HorizontalAlignment = HorizontalAlignment.Center;
            selectedImage.Stretch = Stretch.Uniform;
            selectedImage.VerticalAlignment = VerticalAlignment.Center;
            */

            /*selectedImage.Source = image;
            ContentPanel.Children.Add(selectedImage);
            */
            
            /*
            Button DoneButton = new Button();
            DoneButton.Height = 72;
            DoneButton.Width = 160;
            DoneButton.Margin = new Thickness(50,600,0,0);
            DoneButton.VerticalAlignment = VerticalAlignment.Top;
            DoneButton.HorizontalAlignment = HorizontalAlignment.Left;
            //DoneButton.Foreground = new SolidColorBrush(Colors.Green);
            DoneButton.Content = "accept";
            DoneButton.Click += new RoutedEventHandler(onDoneButtonClick);

            Button SAButton = new Button();
            SAButton.Height = 72;
            SAButton.Width = 220;
            SAButton.Margin = new Thickness(226,600,0,0);
            SAButton.VerticalAlignment = VerticalAlignment.Top;
            SAButton.HorizontalAlignment = HorizontalAlignment.Left;
            //SAButton.Foreground = new SolidColorBrush(Colors.Green);
            SAButton.Content = "retake";
            SAButton.Click += new RoutedEventHandler(onSAButtonClick);

            ContentPanel.Children.Add(SAButton);
            ContentPanel.Children.Add(DoneButton);
            */
           
            //this.NavigationService.Navigate(new Uri("/ConfirmPictureSelected.xaml", UriKind.RelativeOrAbsolute));
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
        /*
        void onSAButtonClick(object sender, RoutedEventArgs e)
        {
            SelectPictureButton_Click(sender, e);
        }

        void onDoneButtonClick(object sender, RoutedEventArgs e)
        {
            //Goto scoop page; The image is in MainViewModel
            this.NavigationService.Navigate(new Uri("/Scoop.xaml", UriKind.RelativeOrAbsolute));
        }*/

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
            }


        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();// .Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void noPictureButton_Click(object sender, RoutedEventArgs e)
        {
            //this.NavigationService.Navigate(new Uri("/MySightings.xaml", UriKind.RelativeOrAbsolute));
            //Trying the webserver :)
            //App.SSAPI.TestSuggestion();
//            System.Windows.MessageBox.Show("No picture taken. Con)
            //this.NavigationService.Navigate(new Uri("/RadControlsWhereInput.xaml", UriKind.RelativeOrAbsolute));
        }

        /*
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

 
                if (App.ViewModel.fromChooserTask)
                {
                    //Goto scoop page; The image is in MainViewModel
                    App.ViewModel.fromChooserTask = false;
                    this.NavigationService.Navigate(new Uri("/ConfirmPictureSelected.xaml", UriKind.RelativeOrAbsolute));
                }

                if (App.ViewModel.fromCameraTask)
                {
                    //Goto scoop page; The image is in MainViewModel
                    App.ViewModel.fromCameraTask = false;
                    this.NavigationService.Navigate(new Uri("/Scoop.xaml", UriKind.RelativeOrAbsolute));
                }
        } */
          
            /*
            if (this.NavigationContext.QueryString.ContainsKey("ProductId"))
            {
                productID = this.NavigationContext.QueryString["ProductId"];
            }
            else
            {
                productID = App.Current.Resources["FeaturedProductID"].ToString();
            }*/


        private void OnDetailsTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/WhoDidUSee.xaml", UriKind.RelativeOrAbsolute));
        }

        private void OnAcceptTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Scoop.xaml", UriKind.RelativeOrAbsolute));
        }

        private void OnRetakeTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SelectPictureButton_Click(sender, e);
        }

    }
}