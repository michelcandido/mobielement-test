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
using Microsoft.Phone.Shell;

namespace StarSightings
{
    public partial class Story : PhoneApplicationPage
    {
        private ApplicationBarIconButton btnBack, btnNext;
        private bool edit;
        private string editStory;

        public Story()
        {
            InitializeComponent();
            DataContext = App.ViewModel;

            btnBack = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            btnNext = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
        }        

        private void OnBackClick(object sender, EventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void OnNextClick(object sender, EventArgs e)
        {
            App.ViewModel.PicStory = storyBox.Text;
            if (edit)
                this.NavigationService.GoBack();
            else
                this.NavigationService.Navigate(new Uri("/SightingDetail.xaml", UriKind.RelativeOrAbsolute));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            storyBox.Focus();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
            {
                if (NavigationContext.QueryString.ContainsKey("edit"))
                {
                    edit = true;
                    editStory = App.ViewModel.PicStory;
                }

                if (edit)
                {
                    btnBack.IconUri = new Uri(Constants.ICON_URI_CANCEL, UriKind.RelativeOrAbsolute);
                    btnNext.IconUri = new Uri(Constants.ICON_URI_CONFIRM, UriKind.RelativeOrAbsolute);

                }
                else
                {
                    btnBack.IconUri = new Uri(Constants.ICON_URI_BACK, UriKind.RelativeOrAbsolute);
                    btnNext.IconUri = new Uri(Constants.ICON_URI_NEXT, UriKind.RelativeOrAbsolute);
                }

            }
        }        
    }
}