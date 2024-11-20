// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class IconTextEntry : TextEntry
    {
        [ObservableProperty]
        private string? _icon;

        private IconTextEntry(string icon, string text)
            : base(text)
        {
            Icon = icon;
        }

        public static IconTextEntry Import(string icon, string text)
        {
            return new IconTextEntry(icon, text);
        }
    }
}
