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
        //	public static const String SERVER_NAME = "http://starsightings.com";

        //Basic HTTP authorizaton
        public const string BASE_AUTH_USERNAME = "starsight0";
        public const string BASE_AUTH_PASSWORD = "20zohtNAbcSzE15";

        //URLS
        public const string URL_REGISTER_DEVICE = "/index.php?page=device&mode=register&mobile=1";
        public const string URL_UNREGISTER_DEVICE = "/index.php?page=device&mode=unregister&mobile=1";
    }
}
