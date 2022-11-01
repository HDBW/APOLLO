// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Helper;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;

namespace De.HDBW.Apollo.Client.Models
{
    public partial class UserProfileEntry : ObservableObject
    {
        [ObservableProperty]
        private string? _firstName;

        [ObservableProperty]
        private string? _lastName;

        [ObservableProperty]
        private string? _imagePath;

        [ObservableProperty]
        private string? _goal;

        private UserProfile _userProfile;

        private UserProfileEntry(UserProfile userProfile)
        {
            ArgumentNullException.ThrowIfNull(userProfile);

            _userProfile = userProfile;
            FirstName = userProfile.FirstName;
            LastName = userProfile.LastName;
            ImagePath = userProfile.Image?.ToUniformedName();
            Goal = userProfile.Goal;
            OnPropertyChanged(nameof(DisplayName));
        }

        public string DisplayName
        {
            get
            {
                var parts = new List<string?>();
                parts.Add(FirstName);
                parts.Add(LastName);
                return string.Join(" ", parts.Where(s => !string.IsNullOrWhiteSpace(s)));
            }
        }

        public static UserProfileEntry Import(UserProfile userProfile)
        {
            return new UserProfileEntry(userProfile);
        }
    }
}
