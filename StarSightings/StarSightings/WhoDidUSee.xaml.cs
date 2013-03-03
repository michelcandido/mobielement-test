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
    public partial class WhoDidUSee : PhoneApplicationPage
    {        
        private ApplicationBarIconButton btnBack, btnNext;
        private bool edit = false;
        private string editName;

        public WhoDidUSee()
        {
            InitializeComponent();
            DataContext = App.ViewModel;

            btnBack = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            btnNext = (ApplicationBarIconButton)ApplicationBar.Buttons[1];

            onTextChange(this, null);
        }

        private void onTextChange(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbName.Text))
            {
                btnNext.IsEnabled = false;                
            }
            else
            {
                btnNext.IsEnabled = true;                
            }

        }        

        private void OnBackClick(object sender, EventArgs e)
        {
            if (edit)
            { //edit mode, discard the change and restore the celebName to unchanged value
                tbName.Text = editName;
                App.ViewModel.CelebName = editName;                
            }
            this.NavigationService.GoBack();
        }

        private void OnNextClick(object sender, EventArgs e)
        {
            //App.ViewModel.CelebNameList.Clear();
            if (!App.ViewModel.CelebNameList.Contains(tbName.Text))
                App.ViewModel.CelebNameList.Add(tbName.Text);

            if (edit)
            { //edit mode, next page should be edit more too, we should also remove the old name
                if (tbName.Text != editName)
                    App.ViewModel.CelebNameList.Remove(editName);
                // keep an origional copy
                this.NavigationService.Navigate(new Uri(string.Format("/AddWho.xaml?edit&name={0}",editName), UriKind.RelativeOrAbsolute));
            }
            else
            {
                this.NavigationService.Navigate(new Uri("/AddWho.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
            {
                if (NavigationContext.QueryString.ContainsKey("edit"))
                {
                    edit = true;
                    editName = App.ViewModel.CelebName;
                }
                /*
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
                 * */
            }
        }        
    }
}