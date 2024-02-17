// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class LineItem : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasIcon))]
        private string? _icon;

        [ObservableProperty]
        private string? _text;

        public LineItem(string? icon, string text)
        {
            Icon = icon;
            Text = text;
        }

        public bool HasIcon
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Icon);
            }
        }

        public static LineItem Import(string? icon, string text)
        {
            return new LineItem(icon, text);
        }
    }
}
