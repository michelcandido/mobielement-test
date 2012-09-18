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

namespace StarSightings
{
    public class ServerAPI
    {
        private const bool TESTSERVER = true;
        
        private WebClient GetWebClient()
        {
            WebClient webClient = new WebClient();

            if (TESTSERVER)
            {
                string auth = Constants.BASE_AUTH_USERNAME + ":" + Constants.BASE_AUTH_PASSWORD;
                string authString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(auth));
                webClient.Headers["Authorization"] = "Basic " + authString;
            }
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
    }

    public delegate void RegisterEventHandler(object sender, RegisterEventArgs e);
}
