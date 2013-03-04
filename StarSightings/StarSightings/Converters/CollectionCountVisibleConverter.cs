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
using System.Windows.Data;
using System.Collections.ObjectModel;
using StarSightings.ViewModels;

namespace StarSightings.Converters
{
    public class CollectionCountVisibleConverter<T> : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;
            Collection<T> set = (Collection<T>)value;
            if (parameter != null)
            {
                if (set.Count == 0)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            else
            {
                if (set.Count == 0)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CommentVisibleConverter : CollectionCountVisibleConverter<CommentViewModel> { }
    public class FollowingVisibleConverter : CollectionCountVisibleConverter<UserViewModel> { }
    public class SightingsVisibleConverter : CollectionCountVisibleConverter<ItemViewModel> { }
}
