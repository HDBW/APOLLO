using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Data.Repositories;
using De.HDBW.Apollo.Data.Services;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;
using User = Invite.Apollo.App.Graph.Common.Models.UserProfile.User;
using UserProfile = Invite.Apollo.App.Graph.Common.Models.UserProfile.UserProfile;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public abstract class AbstractProfileEditorViewModel<TU> : AbstractSaveDataViewModel
    {
        private string? _enityId;

        protected AbstractProfileEditorViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger logger,
            IUserRepository userRepository,
            IUserService userService)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(userRepository);
            ArgumentNullException.ThrowIfNull(userService);
            UserRepository = userRepository;
            UserService = userService;
        }

        protected User? User { get; set; }

        protected IUserRepository UserRepository { get; }

        protected TU? Entry { get; set; }

        private IUserService UserService { get; }

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            _enityId = navigationParameters.GetValue<string?>(NavigationParameter.Id);
        }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    User = await UserRepository.GetItemAsync(worker.Token).ConfigureAwait(false);
                    Entry = await LoadDataAsync(_enityId, worker.Token).ConfigureAwait(false);
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
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        protected abstract Task<TU?> LoadDataAsync(string? enityId, CancellationToken token);

        protected override async Task<bool> SaveAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (User == null || !IsDirty)
            {
                return !IsDirty;
            }

            User.Profile = User.Profile ?? new UserProfile();
            TU entity = Entry ?? CreateNewEntry();
            ApplyChanges(entity);

            var response = await UserService.SaveAsync(User, token).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(response))
            {
                Logger.LogError($"Unable to save {nameof(TU)} remotely {nameof(SaveAsync)} in {GetType().Name}.");
                return !IsDirty;
            }

            User.Id = response;
            var userResult = await UserService.GetUserAsync(User.Id, token).ConfigureAwait(false);
            if (userResult == null || !await UserRepository.SaveAsync(userResult, CancellationToken.None).ConfigureAwait(false))
            {
                Logger.LogError($"Unable to save {nameof(TU)} locally {nameof(SaveAsync)} in {GetType().Name}.");
                return !IsDirty;
            }

            IsDirty = false;
            return !IsDirty;
        }

        protected abstract TU? CreateNewEntry();
        protected abstract void ApplyChanges(TU entity);
    }
}
