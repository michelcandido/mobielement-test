using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Windows.Media;
using System.Collections;
using System.Collections.Generic;

namespace StarSightings.Converters
{
    public class TextLengthConverter : IValueConverter
    {
        //str = str.Insert(10, Environment.NewLine);
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || parameter == null)
                return 20;
            string name = (string)value;
            string sizeCase = (string)parameter;
            if (name.Contains(",")) 
            {
                if (sizeCase == "Summary")
                    return 20;
                else if (sizeCase == "List")
                    return 17.333;
                else
                    return 0;
            }
            else
                return 22.667;                
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static string TruncateTextToFitAvailableWidth(string text, double availableWidth, string fontName, double fontSize)
        {
            if (availableWidth <= 0)
                return string.Empty;

            
            
            /*
            if (availableWidth <= 0)
                return string.Empty;
            
            IEnumerator<Typeface> ie = Fonts.SystemTypefaces.GetEnumerator;
            while (ie.MoveNext())
            {
                
            }

            int foundCharIndex = BinarySearch(
                text.Length,
                availableWidth,
                predicate: (idxValue1, value2) =>
                {
                    FormattedText ft = new FormattedText(
                        text.Substring(0, idxValue1 + 1),
                        CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        typeface,
                        fontSize,
                        Brushes.Black,
                        numberSubstitution: null,
                        textFormattingMode: TextFormattingMode.Ideal);

                    return ft.WidthIncludingTrailingWhitespace.CompareTo(value2);
                });

            int numChars = (foundCharIndex < 0) ? ~foundCharIndex : foundCharIndex + 1;

            return text.Substring(0, numChars);
             * */
            return null;
        }

        /**
        <summary>
        See <see cref="T:System.Array.BinarySearch"/>. This implementation is exactly the same,
        except that it is not bound to any specific type of collection. The behavior of the
        supplied predicate should match that of the T.Compare method (for example, 
        <see cref="T:System.String.Compare"/>).
        </summary>
        */
        public static int BinarySearch<T>(
            int length,
            T value,
            Func<int, T, int> predicate) // idxValue1, value2, compareResult
        {
            return BinarySearch(0, length, value, predicate);
        }

        public static int BinarySearch<T>(
            int index,
            int length,
            T value,
            Func<int, T, int> predicate)
        {
            int lo = index;
            int hi = (index + length) - 1;

            while (lo <= hi)
            {
                int mid = lo + ((hi - lo) / 2);

                int compareResult = predicate(mid, value);

                if (compareResult == 0)
                    return mid;
                else if (compareResult < 0)
                    lo = mid + 1;
                else
                    hi = mid - 1;
            }

            return ~lo;
        }
    }
}
