#undef TESTSERVER
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.IO;
using StarSightings.Events;
using System.IO.IsolatedStorage;
using System.Text;
using System.Collections.ObjectModel;

namespace StarSightings
{
    public class ServerAPI
    {                
        private WebClient GetWebClient()
        {
            WebClient webClient = new WebClient();

#if (TESTSERVER)            
                string auth = Constants.BASE_AUTH_USERNAME + ":" + Constants.BASE_AUTH_PASSWORD;
                string authString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(auth));
                webClient.Headers["Authorization"] = "Basic " + authString;            
#endif
            return webClient;
        }

        public void RegisterDevice()
        {
            WebClient webClient = GetWebClient();
            string baseUri = Constants.SERVER_NAME + Constants.URL_REGISTER_DEVICE;
            string query = "device_id=3_" /*+ Convert.ToBase64String(Utils.GetDeviceUniqueID())*//*GetWindowsLiveAnonymousID()*/ + "&device_token="+Utils.GetManufacturer();
            Uri uri = Utils.BuildUriWithAppendedParams(baseUri, query);

            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(HandleRegisterDevice);
            webClient.DownloadStringAsync(uri);
        }

        private void HandleRegisterDevice(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    // Showing the exact error message is useful for debugging. In a finalized application, 
                    // output a friendly and applicable string to the user instead. 
                    //MessageBox.Show(e.Error.Message);
                    App.Logger.log(LogLevel.error, e.Error.Message);
                });
                RegisterEventArgs re = new RegisterEventArgs(false);
                OnRegister(re);
            }
            else
            {
                // Save the feed into the State property in case the application is tombstoned.                 
                    //this.State["feed"] = e.Result;

                XElement xmlResponse = XElement.Parse(e.Result);//Load(new StringReader(e.Result));
                XElement xmlStatus = xmlResponse.Element("status");

                if (xmlStatus != null && String.Compare(xmlStatus.Value, "OK", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    RegisterEventArgs re = new RegisterEventArgs(true);
                    XElement xmlDeviceId = xmlResponse.Element("new_device_id");
                    if (xmlDeviceId != null)
                    {
                        re.DeviceId = xmlDeviceId.Value;
                        Utils.AddOrUpdateIsolatedStorageSettings("DeviceId", xmlDeviceId.Value);
                    }
                    OnRegister(re);
                }
                else
                {
                    RegisterEventArgs re = new RegisterEventArgs(false);
                    OnRegister(re);
                }                
                //UpdateFeedList(e.Result);                
            }
        }

        public void UnregisterDevice()
        {
            WebClient webClient = GetWebClient();
            string baseUri = Constants.SERVER_NAME + Constants.URL_UNREGISTER_DEVICE;
            string query = "device_id=" + (string)Utils.GetIsolatedStorageSettings("DeviceId");
            Uri uri = Utils.BuildUriWithAppendedParams(baseUri, query);

            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(HandleUnregisterDevice);
            webClient.DownloadStringAsync(uri);
        }

        private void HandleUnregisterDevice(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    // Showing the exact error message is useful for debugging. In a finalized application, 
                    // output a friendly and applicable string to the user instead. 
                    //MessageBox.Show(e.Error.Message);
                    App.Logger.log(LogLevel.error, e.Error.Message);
                });
                RegisterEventArgs re = new RegisterEventArgs(false);
                OnRegister(re);
            }
            else
            {                
                XElement xmlResponse = XElement.Parse(e.Result);//Load(new StringReader(e.Result));
                XElement xmlStatus = xmlResponse.Element("status");

                if (xmlStatus != null && String.Compare(xmlStatus.Value, "OK", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    RegisterEventArgs re = new RegisterEventArgs(true);
                    Utils.RemoveIsolatedStorageSettings("DeviceId");                    
                    OnRegister(re);
                }
                else
                {
                    RegisterEventArgs re = new RegisterEventArgs(false);
                    OnRegister(re);
                }                
            }
        }       

        public event RegisterEventHandler Register;

        protected virtual void OnRegister(RegisterEventArgs e)
        {
            if (Register != null)
            {
                Register(this, e);
            }
        }

        public void DoSearch(SearchParams searchParams, SearchToken token)
        {
            WebClient webClient = GetWebClient();
            string baseUri;
            if (token.searchGroup == Constants.SEARCH_POPULAR)
               baseUri  = Constants.SERVER_NAME + Constants.URL_INDEX_PAGE_SEARCH;
            else
                baseUri = Constants.SERVER_NAME + Constants.URL_SEARCH;
            string query = searchParams.ToString(); 
            Uri uri = Utils.BuildUriWithAppendedParams(baseUri, query);

            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(HandleSearchResult);
            webClient.DownloadStringAsync(uri, token);
        }

        private void HandleSearchResult(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    // Showing the exact error message is useful for debugging. In a finalized application, 
                    // output a friendly and applicable string to the user instead. 
                    //MessageBox.Show(e.Error.Message);
                    App.Logger.log(LogLevel.error, e.Error.Message);
                });
                SearchEventArgs se = new SearchEventArgs(false);
                se.SearchToken = (SearchToken)e.UserState;
                OnSearch(se);
            }
            else
            {
                XElement xmlResponse = XElement.Parse(e.Result);
                XElement xmlItems = xmlResponse.Element("items");

                if (xmlItems != null)
                {
                    ObservableCollection<ItemViewModel> items = new ObservableCollection<ItemViewModel>();
                    foreach (XElement xmlItem in xmlItems.Elements("item"))
                    {                        
                        ItemViewModel item = new ItemViewModel();
                        item.PhotoId = xmlItem.Element("photo_id").Value;
                        item.GeoLat = xmlItem.Element("geo_lat").Value;
                        item.GeoLng = xmlItem.Element("geo_lng").Value;
                        item.Source = xmlItem.Element("source").Value;
                        item.ForumId = xmlItem.Element("forum_id").Value;
                        item.TopicId = xmlItem.Element("topic_id").Value;
                        item.PostId = xmlItem.Element("post_id").Value;
                        item.Name = xmlItem.Element("name").Value;
                        item.Descr = xmlItem.Element("descr").Value;
                        item.Location = xmlItem.Element("location").Value;
                        item.EventName = xmlItem.Element("event").Value;
                        item.Place = xmlItem.Element("place").Value;
                        item.SourceUrl = xmlItem.Element("source_url").Value;
                        item.ViewCnt = xmlItem.Element("view_cnt").Value;
                        item.UserId = xmlItem.Element("user_id").Value;                                                
                        item.CanEdit = xmlItem.Element("hidden").Value == "1";                        
                        item.ThumbUserSmall = xmlItem.Element("thumb_user_small").Value;
                        item.ThumbUserLarge = xmlItem.Element("thumb_user_large").Value;
                        item.ThumbOrigSmall = xmlItem.Element("thumb_orig_small").Value;
                        item.ThumbOrigLarge = Constants.SERVER_NAME + xmlItem.Element("thumb_orig_large").Value;
                        item.Cat = xmlItem.Element("cat").Value;
                        item.Types = xmlItem.Element("types").Value;
                        item.MaxBid = xmlItem.Element("max_bid").Value;
                        item.MaxBidTime = xmlItem.Element("max_bid_time").Value;
                        item.BidCnt = xmlItem.Element("bid_cnt").Value;
                        item.VisibleMode = xmlItem.Element("visible_mode").Value;                        
                        item.Hidden = xmlItem.Element("hidden").Value == "1";
                        item.Rights = xmlItem.Element("rights").Value;
                        item.HasPhoto = xmlItem.Element("has_photo").Value == "1";
                        item.Time = xmlItem.Element("time").Value;
                        item.LocalTime = xmlItem.Element("local_time").Value;
                        item.LocalOffset = xmlItem.Element("local_offset").Value;
                        item.Vote = xmlItem.Element("vote").Value;

                        items.Add(item);
                    }
                    SearchEventArgs se = new SearchEventArgs(true);
                    se.Items = items;
                    se.SearchToken = (SearchToken)e.UserState;
                    OnSearch(se);
                }
                else
                {
                    SearchEventArgs se = new SearchEventArgs(false);                    
                    OnSearch(se);
                }                
            }
        }

        public event SearchEventHandler Search;

        protected virtual void OnSearch(SearchEventArgs e)
        {
            if (Search != null)
            {
                Search(this, e);
            }
        }
    }

    public delegate void RegisterEventHandler(object sender, RegisterEventArgs e);
    public delegate void SearchEventHandler(object sender, SearchEventArgs e);

    public class SearchParams
    {	
	    public const int CAT_NAME = 0;
	    public const int EVENT_NAME = 1;
	    public const int PLACE_NAME = 2;
	    public const int LOCATION_NAME = 3;
	
	    public int start=0;
	    public int limit= Constants.LIMIT;
	    public String search_types;
	    public String filter;
	
	    public String mode;
	    public String order_by;
	    public String order_dir;
	    public String page;
	    public String search_autogroup;
	
	    public Double search_lat;
	    public Double search_lng;

	    public String token;
	    public String search_cat_name;
	    public String search_event_name;
	    public String search_place_name;
	    public String search_location_name;
	
	    public bool isAlert;
	
	    private String searchParams=null;

        public void setSearchParams(String searchParams)
        {
            this.searchParams = searchParams;
        }

        public string ToString()
        {
            if (searchParams == null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("start=" + start + "&limit=" + limit);
                if (order_by != null)
                    sb.Append("&order_by=" + order_by);
                if (filter != null)
                    sb.Append("&filter=" + filter);
                if (order_dir != null)
                    sb.Append("&order_dir=" + order_dir);
                if (search_types != null)
                    sb.Append("&search_types=" + search_types);
                return sb.ToString();
            }
            else if (start > 0)
            {
                searchParams = Utils.UpdateStartIndex(searchParams, start);
            }
            return searchParams;
        }
    }

    public class SearchType 
    {
        public static string[] CATEGORY_FILTER_NAMES = { "", "celebrities", "musicians", "politicians", "models", "athletes" };
	}

    public class SearchToken
    {
        public int searchGroup;
        public bool isFresh;
        public int start;
    }
	
}
