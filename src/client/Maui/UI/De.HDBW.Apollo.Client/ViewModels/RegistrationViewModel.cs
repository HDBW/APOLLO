// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class RegistrationViewModel : BaseViewModel
    {
        private readonly ObservableCollection<InstructionEntry> _instructions = new ObservableCollection<InstructionEntry>();

        public RegistrationViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            IAuthService authService,
            ISessionService sessionService,
            IPreferenceService preferenceService,
            IUserService userService,
            IProfileService profileService,
            IUserRepository userRepository,
            IUnregisterUserService unregisterUserService,
            IApolloListService apolloListService,
            ILogger<RegistrationViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(authService);
            ArgumentNullException.ThrowIfNull(sessionService);
            ArgumentNullException.ThrowIfNull(preferenceService);
            ArgumentNullException.ThrowIfNull(userService);
            ArgumentNullException.ThrowIfNull(userRepository);
            ArgumentNullException.ThrowIfNull(unregisterUserService);
            ArgumentNullException.ThrowIfNull(apolloListService);
            AuthService = authService;
            SessionService = sessionService;
            PreferenceService = preferenceService;
            UserService = userService;
            ProfileService = profileService;
            UserRepository = userRepository;
            UnregisterUserService = unregisterUserService;
            ApolloListService = apolloListService;
            Instructions.Add(InstructionEntry.Import("splashdeco1.png", null, Resources.Strings.Resources.RegistrationView_Instruction1, null));
            Instructions.Add(InstructionEntry.Import("splashdeco2.png", null, Resources.Strings.Resources.RegistrationView_Instruction2, null));
        }

        public ObservableCollection<InstructionEntry> Instructions
        {
            get { return _instructions; }
        }

        public bool HasRegisterdUser
        {
            get
            {
                return SessionService.HasRegisteredUser;
            }
        }

        private IAuthService AuthService { get; }

        private IUserService UserService { get; }

        private IUnregisterUserService UnregisterUserService { get; }

        private IApolloListService ApolloListService { get; }

        private IProfileService ProfileService { get; }

        private ISessionService SessionService { get; }

        private IPreferenceService PreferenceService { get; }

        private IUserRepository UserRepository { get; }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            SkipCommand?.NotifyCanExecuteChanged();
            RegisterCommand?.NotifyCanExecuteChanged();
            UnRegisterCommand?.NotifyCanExecuteChanged();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSkip))]
        private async Task Skip(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    SessionService.UpdateRegisteredUser(null, null);
                    await NavigationService.PushToRootAsync(Routes.Shell, worker.Token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Skip)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Skip)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(Skip)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanSkip()
        {
            return !IsBusy;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanUnRegister))]
        private async Task UnRegister(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                AuthenticationResult? authentication = null;
                try
                {
                    if (!string.IsNullOrWhiteSpace(SessionService.AccessToken))
                    {
                        if (!await UnregisterUserService.DeleteAsync(SessionService.AccessToken, worker.Token))
                        {
                            Logger?.LogWarning($"User deletion unsuccessful while unregistering user in {GetType().Name}.");
                            return;
                        }
                    }
#if !DEBUG
                    await AuthService.LogoutAsync(CancellationToken.None);
                    authentication = await AuthService.AcquireTokenSilent(CancellationToken.None);
#endif
                    if (authentication == null)
                    {
                        await UserRepository.DeleteUserAsync(CancellationToken.None).ConfigureAwait(false);
                        PreferenceService.SetValue<string?>(Preference.RegisteredUserId, null);
                        UserService.UpdateAuthorizationHeader(null);
                        ProfileService.UpdateAuthorizationHeader(null);
                        UnregisterUserService.UpdateAuthorizationHeader(null);
                        ApolloListService.UpdateAuthorizationHeader(null);
                        SessionService.UpdateRegisteredUser(authentication?.AccessToken, authentication?.Account?.HomeAccountId);
                    }
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(UnRegister)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(UnRegister)} in {GetType().Name}.");
                }
                catch (MsalException ex)
                {
                    Logger?.LogWarning(ex, $"Error while unregistering user in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(UnRegister)} in {GetType().Name}.");
                }
                finally
                {
                    await ExecuteOnUIThreadAsync(() => { OnPropertyChanged(nameof(HasRegisterdUser)); }, CancellationToken.None);
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanUnRegister()
        {
            return !IsBusy && HasRegisterdUser;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanRegister))]
        private async Task Register(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                AuthenticationResult? authentication = null;
                try
                {
#if !DEBUG
                    authentication = await AuthService.SignInInteractively(worker.Token);
#else
                    authentication = new AuthenticationResult(
                          accessToken: "Mock",
                          isExtendedLifeTimeToken: true,
                          uniqueId: "Mock",
                          expiresOn: DateTimeOffset.MaxValue,
                          extendedExpiresOn: DateTimeOffset.MaxValue,
                          tenantId: "Mock",
                          account: new DummyAccount(),
                          idToken: "Mock",
                          scopes: new List<string>(),
                          correlationId: Guid.Empty,
                          tokenType: "Bearer",
                          authenticationResultMetadata: null,
                          claimsPrincipal: null,
                          spaAuthCode: null,
                          additionalResponseParameters: null);
#endif
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Register)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Register)} in {GetType().Name}.");
                }
                catch (MsalException ex)
                {
                    Logger?.LogWarning(ex, $"Error while registering user in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(Register)} in {GetType().Name}.");
                }
                finally
                {
                    UserService.UpdateAuthorizationHeader(authentication?.CreateAuthorizationHeader());
                    ProfileService.UpdateAuthorizationHeader(authentication?.CreateAuthorizationHeader());
                    UnregisterUserService.UpdateAuthorizationHeader(authentication?.CreateAuthorizationHeader());
                    ApolloListService.UpdateAuthorizationHeader(authentication?.CreateAuthorizationHeader());
                    SessionService.UpdateRegisteredUser(authentication?.AccessToken, authentication?.Account.HomeAccountId);
                    if (SessionService.HasRegisteredUser)
                    {
                        await NavigationService.PushToRootAsync(Routes.PickUserNameView, worker.Token);
                    }

                    OnPropertyChanged(nameof(HasRegisterdUser));
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanRegister()
        {
            return !IsBusy && !HasRegisterdUser;
        }
    }
}
