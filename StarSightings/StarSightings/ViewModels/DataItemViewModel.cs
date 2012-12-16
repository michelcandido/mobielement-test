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
using Telerik.Windows.Controls;

namespace StarSightings
{
   public class DataItemViewModel : ViewModelBase
    {
        private Uri imageSource;
        private Uri imageThumbnailSource;
        private string title;
        private string information;
        private string group;

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
                    this.OnPropertyChanged("ImageSource");
                }
            }
        }

        /// <summary>
        /// Gets or sets the image thumbnail source.
        /// </summary>
        public Uri ImageThumbnailSource
        {
            get
            {
                return this.imageThumbnailSource;
            }
            set
            {
                if (this.imageThumbnailSource != value)
                {
                    this.imageThumbnailSource = value;
                    this.OnPropertyChanged("ImageThumbnailSource");
                }
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                if (this.title != value)
                {
                    this.title = value;
                    this.OnPropertyChanged("Title");
                }
            }
        }

        /// <summary>
        /// Gets or sets the information.
        /// </summary>
        public string Information
        {
            get
            {
                return this.information;
            }
            set
            {
                if (this.information != value)
                {
                    this.information = value;
                    this.OnPropertyChanged("Information");
                }
            }
        }

        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        public string Group
        {
            get
            {
                return this.group;
            }
            set
            {
                if (this.group != value)
                {
                    this.group = value;
                    this.OnPropertyChanged("Group");
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.title;
        }
    }
}
