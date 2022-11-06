﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class AppShellViewModel : BaseViewModel
    {
        [ObservableProperty]
        private UserProfileEntry? _userProfile = UserProfileEntry.Import(new UserProfile());

        public AppShellViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            IUserProfileRepository userProfileRepository,
            ILogger<StartViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(userProfileRepository);
            UserProfileRepository = userProfileRepository;
        }

        private IUserProfileRepository UserProfileRepository { get; }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var user = await UserProfileRepository.GetItemByIdAsync(1, worker.Token).ConfigureAwait(false);
                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(user), worker.Token);
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private void LoadonUIThread(UserProfile? user)
        {
            UserProfile = UserProfileEntry.Import(user ?? new UserProfile());
        }
    }
}
