// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Helper
{
    public static class DateTimeHelper
    {
        public static DateTime? ToUIDate(this DateTime? dateTime)
        {
            return dateTime != null ? new DateTime(dateTime.Value.Year, dateTime.Value.Month, dateTime.Value.Day, 0, 0, 0, DateTimeKind.Local) : null;
        }

        public static DateTime? ToDTODate(this DateTime? dateTime)
        {
            return dateTime != null ? new DateTime(dateTime.Value.Year, dateTime.Value.Month, dateTime.Value.Day, 0, 0, 0, DateTimeKind.Utc) : null;
        }
    }
}
