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
    public partial class Place : PhoneApplicationPage
    {
        private WebServiceAutoCompleteProvider provider;
        private KeywordEventHandler keywordHandler;

        public Place()
        {
            InitializeComponent();
            DataContext = App.ViewModel;

            App.SSAPI.ObtainPlaceSuggestions(App.SSAPI.getPlaceSuggestionString()); 

            //AutoCompleteBox initialization
            this.provider = new WebServiceAutoCompleteProvider();
            this.radAutoCompleteBox.InitSuggestionsProvider(this.provider);
            this.provider.InputChanged += this.OnProvider_InputChanged;
        }        

        private void OnBackClick(object sender, EventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void OnNextClick(object sender, EventArgs e)
        {
            App.ViewModel.StoryPlace = this.provider.InputString;
            this.NavigationService.Navigate(new Uri("/Event.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Place_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.radAutoCompleteBox.Text = (App.ViewModel.PlaceList.ElementAt((sender as ListBox).SelectedIndex));
            App.ViewModel.PlaceList.Clear();
        }

        private void OnProvider_InputChanged(object sender, EventArgs e)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                RadMessageBox.Show("No Internet connection. StarSightings needs Internet access to function properly.");
                return;
            }
            string inputString = this.provider.InputString;
            if (!string.IsNullOrEmpty(inputString))
            {
                //this.busyIndicator.IsRunning = true;
                //FlixsterApi.GetMovieInfoByTitle(this.provider.InputString, 20, 1, this.OnMoviesDelivered);
                keywordHandler = new KeywordEventHandler(KeywordSearchCompleted);
                App.SSAPI.KeywordHandler += keywordHandler;
                App.SSAPI.Keyword(Constants.KEYWORD_PLACE, this.provider.InputString);
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
    }
}