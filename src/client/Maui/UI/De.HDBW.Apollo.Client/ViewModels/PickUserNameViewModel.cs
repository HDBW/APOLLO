// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Dialogs;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Data.Services;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class PickUserNameViewModel : BaseViewModel
    {
        private string? _name;

        public PickUserNameViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<PickUserNameViewModel> logger,
            IUserRepository userRepository,
            IUserService userService,
            ISessionService sessionService)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(userRepository);
            ArgumentNullException.ThrowIfNull(userService);
            ArgumentNullException.ThrowIfNull(sessionService);
            UserRepository = userRepository;
            UserService = userService;
            SessionService = sessionService;
        }

        public string? Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (SetProperty(ref _name, value))
                {
                    RefreshCommands();
                }
            }
        }

        private IUserRepository UserRepository { get; }

        private IUserService UserService { get; }

        private ISessionService SessionService { get; }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            CreateAccountCommand?.NotifyCanExecuteChanged();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanCreateAccount))]
        private async Task CreateAccount(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var user = new User();
                    user.Name = Name!;
                    user.ObjectId = SessionService.AccountId!.ObjectId;
                    var result = await UserService.SaveAsync(user, token).ConfigureAwait(false);
                    if (result == null)
                    {
                        throw new NotSupportedException("Unable to create user.");
                    }

                    await UserRepository.SaveAsync(result, token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
                    var parameters = new NavigationParameters();
                    parameters.AddValue(NavigationParameter.Data, Resources.Strings.Resources.GlobalError_UnableToSaveData);
                    await DialogService.ShowPopupAsync<ErrorDialog, NavigationParameters, NavigationParameters>(parameters, token).ConfigureAwait(false);
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanCreateAccount()
        {
            return !IsBusy && Name?.Length > 4 && !string.IsNullOrWhiteSpace(SessionService.AccountId?.ObjectId);
        }
    }
}
