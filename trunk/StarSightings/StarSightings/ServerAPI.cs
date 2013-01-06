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
using StarSightings.ViewModels;
using System.Device.Location;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace StarSightings
{
    public class ServerAPI
    {                
        private WebClient GetWebClient()
        {
            WebClient webClient = new WebClient();

#if (DEBUG)            
                string auth = Constants.BASE_AUTH_USERNAME + ":" + Constants.BASE_AUTH_PASSWORD;
                string authString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(auth));
                webClient.Headers["Authorization"] = "Basic " + authString;            
#endif
            return webClient;
        }


        public void ObtainLocationSuggestions(string query)
        {
            WebClient webClient = GetWebClient();
            string baseUri = Constants.SERVER_NAME + Constants.URL_SUGGEST;
            Uri uri = Utils.BuildUriWithAppendedParams(baseUri, query);
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(HandleSmartLocationSuggestion);
            webClient.DownloadStringAsync(uri);
        }

        private void HandleSmartLocationSuggestion(object sender, DownloadStringCompletedEventArgs e)
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
                //SuggestionEventArgs re = new SuggestionEventArgs(false);
                //OnSuggestion(re);
            }
            else
            {
                // Save the feed into the State property in case the application is tombstoned.                 
                //this.State["feed"] = e.Result;

                XElement xmlResponse = XElement.Parse(e.Result);

                XElement xmlValues = xmlResponse.Element("suggestions").Element("values");

                App.ViewModel.LocationList.Clear();

                if (xmlValues != null)
                {
                    foreach (XElement xmlItem in xmlValues.Elements("value"))
                    {
                        App.ViewModel.LocationList.Add(xmlItem.Value);
                    }
                }
            }
        }

        public void ObtainPlaceSuggestions(string query)
        {
            WebClient webClient = GetWebClient();
            string baseUri = Constants.SERVER_NAME + Constants.URL_SUGGEST;
            Uri uri = Utils.BuildUriWithAppendedParams(baseUri, query);
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(HandleSmartPlaceSuggestion);
            webClient.DownloadStringAsync(uri);
        }

        private void HandleSmartPlaceSuggestion(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    App.Logger.log(LogLevel.error, e.Error.Message);
                });
            }
            else
            {

                XElement xmlResponse = XElement.Parse(e.Result);
                XElement xmlValues = xmlResponse.Element("suggestions").Element("values");
                App.ViewModel.PlaceList.Clear();

                if (xmlValues != null)
                {
                    foreach (XElement xmlItem in xmlValues.Elements("value"))
                    {
                        App.ViewModel.PlaceList.Add(xmlItem.Value);
                    }
                }
            }
        }

        public void ObtainEventSuggestions(string query)
        {
            WebClient webClient = GetWebClient();
            string baseUri = Constants.SERVER_NAME + Constants.URL_SUGGEST;
            Uri uri = Utils.BuildUriWithAppendedParams(baseUri, query);
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(HandleSmartEventSuggestion);
            webClient.DownloadStringAsync(uri);
        }

        private void HandleSmartEventSuggestion(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    App.Logger.log(LogLevel.error, e.Error.Message);
                });
            }
            else
            {

                XElement xmlResponse = XElement.Parse(e.Result);
                XElement xmlValues = xmlResponse.Element("suggestions").Element("values");
                App.ViewModel.EventList.Clear();

                if (xmlValues != null)
                {
                    foreach (XElement xmlItem in xmlValues.Elements("value"))
                    {
                        App.ViewModel.EventList.Add(xmlItem.Value);
                    }
                }
            }
        }

        private static String toWebString(String inputString)
        {
            char[] inputs = inputString.ToCharArray();

            string outputString = "";

            for (int i = 0; i < inputs.Length; i++)
            {
                char input = inputs[i];

                if (input == ' ')
                {
                    outputString += "%20";
                }
                else
                    outputString += input;
            }
            return outputString;
        }

        public String getCatList()
        {
            string result = "";
            foreach (string celeb in App.ViewModel.CelebNameList)
                result += HttpUtility.UrlEncode(celeb) + ";";
            result = result.Replace("+", "%20");
            return result.Substring(0, result.Length - 1);
        }

        public String getLocationSuggestionString()
        {
            //return "page=suggest&mode=place&mobile=1&v=3&cat=Bono&local_offset=-25200&location=Hoboken,%20New%20Jersey&geo_lat=40.74541&geo_lng=-74.03509&search_feets=105680&time=1350923604.361893";
            String toReturn = "mode=location&cat=";
            toReturn += getCatList();
            toReturn += "&time=" + Utils.ConvertToUnixTimestamp(App.ViewModel.StoryTime);
            if (App.ViewModel.StoryLat !=0.0)
                toReturn += "&geo_lat=" + App.ViewModel.StoryLat;
            if (App.ViewModel.StoryLng != 0.0)
                toReturn += "&geo_lng=" + App.ViewModel.StoryLng;
            toReturn += "&device_id=" + App.ViewModel.DeviceId;
            return toReturn;
        }

        public String getPlaceSuggestionString()
        {
            String toReturn = "mode=place&cat=";
            toReturn += getCatList();
            toReturn += "&time=" + Utils.ConvertToUnixTimestamp(App.ViewModel.StoryTime);
            if (App.ViewModel.StoryLat != 0.0)
                toReturn += "&geo_lat=" + App.ViewModel.StoryLat;
            if (App.ViewModel.StoryLng != 0.0)
                toReturn += "&geo_lng=" + App.ViewModel.StoryLng;
            toReturn += "&location=" + HttpUtility.UrlEncode(App.ViewModel.StoryLocation);
            toReturn += "&device_id=" + App.ViewModel.DeviceId;

            //return (toReturn + "&location=" + toWebString(App.ViewModel.StoryLocation)); 
            return toReturn;
        }

        public String getEventSuggestionString()
        {
            String toReturn = "mode=event&cat=";
            toReturn += getCatList();
            toReturn += "&time=" + Utils.ConvertToUnixTimestamp(App.ViewModel.StoryTime);
            if (App.ViewModel.StoryLat != 0.0)
                toReturn += "&geo_lat=" + App.ViewModel.StoryLat;
            if (App.ViewModel.StoryLng != 0.0)
                toReturn += "&geo_lng=" + App.ViewModel.StoryLng;
            toReturn += "&location=" + HttpUtility.UrlEncode(App.ViewModel.StoryLocation);
            if (!string.IsNullOrEmpty(App.ViewModel.StoryPlace) && !string.IsNullOrWhiteSpace(App.ViewModel.StoryPlace))
                toReturn += "&place=" + HttpUtility.UrlEncode(App.ViewModel.StoryPlace); 
            toReturn += "&device_id=" + App.ViewModel.DeviceId;

            return toReturn;
            //return (toReturn + "&location=" + toWebString(App.ViewModel.StoryLocation) + "&place=" + toWebString(App.ViewModel.StoryPlace));
        }


        public void RegisterDevice()
        {
            WebClient webClient = GetWebClient();
            string baseUri = Constants.SERVER_NAME + Constants.URL_REGISTER_DEVICE;
            string query = "device_id=2_" /*+ Convert.ToBase64String(Utils.GetDeviceUniqueID())*//*GetWindowsLiveAnonymousID()*/ + "&device_token="+Utils.GetManufacturer();
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

        public event RegisterEventHandler RegisterHandler;
        
        protected virtual void OnRegister(RegisterEventArgs e)
        {
            if (RegisterHandler != null)
            {
                RegisterHandler(this, e);
            }
        }

        public void RegisterUser()
        {
            WebClient webClient = GetWebClient();
            webClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";

            string baseUri = Constants.SERVER_NAME + Constants.URL_REGISTER_USER;
            string query = "username=" + App.ViewModel.User.UserName + "&password=" + App.ViewModel.User.Password + "&password_confirm=" + App.ViewModel.User.PasswordConfirm + "&email=" + App.ViewModel.User.UserEmail.Replace("@", "%40") + "&device_id=" + App.ViewModel.DeviceId + "&persistent=1";
            Uri uri = Utils.BuildUriWithAppendedParams(baseUri, "");

            webClient.UploadStringCompleted +=new UploadStringCompletedEventHandler(HandleRegisterUser);
            webClient.UploadStringAsync(uri, query);
        }

        private void HandleRegisterUser(object sender, UploadStringCompletedEventArgs e)
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
                UserViewModel user = this.GetUserInfoFromXML(e.Result);
                if (user != null)
                {
                    user.Password = App.ViewModel.User.Password;
                    user.PasswordConfirm = App.ViewModel.User.PasswordConfirm;
                    user.UserEmail = App.ViewModel.User.UserEmail;

                    RegisterEventArgs re = new RegisterEventArgs(true);
                    re.User = user;
                    OnRegister(re);
                }
                else
                {
                    RegisterEventArgs re = new RegisterEventArgs(false);
                    OnRegister(re);
                }                                
            }
        }

        private UserViewModel GetUserInfoFromXML(string data)
        {
            XElement xmlResponse = XElement.Parse(data);//Load(new StringReader(e.Result));
            XElement xmlToken = xmlResponse.Element("token");
            XElement xmlTokenExpiration = xmlResponse.Element("token_expiration_time");

            if (xmlToken != null)
            {
                UserViewModel user = new UserViewModel();
                user.Token = xmlToken.Value;
                double expiration;
                if (Double.TryParse(xmlTokenExpiration.Value, out expiration))
                    user.TokenExpiration = expiration;
                XElement xmlUserInfo = xmlResponse.Element("userinfo");
                user.UserName = xmlUserInfo.Element("username").Value;
                user.UserId = xmlUserInfo.Element("user_id").Value;

                

                return user;
            }
            else
            {
                return null;
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
                        
                        ItemViewModel item = getItemInfoFromXML(xmlItem);
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

        private ItemViewModel getItemInfoFromXML(XElement xmlItem)
        {
            ItemViewModel item = new ItemViewModel();
            item.PhotoId = xmlItem.Element("photo_id").Value;
            item.GeoLat = xmlItem.Element("geo_lat").Value;
            item.GeoLng = xmlItem.Element("geo_lng").Value;
            item.Source = xmlItem.Element("source").Value.Trim(); ;
            item.ForumId = xmlItem.Element("forum_id").Value;
            item.TopicId = xmlItem.Element("topic_id").Value;
            item.PostId = xmlItem.Element("post_id").Value;
            item.Name = xmlItem.Element("name").Value;
            item.Descr = xmlItem.Element("descr").Value;
            item.Location = xmlItem.Element("location").Value;
            item.EventName = xmlItem.Element("event").Value;
            item.Place = xmlItem.Element("place").Value;
            item.SourceUrl = xmlItem.Element("source_url").Value.Trim();
            item.ViewCnt = xmlItem.Element("view_cnt").Value;
            item.UserId = xmlItem.Element("user_id").Value;
            item.CanEdit = xmlItem.Element("can_edit").Value.Trim() == "1";
            item.ThumbUserSmall = xmlItem.Element("thumb_user_small").Value;
            item.ThumbUserLarge = xmlItem.Element("thumb_user_large").Value;
            item.ThumbOrigSmall = xmlItem.Element("thumb_orig_small").Value;
            item.ThumbOrigLarge = xmlItem.Element("thumb_orig_large").Value;
            item.Cat = xmlItem.Element("cat").Value;
            item.Types = xmlItem.Element("types").Value;
            item.MaxBid = xmlItem.Element("max_bid").Value;
            item.MaxBidTime = xmlItem.Element("max_bid_time").Value;
            item.BidCnt = xmlItem.Element("bid_cnt").Value;
            item.VisibleMode = xmlItem.Element("visible_mode").Value;
            item.Hidden = xmlItem.Element("hidden").Value.Trim() == "1";
            item.Rights = xmlItem.Element("rights").Value.Trim();
            item.HasPhoto = xmlItem.Element("has_photo").Value.Trim() == "1";
            item.Time = xmlItem.Element("time").Value;
            item.LocalTime = xmlItem.Element("local_time").Value;
            item.LocalOffset = xmlItem.Element("local_offset").Value;

#if (DEBUG)
            item.CommentsCnt = xmlItem.Element("comments").Attribute("count").Value;
            XElement xmlComments = xmlItem.Element("comments");
            ObservableCollection<CommentViewModel> comments = new ObservableCollection<CommentViewModel>();
            if (xmlComments != null)
            {
                foreach (XElement xmlComment in xmlComments.Elements("c"))
                {
                    CommentViewModel comment = new CommentViewModel();
                    comment.CommentId = xmlComment.Attribute("id").Value;
                    comment.CommentType = xmlComment.Attribute("type").Value;
                    comment.Promoted = xmlComment.Attribute("p").Value == "1";
                    comment.Time = xmlComment.Element("time").Value;
                    comment.CommentValue = xmlComment.Element("value").Value;
                    comment.UserId = xmlComment.Element("user_id").Value;
                    comment.User = xmlComment.Element("user").Value;
                    comments.Add(comment);
                }

                item.Comments = comments;
                item.CommentsSummaryList.Source = item.Comments;
                item.CommentsSummaryList.Filter += (s, a) => a.Accepted = item.Comments.IndexOf((CommentViewModel)a.Item) < Constants.COMMENT_COUNT;
            }
#else
                        item.CommentsCnt = xmlItem.Element("comments_count").Value;
                        XElement xmlComments = xmlItem.Element("comments");
                        ObservableCollection<CommentViewModel> comments = new ObservableCollection<CommentViewModel>();
                        if (xmlComments != null)
                        {
                            foreach (XElement xmlComment in xmlComments.Elements("comment"))
                            {
                                CommentViewModel comment = new CommentViewModel();
                                comment.CommentId = xmlComment.Element("comment_id").Value;
                                comment.Time = xmlComment.Element("time").Value;
                                comment.CommentType = xmlComment.Element("comment_type").Value;
                                comment.Promoted = xmlComment.Element("promoted").Value == "1";                                
                                comment.CommentValue = xmlComment.Element("value").Value;
                                comment.UserId = xmlComment.Element("user_id").Value;
                                comment.User = xmlComment.Element("user").Value;
                                comment.UserLevel = xmlComment.Element("user_level").Value;
                                if (xmlComment.Element("facebook_uid") != null)
                                    comment.FacebookUid = xmlComment.Element("facebook_uid").Value;
                                if (xmlComment.Element("button_template_id") != null)
                                    comment.ButtonTemplateId = xmlComment.Element("button_template_id").Value;
                                if (xmlComment.Element("button_template_prompt") != null)
                                    comment.ButtonTemplatePrompt = xmlComment.Element("button_template_prompt").Value;
                                comments.Add(comment);
                            }
                            item.Comments = comments;
                            item.CommentsSummaryList.Source = item.Comments;
                            item.CommentsSummaryList.Filter += (s, a) => a.Accepted = item.Comments.IndexOf((CommentViewModel)a.Item) < Constants.COMMENT_COUNT;
                        }
#endif
            //item.Vote = xmlItem.Element("vote").Value;

            // computed properties
            item.GeoLocation = getGeoCoordinate(item.GeoLat, item.GeoLng);
            item.Distance = Math.Round(Utils.Between(Utils.DistanceIn.Miles, App.ViewModel.MyLocation, item.GeoLocation));
            item.EventLocation = item.Place + (item.Place.Length == 0 ? "" : " in ") + item.Location;
            item.EventDescr = item.Descr.Length != 0 ? item.Descr : item.EventName;
            item.ThumbOrigLarge = Constants.SERVER_NAME + item.ThumbOrigLarge;
            item.Celebs = item.Cat.Split(new Char[] { ';' });
            item.Cat = item.Cat.Replace(";", ", ");

            string filename = item.ThumbOrigLarge;
            if (filename.Contains("thumb"))
                filename = filename.Substring(filename.IndexOf("thumb"));
            else if (filename.Contains("nophoto"))
                filename = filename.Substring(filename.IndexOf("nophoto"));
            int start = filename.IndexOf('.') + 1;
            int end = filename.IndexOf('x');
            string size_string = filename.Substring(start, end - start);
            int size_int = 160;
            Int32.TryParse(size_string, out size_int);
            if (item.Rights == "1" || item.Rights == "3")
                item.DetailPagePhotoSize = Math.Min(480, size_int * 2);
            else
                item.DetailPagePhotoSize = 160;

            item.EventSourceMode = string.Empty;
            string host = string.Empty;
            try
            {
                host = new Uri(item.SourceUrl).Host;
            }
            catch (Exception)
            {
            }
            if (string.IsNullOrEmpty(item.Source) || item.Source.Equals("ssbot", StringComparison.CurrentCultureIgnoreCase))
            {
                if (string.IsNullOrEmpty(item.SourceUrl))
                {
                    item.EventSource = string.Empty;
                    item.EventSourceMode = string.Empty;
                }
                else
                {
                    item.EventSourceMode = "source";
                    item.EventSource = host;
                }
            }
            else
            {
                if (item.Rights.Equals("1"))
                {
                    item.EventSourceMode = "by";
                    item.EventSource = item.Source;
                }
                else
                {
                    item.EventSourceMode = "shared by";
                    item.EventSource = item.Source;
                }
            }
            if (string.IsNullOrEmpty(item.SourceUrl))
            {
                item.EventFooter = string.Empty;
            }
            else
            {
                item.EventFooter = host;
            }

            return item;
        }

        private GeoCoordinate getGeoCoordinate(string geoLat, string geoLng)
        {
            Double lat = 0.0, lng = 0.0;
            Double.TryParse(geoLat, out lat);
            Double.TryParse(geoLng, out lng);
            return new GeoCoordinate(lat, lng);
        }

        public event SearchEventHandler SearchHandler;

        protected virtual void OnSearch(SearchEventArgs e)
        {
            if (SearchHandler != null)
            {
                SearchHandler(this, e);
            }
        }

        public void Login(int accountType, string query)
        {            
            WebClient webClient = GetWebClient();
            webClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";
            
            string baseUri = Constants.SERVER_NAME + Constants.URL_LOGIN;
            string param = query + "&persistent=1";
            Uri uri = Utils.BuildUriWithAppendedParams(baseUri, param);

            webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(HandleLogin);
            webClient.UploadStringAsync(uri,param);            
        }

        public void HandleLogin(object sender, UploadStringCompletedEventArgs e)
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
                LoginEventArgs le = new LoginEventArgs(false);
                OnLogin(le);
            }
            else
            {
                UserViewModel user = this.GetUserInfoFromXML(e.Result);
                if (user != null)
                {                    
                    LoginEventArgs le = new LoginEventArgs(true);
                    le.User = user;
                    /*
                    // we can also get the alets information from here                    
                    XElement xmlResponse = XElement.Parse(e.Result);
                    XElement xmlAlerts = xmlResponse.Element("alerts");
                    ObservableCollection<AlertViewModel> alerts = UpdateAlerts(xmlAlerts);
                    le.Alerts = alerts;
                    */
                    OnLogin(le);
                }
                else
                {
                    LoginEventArgs le = new LoginEventArgs(false);
                    OnLogin(le);
                }
            }
        }

        public void Logout()
        {
            WebClient webClient = GetWebClient();
            string baseUri = Constants.SERVER_NAME + Constants.URL_LOGOUT;
            string query = "";
            Uri uri = Utils.BuildUriWithAppendedParams(baseUri, query);

            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(HandleLogout);
            webClient.DownloadStringAsync(uri);
        }

        private void HandleLogout(object sender, DownloadStringCompletedEventArgs e)
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
                LoginEventArgs re = new LoginEventArgs(false);
                OnLogin(re);
            }
            else
            {
                // Save the feed into the State property in case the application is tombstoned.                 
                //this.State["feed"] = e.Result;

                XElement xmlResponse = XElement.Parse(e.Result);//Load(new StringReader(e.Result));
                XElement xmlStatus = xmlResponse.Element("status");

                if (xmlStatus != null && String.Compare(xmlStatus.Value, "OK", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    LoginEventArgs re = new LoginEventArgs(true);                    
                    OnLogin(re);
                }
                else
                {
                    LoginEventArgs re = new LoginEventArgs(false);
                    OnLogin(re);
                }
                //UpdateFeedList(e.Result);                
            }
        }
        public event LoginEventHandler LoginHandler;

        protected virtual void OnLogin(LoginEventArgs e)
        {
            if (LoginHandler != null)
            {
                LoginHandler(this, e);
            }
        }

        public void Alert(string alertMethod, string subjectType, string subjectName)
        {
            WebClient webClient = GetWebClient();
            string baseUri;
            if (string.IsNullOrEmpty(subjectType))
                baseUri = Constants.SERVER_NAME + Constants.URL_ALERT + alertMethod + "?mobile=1&v=3";
            else
                baseUri = Constants.SERVER_NAME + Constants.URL_ALERT + alertMethod + subjectType + HttpUtility.UrlEncode(subjectName) + "?mobile=1&v=3";
            string query = "token="+App.ViewModel.User.Token;
            Uri uri = Utils.BuildUriWithAppendedParams(baseUri, query);

            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(HandleAlert);
            webClient.DownloadStringAsync(uri);
        }

        private void HandleAlert(object sender, DownloadStringCompletedEventArgs e)
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
                AlertEventArgs ae = new AlertEventArgs(false);
                OnAlert(ae);
            }
            else
            {
                // Save the feed into the State property in case the application is tombstoned.                 
                //this.State["feed"] = e.Result;

                XElement xmlResponse = XElement.Parse(e.Result);//Load(new StringReader(e.Result));
                XElement xmlAlerts = xmlResponse.Element("alerts");

                if (xmlAlerts != null)
                {
                    AlertEventArgs ae = new AlertEventArgs(true);                                        
                    ae.Alerts = UpdateAlerts(xmlAlerts);
                    OnAlert(ae);
                }
                else
                {
                    AlertEventArgs ae = new AlertEventArgs(false);
                    OnAlert(ae);
                }                 
            }
        }

        public event AlertEventHandler AlertHandler;

        protected virtual void OnAlert(AlertEventArgs e)
        {
            if (AlertHandler != null)
            {
                AlertHandler(this, e);
            }
        }

        private ObservableCollection<AlertViewModel> UpdateAlerts(XElement xmlAlerts)
        {
            ObservableCollection<AlertViewModel> alerts = new ObservableCollection<AlertViewModel>();
            foreach (XElement xmlAlert in xmlAlerts.Elements("alert"))
            {
                AlertViewModel alert = new AlertViewModel();
                alert.Type = xmlAlert.Element("type").Value.Trim();
                if (alert.Type.Equals("geo"))
                {
                    alert.GeoLat = xmlAlert.Element("lat").Value.Trim();
                    alert.GeoLng = xmlAlert.Element("lng").Value.Trim();
                }
                else
                {
                    alert.Name = xmlAlert.Element("name").Value.Trim();
                }
                alert.Active = xmlAlert.Element("active").Value.Trim() == "1";
                alerts.Add(alert);
            }
            return alerts;
        }

        public void Keyword(string page, string q)
        {
            WebClient webClient = GetWebClient();
            string baseUri = Constants.SERVER_NAME + Constants.URL_KEYWORD;
            string query = "page=" + page + "&device_id="+App.ViewModel.DeviceId + "&q=" + HttpUtility.UrlEncode(q);
            Uri uri = Utils.BuildUriWithAppendedParams(baseUri, query);

            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(HandleKeyword);
            webClient.DownloadStringAsync(uri);
        }

        private void HandleKeyword(object sender, DownloadStringCompletedEventArgs e)
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
                KeywordEventArgs ke = new KeywordEventArgs(false);
                OnKeyword(ke);
            }
            else
            {
                // Save the feed into the State property in case the application is tombstoned.                 
                //this.State["feed"] = e.Result;

                XElement xmlResponse = XElement.Parse(e.Result);
                XElement xmlSuggestions = xmlResponse.Element("suggestions");
                XElement xmlValues = xmlSuggestions.Element("values");

                if (xmlValues != null)
                {
                    List<string> values = new List<string>();
                    foreach (XElement xmlValue in xmlValues.Elements("value"))
                    {
                        if (!string.IsNullOrEmpty(xmlValue.Value))
                        {
                            values.Add(xmlValue.Value);
                        }
                    }
                    KeywordEventArgs ke = new KeywordEventArgs(false);
                    ke.Keywords = values;
                    OnKeyword(ke);
                }
                else
                {
                    KeywordEventArgs ke = new KeywordEventArgs(false);
                    OnKeyword(ke);
                }
            }
        }

        public event KeywordEventHandler KeywordHandler;

        protected virtual void OnKeyword(KeywordEventArgs e)
        {
            if (KeywordHandler != null)
            {
                KeywordHandler(this, e);
            }
        }

        public void NewComment(string comment)
        {
            WebClient webClient = GetWebClient();
            webClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";

            string baseUri = Constants.SERVER_NAME + Constants.URL_COMMENT_NEW;
            string query = "token=" + App.ViewModel.User.Token + "&photo_id=" + App.ViewModel.SelectedItem.PhotoId + "&value=" + HttpUtility.UrlEncode(comment);
            Uri uri = Utils.BuildUriWithAppendedParams(baseUri, "");

            webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(HandleNewComment);
            webClient.UploadStringAsync(uri, query);
        }

        private void HandleNewComment(object sender, UploadStringCompletedEventArgs e)
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
                CommentEventArgs ce = new CommentEventArgs(false);
                OnComment(ce);
            }
            else
            {
                XElement xmlResponse = XElement.Parse(e.Result);
                XElement xmlItems = xmlResponse.Element("items");

                ItemViewModel item = null;
                if (xmlItems != null)
                {                    
                    foreach (XElement xmlItem in xmlItems.Elements("item"))
                    {
                        item = getItemInfoFromXML(xmlItem);
                    }

                    CommentEventArgs ce = new CommentEventArgs(true);
                    if (item != null)
                        ce.Item = item;
                    OnComment(ce);
                }                
                else
                {
                    CommentEventArgs ce = new CommentEventArgs(false);
                    OnComment(ce);
                }
            }
        }

        public event CommentEventHandler CommentHandler;

        protected virtual void OnComment(CommentEventArgs e)
        {
            if (CommentHandler != null)
            {
                CommentHandler(this, e);
            }
        }

        public  void NewPost()
        {
            string url = Constants.SERVER_NAME + Constants.URL_POST_NEW;
            string file = "wpupload"; 
            string paramName = "file";
            string contentType = "image/jpeg";
            Dictionary<string, string> nvc = new Dictionary<string, string>();

            nvc.Add("cat", App.SSAPI.getCatList());
            nvc.Add("time", Utils.ConvertToUnixTimestamp(App.ViewModel.StoryTime).ToString());


            if (App.ViewModel.StoryLat != 0.0)
                nvc.Add("geo_lat", App.ViewModel.StoryLat.ToString());
            if (App.ViewModel.StoryLng != 0.0)
                nvc.Add("geo_lng", App.ViewModel.StoryLng.ToString());
            nvc.Add("location", HttpUtility.UrlEncode(App.ViewModel.StoryLocation).Replace("+", "%20"));

            if (!string.IsNullOrEmpty(App.ViewModel.StoryPlace) && !string.IsNullOrWhiteSpace(App.ViewModel.StoryPlace))
                nvc.Add("place", HttpUtility.UrlEncode(App.ViewModel.StoryPlace).Replace("+", "%20"));
            if (!string.IsNullOrEmpty(App.ViewModel.StoryEvent) && !string.IsNullOrWhiteSpace(App.ViewModel.StoryEvent))
                nvc.Add("event", HttpUtility.UrlEncode(App.ViewModel.StoryEvent).Replace("+", "%20"));
            if (!string.IsNullOrEmpty(App.ViewModel.PicStory) && !string.IsNullOrWhiteSpace(App.ViewModel.PicStory))
                nvc.Add("descr", HttpUtility.UrlEncode(App.ViewModel.PicStory).Replace("+", "%20"));
            nvc.Add("token", App.ViewModel.User.Token);

            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            httpWebRequest.Method = "POST";
            //wr.KeepAlive = true;
            //wr.Credentials = System.Net.CredentialCache.DefaultCredentials;
#if (DEBUG)
            string auth = Constants.BASE_AUTH_USERNAME + ":" + Constants.BASE_AUTH_PASSWORD;
            string authString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(auth));
            httpWebRequest.Headers[HttpRequestHeader.Authorization] = "Basic " + authString;
