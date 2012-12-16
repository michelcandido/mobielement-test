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
using Microsoft.Phone.Tasks;

namespace StarSightings
{
    public partial class VoteCommentPage : PhoneApplicationPage
    {
        public VoteCommentPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
        }

        private void htmlTextBlock_NavigationRequested(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();

            try
            {
                webBrowserTask.Uri = new Uri(((e.Content as Hyperlink).CommandParameter as string), UriKind.Absolute);
                webBrowserTask.Show();
            }
            catch (System.Exception)
            {

                //throw;
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
            {
                if (NavigationContext.QueryString.ContainsKey("pivotItem"))
                {
                    string pivotItem = NavigationContext.QueryString["pivotItem"];
                    if (pivotItem == "comment")
                        this.pivotControl.SelectedIndex = 1;                    
                }
            }            
        }

        private void CommentButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/CommentInputPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}