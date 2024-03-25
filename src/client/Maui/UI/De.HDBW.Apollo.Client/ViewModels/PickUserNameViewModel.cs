// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
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
        private User? _user;

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
                value = value?.Replace(" ", string.Empty);
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

        public async override Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    _user = null;
                    _user = await UserService.GetAsync(SessionService.UniqueId!, worker.Token).ConfigureAwait(false);

                    if (_user == null)
                    {
                        _user = new User();
                        _user.ObjectId = SessionService.UniqueId!;
                    }

                    _user.Upn = SessionService.AccountId!.Identifier;
                    _user.IdentityProvicer = SessionService.AccountId!.TenantId;
                    await ExecuteOnUIThreadAsync(
                       () =>
                       {
                           Name = _user.Name;
                           ValidateCommand?.Execute(null);
                           RefreshCommands();
                       }, CancellationToken.None);
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
                    await ShowErrorAsync(Resources.Strings.Resources.GlobalError_UnableToSaveData, worker.Token).ConfigureAwait(false);
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            CreateAccountCommand?.NotifyCanExecuteChanged();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanCreateAccount))]
        private async Task CreateAccount(CancellationToken token)
        {
            Logger.LogInformation($"Invoked {nameof(CreateAccountCommand)} in {GetType().Name}.");
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    _user!.Name = Name!;
                    var result = await UserService.SaveAsync(_user, worker.Token).ConfigureAwait(false);

                    if (string.IsNullOrWhiteSpace(result))
                    {
                        throw new NotSupportedException("Unable to create user.");
                    }

                    _user = await UserService.GetAsync(result, worker.Token).ConfigureAwait(false);
                    if (_user == null)
                    {
                        throw new NotSupportedException("Unable to create user.");
                    }

                    if (string.IsNullOrWhiteSpace(_user.Id))
                    {
                        throw new NotSupportedException("User does not have id.");
                    }

                    if (!await UserRepository.SaveAsync(_user, worker.Token).ConfigureAwait(false))
                    {
                        throw new NotSupportedException("Unable to save user.");
                    }

                    PreferenceService.SetValue(Preference.RegisteredUserId, _user.Id);

                    await NavigationService.PushToRootAsync(Routes.Shell, CancellationToken.None);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(CreateAccount)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(CreateAccount)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(CreateAccount)} in {GetType().Name}.");
                    await ShowErrorAsync(Resources.Strings.Resources.GlobalError_UnableToSaveData, worker.Token).ConfigureAwait(false);
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanCreateAccount()
        {
            return !IsBusy && !HasErrors && _user != null && !string.IsNullOrWhiteSpace(SessionService.UniqueId);
        }
    }
}
