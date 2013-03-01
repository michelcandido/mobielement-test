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
using Microsoft.Phone.BackgroundTransfer;
using System.Windows.Resources;
using System.Linq;
using Facebook;

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

                XElement xmlValues = null;
                if (xmlResponse.Element("suggestions") != null)
                    xmlValues = xmlResponse.Element("suggestions").Element("values");

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
                XElement xmlValues = null;
                if (xmlResponse.Element("suggestions") != null)
                    xmlValues = xmlResponse.Element("suggestions").Element("values");
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
                XElement xmlValues = null;
                if (xmlResponse.Element("suggestions") != null)
                    xmlValues = xmlResponse.Element("suggestions").Element("values");
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

        public String getCatList(bool encode)
        {
            string result = "";
            foreach (string celeb in App.ViewModel.CelebNameList)
            {
                if (encode)
                    result += HttpUtility.UrlEncode(celeb) + ";";
                else
                    result += celeb + ";";
            }
            if (encode)
                result = result.Replace("+", "%20");
            return result.Substring(0, result.Length - 1);
        }

        public String getLocationSuggestionString()
        {
            //return "page=suggest&mode=place&mobile=1&v=3&cat=Bono&local_offset=-25200&location=Hoboken,%20New%20Jersey&geo_lat=40.74541&geo_lng=-74.03509&search_feets=105680&time=1350923604.361893";
            String toReturn = "mode=location&cat=";
            toReturn += getCatList(true);
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
            toReturn += getCatList(true);
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
            toReturn += getCatList(true);
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
                if (xmlUserInfo != null)
                {
                    user.UserName = getElementValue(xmlUserInfo, "username");
                    user.UserId = getElementValue(xmlUserInfo, "user_id");
                }
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
            baseUri += "&token=" + App.ViewModel.User.Token;
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

                    XElement xmlEntityInfo = xmlResponse.Element("entity_info");


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
            item.PhotoId = getElementValue(xmlItem,"photo_id");
            item.GeoLat = getElementValue(xmlItem, "geo_lat");
            item.GeoLng = getElementValue(xmlItem,"geo_lng");
            item.Source = getElementValue(xmlItem,"source");
            item.ForumId = getElementValue(xmlItem,"forum_id");
            item.TopicId = getElementValue(xmlItem,"topic_id");
            item.PostId = getElementValue(xmlItem,"post_id");
            item.Name = getElementValue(xmlItem,"name");
            item.Descr = getElementValue(xmlItem,"descr");
            item.Location = getElementValue(xmlItem,"location");
            item.EventName = getElementValue(xmlItem,"event");
            item.Place = getElementValue(xmlItem,"place");
            item.SourceUrl = getElementValue(xmlItem,"source_url");
            item.ViewCnt = getElementValue(xmlItem,"view_cnt");
            item.UserId = getElementValue(xmlItem,"user_id");
            item.CanEdit = getElementValue(xmlItem,"can_edit") == "1";
            item.ThumbUserSmall = getElementValue(xmlItem,"thumb_user_small");
            item.ThumbUserLarge = getElementValue(xmlItem,"thumb_user_large");
            item.ThumbOrigSmall = getElementValue(xmlItem,"thumb_orig_small");
            item.ThumbOrigLarge = getElementValue(xmlItem,"thumb_orig_large");
            item.Cat = getElementValue(xmlItem,"cat");
            item.Types = getElementValue(xmlItem,"types");
            item.MaxBid = getElementValue(xmlItem,"max_bid");
            item.MaxBidTime = getElementValue(xmlItem,"max_bid_time");
            item.BidCnt = getElementValue(xmlItem,"bid_cnt");
            item.VisibleMode = getElementValue(xmlItem,"visible_mode");
            item.Hidden = getElementValue(xmlItem,"hidden") == "1";
            item.Rights = getElementValue(xmlItem,"rights");
            item.HasPhoto = getElementValue(xmlItem,"has_photo") == "1";
            
            item.Time = getElementValue(xmlItem,"time");
            item.LocalTime = getElementValue(xmlItem,"local_time");
            item.LocalOffset = getElementValue(xmlItem,"local_offset");

//#if (DEBUG)
            if (xmlItem.Element("comments") != null && xmlItem.Element("comments").Attribute("count") != null) 
                item.CommentsCnt = xmlItem.Element("comments").Attribute("count").Value.Trim();
            
            XElement xmlComments = xmlItem.Element("comments");
            ObservableCollection<CommentViewModel> comments = new ObservableCollection<CommentViewModel>();
            if (xmlComments != null)
            {
                foreach (XElement xmlComment in xmlComments.Elements("c"))
                {
                    CommentViewModel comment = new CommentViewModel();
                    comment.CommentId = getElementAttribute(xmlComment, "id");
                    comment.CommentType = getElementAttribute(xmlComment, "type");
                    comment.Promoted = getElementAttribute(xmlComment, "p") == "1";
                    comment.Time = getElementValue(xmlComment, "time");
                    comment.CommentValue = getElementValue(xmlComment, "value");
                    comment.UserId = getElementValue(xmlComment, "user_id");
                    comment.User = getElementValue(xmlComment, "user");
                    comments.Insert(0,comment);
                }
                
                item.Comments = comments;
                item.CommentsSummaryList.Source = item.Comments;
                item.CommentsSummaryList.Filter += (s, a) => a.Accepted = item.Comments.IndexOf((CommentViewModel)a.Item) < Constants.COMMENT_COUNT;
            }
//#else
            /*
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
             * */
//#endif
            if (xmlItem.Element("votes") != null && xmlItem.Element("votes").Attribute("prompt") != null)
                item.VotesPrompt = xmlItem.Element("votes").Attribute("prompt").Value.Trim();

            XElement xmlVotes = xmlItem.Element("votes");
            ObservableCollection<VoteViewModel> votes = new ObservableCollection<VoteViewModel>();
            if (xmlVotes != null)
            {
                foreach (XElement xmlVote in xmlVotes.Elements("v"))
                {
                    VoteViewModel vote = new VoteViewModel();
                    vote.VoteValue = getElementAttribute(xmlVote, "value");
                    vote.Selected = getElementAttribute(xmlVote, "selected") == "1";
                    vote.ImageFilename = getElementAttribute(xmlVote, "img_file");                    
                    //vote.ImageFilename = Constants.SERVER_NAME + vote.ImageFilename + "_winphone.png";
                    vote.ImageFilename = vote.ImageFilename + "_winphone.png";
                    vote.Count = getElementValue(xmlVote, "count");
                    votes.Add(vote);
                }

                item.Votes = votes;                
            }

            // computed properties
            item.GeoLocation = getGeoCoordinate(item.GeoLat, item.GeoLng);
            item.Distance = Math.Round(Utils.Between(Utils.DistanceIn.Miles, App.ViewModel.MyLocation, item.GeoLocation));
            item.EventLocation = item.Place + (item.Place.Length == 0 ? "" : " in ") + item.Location;
            item.EventDescr = item.Descr.Length != 0 ? item.Descr : item.EventName;
            //item.ThumbOrigLarge = Constants.SERVER_NAME + item.ThumbOrigLarge;
            
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
                baseUri = Constants.SERVER_NAME + Constants.URL_ALERT + alertMethod + "?mobile=1&v=4";
            else
                baseUri = Constants.SERVER_NAME + Constants.URL_ALERT + alertMethod + subjectType + HttpUtility.UrlEncode(subjectName) + "?mobile=1&v=4";
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
                alert.Type = getElementValue(xmlAlert, "type");
                if (alert.Type.Equals("geo"))
                {
                    alert.GeoLat = xmlAlert.Element("lat").Value.Trim();
                    alert.GeoLng = xmlAlert.Element("lng").Value.Trim();
                }
                else
                {
                    alert.Name = getElementValue(xmlAlert, "name");
                }
                alert.Active = getElementValue(xmlAlert, "active") == "1";
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
                XElement xmlValues = null;
                if (xmlSuggestions != null)
                    xmlValues = xmlSuggestions.Element("values");

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

        public void SetVote(string value, int selected)
        {
            WebClient webClient = GetWebClient();
            webClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";

            string baseUri = Constants.SERVER_NAME + Constants.URL_VOTE;
            string query = "token=" + App.ViewModel.User.Token + "&photo_id=" + App.ViewModel.SelectedItem.PhotoId + "&value=" + HttpUtility.UrlEncode(value).Replace("+","%20") + "&selected=" + selected;
            Uri uri = Utils.BuildUriWithAppendedParams(baseUri, "");

            webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(HandleNewComment);
            webClient.UploadStringAsync(uri, query);
        }

        
        /*
        public event CommentEventHandler CommentHandler;

        protected virtual void OnComment(CommentEventArgs e)
        {
            if (CommentHandler != null)
            {
                CommentHandler(this, e);
            }
        }
        */
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
                XElement xmlError = xmlResponse.Element("error");

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
                else if (xmlError != null)
                {
                    string errorCode = string.Empty;                    
                    errorCode = getElementValue(xmlError,"code");
                    
                    CommentEventArgs ce = new CommentEventArgs(false);
                    ce.ErrorCode = errorCode;
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

        public void PostOnFacebook()
        {
            var fb = new FacebookClient(App.ViewModel.User.FBToken);

            byte[] data = null;
            using (MemoryStream stream = new MemoryStream())
            {
                App.ViewModel.WriteableSelectedBitmap.SaveJpeg(stream, App.ViewModel.WriteableSelectedBitmap.PixelWidth, App.ViewModel.WriteableSelectedBitmap.PixelHeight, 0, 100);
                stream.Seek(0, SeekOrigin.Begin);
                data = stream.GetBuffer();
                stream.Close();
            }

            fb.PostCompleted += (o, e) =>
            {
                if (e.Cancelled || e.Error != null)
                {
                    return;
                }

                var result = e.GetResultData();
            };

            var parameters = new Dictionary<string, object>();
            parameters["message"] = App.SSAPI.getCatList(false) + " @ " + App.ViewModel.StoryLocation + "\n" + App.ViewModel.PicStory;
            //parameters["place"] = App.ViewModel.StoryPlace;
            
            //parameters["name"] = "name";
            //parameters["caption"] = "caption";
            //parameters["description"] = "description";
            
            parameters["file"] = new FacebookMediaObject
                        {
                            ContentType = "image/jpeg",
                            FileName = "image.jpeg"
                        }.SetValue(data);

            fb.PostAsync("me/photos", parameters);
        }

        public void NewPost(bool test)
        {
            // prepare upload uri
            string baseUri = Constants.SERVER_NAME + Constants.URL_POST_NEW;
            /*
            string query = "cat=" + App.SSAPI.getCatList(true);
            query += "&time=" + Utils.ConvertToUnixTimestamp(App.ViewModel.StoryTime);
            query += "&location=" + HttpUtility.UrlEncode(App.ViewModel.StoryLocation).Replace("+", "%20");
            query += "&token=" + App.ViewModel.User.Token;

            if (App.ViewModel.StoryLat != 0.0 && App.ViewModel.StoryLng != 0.0)
            {
                query += "&geo_lat=" + App.ViewModel.StoryLat + "&geo_lng=" + App.ViewModel.StoryLng;                
            }
            if (!string.IsNullOrEmpty(App.ViewModel.StoryPlace) && !string.IsNullOrWhiteSpace(App.ViewModel.StoryPlace))
                query += "&place" + HttpUtility.UrlEncode(App.ViewModel.StoryPlace);
            if (!string.IsNullOrEmpty(App.ViewModel.StoryEvent) && !string.IsNullOrWhiteSpace(App.ViewModel.StoryEvent))
                query += "&event" + HttpUtility.UrlEncode(App.ViewModel.StoryEvent);
            if (!string.IsNullOrEmpty(App.ViewModel.PicStory) && !string.IsNullOrWhiteSpace(App.ViewModel.PicStory))
                query += "&descr" + HttpUtility.UrlEncode(App.ViewModel.PicStory);

            Uri uri = Utils.BuildUriWithAppendedParams(baseUri, query);
            */

            // save the picture to upload location
            Dictionary<string, string> nvc = new Dictionary<string, string>();

            if (test)
            {
                nvc.Add("visible_mode", "4");
            }

            nvc.Add("camera_info", App.ViewModel.CameraInfo);

            nvc.Add("cat", App.SSAPI.getCatList(false));
            nvc.Add("time", Utils.ConvertToUnixTimestamp(App.ViewModel.StoryTime).ToString());


            if (App.ViewModel.StoryLat != 0.0 && App.ViewModel.StoryLng != 0.0)
            {
                nvc.Add("geo_lat", App.ViewModel.StoryLat.ToString());
                nvc.Add("geo_lng", App.ViewModel.StoryLng.ToString());
            }

            nvc.Add("location", App.ViewModel.StoryLocation);

            if (!string.IsNullOrEmpty(App.ViewModel.StoryPlace) && !string.IsNullOrWhiteSpace(App.ViewModel.StoryPlace))
                nvc.Add("place", App.ViewModel.StoryPlace);
            if (!string.IsNullOrEmpty(App.ViewModel.StoryEvent) && !string.IsNullOrWhiteSpace(App.ViewModel.StoryEvent))
                nvc.Add("event", App.ViewModel.StoryEvent);
            if (!string.IsNullOrEmpty(App.ViewModel.PicStory) && !string.IsNullOrWhiteSpace(App.ViewModel.PicStory))
                nvc.Add("descr", App.ViewModel.PicStory);
            nvc.Add("token", App.ViewModel.User.Token);

            string FileName = @"/shared\transfers/" + Utils.ConvertToUnixTimestamp(App.ViewModel.StoryTime) + "_" + (new Random()).Next();
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (myIsolatedStorage.FileExists(FileName))
                {
                    myIsolatedStorage.DeleteFile(FileName);
                }

                using (IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(FileName, FileMode.Create, myIsolatedStorage))
                {
                    using (BinaryWriter writer = new BinaryWriter(fileStream))
                    {
                        string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                        foreach (string key in nvc.Keys)
                        {
                            writer.Write(boundarybytes, 0, boundarybytes.Length);
                            string formitem = string.Format(formdataTemplate, key, nvc[key]);
                            byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                            writer.Write(formitembytes, 0, formitembytes.Length);
                        }
                        writer.Write(boundarybytes, 0, boundarybytes.Length);

                        string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                        string header = string.Format(headerTemplate, "file", FileName, "image/jpeg");
                        byte[] headerbytes = Encoding.UTF8.GetBytes(header);
                        writer.Write(headerbytes, 0, headerbytes.Length);

                        using (MemoryStream stream = new MemoryStream())
                        {
                            App.ViewModel.WriteableSelectedBitmap.SaveJpeg(stream, App.ViewModel.WriteableSelectedBitmap.PixelWidth, App.ViewModel.WriteableSelectedBitmap.PixelHeight, 0, 100);
                            stream.Seek(0, SeekOrigin.Begin);
                            byte[] buffer = new byte[4096];
                            int bytesRead = 0;
                            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                writer.Write(buffer, 0, bytesRead);
                            }
                            stream.Close();
                        }

                        byte[] trailer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                        writer.Write(trailer, 0, trailer.Length);
                        
                        writer.Close();
                    }
                }
            }
            /*
            String tempJPEG = @"/shared\transfers/" + Utils.ConvertToUnixTimestamp(App.ViewModel.StoryTime) + "_" + (new Random()).Next() + ".jpg";

            // Create virtual store and file stream. Check for duplicate tempJPEG files.
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (myIsolatedStorage.FileExists(tempJPEG))
                {
                    myIsolatedStorage.DeleteFile(tempJPEG);
                }

                IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(tempJPEG);

                StreamResourceInfo sri = null;
                Uri picuri = new Uri(tempJPEG, UriKind.Relative);
                sri = Application.GetResourceStream(picuri);

                WriteableBitmap wb = App.ViewModel.WriteableSelectedBitmap;

                // Encode WriteableBitmap object to a JPEG stream.
                System.Windows.Media.Imaging.Extensions.SaveJpeg(wb, fileStream, wb.PixelWidth, wb.PixelHeight, 0, 85);

                //wb.SaveJpeg(fileStream, wb.PixelWidth, wb.PixelHeight, 0, 85);
                fileStream.Close();
            }
            */
            // create transfer request
            var btr = new BackgroundTransferRequest(new Uri(baseUri));
            btr.TransferPreferences = TransferPreferences.AllowCellularAndBattery;
            btr.Method = "POST";
            btr.Tag = FileName; 
            btr.Headers.Add("Content-Type", "multipart/form-data; boundary=" + boundary);
            //btr.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

#if (DEBUG)
            string auth = Constants.BASE_AUTH_USERNAME + ":" + Constants.BASE_AUTH_PASSWORD;
            string authString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(auth));
            btr.Headers.Add("Authorization", "Basic " + authString);
