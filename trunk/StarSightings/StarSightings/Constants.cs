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

namespace StarSightings
{
    public class Constants
    {
        //Server name
        public const string SERVER_NAME = "http://test.starsightings.com";
        //public const String SERVER_NAME = "http://www.starsightings.com";

        //Basic HTTP authorizaton
        public const string BASE_AUTH_USERNAME = "starsight0";
        public const string BASE_AUTH_PASSWORD = "20zohtNAbcSzE15";

        //URLS
        public const string URL_REGISTER_DEVICE = "/index.php?page=device&mode=register&mobile=1";
        public const string URL_UNREGISTER_DEVICE = "/index.php?page=device&mode=unregister&mobile=1";
        public const string URL_SEARCH = "/index.php?page=photo&mode=list&mobile=1";
        public const string URL_INDEX_PAGE_SEARCH = "/index.php?mobile=1";
        public const string URL_GET_DETAILS = "/index.php?page=post&mode=view&photo_id=";
        public const string URL_REGISTER_USER = "/index.php?page=profile&mode=register&mobile=1";

        public const int LIMIT=15;
        public const int SEARCH_POPULAR = 0;
        public const int SEARCH_LATEST = 1;
        public const int SEARCH_NEAREST = 2;
        public const int SEARCH_FOLLOWING = 3;
    }
}
