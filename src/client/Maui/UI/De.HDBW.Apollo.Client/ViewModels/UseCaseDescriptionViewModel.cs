// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Data.Services;
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
        private UserProfileEntry? _userProfile = UserProfileEntry.Import(new UserProfile());

        [ObservableProperty]
        private string? _age;

        [ObservableProperty]
        private string? _job;

        [ObservableProperty]
        private string? _scenario;

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
           IUserProfileRepository userProfileRepository,
           ISessionService sessionService,
           IUseCaseBuilder builder,
           ILogger<UseCaseDescriptionViewModel> logger)
           : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(userProfileRepository);
            ArgumentNullException.ThrowIfNull(sessionService);
            UseCaseBuilder = builder;
            UserProfileRepository = userProfileRepository;
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
                return string.Format(Resources.Strings.Resource.UseCaseDescriptionView_AgeFormat, Age);
            }
        }

        public string DisplayJob
        {
            get
            {
                return string.Format(Resources.Strings.Resource.UseCaseDescriptionView_JobFormat, Job);
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

        private IUserProfileRepository UserProfileRepository { get; }

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
                        await NavigationService.PushToRootAsnc(token);
                    }
                    else
                    {
                        await NavigationService.PushToRootAsnc(Routes.Shell, token);
                    }
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(CreateUseCase)} in {GetType()}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(CreateUseCase)} in {GetType()}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(CreateUseCase)} in {GetType()}.");
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
                    if (!await UseCaseBuilder.BuildAsync(UseCase, worker.Token).ConfigureAwait(false))
                    {
                        return;
                    }

                    var user = await UserProfileRepository.GetItemByIdAsync(1, worker.Token).ConfigureAwait(false);

                    string? age = null;
                    string? job = null;
                    string? scenario = null;
                    string? experience = null;
                    string? story = null;
                    string? goal = null;
                    switch (UseCase)
                    {
                        case UseCase.A:
                            age = Resources.Strings.Resource.UseCaseDetail_A_Age;
                            job = Resources.Strings.Resource.UseCaseDetail_A_Job;
                            scenario = Resources.Strings.Resource.UseCaseDetail_A_Scenario;
                            experience = Resources.Strings.Resource.UseCaseDetail_A_Experience;
                            story = Resources.Strings.Resource.UseCaseDetail_A_Story;
                            goal = Resources.Strings.Resource.UseCaseDetail_A_Goal;
                            break;
                        case UseCase.B:
                            age = Resources.Strings.Resource.UseCaseDetail_B_Age;
                            job = Resources.Strings.Resource.UseCaseDetail_B_Job;
                            scenario = Resources.Strings.Resource.UseCaseDetail_B_Scenario;
                            experience = Resources.Strings.Resource.UseCaseDetail_B_Experience;
                            story = Resources.Strings.Resource.UseCaseDetail_B_Story;
                            goal = Resources.Strings.Resource.UseCaseDetail_B_Goal;
                            break;
                        case UseCase.C:
                            age = Resources.Strings.Resource.UseCaseDetail_C_Age;
                            job = Resources.Strings.Resource.UseCaseDetail_C_Job;
                            scenario = Resources.Strings.Resource.UseCaseDetail_C_Scenario;
                            experience = Resources.Strings.Resource.UseCaseDetail_C_Experience;
                            story = Resources.Strings.Resource.UseCaseDetail_C_Story;
                            goal = Resources.Strings.Resource.UseCaseDetail_C_Goal;
                            break;
                    }

                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(user, age, job, scenario, experience, story, goal), worker.Token);
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

        private void LoadonUIThread(UserProfile? user, string? age, string? job, string? scenario, string? experience, string? story, string? goal)
        {
            UserProfile = UserProfileEntry.Import(user ?? new UserProfile());
            Age = age;
            Job = job;
            Scenario = scenario;
            Experience = experience;
            Story = story;
            Goal = goal;
            OnPropertyChanged((string?)null);
        }
    }
}
