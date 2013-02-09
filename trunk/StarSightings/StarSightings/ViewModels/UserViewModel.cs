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
    public class UserViewModel : INotifyPropertyChanged
    {
        
        private string username;
        public string UserName
        {
            get
            {
                return this.username;
            }
            set
            {
                if (value != username)
                {
                    username = value;
                    NotifyPropertyChanged("UserName");
                }
            }
        }

        private string userType;
        public string UserType
        {
            get
            {
                return this.userType;
            }
            set
            {
                if (value != userType)
                {
                    userType = value;
                    NotifyPropertyChanged("UserType");
                }
            }
        }

        private string password;
        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                if (value != password)
                {
                    password = value;
                    NotifyPropertyChanged("Password");
                }
            }
        }

        private string passwordconfirm;
        public string PasswordConfirm
        {
            get
            {
                return this.passwordconfirm;
            }
            set
            {
                if (value != passwordconfirm)
                {
                    passwordconfirm = value;
                    NotifyPropertyChanged("PasswordConfirm");
                }
            }
        }

        private string useremail;
        public string UserEmail
        {
            get
            {
                return this.useremail;
            }
            set
            {
                if (value != useremail)
                {
                    useremail = value;
                    NotifyPropertyChanged("UserEmail");
                }
            }
        }

        private string token;
        public string Token
        {
            get
            {
                return this.token;
            }
            set
            {
                if (value != token)
                {
                    token = value;
                    NotifyPropertyChanged("Token");
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

        private double tokenExpiration;
        public double TokenExpiration
        {
            get
            {
                return this.tokenExpiration;
            }
            set
            {
                if (value != tokenExpiration)
                {
                    tokenExpiration = value;
                    NotifyPropertyChanged("TokenExpiration");
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
