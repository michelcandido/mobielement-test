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
using Telerik.Windows.Controls;
using Microsoft.Phone.Net.NetworkInformation;
using StarSightings.Events;


namespace StarSightings
{
    public partial class Location : PhoneApplicationPage
    {
        private WebServiceAutoCompleteProvider provider;
        private KeywordEventHandler keywordHandler;
        
        public Location()
        {
            InitializeComponent();
            DataContext = App.ViewModel;

            App.SSAPI.ObtainLocationSuggestions(App.SSAPI.getLocationSuggestionString());

            //AutoCompleteBox initialization
            this.provider = new WebServiceAutoCompleteProvider();
            this.radAutoCompleteBox.InitSuggestionsProvider(this.provider);
            this.provider.InputChanged += this.OnProvider_InputChanged;
        }

        private void initializeList()
        {
            
        }

        private void OnNextTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ViewModel.StoryLocation = this.provider.InputString;
            this.NavigationService.Navigate(new Uri("/Place.xaml", UriKind.RelativeOrAbsolute));
        }

        private void OnBackTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/AddWho.xaml", UriKind.RelativeOrAbsolute));
        }

        private void OnInitSuggestions(object sender, EventArgs e)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                RadMessageBox.Show("No Internet connection. This example needs Internet access to function properly.");
                return;
            }

            keywordHandler = new KeywordEventHandler(KeywordSearchCompleted);
            App.SSAPI.KeywordHandler += keywordHandler;
            App.SSAPI.Keyword(Constants.KEYWORD_LOCATION, this.provider.InputString);
            

        }

        private void OnProvider_InputChanged(object sender, EventArgs e)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                RadMessageBox.Show("No Internet connection. This example needs Internet access to function properly.");
                return;
            }
            string inputString = this.provider.InputString;
            if (!string.IsNullOrEmpty(inputString))
            {
                //this.busyIndicator.IsRunning = true;
                //FlixsterApi.GetMovieInfoByTitle(this.provider.InputString, 20, 1, this.OnMoviesDelivered);
                keywordHandler = new KeywordEventHandler(KeywordSearchCompleted);
                App.SSAPI.KeywordHandler += keywordHandler;
                App.SSAPI.Keyword(Constants.KEYWORD_LOCATION, this.provider.InputString);
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

        private void Location_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.radAutoCompleteBox.Text=(App.ViewModel.LocationList.ElementAt((sender as ListBox).SelectedIndex));
            App.ViewModel.LocationList.Clear();
        }

    }

}