#endif

            //Stream rs = wr.GetRequestStream();
            httpWebRequest.BeginGetRequestStream((result) =>
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)result.AsyncState;
                    using (Stream requestStream = request.EndGetRequestStream(result))
                    {
                        string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                        foreach (string key in nvc.Keys)
                        {
                            requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                            string formitem = string.Format(formdataTemplate, key, nvc[key]);
                            byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                            requestStream.Write(formitembytes, 0, formitembytes.Length);
                        }
                        requestStream.Write(boundarybytes, 0, boundarybytes.Length);

                        string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                        string header = string.Format(headerTemplate, paramName, file, contentType);
                        byte[] headerbytes = Encoding.UTF8.GetBytes(header);
                        requestStream.Write(headerbytes, 0, headerbytes.Length);

                        using (MemoryStream stream = new MemoryStream())
                        {                            
                            App.ViewModel.WriteableSelectedBitmap.SaveJpeg(stream, App.ViewModel.WriteableSelectedBitmap.PixelWidth, App.ViewModel.WriteableSelectedBitmap.PixelHeight, 0, 100);
                            stream.Seek(0, SeekOrigin.Begin);
                            byte[] buffer = new byte[4096];
                            int bytesRead = 0;
                            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                requestStream.Write(buffer, 0, bytesRead);
                            }
                            stream.Close();
                        }

                        byte[] trailer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                        requestStream.Write(trailer, 0, trailer.Length);

                        requestStream.Close();
                    }

                    request.BeginGetResponse(a =>
                    {
                        try
                        {
                            var response = request.EndGetResponse(a);
                            var responseStream = response.GetResponseStream();
                            using (var sr = new StreamReader(responseStream))
                            {
                                string serverresult = string.Format(sr.ReadToEnd());

                                Deployment.Current.Dispatcher.BeginInvoke(()=>
                                {
                                    XElement xmlResponse = XElement.Parse(serverresult);
                                    XElement xmlItems = xmlResponse.Element("items");

                                    if (xmlItems != null)
                                    {
                                        ObservableCollection<ItemViewModel> items = new ObservableCollection<ItemViewModel>();
                                        foreach (XElement xmlItem in xmlItems.Elements("item"))
                                        {

                                            ItemViewModel item = getItemInfoFromXML(xmlItem);
                                            items.Add(item);
                                        }
                                        PostEventArgs pe = new PostEventArgs(true);
                                        pe.Items = items;
                                        OnNewPost(pe);
                                    }
                                    else
                                    {
                                        PostEventArgs pe = new PostEventArgs(false);
                                        OnNewPost(pe);
                                    }
                                             
                                });

                                            
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.StackTrace);
                            PostEventArgs pe = new PostEventArgs(false);
                            OnNewPost(pe);
                        }
                    }, null);
                }
                catch (Exception e)
                {
                    PostEventArgs pe = new PostEventArgs(false);
                    OnNewPost(pe);
                }

            }, httpWebRequest);
        }
        public event PostEventHandler NewPostHandler;

        protected virtual void OnNewPost(PostEventArgs e)
        {
            if (NewPostHandler != null)
            {
                NewPostHandler(this, e);
            }
        }

    }

    public delegate void RegisterEventHandler(object sender, RegisterEventArgs e);
    public delegate void SearchEventHandler(object sender, SearchEventArgs e);
    public delegate void LoginEventHandler(object sender, LoginEventArgs e);
    public delegate void AlertEventHandler(object sender, AlertEventArgs e);
    public delegate void KeywordEventHandler(object sender, KeywordEventArgs e);
    public delegate void CommentEventHandler(object sender, CommentEventArgs e);
    public delegate void PostEventHandler(object sender, PostEventArgs e);

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
	
	    public Double search_lat = Double.NaN;
        public Double search_lng = Double.NaN;

	    public String token;
	    public String search_cat_name;
	    public String search_event_name;
	    public String search_place_name;
	    public String search_location_name;
        public String search_user_name;
        public String logic;
	
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
                if (!search_lat.Equals(Double.NaN))
                    sb.Append("&search_lat=" + search_lat);
                if (!search_lng.Equals(Double.NaN))
                    sb.Append("&search_lng=" + search_lng);
                if (search_user_name != null)
                    sb.Append("&search_user_name=" + search_user_name);
                if (search_cat_name != null)
                    sb.Append("&search_cat_name=" + search_cat_name);
                if (logic != null)
                    sb.Append("&logic=" + logic);
                if (search_event_name != null)
                    sb.Append("&search_event_name=" + search_event_name);
                if (search_place_name != null)
                    sb.Append("&search_place_name=" + search_place_name);
                if (search_location_name != null)
                    sb.Append("&search_location_name=" + search_location_name);
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
        public static string[] CATEGORY_FILTER_NAMES = { null, "celebrities", "musicians", "politicians", "models", "athletes" };
	}

    public class SearchToken
    {
        public int searchGroup;
        public bool isFresh;
        public int start;
    }
	
}
