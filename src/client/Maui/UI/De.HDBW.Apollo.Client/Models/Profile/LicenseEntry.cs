// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;

namespace De.HDBW.Apollo.Client.Models.Profile
{
    public partial class LicenseEntry : AbstractProfileEntry<License>
    {
        private LicenseEntry(
            License data,
            Func<AbstractProfileEntry<License>, Task> editHandle,
            Func<AbstractProfileEntry<License>, bool> canEditHandle,
            Func<AbstractProfileEntry<License>, Task> deleteHandle,
            Func<AbstractProfileEntry<License>, bool> canDeleteHandle)
            : base(data, editHandle, canEditHandle, deleteHandle, canDeleteHandle)
        {
        }

        public static LicenseEntry Import(License data, Func<AbstractProfileEntry<License>, Task> editHandle, Func<AbstractProfileEntry<License>, bool> canEditHandle, Func<AbstractProfileEntry<License>, Task> deleteHandle, Func<AbstractProfileEntry<License>, bool> canDeleteHandle)
        {
            return new LicenseEntry(data, editHandle, canEditHandle,  deleteHandle, canDeleteHandle);
        }

        protected override ObservableCollection<string> GetAdditionalLines(License data)
        {
            var items = new List<string?>();
            items.Add(data.Name);
            items.Add(data.Granted?.ToShortDateString());
            items.Add(data.Expires?.ToShortDateString());
            return new ObservableCollection<string>(items.Where(x => !string.IsNullOrWhiteSpace(x) && x != FirstLine).OfType<string>());
        }

        protected override string GetFristLine(License data)
        {
            var items = new List<string?>();
            items.Add(data.Name);
            items.Add(data.Granted?.ToShortDateString());
            items.Add(data.Expires?.ToShortDateString());
            return items.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)) ?? string.Empty;
        }
    }
}
