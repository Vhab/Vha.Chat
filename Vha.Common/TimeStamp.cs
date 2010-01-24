/*
* Vha.Common
* Copyright (C) 2005-2010 Remco van Oosterhout
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

namespace Vha.Common
{
    public static class TimeStamp
    {
        /// <summary>
        /// Converts a DateTime to UnixTimeStamp. Assumes the time it has been given is GMT
        /// </summary>
        /// <param name="time"></param>
        /// <returns>UnixTimeStamp</returns>
        public static Int64 FromDateTime(DateTime time)
        {
            time = time.ToUniversalTime();
            TimeSpan span = time - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64(span.TotalSeconds);
        }

        /// <summary>
        /// Converts a UnixTimeStamp to DateTime.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(Int64 time)
        {
            DateTime datetime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            datetime = datetime.AddSeconds(Convert.ToDouble(time));
            return datetime;
        }

        /// <summary>
        /// Returns the current time in UnixTimeStamp format as GMT
        /// </summary>
        public static Int64 Now { get { return FromDateTime(DateTime.Now); } }
    }
}
