// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.Client.Models.Generic;
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
            return new LicenseEntry(data, editHandle, canEditHandle, deleteHandle, canDeleteHandle);
        }

        protected override ObservableCollection<StringValue> GetAllLines(License data)
        {
            var items = new List<StringValue>();
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_Designation, data.Value));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_Validity, GetDateRangeText(data.Granted, data.Expires)));
            return new ObservableCollection<StringValue>(items.Where(x => !string.IsNullOrWhiteSpace(x.Data)));
        }
    }
}
