// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Data.Extensions
{
    internal static class TimeSpanExtensions
    {
        internal static string ToTotalHoursAndMinutes(this TimeSpan? timeSpan)
        {
            string result = string.Empty;
            if (timeSpan == null)
            {
                return result;
            }

            return $"{timeSpan.Value.TotalHours:00}:{timeSpan.Value.Minutes:00)}";
        }
    }
}
