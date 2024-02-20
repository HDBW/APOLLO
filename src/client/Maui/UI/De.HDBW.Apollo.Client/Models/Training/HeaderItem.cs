// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class HeaderItem : ObservableObject
    {
        [ObservableProperty]
        private string? _text;

        protected HeaderItem(string text)
        {
            Text = text;
        }

        public static HeaderItem Import(string text)
        {
            return new HeaderItem(text);
        }
    }
}
