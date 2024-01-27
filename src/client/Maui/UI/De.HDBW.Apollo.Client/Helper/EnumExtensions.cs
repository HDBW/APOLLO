using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;

namespace De.HDBW.Apollo.Client.Helper
{
    public static class EnumExtensions
    {
        public static string GetLocalizedString(this ContactType careerType)
        {
            return GetLocalizedString((ContactType?)careerType);
        }

        public static string GetLocalizedString(this ContactType? contactType)
        {
            switch (contactType)
            {
                case ContactType.Professional:
                    return Resources.Strings.Resources.ContactType_Professional;
                case ContactType.Private:
                    return Resources.Strings.Resources.ContactType_Private;
                default:
                    return string.Empty;
            }
        }

        public static string GetLocalizedString(this CareerType careerType)
        {
            return GetLocalizedString((CareerType?)careerType);
        }

        public static string GetLocalizedString(this CareerType? careerType)
        {
            switch (careerType)
            {
                case CareerType.Other:
                    return Resources.Strings.Resources.CareerType_Other;
                case CareerType.WorkExperience:
                    return Resources.Strings.Resources.CareerType_WorkExperience;
                case CareerType.PartTimeWorkExperience:
                    return Resources.Strings.Resources.CareerType_PartTimeWorkExperience;
                case CareerType.Internship:
                    return Resources.Strings.Resources.CareerType_Internship;
                case CareerType.SelfEmployment:
                    return Resources.Strings.Resources.CareerType_SelfEmployment;
                case CareerType.Service:
                    return Resources.Strings.Resources.ServiceType_MilitaryService;
                case CareerType.CommunityService:
                    return Resources.Strings.Resources.CareerType_CommunityService;
                case CareerType.VoluntaryService:
                    return Resources.Strings.Resources.CareerType_VoluntaryService;
                case CareerType.ParentalLeave:
                    return Resources.Strings.Resources.CareerType_ParentalLeave;
                case CareerType.Homemaker:
                    return Resources.Strings.Resources.CareerType_Homemaker;
                case CareerType.ExtraOccupationalExperience:
                    return Resources.Strings.Resources.CareerType_ExtraOccupationalExperience;
                case CareerType.PersonCare:
                    return Resources.Strings.Resources.CareerType_PersonCare;
                default:
                    return string.Empty;
            }
        }

        public static string GetLocalizedString(this ServiceType timeModel)
        {
            return GetLocalizedString((ServiceType?)timeModel);
        }

        public static string GetLocalizedString(this ServiceType? serviceType)
        {
            switch (serviceType)
            {
                case ServiceType.CivilianService:
                    return Resources.Strings.Resources.ServiceType_CivilianService;
                case ServiceType.MilitaryService:
                    return Resources.Strings.Resources.ServiceType_MilitaryService;
                case ServiceType.VoluntaryMilitaryService:
                    return Resources.Strings.Resources.ServiceType_VoluntaryMilitaryService;
                case ServiceType.MilitaryExercise:
                    return Resources.Strings.Resources.ServiceType_MilitaryExercise;
                default:
                    return string.Empty;
            }
        }

        public static string GetLocalizedString(this VoluntaryServiceType timeModel)
        {
            return GetLocalizedString((VoluntaryServiceType?)timeModel);
        }

        public static string GetLocalizedString(this VoluntaryServiceType? serviceType)
        {
            switch (serviceType)
            {
                case VoluntaryServiceType.Other:
                    return Resources.Strings.Resources.VoluntaryServiceType_Other;
                case VoluntaryServiceType.VoluntarySocialYear:
                    return Resources.Strings.Resources.VoluntaryServiceType_VoluntarySocialYear;
                case VoluntaryServiceType.FederalVolunteerService:
                    return Resources.Strings.Resources.VoluntaryServiceType_FederalVolunteerService;
                case VoluntaryServiceType.VoluntaryEcologicalYear:
                    return Resources.Strings.Resources.VoluntaryServiceType_VoluntaryEcologicalYear;
                case VoluntaryServiceType.VoluntarySocialTrainingYear:
                    return Resources.Strings.Resources.VoluntaryServiceType_VoluntarySocialTrainingYear;
                case VoluntaryServiceType.VoluntaryCulturalYear:
                    return Resources.Strings.Resources.VoluntaryServiceType_VoluntaryCulturalYear;
                case VoluntaryServiceType.VoluntarySocialYearInSport:
                    return Resources.Strings.Resources.VoluntaryServiceType_VoluntarySocialYearInSport;
                case VoluntaryServiceType.VoluntaryYearInMonumentConservation:
                    return Resources.Strings.Resources.VoluntaryServiceType_VoluntaryYearInMonumentConservation;
                default:
                    return string.Empty;
            }
        }

        public static string GetLocalizedString(this WorkingTimeModel timeModel)
        {
            return GetLocalizedString((WorkingTimeModel?)timeModel);
        }

        public static string GetLocalizedString(this WorkingTimeModel? timeModel)
        {
            switch (timeModel)
            {
                case WorkingTimeModel.FULLTIME:
                    return Resources.Strings.Resources.WorkingTimeModel_FullTime;
                case WorkingTimeModel.PARTTIME:
                    return Resources.Strings.Resources.WorkingTimeModel_PartTime;
                case WorkingTimeModel.SHIFT_NIGHT_WORK_WEEKEND:
                    return Resources.Strings.Resources.WorkingTimeModel_ShiftNightWorkWeekend;
                case WorkingTimeModel.MINIJOB:
                    return Resources.Strings.Resources.WorkingTimeModel_Minijob;
                case WorkingTimeModel.HOME_TELEWORK:
                    return Resources.Strings.Resources.WorkingTimeModel_HomeTeleWork;
                default:
                    return string.Empty;
            }
        }
    }
}
