/*
* VhaBot.Common
* Copyright (C) 2005-2008 Remco van Oosterhout
* See Credits.txt for all aknowledgements.
*
* This program is free software; you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation; version 2 of the License only.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307
* USA
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace VhaBot.Common
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
