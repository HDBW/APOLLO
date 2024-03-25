// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Data.Helper;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;

namespace De.HDBW.Apollo.Client.Selector
{
    public class EducationInfoTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? EducationTemplate { get; set; }

        public DataTemplate? CompanyBasedVocationalTrainingTemplate { get; set; }

        public DataTemplate? StudyTemplate { get; set; }

        public DataTemplate? VocationalTrainingTemplate { get; set; }

        public DataTemplate? FurtherEducationTemplate { get; set; }

        public DataTemplate? DefaultTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            var info = item as EducationInfo;
            switch (info?.EducationType.AsEnum<EducationType>())
            {
                case EducationType.Education:
                    return EducationTemplate;
                case EducationType.CompanyBasedVocationalTraining:
                    return CompanyBasedVocationalTrainingTemplate;
                case EducationType.Study:
                    return StudyTemplate;
                case EducationType.VocationalTraining:
                    return VocationalTrainingTemplate;
                case EducationType.FurtherEducation:
                    return FurtherEducationTemplate;
                default:
                    return DefaultTemplate;
            }
        }
    }
}
