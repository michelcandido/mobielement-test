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
    public class CommandSampleViewModel
    {
        public ICommand RateThisAppCommand
        {
            get;
            private set;
        }

        public ICommand SendAnEmailCommand
        {
            get;
            private set;
        }

        public CommandSampleViewModel()
        {
            RateThisAppCommand = new RateThisAppCommand();
            SendAnEmailCommand = new SendAnEmailCommand();            
            AppVersion = System.Reflection.Assembly.GetExecutingAssembly().FullName.Split('=')[1].Split(',')[0];            
        }
        
        public string AppVersion
        {
            get;
            private set;
        }
    }
}
