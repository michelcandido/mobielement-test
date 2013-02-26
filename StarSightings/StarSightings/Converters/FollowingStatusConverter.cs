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
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace StarSightings.Converters
{
    public class FollowingStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int mode = 0;
            string type = (string)parameter;
            string name = (string)value;
            bool isFollowing = false;
            if (type.Contains('@'))
                mode = 1;
            if (mode == 0)
            {
                isFollowing = App.ViewModel.MyFollowingCelebs.Where(user => user.UserName == name).Count() != 0;
            }
            else if (mode == 1)
            {
                type = type.Substring(1);
                isFollowing = App.ViewModel.Alerts.Where(alert => alert.Type == App.ViewModel.KeywordType).Where(alert => alert.Name.Equals(App.ViewModel.SearchKeywords, StringComparison.OrdinalIgnoreCase)).Count() != 0;                
            }

            if (type == "Color")
            {
                if (isFollowing)
                    return "#757a7c";
                else
                    return "#ee005e";
            }
            else if (type == "Text")
            {
                if (isFollowing)
                    return "Following";
                else
                    return "Follow";
            } 
            else if (type == "Visibility")
            {
                if (isFollowing)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
