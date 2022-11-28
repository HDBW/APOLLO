// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.Course.Enums;

namespace De.HDBW.Apollo.Client.Converter
{
    public class CourseTagTypeToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var course = value as CourseItem;
            var result = string.Empty;
            switch (course?.CourseTagType)
            {
                case CourseTagType.AttendeeCertificate:
                    result = Resources.Strings.Resource.CourseTagType_AttendeeCertificate;
                    break;
                case CourseTagType.Admission:
                    result = Resources.Strings.Resource.CourseTagType_Admission;
                    break;
                case CourseTagType.Certificate:
                    result = Resources.Strings.Resource.CourseTagType_Certificate;
                    break;
                case CourseTagType.Course:
                    result = Resources.Strings.Resource.CourseTagType_Course;
                    break;
                case CourseTagType.InfoEvent:
                    result = Resources.Strings.Resource.CourseTagType_InfoEvent;
                    break;
                case CourseTagType.PartialQualification:
                    result = Resources.Strings.Resource.CourseTagType_PartialQualification;
                    break;
                case CourseTagType.Qualification:
                    result = Resources.Strings.Resource.CourseTagType_Qualification;
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
