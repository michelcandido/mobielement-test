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
using StarSightings.ViewModels;
using System.Collections.Generic;

namespace StarSightings.Events
{
    public class KeywordEventArgs : EventArgs
    {
        private readonly bool successful = false;
        private List<string> keywords;


        public KeywordEventArgs(bool successful)
        {
            this.successful = successful;
        }

        private string errorCode;
        public string ErrorCode
        {
            get
            {
                return this.errorCode;
            }
            set
            {
                this.errorCode = value;
            }
        }

        public List<string> Keywords
        {
            get
            {
                return this.keywords;
            }
            set
            {
                this.keywords = value;
            }
        }

        public bool Successful
        {
            get { return successful; }
        }

        
    }
}
