// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Data.Extensions
{
    internal static class StringExtensions
    {
        internal static long TryToLong(this string? item)
        {
            if (long.TryParse(item, out long result))
            {
                return result;
            }

            return 0;
        }
    }
}
