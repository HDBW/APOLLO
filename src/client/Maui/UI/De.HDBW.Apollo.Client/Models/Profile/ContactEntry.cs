// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using System.Collections.ObjectModel;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Models.Generic;
using De.HDBW.Apollo.Data.Helper;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;
using Contact = Invite.Apollo.App.Graph.Common.Models.Contact;

namespace De.HDBW.Apollo.Client.Models.Profile
{
    public partial class ContactEntry : AbstractProfileEntry<Contact>
    {
        private ContactEntry(
            Contact data,
            Func<AbstractProfileEntry<Contact>, Task> editHandle,
            Func<AbstractProfileEntry<Contact>, bool> canEditHandle,
            Func<AbstractProfileEntry<Contact>, Task> deleteHandle,
            Func<AbstractProfileEntry<Contact>, bool> canDeleteHandle)
            : base(data, editHandle, canEditHandle, deleteHandle, canDeleteHandle)
        {
        }

        public static ContactEntry Import(Contact data, Func<AbstractProfileEntry<Contact>, Task> editHandle, Func<AbstractProfileEntry<Contact>, bool> canEditHandle, Func<AbstractProfileEntry<Contact>, Task> deleteHandle, Func<AbstractProfileEntry<Contact>, bool> canDeleteHandle)
        {
            return new ContactEntry(data, editHandle, canEditHandle,  deleteHandle, canDeleteHandle);
        }

        protected override ObservableCollection<StringValue> GetAllLines(Contact data)
        {
            var items = new List<StringValue>();
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_ContactType, data.ContactType.AsEnum<ContactType>().GetLocalizedString()));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_Address, data.Address));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_ZipCode, data.ZipCode));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_City, data.City));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_Region, data.Region));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_Country, data.Country));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_Email, data.Mail));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_Phone, data.Phone));
            return new ObservableCollection<StringValue>(items.Where(x => !string.IsNullOrWhiteSpace(x.Data)));
        }
    }
}
