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
            string type = (string)parameter;
            string name = (string)value;
            bool isFollowing = App.ViewModel.MyFollowingCelebs.Where(user => user.UserName == name).Count() != 0;
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
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
