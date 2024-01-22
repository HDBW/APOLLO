// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using System.Collections.ObjectModel;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;
using Contact = Invite.Apollo.App.Graph.Common.Models.Contact;

namespace De.HDBW.Apollo.Client.Models.Profile
{
    public partial class ContactEntry : AbstractProfileEntry<Contact>
    {
        private ContactEntry(
            Contact data,
            Func<AbstractProfileEntry<Contact>, Task> deleteHandle,
            Func<AbstractProfileEntry<Contact>, bool> canDeleteHandle)
            : base(data, deleteHandle, canDeleteHandle)
        {
        }

        public static ContactEntry Import(Contact data, Func<AbstractProfileEntry<Contact>, Task> deleteHandle, Func<AbstractProfileEntry<Contact>, bool> canDeleteHandle)
        {
            return new ContactEntry(data, deleteHandle, canDeleteHandle);
        }

        protected override ObservableCollection<string> GetAdditionalLines(Contact data)
        {
            var items = new List<string>();
            items.Add(data.Address);
            items.Add(data.City);
            items.Add(data.ZipCode);

            //TODO:
            //items.Add(data.Region);
            //items.Add(data.Country);
            items.Add(data.Mail);
            items.Add(data.Phone);
            return new ObservableCollection<string>(items.Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        protected override string GetFristLine(Contact data)
        {
            switch (data.ContactType)
            {
                case ContactType.Professional:
                    return Resources.Strings.Resources.ContactType_Professional;
                case ContactType.Private:
                    return Resources.Strings.Resources.ContactType_Private;
                default:
                    return string.Empty;
            }
        }
    }
}
