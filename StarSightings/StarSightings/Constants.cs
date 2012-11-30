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
#if (DEBUG)
        public const string SERVER_NAME = "http://test.starsightings.com";
#else
        public const String SERVER_NAME = "http://www.starsightings.com";
#endif
        //Basic HTTP authorizaton
        public const string BASE_AUTH_USERNAME = "starsight0";
        public const string BASE_AUTH_PASSWORD = "20zohtNAbcSzE15";

        //URLS
        public const string URL_REGISTER_DEVICE = "/index.php?page=device&mode=register&mobile=1&v=3";
        public const string URL_UNREGISTER_DEVICE = "/index.php?page=device&mode=unregister&mobile=1&v=3";
        public const string URL_SEARCH = "/index.php?page=photo&mode=list&mobile=1&v=3";
        public const string URL_INDEX_PAGE_SEARCH = "/index.php?mobile=1&v=3";
        public const string URL_GET_DETAILS = "/index.php?page=post&v=3&mode=view&photo_id=";
        public const string URL_REGISTER_USER = "/index.php?page=profile&mode=register&mobile=1&v=3";
        public const string URL_LOGIN = "/index.php?page=profile&mode=login&mobile=1&v=3";
        public const string URL_LOGOUT = "/index.php?page=profile&mode=logout&mobile=1&v=3";
        public const string URL_ALERT = "/alerts/";
        public const string URL_KEYWORD = "/index.php?mode=suggest&mobile=1&v=3";

        public const int LIMIT=15;
        public const int SUMMARY_COUNT = 4;
        public const int COMMENT_COUNT = 2;
        public const int SEARCH_POPULAR = 0;
        public const int SEARCH_LATEST = 1;
        public const int SEARCH_NEAREST = 2;
        public const int SEARCH_FOLLOWING = 3;
        public const int SEARCH_KEYWORDSEARCH = 4;

        public const int ACCOUNT_TYPE_DEVICE = 0;
        public const int ACCOUNT_TYPE_SS = 1;
        public const int ACCOUNT_TYPE_FACEBOOK = 2;

        public const string KEYWORD_EVENT = "event";
        public const string KEYWORD_PLACE = "place";
        public const string KEYWORD_LOCATION = "location";
        public const string KEYWORD_NAME = "cat";

        public const string ALERT_SET = "set/";
        public const string ALERT_REMOVE = "remove/";
        public const string ALERT_PAUSE = "pause/";
        public const string ALERT_RESUME = "resume/";
        public const string ALERT_GET = "get/";

        public const string ALERT_TYPE_EVENT = "event/";
        public const string ALERT_TYPE_LOCATION = "location/";
        public const string ALERT_TYPE_PLACE = "place/";
        public const string ALERT_TYPE_CELEBRITY = "celebrity/";
        public const string ALERT_TYPE_PHOTOGRAPHER = "photographer/";
        public const string ALERT_TYPE_GEO = "geo/";
        public const string ALERT_TYPE_ALL = "all";
    }

}
