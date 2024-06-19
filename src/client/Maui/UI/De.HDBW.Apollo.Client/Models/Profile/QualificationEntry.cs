// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using System.Collections.ObjectModel;
using De.HDBW.Apollo.Client.Models.Generic;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;

namespace De.HDBW.Apollo.Client.Models.Profile
{
    public partial class QualificationEntry : AbstractProfileEntry<Qualification>
    {
        private QualificationEntry(
            Qualification data,
            Func<AbstractProfileEntry<Qualification>, Task> editHandle,
            Func<AbstractProfileEntry<Qualification>, bool> canEditHandle,
            Func<AbstractProfileEntry<Qualification>, Task> deleteHandle,
            Func<AbstractProfileEntry<Qualification>, bool> canDeleteHandle)
            : base(data, editHandle, canEditHandle, deleteHandle, canDeleteHandle)
        {
        }

        public static QualificationEntry Import(Qualification data, Func<AbstractProfileEntry<Qualification>, Task> editHandle, Func<AbstractProfileEntry<Qualification>, bool> canEditHandle, Func<AbstractProfileEntry<Qualification>, Task> deleteHandle, Func<AbstractProfileEntry<Qualification>, bool> canDeleteHandle)
        {
            return new QualificationEntry(data, editHandle, canEditHandle, deleteHandle, canDeleteHandle);
        }

        protected override ObservableCollection<StringValue> GetAllLines(Qualification data)
        {
            var items = new List<StringValue>();
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_Qualification, data.Name));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_Description, data.Description));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_Validity, GetDateRangeText(data.IssueDate, data.ExpirationDate)));
            return new ObservableCollection<StringValue>(items.Where(x => !string.IsNullOrWhiteSpace(x.Data)));
        }
    }
}
