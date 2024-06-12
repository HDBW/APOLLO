// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Models;
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

        public static string GetLocalizedString(this EducationType educationType)
        {
            return GetLocalizedString((EducationType?)educationType);
        }

        public static string GetLocalizedString(this EducationType? educationType)
        {
            switch (educationType)
            {
                case EducationType.CompanyBasedVocationalTraining:
                    return Resources.Strings.Resources.EducationType_CompanyBasedVocationalTraining;
                case EducationType.Education:
                    return Resources.Strings.Resources.EducationType_Education;
                case EducationType.FurtherEducation:
                    return Resources.Strings.Resources.EducationType_FurtherEducation;
                case EducationType.Study:
                    return Resources.Strings.Resources.EducationType_Study;
                case EducationType.VocationalTraining:
                    return Resources.Strings.Resources.EducationType_VocationalTraining;
                default:
                    return string.Empty;
            }
        }

        public static string GetLocalizedString(this TypeOfSchool typeOfSchool)
        {
            return GetLocalizedString((TypeOfSchool?)typeOfSchool);
        }

        public static string GetLocalizedString(this TypeOfSchool? typeOfSchool)
        {
            switch (typeOfSchool)
            {
                case TypeOfSchool.Other:
                    return Resources.Strings.Resources.TypeOfSchool_Other;
                case TypeOfSchool.HighSchool:
                    return Resources.Strings.Resources.TypeOfSchool_HighSchool;
                case TypeOfSchool.SecondarySchool:
                    return Resources.Strings.Resources.TypeOfSchool_SecondarySchool;
                case TypeOfSchool.VocationalCollege:
                    return Resources.Strings.Resources.TypeOfSchool_VocationalCollege;
                case TypeOfSchool.MainSchool:
                    return Resources.Strings.Resources.TypeOfSchool_MainSchool;
                case TypeOfSchool.VocationalHighSchool:
                    return Resources.Strings.Resources.TypeOfSchool_VocationalHighSchool;
                case TypeOfSchool.VocationalSchool:
                    return Resources.Strings.Resources.TypeOfSchool_VocationalSchool;
                case TypeOfSchool.SpecialSchool:
                    return Resources.Strings.Resources.TypeOfSchool_SpecialSchool;
                case TypeOfSchool.IntegratedComprehensiveSchool:
                    return Resources.Strings.Resources.TypeOfSchool_IntegratedComprehensiveSchool;
                case TypeOfSchool.SchoolWithMultipleCourses:
                    return Resources.Strings.Resources.TypeOfSchool_SchoolWithMultipleCourses;
                case TypeOfSchool.TechnicalCollege:
                    return Resources.Strings.Resources.TypeOfSchool_TechnicalCollege;
                case TypeOfSchool.TechnicalHighSchool:
                    return Resources.Strings.Resources.TypeOfSchool_TechnicalHighSchool;
                case TypeOfSchool.TechnicalSchool:
                    return Resources.Strings.Resources.TypeOfSchool_TechnicalSchool;
                case TypeOfSchool.Colleague:
                    return Resources.Strings.Resources.TypeOfSchool_Colleague;
                case TypeOfSchool.EveningHighSchool:
                    return Resources.Strings.Resources.TypeOfSchool_EveningHighSchool;
                case TypeOfSchool.VocationalTrainingSchool:
                    return Resources.Strings.Resources.TypeOfSchool_VocationalTrainingSchool;
                case TypeOfSchool.NightSchool:
                    return Resources.Strings.Resources.TypeOfSchool_NightSchool;
                case TypeOfSchool.EveningSchool:
                    return Resources.Strings.Resources.TypeOfSchool_EveningSchool;
                case TypeOfSchool.WaldorfSchool:
                    return Resources.Strings.Resources.TypeOfSchool_WaldorfSchool;
                case TypeOfSchool.TechnicalAcademy:
                    return Resources.Strings.Resources.TypeOfSchool_TechnicalAcademy;
                case TypeOfSchool.UniversityOfAppliedScience:
                    return Resources.Strings.Resources.TypeOfSchool_UniversityOfAppliedScience;
                default:
                    return string.Empty;
            }
        }

        public static string GetLocalizedString(this SchoolGraduation schoolGraduation)
        {
            return GetLocalizedString((SchoolGraduation?)schoolGraduation);
        }

        public static string GetLocalizedString(this SchoolGraduation? schoolGraduation)
        {
            switch (schoolGraduation)
            {
                case SchoolGraduation.SecondarySchoolCertificate:
                    return Resources.Strings.Resources.SchoolGraduation_SecondarySchoolCertificate;
                case SchoolGraduation.AdvancedTechnicalCollegeCertificate:
                    return Resources.Strings.Resources.SchoolGraduation_AdvancedTechnicalCollegeCertificate;
                case SchoolGraduation.HigherEducationEntranceQualificationALevel:
                    return Resources.Strings.Resources.SchoolGraduation_HigherEducationEntranceQualificationALevel;
                case SchoolGraduation.IntermediateSchoolCertificate:
                    return Resources.Strings.Resources.SchoolGraduation_IntermediateSchoolCertificate;
                case SchoolGraduation.ExtendedSecondarySchoolLeavingCertificate:
                    return Resources.Strings.Resources.SchoolGraduation_ExtendedSecondarySchoolLeavingCertificate;
                case SchoolGraduation.NoSchoolLeavingCertificate:
                    return Resources.Strings.Resources.SchoolGraduation_NoSchoolLeavingCertificate;
                case SchoolGraduation.SpecialSchoolLeavingCertificate:
                    return Resources.Strings.Resources.SchoolGraduation_SpecialSchoolLeavingCertificate;
                case SchoolGraduation.SubjectRelatedEntranceQualification:
                    return Resources.Strings.Resources.SchoolGraduation_SubjectRelatedEntranceQualification;
                case SchoolGraduation.AdvancedTechnicalCollegeWithoutCertificate:
                    return Resources.Strings.Resources.SchoolGraduation_AdvancedTechnicalCollegeWithoutCertificate;
                default:
                    return string.Empty;
            }
        }

        public static string GetLocalizedString(this UniversityDegree universityDegree)
        {
            return GetLocalizedString((UniversityDegree?)universityDegree);
        }

        public static string GetLocalizedString(this UniversityDegree? universityDegree)
        {
            switch (universityDegree)
            {
                case UniversityDegree.Master:
                    return Resources.Strings.Resources.UniversityDegree_Master;
                case UniversityDegree.Bachelor:
                    return Resources.Strings.Resources.UniversityDegree_Bachelor;
                case UniversityDegree.Pending:
                    return Resources.Strings.Resources.UniversityDegree_Pending;
                case UniversityDegree.Doctorate:
                    return Resources.Strings.Resources.UniversityDegree_Doctorate;
                case UniversityDegree.StateExam:
                    return Resources.Strings.Resources.UniversityDegree_StateExam;
                case UniversityDegree.UnregulatedUnrecognized:
                    return Resources.Strings.Resources.UniversityDegree_UnregulatedUnrecognized;
                case UniversityDegree.RegulatedUnrecognized:
                    return Resources.Strings.Resources.UniversityDegree_RegulatedUnrecognized;
                case UniversityDegree.PartialRecognized:
                    return Resources.Strings.Resources.UniversityDegree_PartialRecognized;
                case UniversityDegree.EcclesiasticalExam:
                    return Resources.Strings.Resources.UniversityDegree_EcclesiasticalExam;
                case UniversityDegree.Other:
                    return Resources.Strings.Resources.UniversityDegree_Other;
                default:
                    return string.Empty;
            }
        }

        public static string GetLocalizedString(this CompletionState completionState)
        {
            return GetLocalizedString((CompletionState?)completionState);
        }

        public static string GetLocalizedString(this CompletionState? completionState)
        {
            switch (completionState)
            {
                case CompletionState.Completed:
                    return Resources.Strings.Resources.CompletionState_Completed;
                case CompletionState.Failed:
                    return Resources.Strings.Resources.CompletionState_Failed;
                case CompletionState.Ongoing:
                    return Resources.Strings.Resources.CompletionState_Ongoing;
                default:
                    return string.Empty;
            }
        }

        public static string GetLocalizedString(this Willing willing)
        {
            return GetLocalizedString((Willing?)willing);
        }

        public static string GetLocalizedString(this Willing? willing)
        {
            switch (willing)
            {
                case Willing.Yes:
                    return Resources.Strings.Resources.Willing_Yes;
                case Willing.No:
                    return Resources.Strings.Resources.Willing_No;
                case Willing.Partly:
                    return Resources.Strings.Resources.Willing_Partly;
                default:
                    return string.Empty;
            }
        }

        public static string ToFontIcon(this DriversLicense enumValue)
        {
            switch (enumValue)
            {
                case DriversLicense.B:
                    return KnonwIcons.DriversLicenseB;
                case DriversLicense.BE:
                    return KnonwIcons.DriversLicenseBE;
                case DriversLicense.Forklift:
                    return KnonwIcons.DriversLicenseForklift;
                case DriversLicense.C1E:
                    return KnonwIcons.DriversLicenseC1E;
                case DriversLicense.C1:
                    return KnonwIcons.DriversLicenseC1;
                case DriversLicense.L:
                    return KnonwIcons.DriversLicenseL;
                case DriversLicense.AM:
                    return KnonwIcons.DriversLicenseAM;
                case DriversLicense.A:
                    return KnonwIcons.DriversLicenseA;
                case DriversLicense.CE:
                    return KnonwIcons.DriversLicenseCE;
                case DriversLicense.C:
                    return KnonwIcons.DriversLicenseC;
                case DriversLicense.A1:
                    return KnonwIcons.DriversLicenseA1;
                case DriversLicense.B96:
                    return KnonwIcons.DriversLicenseB96;
                case DriversLicense.T:
                    return KnonwIcons.DriversLicenseT;
                case DriversLicense.A2:
                    return KnonwIcons.DriversLicenseA2;
                case DriversLicense.Moped:
                    return KnonwIcons.DriversLicenseMoped;
                case DriversLicense.Drivercard:
                    return KnonwIcons.DriversLicenseDrivercard;
                case DriversLicense.PassengerTransport:
                    return KnonwIcons.DriversLicensePassengerTransport;
                case DriversLicense.D:
                    return KnonwIcons.DriversLicenseD;
                case DriversLicense.InstructorBE:
                    return KnonwIcons.DriversLicenseInstructorBE;
                case DriversLicense.ConstructionMachines:
                    return KnonwIcons.DriversLicenseConstructionMachines;
                case DriversLicense.DE:
                    return KnonwIcons.DriversLicenseDE;
                case DriversLicense.D1:
                    return KnonwIcons.DriversLicenseD1;
                case DriversLicense.D1E:
                    return KnonwIcons.DriversLicenseD1E;
                case DriversLicense.InstructorA:
                    return KnonwIcons.DriversLicenseInstructorA;
                case DriversLicense.InstructorCE:
                    return KnonwIcons.DriversLicenseInstructorCE;
                case DriversLicense.TrailerDriving:
                    return KnonwIcons.DriversLicenseTrailerDriving;
                case DriversLicense.InstructorDE:
                    return KnonwIcons.DriversLicenseInstructorDE;
                case DriversLicense.Class1:
                    return KnonwIcons.DriversLicenseClass1;
                case DriversLicense.Class3:
                    return KnonwIcons.DriversLicenseClass2;
                case DriversLicense.Class2:
                    return KnonwIcons.DriversLicenseClass3;
                case DriversLicense.InstructorASF:
                    return KnonwIcons.DriversLicenseInstructorASF;
                case DriversLicense.InstructorASP:
                    return KnonwIcons.DriversLicenseInstructorASP;

                default:
                    return string.Empty;
            }
        }
    }
}
