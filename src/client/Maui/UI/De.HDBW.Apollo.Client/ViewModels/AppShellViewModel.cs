// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Data.Services;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class AppShellViewModel : BaseViewModel
    {
        [ObservableProperty]
        private UserProfileEntry? _userProfile = UserProfileEntry.Import(new UserProfileItem());

        public AppShellViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<StartViewModel> logger,
            IUserProfileItemRepository userProfileItemRepository,
            ISessionService sessionService)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(userProfileItemRepository);
            ArgumentNullException.ThrowIfNull(sessionService);
            UserProfileItemRepository = userProfileItemRepository;
            SessionService = sessionService;
        }

        private IUserProfileItemRepository UserProfileItemRepository { get; }

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
                            var user = await UserProfileItemRepository.GetItemByIdAsync(1, worker.Token).ConfigureAwait(false);
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

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanChangeUseCase), FlowExceptionsToTaskScheduler = false, IncludeCancelCommand = false)]
        private async Task ChangeUseCase(CancellationToken token)
        {
            using (var work = ScheduleWork(token))
            {
                try
                {
                    var parameters = new NavigationParameters();
                    parameters.Add(NavigationParameter.Data, true);
                    await NavigationService.PushToRootAsnc(Routes.UseCaseSelectionView, token, parameters);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(ChangeUseCase)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(ChangeUseCase)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(ChangeUseCase)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(work);
                }
            }
        }

        private bool CanChangeUseCase()
        {
            return !IsBusy;
        }

        private void LoadonUIThread(UserProfileItem? user)
        {
            UserProfile = UserProfileEntry.Import(user ?? new UserProfileItem());
        }
    }
}
