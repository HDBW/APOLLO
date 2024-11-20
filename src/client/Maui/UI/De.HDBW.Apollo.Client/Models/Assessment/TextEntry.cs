// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class TextEntry : ObservableObject
    {
        [ObservableProperty]
        private string? _text;

        protected TextEntry(string? text)
        {
            Text = text;
        }

        public static TextEntry Import(string text)
        {
            return new TextEntry(text);
        }
    }
}
