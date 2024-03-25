// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class TagItem : ObservableObject
    {
        [ObservableProperty]
        private string? _headline;

        [ObservableProperty]
        private ObservableCollection<string> _items = new ObservableCollection<string>();

        private TagItem(
            string? headline,
            IEnumerable<string> items)
        {
            Headline = headline;
            Items = new ObservableCollection<string>(items);
        }

        public static TagItem Import(
            string? headline,
            IEnumerable<string> items)
        {
            return new TagItem(headline, items);
        }
    }
}
