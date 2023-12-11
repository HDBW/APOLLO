// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.Course.Enums;

namespace De.HDBW.Apollo.Client.Converter
{
    public class CourseTypeToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var course = value as CourseItem;
            var result = string.Empty;
            switch (course?.CourseType)
            {
                case CourseType.InPerson:
                    result = Resources.Strings.Resources.CourseType_InPerson;
                    break;
                case CourseType.Online:
                    result = Resources.Strings.Resources.CourseType_Online;
                    break;
                case CourseType.OnAndOffline:
                    result = Resources.Strings.Resources.CourseType_OnAndOffline;
                    break;
                case CourseType.InHouse:
                    result = Resources.Strings.Resources.CourseType_InHouse;
                    break;
                case CourseType.All:
                    result = Resources.Strings.Resources.CourseType_All;
                    break;
            }

            return result;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"Twoway Binding not supported in {GetType().Name}.");
        }
    }
}