#endif

            //btr.UploadLocation = new Uri(tempJPEG, UriKind.Relative);
            btr.UploadLocation = new Uri(FileName, UriKind.Relative);
            btr.TransferStatusChanged += new EventHandler<BackgroundTransferEventArgs>(btr_TransferStatusChanged);
            btr.TransferProgressChanged += new EventHandler<BackgroundTransferEventArgs>(btr_TransferProgressChanged);

            try
            {
                BackgroundTransferService.Add(btr);
                App.ViewModel.IsUploading = true;
            }
            catch (Exception e)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    // Showing the exact error message is useful for debugging. In a finalized application, 
                    // output a friendly and applicable string to the user instead. 
                    MessageBox.Show("Unable to add background transfer request.");
                    App.Logger.log(LogLevel.error, e.Message);
                });                 
            }
        }

        public void btr_TransferProgressChanged(object sender, BackgroundTransferEventArgs e)
        {
            
        }

        public void btr_TransferStatusChanged(object sender, BackgroundTransferEventArgs e)
        {
            ProcessTransfer(e.Request);
        }

        private void ProcessTransfer(BackgroundTransferRequest transfer)
        {
            switch (transfer.TransferStatus)
            {
                case TransferStatus.Completed:

                    // If the status code of a completed transfer is 200 or 206, the
                    // transfer was successful
                    if (transfer.StatusCode == 200 || transfer.StatusCode == 206)
                    {
                        // Remove the transfer request in order to make room in the 
                        // queue for more transfers. Transfers are not automatically
                        // removed by the system.
                        RemoveTransferRequest(transfer.RequestId);

                        // In this example, the downloaded file is moved into the root
                        // Isolated Storage directory
                        using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                        {
                            string filename = transfer.Tag;
                            if (isoStore.FileExists(filename))
                            {
                                isoStore.DeleteFile(filename);
                            }                            
                        }
                        /*
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            // Showing the exact error message is useful for debugging. In a finalized application, 
                            // output a friendly and applicable string to the user instead. 
                            MessageBox.Show("Your post has been submitted successfully.");                            
                        });
                         * */
                        PostEventArgs pe = new PostEventArgs(true);                        
                        OnNewPost(pe);
                    }
                    else
                    {
                        // This is where you can handle whatever error is indicated by the
                        // StatusCode and then remove the transfer from the queue. 
                        RemoveTransferRequest(transfer.RequestId);

                        if (transfer.TransferError != null)
                        {
                            // Handle TransferError, if there is one.
                            /*
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                // Showing the exact error message is useful for debugging. In a finalized application, 
                                // output a friendly and applicable string to the user instead. 
                                MessageBox.Show("Errors in your submission, please try again.");
                            });
                             * */
                        }
                        PostEventArgs pe = new PostEventArgs(false);
                        OnNewPost(pe);
                    }
                    break;

                case TransferStatus.WaitingForExternalPower:
                    //WaitingForExternalPower = true;
                    break;

                case TransferStatus.WaitingForExternalPowerDueToBatterySaverMode:
                    //WaitingForExternalPowerDueToBatterySaverMode = true;
                    break;

                case TransferStatus.WaitingForNonVoiceBlockingNetwork:
                    //WaitingForNonVoiceBlockingNetwork = true;
                    break;

                case TransferStatus.WaitingForWiFi:
                    //WaitingForWiFi = true;
                    break;
            }
        }

        public void RemoveTransferRequest(string transferID)
        {
            // Use Find to retrieve the transfer request with the specified ID.
            BackgroundTransferRequest transferToRemove = BackgroundTransferService.Find(transferID);

            // Try to remove the transfer from the background transfer service.
            try
            {
                BackgroundTransferService.Remove(transferToRemove);
                if (BackgroundTransferService.Requests.Count<BackgroundTransferRequest>() <= 0)
                {
                    App.ViewModel.IsUploading = false;
                }
                else
                {
                    App.ViewModel.IsUploading = true;
                }
            }
            catch (Exception e)
            {
                // Handle the exception.
            }
        }

        public void UpdateRequestsList()
        {
            // The Requests property returns new references, so make sure that
            // you dispose of the old references to avoid memory leaks.
            if (App.ViewModel.TransferRequests != null)
            {
                foreach (var request in App.ViewModel.TransferRequests)
                {
                    request.Dispose();
                }
            }
            App.ViewModel.TransferRequests = BackgroundTransferService.Requests;
            if (App.ViewModel.TransferRequests.Count<BackgroundTransferRequest>() <= 0)
            {
                App.ViewModel.IsUploading = false;
            }
            else
            {
                App.ViewModel.IsUploading = true;
            }

        }

        public void InitialTansferStatusCheck()
        {
            UpdateRequestsList();

            foreach (var transfer in App.ViewModel.TransferRequests)
            {
                transfer.TransferStatusChanged += new EventHandler<BackgroundTransferEventArgs>(btr_TransferStatusChanged);
                transfer.TransferProgressChanged += new EventHandler<BackgroundTransferEventArgs>(btr_TransferProgressChanged);
                ProcessTransfer(transfer);                 
            }
        }

        public  void NewPost2(bool test)
        {
            string url = Constants.SERVER_NAME + Constants.URL_POST_NEW;
            string file = "wpupload"; 
            string paramName = "file";
            string contentType = "image/jpeg";
            Dictionary<string, string> nvc = new Dictionary<string, string>();

            if (test)
            {
                nvc.Add("visible_mode", "4");
            }

            nvc.Add("camera_info", App.ViewModel.CameraInfo);

            nvc.Add("cat", App.SSAPI.getCatList(false));
            nvc.Add("time", Utils.ConvertToUnixTimestamp(App.ViewModel.StoryTime).ToString());


            if (App.ViewModel.StoryLat != 0.0 && App.ViewModel.StoryLng != 0.0)
            {
                nvc.Add("geo_lat", App.ViewModel.StoryLat.ToString());
                nvc.Add("geo_lng", App.ViewModel.StoryLng.ToString());
            }
            
            nvc.Add("location", App.ViewModel.StoryLocation);

            if (!string.IsNullOrEmpty(App.ViewModel.StoryPlace) && !string.IsNullOrWhiteSpace(App.ViewModel.StoryPlace))
                nvc.Add("place", App.ViewModel.StoryPlace);
            if (!string.IsNullOrEmpty(App.ViewModel.StoryEvent) && !string.IsNullOrWhiteSpace(App.ViewModel.StoryEvent))
                nvc.Add("event", App.ViewModel.StoryEvent);
            if (!string.IsNullOrEmpty(App.ViewModel.PicStory) && !string.IsNullOrWhiteSpace(App.ViewModel.PicStory))
                nvc.Add("descr", App.ViewModel.PicStory);
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
                        /*
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
                        */
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

        private string getElementValue(XElement element, XName name)
        {
            if (element.Element(name) != null)
                return element.Element(name).Value.Trim();
            else
                return string.Empty;
        }

        private string getElementAttribute(XElement element, XName name)
        {
            if (element.Attribute(name) != null)
                return element.Attribute(name).Value.Trim();
            else
                return string.Empty;
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
        public String search_photographer_name; 
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
                if (search_photographer_name != null)
                    sb.Append("&search_photographer_name=" + search_photographer_name);
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
