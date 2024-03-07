// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Messages;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class AppShellViewModel : BaseViewModel
    {
        [ObservableProperty]
        private UserProfileEntry? _userProfile = UserProfileEntry.Import(new User());

        private bool _isFlyoutPresented;

        public AppShellViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<StartViewModel> logger,
            ISessionService sessionService,
            IUserRepository userRepository)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(sessionService);
            ArgumentNullException.ThrowIfNull(userRepository);
            SessionService = sessionService;
            UserRepository = userRepository;
        }

        public bool IsRegistered
        {
            get
            {
                return SessionService?.HasRegisteredUser ?? false;
            }
        }

        public bool IsFlyoutPresented
        {
            get
            {
                return _isFlyoutPresented;
            }

            set
            {
                if (SetProperty(ref _isFlyoutPresented, value))
                {
                    WeakReferenceMessenger.Default.Send(new FlyoutStateChangedMessage(IsFlyoutPresented));
                }
            }
        }

        private ISessionService SessionService { get; }

        private IUserRepository UserRepository { get; }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var user = await UserRepository.GetItemAsync(worker.Token).ConfigureAwait(false);
                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(user), worker.Token).ConfigureAwait(false);
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

        private void LoadonUIThread(User? user)
        {
            UserProfile = UserProfileEntry.Import(user ?? new User());
            OnPropertyChanged(nameof(IsRegistered));
        }
    }
}
