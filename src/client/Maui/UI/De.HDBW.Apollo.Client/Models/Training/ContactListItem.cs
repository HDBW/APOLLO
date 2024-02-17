﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Contact = Invite.Apollo.App.Graph.Common.Models.Contact;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class ContactListItem : ObservableObject
    {
        [ObservableProperty]
        private string _header;

        [ObservableProperty]
        private ObservableCollection<ContactItem> _items;

        private ContactListItem(string header, IEnumerable<Contact> items)
        {
            Header = header;
            Items = new ObservableCollection<ContactItem>(items.Select(x => ContactItem.Import(x)));
        }

        public static ContactListItem Import(
            string header,
            IEnumerable<Contact> items)
        {
            return new ContactListItem(header, items);
        }
    }
}
