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
using System.ComponentModel;

namespace StarSightings.ViewModels
{
    public class CommentViewModel : INotifyPropertyChanged
    {
        
        private string commentId;
        public string CommentId
        {
            get
            {
                return this.commentId;
            }
            set
            {
                if (value != commentId)
                {
                    commentId = value;
                    NotifyPropertyChanged("CommentId");
                }
            }
        }

        private string time;
        public string Time
        {
            get
            {
                return this.time;
            }
            set
            {
                if (value != time)
                {
                    time = value;
                    NotifyPropertyChanged("Time");
                }
            }
        }

        private string commentType;
        public string CommentType
        {
            get
            {
                return this.commentType;
            }
            set
            {
                if (value != commentType)
                {
                    commentType = value;
                    NotifyPropertyChanged("CommentType");
                }
            }
        }

        private bool promoted;
        public bool Promoted
        {
            get
            {
                return this.promoted;
            }
            set
            {
                if (value != promoted)
                {
                    promoted = value;
                    NotifyPropertyChanged("Promoted");
                }
            }
        }

        private string commentValue;
        public string CommentValue
        {
            get
            {
                return this.commentValue;
            }
            set
            {
                if (value != commentValue)
                {
                    commentValue = value;
                    NotifyPropertyChanged("CommentValue");
                }
            }
        }

        private string userId;
        public string UserId
        {
            get
            {
                return this.userId;
            }
            set
            {
                if (value != userId)
                {
                    userId = value;
                    NotifyPropertyChanged("UserId");
                }
            }
        }

        private string user;
        public string User
        {
            get
            {
                return this.user;
            }
            set
            {
                if (value != user)
                {
                    user = value;
                    NotifyPropertyChanged("User");
                }
            }
        }

        private string userLevel;
        public string UserLevel
        {
            get
            {
                return this.userLevel;
            }
            set
            {
                if (value != userLevel)
                {
                    userLevel = value;
                    NotifyPropertyChanged("UserLevel");
                }
            }
        }

        private string facebookUid;
        public string FacebookUid
        {
            get
            {
                return this.facebookUid;
            }
            set
            {
                if (value != facebookUid)
                {
                    facebookUid = value;
                    NotifyPropertyChanged("FacebookUid");
                }
            }
        }

        private string buttonTemplateId;
        public string ButtonTemplateId
        {
            get
            {
                return this.buttonTemplateId;
            }
            set
            {
                if (value != buttonTemplateId)
                {
                    buttonTemplateId = value;
                    NotifyPropertyChanged("ButtonTemplateId");
                }
            }
        }

        private string buttonTemplatePrompt;
        public string ButtonTemplatePrompt
        {
            get
            {
                return this.buttonTemplatePrompt;
            }
            set
            {
                if (value != buttonTemplatePrompt)
                {
                    buttonTemplatePrompt = value;
                    NotifyPropertyChanged("ButtonTemplatePrompt");
                }
            }
        }
        

       

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
