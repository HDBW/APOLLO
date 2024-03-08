// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Backend.Api;
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

        public override Task OnNavigatedToAsync()
        {
            return ExecuteOnUIThreadAsync(
               () =>
               {
                   ValidateCommand?.Execute(null);
                   RefreshCommands();
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
            Logger.LogInformation($"Invoked {nameof(CreateAccountCommand)} in {GetType().Name}.");
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    User? user = null;
                    
                       user = await UserService.GetAsync(Guid.NewGuid().ToString(), token).ConfigureAwait(false);
                    }
                    catch (ApolloApiException ex)
                    {
                        switch (ex.ErrorCode)
                        {
                            case ErrorCodes.UserErrors.UserNotFound:
                        }
                    }
                    if (user == null)
                    {
                        user = new User();
                        user.ObjectId = SessionService.UniqueId!;
                    }

                    user.Upn = SessionService.AccountId!.Identifier;
                    user.IdentityProvicer = SessionService.AccountId!.TenantId;
                    user.Name = Name!;
                    var result = await UserService.SaveAsync(user, worker.Token).ConfigureAwait(false);

                    if (string.IsNullOrWhiteSpace(result))
                    {
                        throw new NotSupportedException("Unable to create user.");
                    }

                    user = await UserService.GetAsync(result, worker.Token).ConfigureAwait(false);
                    if (user == null)
                    {
                        throw new NotSupportedException("Unable to create user.");
                    }

                    if (!await UserRepository.SaveAsync(user, worker.Token).ConfigureAwait(false))
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
            return !IsBusy && !HasErrors && !string.IsNullOrWhiteSpace(SessionService.UniqueId);
        }
    }
}
