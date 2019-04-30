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

namespace Vha.Common
{
    public static class TimeStamp
    {
        /// <summary>
        /// Converts a DateTime to UnixTimeStamp. Assumes the time it has been given is GMT
        /// </summary>
        /// <param name="time"></param>
        /// <param name="assumeUtc">Whether to assume the time value is UTC</param>
        /// <returns>UnixTimeStamp</returns>
        public static Int64 FromDateTime(DateTime time, bool assumeUtc)
        {
            if (!assumeUtc) time = time.ToUniversalTime();
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
        public static Int64 Now { get { return FromDateTime(DateTime.Now, false); } }
    }
}
