﻿using System;
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
        private bool edit = false;
        //private string editDate;

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
            if (edit)
                this.NavigationService.Navigate(new Uri(string.Format("/TimePickerPage.xaml?edit&date={0}", value.ToShortDateString()), UriKind.RelativeOrAbsolute));
            else
                this.NavigationService.Navigate(new Uri(string.Format("/TimePickerPage.xaml?date={0}",value.ToShortDateString()), UriKind.RelativeOrAbsolute));
            
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.datePicker.MaxValue = DateTime.Now;
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
            {
                if (NavigationContext.QueryString.ContainsKey("edit"))
                {
                    edit = true;
                    //editDate = App.ViewModel.StoryTime.ToShortDateString();
                    this.datePicker.SelectedValue = App.ViewModel.StoryTime.ToLocalTime();
                }                
            }
        }

        private void GoHome(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml?clear", UriKind.RelativeOrAbsolute));
        }    
    }
}