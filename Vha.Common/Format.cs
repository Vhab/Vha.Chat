/*
* Vha.Common
* Copyright (C) 2005-2010 Remco van Oosterhout
* See Credits.txt for all acknowledgements.
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.Text;
using System.Globalization;

namespace Vha.Common
{
    public enum FormatStyle
    {
        Compact,
        Medium,
        Large
    }
    public static class Format
    {
        public static string Date(long timestamp, FormatStyle style) { return Date(TimeStamp.ToDateTime(timestamp), style); }
        public static string Date(DateTime date, FormatStyle style)
        {
            DateTimeFormatInfo dtfi = new CultureInfo("en-US", false).DateTimeFormat;
            switch (style)
            {
                case FormatStyle.Compact:
                    return date.ToString("dd/MM/yyyy", dtfi);
                case FormatStyle.Large:
                    return date.ToString("dddd, MMMM d, yyyy", dtfi);
                default:
                    return date.ToString("d MMMM yyyy", dtfi);
            }
        }

        public static string DateTime(long timestamp, FormatStyle style) { return DateTime(TimeStamp.ToDateTime(timestamp), style); }
        public static string DateTime(DateTime date, FormatStyle style)
        {
            return Date(date, style) + ", " + Time(date, style);
        }

        public static string Time(TimeSpan time, FormatStyle style)
        {
            switch (style)
            {
                case FormatStyle.Compact:
                    return string.Format("{0:00}:{1:00}", Math.Floor(time.TotalHours), time.Minutes);
                case FormatStyle.Large:
                    return string.Format("{0} hours, {1} minutes and {2} seconds", Math.Floor(time.TotalHours), time.Minutes, time.Seconds);
                default:
                    return string.Format("{0:00}:{1:00}:{2:00}", Math.Floor(time.TotalHours), time.Minutes, time.Seconds);
            }
        }

        public static string Time(DateTime time, FormatStyle style)
        {
            switch (style)
            {
                case FormatStyle.Compact:
                    return string.Format("{0:00}:{1:00}", time.Hour, time.Minute);
                default:
                    return string.Format("{0:00}:{1:00}:{2:00}", time.Hour, time.Minute, time.Second);
            }
        }

        public static string UppercaseFirst(string text)
        {
            if (text == null)
                return string.Empty;

            if (text.Length < 1)
                return text;

            char[] charArray = text.ToCharArray();
            StringBuilder builder = new StringBuilder();
            bool first = true;
            for (int i = 0; i < charArray.Length; i++)
            {
                if (first)
                    builder.Append(charArray[i].ToString().ToUpper());
                else
                    builder.Append(charArray[i].ToString().ToLower());
                first = true;
                if (charArray[i] >= 65 && charArray[i] <= 90) // upper case chars
                    first = false;
                if (charArray[i] >= 97 && charArray[i] <= 122) // lower case chars
                    first = false;
                if (charArray[i] >= 48 && charArray[i] <= 57) // numbers
                    first = false;
            }
            return builder.ToString();
        }
    }
}
