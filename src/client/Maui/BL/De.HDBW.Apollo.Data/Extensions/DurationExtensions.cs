// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models;

namespace De.HDBW.Apollo.Data.Extensions
{
    internal static class DurationExtensions
    {
        internal static string ToFormattedString(this Duration? item)
        {
            string result = string.Empty;
            if (item == null)
            {
                return result;
            }

            return $"{item.TotalHours:00}:{item.Minutes:00)}";
        }
    }
}
