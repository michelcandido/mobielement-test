﻿using System;
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
    }
}
