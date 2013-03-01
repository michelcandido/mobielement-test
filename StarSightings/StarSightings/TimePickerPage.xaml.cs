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
    public partial class TimePickerPage : PhoneApplicationPage
    {
        public TimePickerPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
        }

        private void OnBackClick(object sender, EventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void OnNextClick(object sender, EventArgs e)
        {
            DateTime value = this.timePicker.SelectedValue;
            App.ViewModel.StoryTime = storyDateTime.Add(value.TimeOfDay).ToUniversalTime();             
            this.NavigationService.Navigate(new Uri("/WhoDidUSee.xaml", UriKind.RelativeOrAbsolute));
        }

        private DateTime storyDateTime;
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (NavigationContext.QueryString.ContainsKey("date"))
            {
                string dateString = NavigationContext.QueryString["date"];
                storyDateTime = DateTime.Parse(dateString);                
            }
        }
    }
}