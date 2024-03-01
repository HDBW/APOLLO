// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class ExpandedOccurenceItem : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<LineItem> _content;

        private ExpandedOccurenceItem(ObservableCollection<LineItem> content)
        {
            Content = content;
        }

        public static ExpandedOccurenceItem Import(ObservableCollection<LineItem> content)
        {
            return new ExpandedOccurenceItem(content);
        }
    }
}
