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
using Telerik.Windows.Controls;
using StarSightings.Events;
using Microsoft.Phone.Net.NetworkInformation;

namespace StarSightings
{
    public partial class WhoDidUSee : PhoneApplicationPage
    {
        private WebServiceAutoCompleteProvider provider;
        private KeywordEventHandler keywordHandler;

        private ApplicationBarIconButton btnBack, btnNext;
        private bool edit = false;
        private string editName;

        public WhoDidUSee()
        {
            InitializeComponent();
            DataContext = App.ViewModel;

            btnBack = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            btnNext = (ApplicationBarIconButton)ApplicationBar.Buttons[1];

            //AutoCompleteBox initialization
            this.provider = new WebServiceAutoCompleteProvider();
            this.radAutoCompleteBox.InitSuggestionsProvider(this.provider);
            this.provider.InputChanged += this.OnProvider_InputChanged;

            onTextChange(this, null);
        }

        private void onTextChange(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.provider.InputString))
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
            {   //edit mode, discard the change and restore the celebName to unchanged value
                //this.provider.InputString = editName;
                App.ViewModel.CelebName = editName;                
            }
            this.NavigationService.GoBack();
        }

        private void OnNextClick(object sender, EventArgs e)
        {
            //App.ViewModel.CelebNameList.Clear();
            if (!App.ViewModel.CelebNameList.Contains(this.provider.InputString))
                App.ViewModel.CelebNameList.Add(this.provider.InputString);

            if (edit)
            { //edit mode, next page should be edit more too, we should also remove the old name
                if (this.provider.InputString != editName)
                    App.ViewModel.CelebNameList.Remove(editName);
                // keep an origional copy
                this.NavigationService.Navigate(new Uri(string.Format("/AddWho.xaml?edit&name={0}",editName), UriKind.RelativeOrAbsolute));
            }
            else
            {
                this.NavigationService.Navigate(new Uri("/AddWho.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        private void OnProvider_InputChanged(object sender, EventArgs e)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                //MessageBox.Show("No Internet connection. StarSightings needs Internet access to function properly.");
                return;
            }
            string inputString = this.provider.InputString;
            if (!string.IsNullOrEmpty(inputString))
            {
                //this.busyIndicator.IsRunning = true;
                //FlixsterApi.GetMovieInfoByTitle(this.provider.InputString, 20, 1, this.OnMoviesDelivered);
                keywordHandler = new KeywordEventHandler(KeywordSearchCompleted);
                App.SSAPI.KeywordHandler += keywordHandler;
                App.SSAPI.Keyword(Constants.KEYWORD_NAME_SUGGEST, this.provider.InputString);
            }
            else
            {
                //this.busyIndicator.IsRunning = false;
                this.provider.LoadSuggestions(new List<string>());
            }

        }

        public void KeywordSearchCompleted(object sender, KeywordEventArgs e)
        {
            App.SSAPI.KeywordHandler -= keywordHandler;
            //this.busyIndicator.IsRunning = false;

            if (!string.IsNullOrEmpty(this.provider.InputString))
            {
                this.provider.LoadSuggestions(e.Keywords);
            }
            else
            {
                this.provider.LoadSuggestions(new List<string>());
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

        private void GoHome(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml?clear", UriKind.RelativeOrAbsolute));
        }   
    }
}