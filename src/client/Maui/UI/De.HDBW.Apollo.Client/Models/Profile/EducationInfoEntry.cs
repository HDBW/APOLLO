// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using De.HDBW.Apollo.Client.Helper;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;

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

        protected override ObservableCollection<string> GetAdditionalLines(EducationInfo data)
        {
            var items = new List<string?>();
            items.Add(data.NameOfInstitution);
            items.Add(data.Description);
            items.Add(data.ProfessionalTitle?.PreferedTerm?.FirstOrDefault());
            items.Add(data.Start.ToUIDate().ToShortDateString());
            items.Add(data.End.ToUIDate()?.ToShortDateString());
            items.Add(data.City);
            items.Add(data.Country);
            return new ObservableCollection<string>(items.Where(x => !string.IsNullOrWhiteSpace(x) && x != FirstLine).OfType<string>());
        }

        protected override string GetFristLine(EducationInfo data)
        {
            var items = new List<string?>();
            items.Add(data.NameOfInstitution);
            items.Add(data.Description);
            items.Add(data.ProfessionalTitle?.PreferedTerm?.FirstOrDefault());
            items.Add(data.Start.ToUIDate().ToShortDateString());
            items.Add(data.End.ToUIDate()?.ToShortDateString());
            items.Add(data.City);
            items.Add(data.Country);
            return items.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)) ?? string.Empty;
        }
    }

}
