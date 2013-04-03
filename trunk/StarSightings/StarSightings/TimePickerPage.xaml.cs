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
    public partial class TimePickerPage : PhoneApplicationPage
    {
        private ApplicationBarIconButton btnBack, btnNext;
        private string editDate;
        private bool edit;

        public TimePickerPage()
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
            DateTime value = this.timePicker.SelectedValue;
            App.ViewModel.StoryTime = storyDateTime.Add(value.TimeOfDay).ToUniversalTime();
            if (edit)
                this.NavigationService.GoBack();
            else
                this.NavigationService.Navigate(new Uri("/WhoDidUSee.xaml", UriKind.RelativeOrAbsolute));
        }

        private DateTime storyDateTime;
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.timePicker.MaxValue = DateTime.Now;

            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
            {
                if (NavigationContext.QueryString.ContainsKey("date"))
                {
                    string dateString = NavigationContext.QueryString["date"];
                    storyDateTime = DateTime.Parse(dateString);
                }

                if (NavigationContext.QueryString.ContainsKey("edit"))
                {
                    edit = true;
                    this.NavigationService.RemoveBackEntry(); // we don't need to go back to WhoDidUSee, but SightingDetail
                    this.timePicker.SelectedValue = storyDateTime.Add(App.ViewModel.StoryTime.ToLocalTime().TimeOfDay);
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
        private void GoHome(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml?clear", UriKind.RelativeOrAbsolute));
        }       
    }
}