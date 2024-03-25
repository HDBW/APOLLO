// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Data.Helper;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;

namespace De.HDBW.Apollo.Client.Selector
{
    public class CareerInfoTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? OtherTemplate { get; set; }

        public DataTemplate? WorkExperienceTemplate { get; set; }

        public DataTemplate? PartTimeWorkExperienceTemplate { get; set; }

        public DataTemplate? InternshipTemplate { get; set; }

        public DataTemplate? SelfEmploymentTemplate { get; set; }

        public DataTemplate? ServiceTemplate { get; set; }

        public DataTemplate? CommunityServiceTemplate { get; set; }

        public DataTemplate? VoluntaryServiceTemplate { get; set; }

        public DataTemplate? ParentalLeaveTemplate { get; set; }

        public DataTemplate? HomemakerTemplate { get; set; }

        public DataTemplate? ExtraOccupationalExperienceTemplate { get; set; }

        public DataTemplate? PersonCareTemplate { get; set; }

        public DataTemplate? DefaultTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            var info = item as CareerInfo;
            switch (info?.CareerType.AsEnum<CareerType>())
            {
                case CareerType.Other:
                    return OtherTemplate;
                case CareerType.WorkExperience:
                    return WorkExperienceTemplate;
                case CareerType.PartTimeWorkExperience:
                    return PartTimeWorkExperienceTemplate;
                case CareerType.Internship:
                    return InternshipTemplate;
                case CareerType.SelfEmployment:
                    return SelfEmploymentTemplate;
                case CareerType.Service:
                    return ServiceTemplate;
                case CareerType.CommunityService:
                    return CommunityServiceTemplate;
                case CareerType.VoluntaryService:
                    return VoluntaryServiceTemplate;
                case CareerType.ParentalLeave:
                    return ParentalLeaveTemplate;
                case CareerType.Homemaker:
                    return HomemakerTemplate;
                case CareerType.ExtraOccupationalExperience:
                    return ExtraOccupationalExperienceTemplate;
                case CareerType.PersonCare:
                    return PersonCareTemplate;
                default:
                    return DefaultTemplate;
            }
        }
    }
}
