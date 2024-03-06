// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            IAuthService authService,
            ISessionService sessionService,
            IUserService userService,
            IProfileService profileService,
            IUnregisterUserService unregisterUserService,
            IApolloListService apolloListService,
            IPreferenceService preferenceService,
            IUserRepository userRepository,
            IFavoriteRepository favoriteRepository,
            ILogger<SettingsViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(authService);
            ArgumentNullException.ThrowIfNull(sessionService);
            ArgumentNullException.ThrowIfNull(userService);
            ArgumentNullException.ThrowIfNull(profileService);
            ArgumentNullException.ThrowIfNull(unregisterUserService);
            ArgumentNullException.ThrowIfNull(apolloListService);
            ArgumentNullException.ThrowIfNull(preferenceService);
            ArgumentNullException.ThrowIfNull(userRepository);
            ArgumentNullException.ThrowIfNull(favoriteRepository);
            AuthService = authService;
            SessionService = sessionService;
            UserService = userService;
            ProfileService = profileService;
            UnregisterUserService = unregisterUserService;
            ApolloListService = apolloListService;
            PreferenceService = preferenceService;
            UserRepository = userRepository;
            FavoriteRepository = favoriteRepository;
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

        private IProfileService ProfileService { get; }

        private IUnregisterUserService UnregisterUserService { get; }

        private IApolloListService ApolloListService { get; }

        private ISessionService SessionService { get; }

        private IPreferenceService PreferenceService { get; }

        private IUserRepository UserRepository { get; }

        private IFavoriteRepository FavoriteRepository { get; }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanUnRegister))]
        private async Task UnRegister(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    if (SessionService.HasRegisteredUser)
                    {
                        var userId = PreferenceService.GetValue<string>(Preference.RegisteredUserId, null);
                        var uniqueId = SessionService.UniqueId;
                        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(uniqueId))
                        {
                            throw new NotSupportedException("Tokens not set.");
                        }

                        if (!await UnregisterUserService.DeleteAsync(userId, uniqueId, worker.Token))
                        {
                            Logger?.LogWarning($"User deletion unsuccessful while unregistering user in {GetType().Name}.");
                            throw new NotSupportedException("Unable to delete User.");
                        }
                    }

                    await AuthService.LogoutAsync(CancellationToken.None);
                    var authentication = await AuthService.AcquireTokenSilent(CancellationToken.None);
                    if (authentication == null)
                    {
                        await UserRepository.DeleteUserAsync(CancellationToken.None).ConfigureAwait(false);
                        await FavoriteRepository.DeleteFavoritesAsync(CancellationToken.None).ConfigureAwait(false);
                        UserService.UpdateAuthorizationHeader(null);
                        ProfileService.UpdateAuthorizationHeader(null);
                        UnregisterUserService.UpdateAuthorizationHeader(null);
                        ApolloListService.UpdateAuthorizationHeader(null);
                        SessionService.UpdateRegisteredUser(authentication?.UniqueId, authentication?.Account?.HomeAccountId);
                        PreferenceService.Delete();
                        await NavigationService.RestartAsync(CancellationToken.None);
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

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanOpenTerms))]
        private Task OpenTerms(CancellationToken token)
        {
            return OpenUrlAsync(Resources.Strings.Resources.SettingsView_TermsUri, token);
        }

        private bool CanOpenTerms()
        {
            return !IsBusy;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanOpenPrivacy))]
        private Task OpenPrivacy(CancellationToken token)
        {
            return OpenUrlAsync(Resources.Strings.Resources.SettingsView_PrivacyUri, token);
        }

        private bool CanOpenPrivacy()
        {
            return !IsBusy;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanOpenImprint))]
        private Task OpenImprint(CancellationToken token)
        {
            return OpenUrlAsync(Resources.Strings.Resources.SettingsView_ImprintUri, token);
        }

        private bool CanOpenImprint()
        {
            return !IsBusy;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanOpenMail))]
        private async Task OpenMail(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    if (Email.Default.IsComposeSupported)
                    {
                        string subject = Resources.Strings.Resources.SettingsView_EmailSubject;
                        string body = Resources.Strings.Resources.SettingsView_EmailBody;
                        string[] recipients = new[] { Resources.Strings.Resources.SettingsView_QuestionEmail };

                        var message = new EmailMessage
                        {
                            Subject = subject,
                            Body = body,
                            BodyFormat = EmailBodyFormat.PlainText,
                            To = new List<string>(recipients),
                        };

                        await Email.Default.ComposeAsync(message);
                    }
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenMail)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenMail)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(OpenMail)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanOpenMail()
        {
            return !IsBusy;
        }
    }
}
