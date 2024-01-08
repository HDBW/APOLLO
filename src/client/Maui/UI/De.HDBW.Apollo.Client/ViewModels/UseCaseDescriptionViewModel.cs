// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class UseCaseDescriptionViewModel : BaseViewModel
    {
        [ObservableProperty]
        private UserProfileEntry? _userProfile = UserProfileEntry.Import(new UserProfileItem());

        [ObservableProperty]
        private string? _age;

        [ObservableProperty]
        private string? _location;

        [ObservableProperty]
        private string? _experience;

        [ObservableProperty]
        private string? _story;

        [ObservableProperty]
        private string? _goal;

        public UseCaseDescriptionViewModel(
           IDispatcherService dispatcherService,
           INavigationService navigationService,
           IDialogService dialogService,
           IUserProfileItemRepository userProfileItemRepository,
           ISessionService sessionService,
           IUseCaseBuilder builder,
           ILogger<UseCaseDescriptionViewModel> logger)
           : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(userProfileItemRepository);
            ArgumentNullException.ThrowIfNull(sessionService);
            UseCaseBuilder = builder;
            UserProfileItemRepository = userProfileItemRepository;
            SessionService = sessionService;
        }

        public string? ImagePath
        {
            get
            {
                switch (UseCase)
                {
                    case UseCase.A:
                        return "usecase1detail.png";
                    case UseCase.B:
                        return "usecase2detail.png";
                    case UseCase.C:
                        return "usecase3detail.png";
                    default:
                        return null;
                }
            }
        }

        public bool HasImage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ImagePath);
            }
        }

        public bool HasUserProfile
        {
            get
            {
                return UserProfile != null && UserProfile.Id != 0;
            }
        }

        public string? DisplayAge
        {
            get
            {
                return string.Format(Resources.Strings.Resources.UseCaseDescriptionView_AgeFormat, Age);
            }
        }

        public string DisplayLocation
        {
            get
            {
                return string.Format(Resources.Strings.Resources.UseCaseDescriptionView_LocationFormat, Location);
            }
        }

        private UseCase UseCase
        {
            get
            {
                return SessionService.UseCase ?? UseCase.Unknown;
            }
        }

        private bool? IsUseCaseSelectionFromShell { get; set; }

        private IUseCaseBuilder UseCaseBuilder { get; }

        private IUserProfileItemRepository UserProfileItemRepository { get; }

        private ISessionService SessionService { get; }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanCreateUseCase))]
        public async Task CreateUseCase(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    if (IsUseCaseSelectionFromShell == true)
                    {
                        await NavigationService.PushToRootAsync(token);
                    }
                    else
                    {
                        await NavigationService.PushToRootAsync(Routes.Shell, token);
                    }
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(CreateUseCase)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(CreateUseCase)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(CreateUseCase)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        public bool CanCreateUseCase()
        {
            return !IsBusy && UseCase != UseCase.Unknown;
        }

        public async override Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    SessionService.ClearFavorites();
                    if (!await UseCaseBuilder.BuildAsync(UseCase, worker.Token).ConfigureAwait(false))
                    {
                        return;
                    }

                    var user = await UserProfileItemRepository.GetItemByIdAsync(1, worker.Token).ConfigureAwait(false);

                    string? age = null;
                    string? location = null;
                    string? experience = null;
                    string? story = null;
                    string? goal = null;
                    switch (UseCase)
                    {
                        case UseCase.A:
                            age = Resources.Strings.Resources.UseCaseDetail_A_Age;
                            location = Resources.Strings.Resources.UseCaseDetail_A_Location;
                            experience = Resources.Strings.Resources.UseCaseDetail_A_Experience;
                            story = Resources.Strings.Resources.UseCaseDetail_A_Story;
                            goal = Resources.Strings.Resources.UseCaseDetail_A_Goal;
                            break;
                        case UseCase.B:
                            age = Resources.Strings.Resources.UseCaseDetail_B_Age;
                            location = Resources.Strings.Resources.UseCaseDetail_B_Location;
                            experience = Resources.Strings.Resources.UseCaseDetail_B_Experience;
                            story = Resources.Strings.Resources.UseCaseDetail_B_Story;
                            goal = Resources.Strings.Resources.UseCaseDetail_B_Goal;
                            break;
                        case UseCase.C:
                            age = Resources.Strings.Resources.UseCaseDetail_C_Age;
                            location = Resources.Strings.Resources.UseCaseDetail_C_Location;
                            experience = Resources.Strings.Resources.UseCaseDetail_C_Experience;
                            story = Resources.Strings.Resources.UseCaseDetail_C_Story;
                            goal = Resources.Strings.Resources.UseCaseDetail_C_Goal;
                            break;
                    }

                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(user, age, location, experience, story, goal), worker.Token);
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

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            CreateUseCaseCommand?.NotifyCanExecuteChanged();
        }

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            base.OnPrepare(navigationParameters);
            IsUseCaseSelectionFromShell = navigationParameters.GetValue<bool?>(NavigationParameter.Data);
        }

        private void LoadonUIThread(UserProfileItem? user, string? age, string? location, string? experience, string? story, string? goal)
        {
            UserProfile = UserProfileEntry.Import(user ?? new UserProfileItem());
            Age = age;
            Location = location;
            Experience = experience;
            Story = story;
            Goal = goal;
            OnPropertyChanged((string?)null);
        }
    }
}
