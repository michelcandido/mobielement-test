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
using Microsoft.Phone.Shell;


namespace StarSightings
{
    public partial class Location : PhoneApplicationPage
    {
        private WebServiceAutoCompleteProvider provider;
        private KeywordEventHandler keywordHandler;

        private ApplicationBarIconButton btnBack,btnNext;
        private bool edit;
        private string editLocation;

        public Location()
        {
            InitializeComponent();
            DataContext = App.ViewModel;

            App.SSAPI.ObtainLocationSuggestions(App.SSAPI.getLocationSuggestionString());

            //AutoCompleteBox initialization
            this.provider = new WebServiceAutoCompleteProvider();
            this.radAutoCompleteBox.InitSuggestionsProvider(this.provider);
            this.provider.InputChanged += this.OnProvider_InputChanged;

            btnBack = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            btnNext = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
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

        private void initializeList()
        {
            
        }        

        private void OnBackClick(object sender, EventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void OnNextClick(object sender, EventArgs e)
        {
            App.ViewModel.StoryLocation = this.provider.InputString;
            if (edit)
                this.NavigationService.GoBack();
            else
                this.NavigationService.Navigate(new Uri("/Place.xaml", UriKind.RelativeOrAbsolute));
        }

        private void OnInitSuggestions(object sender, EventArgs e)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                //MessageBox.Show("No Internet connection. StarSightings needs Internet access to function properly.");
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

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
            {
                if (NavigationContext.QueryString.ContainsKey("edit"))
                {
                    edit = true;
                    editLocation = App.ViewModel.StoryLocation;
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