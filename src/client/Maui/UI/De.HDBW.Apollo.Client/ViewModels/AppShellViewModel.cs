// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class AppShellViewModel : BaseViewModel
    {
        [ObservableProperty]
        private UserProfileEntry? _userProfile = UserProfileEntry.Import(new User());

        public AppShellViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<StartViewModel> logger,
            ISessionService sessionService)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(sessionService);
            SessionService = sessionService;
        }

        public bool IsRegistered
        {
            get
            {
                return SessionService?.HasRegisteredUser ?? false;
            }
        }

        private ISessionService SessionService { get; }

        public override async Task OnNavigatedToAsync()
        {
            switch (SessionService?.UseCase)
            {
                case UseCase.D:
                    break;
                default:
                    using (var worker = ScheduleWork())
                    {
                        try
                        {
                            User user = null;
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

                    break;
            }
        }

        private void LoadonUIThread(User? user)
        {
            UserProfile = UserProfileEntry.Import(user ?? new User());
            OnPropertyChanged(nameof(IsRegistered));
        }
    }
}
