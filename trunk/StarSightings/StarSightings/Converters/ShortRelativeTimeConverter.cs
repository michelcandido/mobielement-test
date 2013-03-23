
using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.Phone.Controls.LocalizedResources;

namespace StarSightings.Converters
{
    /// <summary>
    /// Time converter to display elapsed time relatively to the present.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    public class ShortRelativeTimeConverter : IValueConverter
    {
        /// <summary>
        /// A minute defined in seconds.
        /// </summary>
        private const double Minute = 60.0;

        /// <summary>
        /// An hour defined in seconds.
        /// </summary>
        private const double Hour = 60.0 * Minute;

        /// <summary>
        /// A day defined in seconds.
        /// </summary>
        private const double Day = 24 * Hour;

        /// <summary>
        /// A week defined in seconds.
        /// </summary>
        private const double Week = 7 * Day;

        /// <summary>
        /// A month defined in seconds.
        /// </summary>
        private const double Month = 30.5 * Day;

        /// <summary>
        /// A year defined in seconds.
        /// </summary>
        private const double Year = 365 * Day;

        /// <summary>
        /// Abbreviation for the default culture used by resources files.
        /// </summary>
        private const string DefaultCulture = "en-US";


        /// <summary>
        /// Converts a 
        /// <see cref="T:System.DateTime"/>
        /// object into a string the represents the elapsed time 
        /// relatively to the present.
        /// </summary>
        /// <param name="value">The given date and time.</param>
        /// <param name="targetType">
        /// The type corresponding to the binding property, which must be of
        /// <see cref="T:System.String"/>.
        /// </param>
        /// <param name="parameter">(Not used).</param>
        /// <param name="culture">
        /// The culture to use in the converter.
        /// When not specified, the converter uses the current culture
        /// as specified by the system locale.
        /// </param>
        /// <returns>The given date and time as a string.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double time = 0;
            Double.TryParse((string)value, out time);

            string result;

            DateTime given = (Utils.ConvertFromUnixTimestamp(time)).ToLocalTime();

            DateTime current = DateTime.Now;

            TimeSpan difference = current - given;
            

            if (DateTimeFormatHelper.IsFutureDateTime(current, given))
            {
                // Future dates and times are not supported, but to prevent crashing an app
                // if the time they receive from a server is slightly ahead of the phone's clock
                // we'll just default to the minimum, which is "2 seconds ago".
                result = "2s";
            }

            if (difference.TotalSeconds >= Year)
            {
                int nYears = (int)(difference.TotalSeconds / Year);
                result = nYears + "y";
            }
            else if (difference.TotalSeconds >= Month)
            {
                // "x months ago"
                int nMonths = (int)(difference.TotalSeconds / Month);
                result = nMonths + "mo";
            }            
            else if (difference.TotalSeconds >= Week)
            {
                int nWeeks = (int)(difference.TotalSeconds / Week);
                result = nWeeks + "w";
            }
            else if (difference.TotalSeconds >= Day)
            {
                int nDays = (int)(difference.TotalSeconds / Day);
                result = nDays + "d";
            }            
            else if (difference.TotalSeconds >= Hour)
            {
                int nHours = (int)(difference.TotalSeconds / Hour);
                result = nHours + "h";
            }            
            else if (difference.TotalSeconds >= Minute)
            {
                int nMinutes = (int)(difference.TotalSeconds / Minute);
                result = nMinutes + "m";
            }
            else
            {
                // "x seconds ago" or default to "2 seconds ago" if less than two seconds.
                int nSeconds = ((int)difference.TotalSeconds > 1.0) ? (int)difference.TotalSeconds : 2;
                result = nSeconds + "s";
            }

            return result;
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="value">(Not used).</param>
        /// <param name="targetType">(Not used).</param>
        /// <param name="parameter">(Not used).</param>
        /// <param name="culture">(Not used).</param>
        /// <returns>null</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
