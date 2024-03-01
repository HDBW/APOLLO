// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class ExpandedListContent : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<string> _content;

        private ExpandedListContent(ObservableCollection<string> content)
        {
            Content = content;
        }

        public static ObservableObject Import(ObservableCollection<string> content)
        {
            return new ExpandedListContent(content);
        }
    }
}
