// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class ExpandableListItem : ObservableObject
    {
        [ObservableProperty]
        private string _header;

        [ObservableProperty]
        private ObservableCollection<string> _content;

        private ExpandableListItem(string header, IEnumerable<string> content)
        {
            Header = header;
            Content = new ObservableCollection<string>(content);
        }

        public static ExpandableListItem Import(
            string header,
            IEnumerable<string> content)
        {
            return new ExpandableListItem(header, content);
        }
    }
}
