// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Models.Generic;
using De.HDBW.Apollo.Data.Helper;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;

namespace De.HDBW.Apollo.Client.Models.Profile
{
    public partial class EducationInfoEntry : AbstractProfileEntry<EducationInfo>
    {
        private EducationInfoEntry(
            EducationInfo data,
            Func<AbstractProfileEntry<EducationInfo>, Task> editHandle,
            Func<AbstractProfileEntry<EducationInfo>, bool> canEditHandle,
            Func<AbstractProfileEntry<EducationInfo>, Task> deleteHandle,
            Func<AbstractProfileEntry<EducationInfo>, bool> canDeleteHandle)
            : base(data, editHandle, canEditHandle, deleteHandle, canDeleteHandle)
        {
        }

        public static EducationInfoEntry Import(EducationInfo data, Func<AbstractProfileEntry<EducationInfo>, Task> editHandle, Func<AbstractProfileEntry<EducationInfo>, bool> canEditHandle, Func<AbstractProfileEntry<EducationInfo>, Task> deleteHandle, Func<AbstractProfileEntry<EducationInfo>, bool> canDeleteHandle)
        {
            return new EducationInfoEntry(data, editHandle, canEditHandle, deleteHandle, canDeleteHandle);
        }

        protected override ObservableCollection<StringValue> GetAllLines(EducationInfo data)
        {
            var items = new List<StringValue>();
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_EducationType, data.EducationType.AsEnum<EducationType>().GetLocalizedString()));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_TypeOfSchool, data.TypeOfSchool?.AsEnum<TypeOfSchool>().GetLocalizedString()));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_Graduation, data.Graduation?.AsEnum<SchoolGraduation>().GetLocalizedString()));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_UniversityDegree, data.UniversityDegree?.AsEnum<UniversityDegree>().GetLocalizedString()));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_CompletionState, data.CompletionState.AsEnum<CompletionState>().GetLocalizedString()));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_Graduation, data.Description));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_Occupation, data.ProfessionalTitle?.PreferedTerm?.FirstOrDefault()));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_DateRange, GetDateRangeText(data.Start, data.End)));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_NameOfInstitution, data.NameOfInstitution));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_City, data.City));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_Country, data.Country));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_RecognitionType, data.Recognition?.ToString()));
            return new ObservableCollection<StringValue>(items.Where(x => !string.IsNullOrWhiteSpace(x.Data)));
        }
    }
}
