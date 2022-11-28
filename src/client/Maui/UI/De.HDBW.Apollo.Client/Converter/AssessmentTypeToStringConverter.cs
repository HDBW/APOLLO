// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace De.HDBW.Apollo.Client.Converter
{
    public class AssessmentTypeToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var assessment = value as AssessmentItem;
            var result = string.Empty;

            switch (assessment?.AssessmentType)
            {
                case AssessmentType.SkillAssessment:
                    result = Resources.Strings.Resource.AssessmentType_SkillAssessment;
                    break;
                case AssessmentType.SoftSkillAssessment:
                    result = Resources.Strings.Resource.AssessmentType_SoftSkillAssessment;
                    break;
                case AssessmentType.Survey:
                    result = Resources.Strings.Resource.AssessmentType_Survey;
                    break;
                case AssessmentType.ExperienceAssessment:
                    result = Resources.Strings.Resource.AssessmentType_ExperienceAssessment;
                    break;
                case AssessmentType.Cloze:
                    result = Resources.Strings.Resource.AssessmentType_Cloze;
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
