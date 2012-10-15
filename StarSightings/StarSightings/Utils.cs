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
using Microsoft.Phone.Info;
using System.IO.IsolatedStorage;
using System.Text.RegularExpressions;
using System.Device.Location;
using System.Diagnostics; 

namespace StarSightings
{
    public static class Utils
    {
        public static Uri BuildUriWithAppendedParams(string baseUri, string query)
        {
            UriBuilder builder = new UriBuilder(baseUri);
            if (builder.Query != null && builder.Query.Length > 1)
                builder.Query = builder.Query.Substring(1) + "&" + query;
            else
                builder.Query = query;
            return builder.Uri;
        }

        private static readonly int ANIDLength = 32;  
        private static readonly int ANIDOffset = 2;  
        public static string GetManufacturer()  
        {  
            string result = string.Empty;  
            object manufacturer;  
            if (DeviceExtendedProperties.TryGetValue("DeviceManufacturer", out manufacturer))  
                result = manufacturer.ToString();  
  
            return result;  
        }  
  
        //Note: to get a result requires ID_CAP_IDENTITY_DEVICE  
        // to be added to the capabilities of the WMAppManifest  
        // this will then warn users in marketplace  
        public static byte[] GetDeviceUniqueID()  
        {  
            byte[] result = null;  
            object uniqueId;  
            if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))  
                result = (byte[])uniqueId;  
  
            return result;  
        }  
  
        // NOTE: to get a result requires ID_CAP_IDENTITY_USER  
        //  to be added to the capabilities of the WMAppManifest  
        // this will then warn users in marketplace  
        public static string GetWindowsLiveAnonymousID()  
        {  
            string result = string.Empty;  
            object anid;  
            if (UserExtendedProperties.TryGetValue("ANID", out anid))  
            {  
                if (anid != null && anid.ToString().Length >= (ANIDLength + ANIDOffset))  
                {  
                    result = anid.ToString().Substring(ANIDOffset, ANIDLength);  
                }  
            }  
  
            return result;  
        }

        /// <summary>
        /// Update a setting value for our application. If the setting does not
        /// exist, then add the setting.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool AddOrUpdateIsolatedStorageSettings(string key, Object value)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            bool valueChanged = false;

            // If the key exists
            if (settings.Contains(key))
            {
                // If the value has changed
                if (settings[key] != value)
                {
                    // Store the new value
                    settings[key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                settings.Add(key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        public static object GetIsolatedStorageSettings(string key)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains(key))
            {
                object value;
                if (settings.TryGetValue(key, out value))
                    return value;
            }
            return null;
        }

        public static bool RemoveIsolatedStorageSettings(string key)
        {
            return IsolatedStorageSettings.ApplicationSettings.Remove(key);
        }

        public static string UpdateStartIndex(String source, int value)
        {
            string start = "start=";
            int startIndex = source.IndexOf(start) + start.Length;

            string firstPart = source.Substring(0, startIndex);

            string lastPart = source.Substring(startIndex);
            int ampersand = lastPart.IndexOf("&");
            lastPart = lastPart.Substring(ampersand);

            return firstPart + value + lastPart;
        }

        public static bool IsValidEmail(string strIn)
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn,
                   @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
        }

        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }


        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        public enum DistanceIn { Miles, Kilometers };
        private static double ToRadian(this double val)
        {
            return (Math.PI / 180) * val;
        }
        public static double Between(this DistanceIn @in, GeoCoordinate here, GeoCoordinate there)
        {
            var r = (@in == DistanceIn.Miles) ? 3960 : 6371;
            var dLat = (there.Latitude - here.Latitude).ToRadian();
            var dLon = (there.Longitude - here.Longitude).ToRadian();
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(here.Latitude.ToRadian()) * Math.Cos(there.Latitude.ToRadian()) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            var d = r * c;
            return d;
        }
    }
}
