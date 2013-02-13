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
    public partial class DatePickerPage : PhoneApplicationPage
    {
        public DatePickerPage()
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
            DateTime value = this.datePicker.SelectedValue;           
            this.NavigationService.Navigate(new Uri(string.Format("/TimePickerPage.xaml?date={0}",value.ToShortDateString()), UriKind.RelativeOrAbsolute));
            
        }
    }
}