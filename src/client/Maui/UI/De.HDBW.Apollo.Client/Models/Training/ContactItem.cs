// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Contact = Invite.Apollo.App.Graph.Common.Models.Contact;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class ContactItem : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<LineItem> _items = new ObservableCollection<LineItem>();

        private Contact _contact;

        private ContactItem(Contact contact)
        {
            _contact = contact;
            var parts = new List<string>();
            parts.Add(contact.Firstname);
            parts.Add(contact.Surname);
            parts = parts.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (parts.Any())
            {
                Items.Add(LineItem.Import(KnonwIcons.Contact, string.Join(" ", parts)));
            }

            if (!string.IsNullOrWhiteSpace(contact.Organization))
            {
                Items.Add(LineItem.Import(null, string.Join(" ", contact.Organization)));
            }

            parts = new List<string>();
            parts.Add(contact.Address);
            parts.Add(contact.ZipCode);
            parts.Add(contact.City);
            parts.Add(contact.Region);
            parts.Add(contact.Country);
            parts = parts.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (parts.Any())
            {
                Items.Add(LineItem.Import(KnonwIcons.Location, string.Join(" ", parts)));
            }

            if (!string.IsNullOrWhiteSpace(contact.Mail))
            {
                Items.Add(LineItem.Import(KnonwIcons.EMail, contact.Mail));
            }

            if (!string.IsNullOrWhiteSpace(contact.Phone))
            {
                Items.Add(LineItem.Import(KnonwIcons.Phone, contact.Phone));
            }
        }

        public static ContactItem Import(Contact contact)
        {
            return new ContactItem(contact);
        }
    }
}
