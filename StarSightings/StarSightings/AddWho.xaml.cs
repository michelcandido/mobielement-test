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
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;

namespace StarSightings
{
    public partial class AddWho : PhoneApplicationPage
    {
        private ApplicationBarIconButton btnBack, btnNext;
        private string editName;
        private bool edit;
        private ObservableCollection<String> celebNameList;

        public AddWho()
        {            
            InitializeComponent();
            DataContext = App.ViewModel;

            btnBack = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            btnNext = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
        }

        private void OnBackClick(object sender, EventArgs e)
        {
            // discard changes;
            if (edit)
            {
                if (!string.IsNullOrEmpty(editName))
                {
                    celebNameList.Add(editName);
                    celebNameList.Remove(App.ViewModel.CelebName);
                    App.ViewModel.CelebName = editName;
                }
                App.ViewModel.CelebNameList = celebNameList;
            }
           this.NavigationService.GoBack();            
        }

        private void OnNextClick(object sender, EventArgs e)
        {
            if (edit)
            {
                this.NavigationService.GoBack();
            }
            else
            {
                this.NavigationService.Navigate(new Uri("/Location.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        private void OnAddTap(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbName.Text) && !App.ViewModel.CelebNameList.Contains(tbName.Text))
            {
                App.ViewModel.CelebNameList.Add(tbName.Text);
                btnNext.IsEnabled = true;  
            }
        }
        

        private void OnTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ViewModel.CelebNameList.RemoveAt((sender as ListBox).SelectedIndex);
            if (App.ViewModel.CelebNameList.Count <= 0)
                btnNext.IsEnabled = false;  
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
            {
                if (NavigationContext.QueryString.ContainsKey("edit"))
                {
                    edit = true;
                    // keep an origional copy
                    celebNameList = new ObservableCollection<String>();
                    foreach (string c in App.ViewModel.CelebNameList)
                    {
                        celebNameList.Add(c);
                    }
                    
                    if (NavigationContext.QueryString.ContainsKey("name"))
                    {
                        editName = NavigationContext.QueryString["name"];// keep an origional copy                        
                        this.NavigationService.RemoveBackEntry(); // we don't need to go back to WhoDidUSee, but SightingDetail
                    }
                }
                
                if (edit)
                {
                    btnBack.IconUri = new Uri(Constants.ICON_URI_CANCEL,UriKind.RelativeOrAbsolute);
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