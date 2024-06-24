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
        private string? _header;

        [ObservableProperty]
        private ObservableCollection<LineItem> _items = new ObservableCollection<LineItem>();

        private Contact _contact;

        protected ContactItem(
            string? header,
            Contact contact,
            Func<string?, CancellationToken, Task>? openMailHandler,
            Func<string?, bool>? canOpenMailHandler,
            Func<string?, CancellationToken, Task>? openDailerHandler,
            Func<string?, bool>? canOpenDailerHandler)
        {
            _contact = contact;
            Header = header;
            var parts = new List<string>();
            parts.Add(contact.Firstname);
            parts.Add(contact.Surname);
            parts = parts.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (parts.Any())
            {
                Items.Add(LineItem.Import(KnownIcons.Contact, string.Join(" ", parts)));
            }

            if (!string.IsNullOrWhiteSpace(contact.Organization))
            {
                Items.Add(LineItem.Import(null, string.Join(" ", contact.Organization)));
            }

            parts = new List<string>();
            parts.Add(contact.Address);
            parts.Add(contact.ZipCode);
            parts.Add(contact.City);
            parts.Add(contact.Region ?? string.Empty);
            parts.Add(contact.Country ?? string.Empty);
            parts = parts.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (parts.Any())
            {
                Items.Add(LineItem.Import(KnownIcons.Location, string.Join(" ", parts)));
            }

            if (!string.IsNullOrWhiteSpace(contact.Mail))
            {
                Items.Add(InteractiveLineItem.Import(KnownIcons.EMail, contact.Mail, openMailHandler, canOpenMailHandler));
            }

            if (!string.IsNullOrWhiteSpace(contact.Phone))
            {
                Items.Add(InteractiveLineItem.Import(KnownIcons.Phone, contact.Phone, openDailerHandler, canOpenDailerHandler));
            }
        }

        public static ContactItem Import(
            string? header,
            Contact contact,
            Func<string?, CancellationToken, Task>? openMailHandler,
            Func<string?, bool>? canOpenMailHandler,
            Func<string?, CancellationToken, Task>? openDailerHandler,
            Func<string?, bool>? canOpenDailerHandler)
        {
            return new ContactItem(header, contact, openMailHandler, canOpenMailHandler, openDailerHandler, canOpenDailerHandler);
        }

        public void RefreshCommands()
        {
            foreach (var item in Items.OfType<InteractiveLineItem>())
            {
                item.InteractCommand?.NotifyCanExecuteChanged();
            }
        }
    }
}
