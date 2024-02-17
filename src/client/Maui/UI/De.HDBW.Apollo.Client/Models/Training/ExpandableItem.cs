// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class ExpandableItem : ObservableObject
    {
        [ObservableProperty]
        private string _header;

        [ObservableProperty]
        private string _content;

        private ExpandableItem(string header, string content)
        {
            Header = header;
            Content = content;
        }

        public static ExpandableItem Import(
            string header,
            string content)
        {
            return new ExpandableItem(header, content);
        }
    }
}
