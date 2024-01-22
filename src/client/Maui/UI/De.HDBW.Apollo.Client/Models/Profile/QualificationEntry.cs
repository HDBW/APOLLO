// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using System.Collections.ObjectModel;
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
            return new QualificationEntry(data, editHandle, canEditHandle,  deleteHandle, canDeleteHandle);
        }

        protected override ObservableCollection<string> GetAdditionalLines(Qualification data)
        {
            var items = new List<string?>();
            items.Add(data.Name);
            items.Add(data.Description);
            items.Add(data.IssueDate?.ToShortDateString());
            items.Add(data.ExpirationDate?.ToShortDateString());
            return new ObservableCollection<string>(items.Where(x => !string.IsNullOrWhiteSpace(x) && x != FirstLine).OfType<string>());
        }

        protected override string GetFristLine(Qualification data)
        {
            var items = new List<string?>();
            items.Add(data.Name);
            items.Add(data.Description);
            items.Add(data.IssueDate?.ToShortDateString());
            items.Add(data.ExpirationDate?.ToShortDateString());
            return items.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)) ?? string.Empty;
        }
    }
}
