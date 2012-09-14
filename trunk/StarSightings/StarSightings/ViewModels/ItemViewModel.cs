﻿using System;
using System.ComponentModel;
using System.Diagnostics;
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
    public class ItemViewModel : INotifyPropertyChanged
    {
        private string photoId;
	    private string geoLat;
	    private string geoLng;
	    private string source;
	    private string forumId;
	    private string topicId;
	    private string postId;
	    private string name;
	    private string descr;
	    private string location;
	    private string eventName;
	    private string place;
	    private string time;
	    private string localTime;
	    private string country;
	    private string sourceUrl;
	    private string view_cnt;
	    private string userId;
	    private Boolean canEdit; //Boolean?
	    private string thumbUserSmall;
	    private string thumbUserLarge;
	    private string thumbOrigSmall;
	    private string thumbOrigLarge;
	    private string cat;
	    private string types;
	    private string maxBid;
	    private string maxBidTime;
	    private string bidCnt;
	    private string visibleMode;
	    private Boolean hidden; //Boolean?
                        
        public string PhotoId
        {
            get
            {
                return this.photoId;
            }
            set
            {
                if (value != this.photoId)
                {
                    this.photoId = value;
                    NotifyPropertyChanged("PhotoId");
                }                
            }
        }

        public string GeoLat
        {
            get
            {
                return this.geoLat;
            }
            set
            {
                if (value != this.geoLat)
                {
                    this.geoLat = value;
                    NotifyPropertyChanged("GeoLat");
                }                          
            }
        }

        public string GeoLng
        {
            get
            {
                return this.geoLng;
            }
            set
            {
                if (value != this.geoLng)
                {
                    this.geoLng = value;
                    NotifyPropertyChanged("GeoLng");
                }
            }
        }

        public string Source
        {
            get
            {
                return this.source;
            }
            set
            {
                if (value != this.source)
                {
                    this.source = value;
                    NotifyPropertyChanged("Source");
                }
            }
        }

        public string ForumId
        {
            get
            {
                return this.forumId;
            }
            set
            {
                if (value != forumId)
                {
                    this.forumId = value;
                    NotifyPropertyChanged("ForumId");
                }
            }
        }

        public string TopicId
        {
            get
            {
                return this.topicId;
            }
            set
            {
                if (value != topicId)
                {
                    this.topicId = value;
                    NotifyPropertyChanged("TopicId");
                }
            }
        }

        public string PostId
        {
            get
            {
                return this.postId;
            }
            set
            {
                if (value != this.postId)
                {
                    this.postId = value;
                    NotifyPropertyChanged("PostId");
                }
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (value != this.name)
                {
                    this.name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        public string Descr
        {
            get
            {
                return this.descr;
            }
            set
            {
                if (value != this.descr)
                {
                    this.descr = value;
                    NotifyPropertyChanged("Descr");
                }
            }
        }

        public string Location
        {
            get
            {
                return this.location;
            }
            set
            {
                if (value != this.location)
                {
                    this.location = value;
                    NotifyPropertyChanged("Location");
                }
            }
        }

        public string EventName
        {
            get
            {
                return this.eventName;
            }
            set
            {
                if (value != this.eventName)
                {
                    this.eventName = value;
                    NotifyPropertyChanged("EventName");
                }
            }
        }

        public string Place
        {
            get
            {
                return this.place;
            }
            set
            {
                if (value != this.place)
                {
                    this.place = value;
                    NotifyPropertyChanged("Place");
                }
            }
        }

        public string Time
        {
            get
            {
                return this.time;
            }
            set
            {
                if (value != this.time)
                {
                    this.time = value;
                    NotifyPropertyChanged("Time");
                }
            }
        }

        public string LocalTime
        {
            get
            {
                return this.localTime;
            }
            set
            {
                if (value != this.localTime)
                {
                    this.localTime = value;
                    NotifyPropertyChanged("LocalTime");
                }
            }
        }

        public string Country
        {
            get
            {
                return this.country;
            }
            set
            {
                if (value != this.country)
                {
                    this.country = value;
                    NotifyPropertyChanged("Country");
                }
            }
        }

        public string SourceUrl
        {
            get
            {
                return this.sourceUrl;
            }
            set
            {
                if (value != this.sourceUrl)
                {
                    this.sourceUrl = value;
                    NotifyPropertyChanged("SourceUrl");
                }
            }
        }

        public string ViewCnt
        {
            get
            {
                return this.view_cnt;
            }
            set
            {
                if (value != this.view_cnt)
                {
                    this.view_cnt = value;
                    NotifyPropertyChanged("ViewCnt");
                }
            }
        }

        public string UserId
        {
            get
            {
                return this.userId;
            }
            set
            {
                if (value != UserId)
                {
                    this.userId = value;
                    NotifyPropertyChanged("UserId");
                }
            }
        }

        public Boolean CanEdit
        {
            get
            {
                return this.canEdit;
            }
            set
            {
                if (value != this.canEdit)
                {
                    this.canEdit = value;
                    NotifyPropertyChanged("CanEdit");
                }
            }
        }

        public string ThumbUserSmall
        {
            get
            {
                return this.thumbUserSmall;
            }
            set
            {
                if (value != this.thumbUserSmall)
                {
                    this.thumbUserSmall = value;
                    NotifyPropertyChanged("ThumbUserSmall");
                }
            }
        }

        public string ThumbUserLarge
        {
            get
            {
                return this.thumbUserLarge;
            }
            set
            {
                if (value != this.thumbUserLarge)
                {
                    this.thumbUserLarge = value;
                    NotifyPropertyChanged("ThumbUserLarge");
                }
            }
        }

        public string ThumbOrigSmall
        {
            get
            {
                return this.thumbOrigSmall;
            }
            set
            {
                if (value != this.thumbOrigSmall)
                {
                    this.thumbOrigSmall = value;
                    NotifyPropertyChanged("ThumbOrigSmall");
                }
            }
        }

        public string ThumbOrigLarge
        {
            get
            {
                return this.thumbOrigLarge;
            }
            set
            {
                if (value != this.thumbOrigLarge)
                {
                    this.thumbOrigLarge = value;
                    NotifyPropertyChanged("ThumbOrigLarge");
                }
            }
        }

        public string Cat
        {
            get
            {
                return this.cat;
            }
            set
            {
                if (value != this.cat)
                {
                    this.cat = value;
                    NotifyPropertyChanged("Cat");
                }
            }
        }

        public string Types
        {
            get
            {
                return this.types;
            }
            set
            {
                if (value != this.types)
                {
                    this.types = value;
                    NotifyPropertyChanged("Types");
                }
            }
        }

        public string MaxBid
        {
            get
            {
                return this.maxBid;
            }
            set
            {
                if (value != this.maxBid)
                {
                    this.maxBid = value;
                    NotifyPropertyChanged("MaxBid");
                }
            }
        }

        public string MaxBidTime
        {
            get
            {
                return this.maxBidTime;
            }
            set
            {
                if (value != this.maxBidTime)
                {
                    this.maxBidTime = value;
                    NotifyPropertyChanged("MaxBidTime");
                }
            }
        }

        public string BidCnt
        {
            get
            {
                return this.bidCnt;
            }
            set
            {
                if (value != this.bidCnt)
                {
                    this.bidCnt = value;
                    NotifyPropertyChanged("BidCnt");
                }
            }
        }

        public string VisibleMode
        {
            get
            {
                return this.visibleMode;
            }
            set
            {
                if (value != this.visibleMode)
                {
                    this.visibleMode = value;
                    NotifyPropertyChanged("VisibleMode");
                }
            }
        }

        public Boolean Hidden
        {
            get
            {
                return this.hidden;
            }
            set
            {
                if (value != this.hidden)
                {
                    this.hidden = value;
                    NotifyPropertyChanged("Hidden");
                }
            }
        }

        private string _lineOne;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string LineOne
        {
            get
            {
                return _lineOne;
            }
            set
            {
                if (value != _lineOne)
                {
                    _lineOne = value;
                    NotifyPropertyChanged("LineOne");
                }
            }
        }

        private string _lineTwo;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string LineTwo
        {
            get
            {
                return _lineTwo;
            }
            set
            {
                if (value != _lineTwo)
                {
                    _lineTwo = value;
                    NotifyPropertyChanged("LineTwo");
                }
            }
        }

        private string _lineThree;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string LineThree
        {
            get
            {
                return _lineThree;
            }
            set
            {
                if (value != _lineThree)
                {
                    _lineThree = value;
                    NotifyPropertyChanged("LineThree");
                }
            }
        }

        private Uri imageSource;
        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        public Uri ImageSource
        {
            get
            {
                return this.imageSource;
            }
            set
            {
                if (this.imageSource != value)
                {
                    this.imageSource = value;
                    NotifyPropertyChanged("ImageSource");
                }
            }
        }

        private int id;
        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        public int ID
        {
            get
            {
                return this.id;
            }
            set
            {
                if (this.id != value)
                {
                    this.id = value;
                    NotifyPropertyChanged("ID");
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
