// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Course;

namespace De.HDBW.Apollo.Client.Converter
{
    public class EntityTypeToColorConverter : IValueConverter
    {
        public Color? AssessmentItemColor { get; set; }

        public Color? CourseItemColor { get; set; }

        public Color? DefaultColor { get; set; }

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || value == BindableProperty.UnsetValue)
            {
                return Binding.DoNothing;
            }

            Type? type = value as Type;
            switch (type?.Name)
            {
                case nameof(AssessmentItem):
                    return AssessmentItemColor;
                case nameof(CourseItem):
                    return CourseItemColor;
                default:
                    return DefaultColor;
            }
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"Twoway Binding not supported in {GetType().Name}.");
        }
    }
}
