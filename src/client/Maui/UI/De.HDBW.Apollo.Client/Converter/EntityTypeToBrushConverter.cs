// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Course;

namespace De.HDBW.Apollo.Client.Converter
{
    public class EntityTypeToBrushConverter : IValueConverter
    {
        public Brush? AssessmentItemBrush { get; set; }

        public Brush? CourseItemBrush { get; set; }

        public Brush? DefaultBrush { get; set; }

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == BindableProperty.UnsetValue)
            {
                return Binding.DoNothing;
            }

            Type? type = value as Type;
            switch (type?.Name)
            {
                case nameof(AssessmentItem):
                    return AssessmentItemBrush;
                case nameof(CourseItem):
                    return CourseItemBrush;
                default:
                    return DefaultBrush;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"Twoway Binding not supported in {GetType()}.");
        }
    }
}
