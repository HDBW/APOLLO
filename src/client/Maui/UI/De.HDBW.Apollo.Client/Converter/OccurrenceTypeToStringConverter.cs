// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.Course.Enums;

namespace De.HDBW.Apollo.Client.Converter
{
    public class OccurrenceTypeToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var course = value as CourseItem;
            var result = string.Empty;
            switch (course?.Occurrence)
            {
                case OccurrenceType.FullTime:
                    result = Resources.Strings.Resource.OccurrenceType_FullTime;
                    break;
                case OccurrenceType.PartTime:
                    result = Resources.Strings.Resource.OccurrenceType_PartTime;
                    break;
                case OccurrenceType.FullTimeAndPartTime:
                    result = Resources.Strings.Resource.OccurrenceType_FullTimeAndPartTime;
                    break;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"Twoway Binding not supported in {GetType().Name}.");
        }
    }
}
