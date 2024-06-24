// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models
{
    public partial class SearchSuggestionEntry : ObservableObject
    {
        [ObservableProperty]
        private string? _name;

        private bool _isRecent;

        protected SearchSuggestionEntry(string name, bool isRecent = false)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            Name = name;
            IsRecent = isRecent;
        }

        public bool IsRecent
        {
            get
            {
                return _isRecent;
            }

            set
            {
                if (SetProperty(ref _isRecent, value))
                {
                    OnPropertyChanged(nameof(Icon));
                }
            }
        }

        public string Icon
        {
            get
            {
                switch (IsRecent)
                {
                    case false:
                        return KnownIcons.Magnify;
                    default:
                        return KnownIcons.History;
                }
            }
        }

        public static SearchSuggestionEntry Import(string name, bool isRecent = false)
        {
            return new SearchSuggestionEntry(name, isRecent);
        }
    }
}
