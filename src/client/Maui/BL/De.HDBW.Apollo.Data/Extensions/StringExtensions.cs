// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.Course.Enums;

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

        internal static CourseTagType ToCourseTagType(this string str)
        {
            if (Enum.TryParse(str, out CourseTagType courseTagType))
            {
                return courseTagType;
            }

            return CourseTagType.Unknown;
        }
    }
}
