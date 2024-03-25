// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;

namespace De.HDBW.Apollo.Client.Helper
{
    public static class DateTimeHelper
    {
        public static DateTime? ToUIDate(this DateTime? dateTime)
        {
            return dateTime != null ? ToUIDate(dateTime.Value) : null;
        }

        public static DateTime ToUIDate(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, DateTimeKind.Local);
        }

        public static DateTime? ToDTODate(this DateTime? dateTime)
        {
            return dateTime != null ? ToDTODate(dateTime.Value) : null;
        }

        public static DateTime ToDTODate(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, DateTimeKind.Utc);
        }

        public static string ToLongDateFormatStringWithoutDay(this DateTime dateTime)
        {
            return dateTime.ToString(GetDatePatternWithoutWeekday(CultureInfo.CurrentUICulture));
        }

        private static string GetDatePatternWithoutWeekday(CultureInfo cultureInfo)
        {
            string[] patterns = cultureInfo.DateTimeFormat.GetAllDateTimePatterns();

            string longPattern = cultureInfo.DateTimeFormat.LongDatePattern;

            string acceptablePattern = string.Empty;

            foreach (string pattern in patterns)
            {
                if (longPattern.Contains(pattern) && !pattern.Contains("ddd") && !pattern.Contains("dddd"))
                {
                    if (pattern.Length > acceptablePattern.Length)
                    {
                        acceptablePattern = pattern;
                    }
                }
            }

            if (string.IsNullOrEmpty(acceptablePattern))
            {
                return longPattern;
            }

            return acceptablePattern;
        }
    }
}
