// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class ExpandedItemContent : ObservableObject
    {
        [ObservableProperty]
        private string _content;

        private ExpandedItemContent(string content)
        {
            Content = content;
        }

        public static ObservableObject Import(string content)
        {
            return new ExpandedItemContent(content);
        }
    }
}
