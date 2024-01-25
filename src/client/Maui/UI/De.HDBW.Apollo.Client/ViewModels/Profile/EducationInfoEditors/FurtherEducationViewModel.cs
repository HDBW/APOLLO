// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile.EducationInfoEditors
{
    public partial class FurtherEducationViewModel : BasicEducationInfoViewModel
    {
        [ObservableProperty]
        private string? _description;

        public FurtherEducationViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<FurtherEducationViewModel> logger,
            IUserRepository userRepository,
            IUserService userService)
            : base(dispatcherService, navigationService, dialogService, logger, userRepository, userService)
        {
        }

        protected override async Task<EducationInfo?> LoadDataAsync(User user, string? entryId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var currentData = await base.LoadDataAsync(user, entryId, token).ConfigureAwait(false);

            await ExecuteOnUIThreadAsync(
                () => LoadonUIThread(currentData), token);
            return currentData;
        }

        protected override void ApplyChanges(EducationInfo entry)
        {
            base.ApplyChanges(entry);
            entry.Description = Description;
        }

        private void LoadonUIThread(EducationInfo? educationInfo)
        {
            Description = educationInfo?.Description ?? string.Empty;
        }

        partial void OnDescriptionChanged(string? value)
        {
            IsDirty = true;
        }
    }
}
