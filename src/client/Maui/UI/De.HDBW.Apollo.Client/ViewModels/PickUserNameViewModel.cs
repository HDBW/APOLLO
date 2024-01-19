// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Dialogs;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Enums;
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
            ISessionService sessionService,
            IPreferenceService preferenceService)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(userRepository);
            ArgumentNullException.ThrowIfNull(userService);
            ArgumentNullException.ThrowIfNull(sessionService);
            ArgumentNullException.ThrowIfNull(preferenceService);
            UserRepository = userRepository;
            UserService = userService;
            SessionService = sessionService;
            PreferenceService = preferenceService;
        }

        [Required(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_NameIsRequired))]
        [MinLength(4, ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_NameMinLength))]
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
                    ValidateProperty(value);
                    RefreshCommands();
                }
            }
        }

        private IUserRepository UserRepository { get; }

        private IUserService UserService { get; }

        private ISessionService SessionService { get; }

        private IPreferenceService PreferenceService { get; }

        public async virtual Task OnNavigatedToAsync()
        {
            await ExecuteOnUIThreadAsync(
                () =>
                {
                    ValidateCommand?.Execute(null);
                }, CancellationToken.None);
        }

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
#if DEBUG
                    user.Id = "User-5DE545AEF9974FD6826151725A9961F8";
#endif
                    user.Name = Name!;
                    user.ObjectId = SessionService.AccountId!.ObjectId;
                    var result = await UserService.SaveAsync(user, token).ConfigureAwait(false);

                    if (string.IsNullOrWhiteSpace(result))
                    {
                        throw new NotSupportedException("Unable to create user.");
                    }

                    user = await UserService.GetUserAsync(result, token).ConfigureAwait(false);
                    if (user == null)
                    {
                        throw new NotSupportedException("Unable to create user.");
                    }

                    if (!await UserRepository.SaveAsync(user, token).ConfigureAwait(false))
                    {
                        throw new NotSupportedException("Unable to save user.");
                    }

                    PreferenceService.SetValue(Preference.RegisteredUserId, user.Id);

                    await NavigationService.PushToRootAsync(Routes.Shell, CancellationToken.None);
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
            return !IsBusy && !HasErrors && !string.IsNullOrWhiteSpace(SessionService.AccountId?.ObjectId);
        }
    }
}
