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
    public partial class SearchInputPage : PhoneApplicationPage
    {
        private WebServiceAutoCompleteProvider provider;
        private KeywordEventHandler keywordHandler;
        public SearchInputPage()
        {
            InitializeComponent();
            this.provider = new WebServiceAutoCompleteProvider();
            this.radAutoCompleteBox.InitSuggestionsProvider(this.provider);
            this.provider.InputChanged += this.OnProvider_InputChanged;
        }

        private int pageMode = 0;
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
            {
                if (NavigationContext.QueryString.ContainsKey("page"))
                {                   
                    int.TryParse(NavigationContext.QueryString["page"], out pageMode);                    
                }

                switch (pageMode)
                {
                    case 0:
                        App.ViewModel.KeywordType = Constants.KEYWORD_NAME;
                        break;
                    case 1:
                        App.ViewModel.KeywordType = Constants.KEYWORD_EVENT;
                        break;
                    case 2:
                        App.ViewModel.KeywordType = Constants.KEYWORD_PLACE;
                        break;
                    case 3:
                        App.ViewModel.KeywordType = Constants.KEYWORD_LOCATION;
                        break;
                    case 4:
                        App.ViewModel.KeywordType = Constants.KEYWORD_USER;
                        break;
                    case 5:
                        App.ViewModel.KeywordType = Constants.KEYWORD_MY;
                        break;
                }
                this.SearchFor.Text = string.Format("Search for {0}", App.ViewModel.KeywordType); 
            }
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

                switch (pageMode)
                {
                    case 0:
                        App.SSAPI.Keyword(Constants.KEYWORD_NAME_SUGGEST, this.provider.InputString);
                        break;
                    case 1:
                        App.SSAPI.Keyword(Constants.KEYWORD_EVENT, this.provider.InputString);
                        break;
                    case 2:
                        App.SSAPI.Keyword(Constants.KEYWORD_PLACE, this.provider.InputString);
                        break;
                    case 3:
                        App.SSAPI.Keyword(Constants.KEYWORD_LOCATION, this.provider.InputString);
                        break;                        
                }
                

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.provider.InputString))
            {
                MessageBox.Show("Please input your query first.");
            }
            else
            {
                /*
                switch (pageMode)
                {
                    case 0:
                        App.ViewModel.KeywordType = Constants.KEYWORD_NAME;
                        break;
                    case 1:                    
                        App.ViewModel.KeywordType = Constants.KEYWORD_EVENT;
                        break;
                    case 2:
                        App.ViewModel.KeywordType = Constants.KEYWORD_PLACE;
                        break;
                    case 3:
                        App.ViewModel.KeywordType = Constants.KEYWORD_LOCATION;
                        break;
                    case 4: 
                        App.ViewModel.KeywordType = Constants.KEYWORD_USER;
                        break;
                    case 5:
                        App.ViewModel.KeywordType = Constants.KEYWORD_MY;
                        break;
                }
                 * */
                App.ViewModel.SearchKeywords = this.provider.InputString;
                App.ViewModel.SearchKeywordSearch(true, 0, null);
                App.ViewModel.KeywordSearchItems.Clear();
                //App.ViewModel.ShowSearchPivotItem = true;
                //this.NavigationService.Navigate(new Uri("/ListPage.xaml?pivotItemId=4", UriKind.RelativeOrAbsolute));
                this.NavigationService.Navigate(new Uri("/SearchResultPage.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        private void GoHome(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml?clear", UriKind.RelativeOrAbsolute));
        }
    }
}