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

        public const string APP_ID_FACEBOOK = "147665825278093";

        //URLS
        public const string URL_REGISTER_DEVICE = "/index.php?page=device&mode=register&mobile=1&v=4";
        public const string URL_UNREGISTER_DEVICE = "/index.php?page=device&mode=unregister&mobile=1&v=4";
        public const string URL_SEARCH = "/index.php?page=photo&mode=list&mobile=1&v=4";
        public const string URL_INDEX_PAGE_SEARCH = "/index.php?mobile=1&v=4";
        public const string URL_GET_DETAILS = "/index.php?page=post&v=4&mode=view&photo_id=";
        public const string URL_REGISTER_USER = "/index.php?page=profile&mode=register&mobile=1&v=4";
        public const string URL_LOGIN = "/index.php?page=profile&mode=login&mobile=1&v=4";
        public const string URL_LOGOUT = "/index.php?page=profile&mode=logout&mobile=1&v=4";
        public const string URL_ALERT = "/alerts/";
        
        public const string URL_SUGGEST = "/index.php?page=suggest&mobile=1&v=4";
        public const string URL_KEYWORD = "/index.php?mode=suggest&mobile=1&v=4";
        public const string URL_COMMENT_NEW = "/index.php?page=comment&mode=new&mobile=1&v=4";
        public const string URL_POST_NEW = "/index.php?page=photo&mode=create&mobile=1&v=4";
        public const string URL_POST_UPDATE = "/index.php?page=photo&mode=edit&mobile=1&v=4";
        public const string URL_VOTE = "/index.php?page=post&mode=vote&mobile=1&v=4";
        public const string URL_VIEW = "/photo/view/";

        public const int LIMIT=15;
        public const int SUMMARY_COUNT = 4;
        public const int COMMENT_COUNT = 10;        

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
        public const string KEYWORD_NAME = "celebrity";
        public const string KEYWORD_NAME_SUGGEST = "person";
        public const string KEYWORD_USER = "photographer";
        public const string KEYWORD_MY = "my";

        public const string ALERT_SET = "set/";
        public const string ALERT_REMOVE = "remove/";
        public const string ALERT_PAUSE = "pause/";
        public const string ALERT_RESUME = "resume/";
        public const string ALERT_GET = "get/";

        public const string ALERT_TYPE_EVENT = "event/";
        public const string ALERT_TYPE_LOCATION = "location/";
        public const string ALERT_TYPE_PLACE = "place/";
        public const string ALERT_TYPE_CELEBRITY = "celebrity/";
        public const string ALERT_TYPE_PHOTOGRAPHER = "photographer/";//"user" is also ok
        public const string ALERT_TYPE_GEO = "geo/";
        public const string ALERT_TYPE_ALL = "all";

        public const string ERROR_VOTE_DENIED = "600";
        public const string ERROR_VOTE_LIMIT = "602";
        public const string ERROR_COMMENT_DENIED = "700";
        public const string ERROR_COMMENT_LIMIT = "702";

        public const string ERROR_LOGIN_USERNAME = "201";
        public const string ERROR_LOGIN_PASSWORD = "203";
        public const string ERROR_REGISTER_USERNAME = "202";
        public const string ERROR_REGISTER_EMAIL_INVALID = "205";
        public const string ERROR_REGISTER_EMAIL_UNAVAILABLE = "206";
        public const string ERROR_PHOTO_TOO_LARGE = "403";

        public const string ERROR_UNKNOWN = "900";
        public const string ERROR_MAINTENANCE = "901";
        public const string ERROR_UPGRADE = "9999";
        public const string ERROR_NONETWORK = "9000";

        public const string ERROR_TOKEN_INVALID = "101";
        
        public const string ERROR_ALERT_SUBJECT_NOT_EXISTS = "501";
        public const string ERROR_ALERT_NOT_EXISTS = "502";

        public const string ICON_URI_BACK = "/Images/appbar.back.rest.png";
        public const string ICON_URI_NEXT = "/Images/appbar.next.rest.png";
        public const string ICON_URI_CANCEL = "/Images/appbar.close.rest.png";
        public const string ICON_URI_CONFIRM = "/Images/appbar.check.rest.png";

        public static readonly string[] categoryFilterNames = new string[] { "All", "Celebrities", "Musicians", "Politicians", "Models", "Athletes" };
        public static readonly string[] mapFilterNames = new string[] { "Near Me", "Near Map Center", "Expand" };
        public static readonly string[] followFilterNames = new string[] { "New", "All", "Photographers", "Friends" };
    }

}
