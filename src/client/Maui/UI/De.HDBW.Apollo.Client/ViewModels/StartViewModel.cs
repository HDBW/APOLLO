namespace De.HDBW.Apollo.Client.ViewModels
{
    using System.Collections.ObjectModel;
    using CommunityToolkit.Mvvm.Input;
    using De.HDBW.Apollo.Client.Contracts;
    using De.HDBW.Apollo.Client.Dialogs;
    using De.HDBW.Apollo.Client.Models;
    using De.HDBW.Apollo.SharedContracts.Enums;
    using De.HDBW.Apollo.SharedContracts.Services;
    using Microsoft.Extensions.Logging;

    public partial class StartViewModel : BaseViewModel
    {
        private readonly ObservableCollection<Interaction> interactions = new ObservableCollection<Interaction>();

        public StartViewModel(
            IPreferenceService preferenceService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<StartViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            this.PreferenceService = preferenceService;
            this.Interactions.Add(Interaction.Import(Resources.Strings.Resource.StartViewModel_InteractionProfile, this.HandleInteract, this.CanHandleInteract));
            this.Interactions.Add(Interaction.Import(Resources.Strings.Resource.StartViewModel_InteractionCareer, this.HandleInteract, this.CanHandleInteract));
            this.Interactions.Add(Interaction.Import(Resources.Strings.Resource.StartViewModel_InteractionRetraining, this.HandleInteract, this.CanHandleInteract));
            this.Interactions.Add(Interaction.Import(Resources.Strings.Resource.StartViewModel_InteractionSkills, this.HandleInteract, this.CanHandleInteract));
            this.Interactions.Add(Interaction.Import(Resources.Strings.Resource.StartViewModel_InteractionCV, this.HandleInteract, this.CanHandleInteract));
        }

        public ObservableCollection<Interaction> Interactions
        {
            get
            {
                return this.interactions;
            }
        }

        private IPreferenceService PreferenceService { get; }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            this.OpenSettingsCommand?.NotifyCanExecuteChanged();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanOpenSettings), FlowExceptionsToTaskScheduler = false, IncludeCancelCommand = false)]
        private async Task OpenSettings(CancellationToken token)
        {
            try
            {
                this.IsBusy = true;
                var parameters = new NavigationParameters();
                parameters.AddValue(NavigationParameter.Id, 0);
                parameters.AddValue(NavigationParameter.Unknown, "Test");
                await this.NavigationService.NavigateAsnc(Routes.EmptyView, token, parameters);
            }
            catch (OperationCanceledException)
            {
                this.Logger?.LogDebug($"Canceled OpenSettings in {this.GetType()}.");
            }
            catch (ObjectDisposedException)
            {
                this.Logger?.LogDebug($"Canceled OpenSettings in {this.GetType()}.");
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex, $"Unknown Error in OpenSettings in {this.GetType()}.");
            }
            finally
            {
                if (!token.IsCancellationRequested)
                {
                    this.IsBusy = false;
                }
            }
        }

        private bool CanOpenSettings()
        {
            return !this.IsBusy;
        }

        [RelayCommand(AllowConcurrentExecutions = false, FlowExceptionsToTaskScheduler = false, IncludeCancelCommand = true)]
        private async Task LoadData(CancellationToken token)
        {
            try
            {
                var taskList = new List<Task>();
                Task<NavigationParameters> dialogTask = null;
                var isFirstTime = this.PreferenceService.GetValue(Preference.IsFirstTime, true);
                if (isFirstTime)
                {
                    this.PreferenceService.SetValue(Preference.IsFirstTime, false);
                    dialogTask = this.DialogService.ShowPopupAsync<FirstTimeDialog, NavigationParameters>(token);
                    taskList.Add(dialogTask);
                }

                if (taskList.Any())
                {
                    await Task.WhenAll(taskList).ConfigureAwait(false);
                }

                var selection = dialogTask?.Result?.GetValue<bool>(NavigationParameter.Result) ?? false;
                if (selection)
                {
                    await this.NavigationService.NavigateAsnc(Routes.TutorialView, token);
                }
            }
            catch (OperationCanceledException)
            {
                this.Logger?.LogDebug($"Canceled LoadData in {this.GetType()}.");
            }
            catch (ObjectDisposedException)
            {
                this.Logger?.LogDebug($"Canceled LoadData in {this.GetType()}.");
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex, $"Unknown Error in LoadData in {this.GetType()}.");
            }
        }

        private bool CanHandleInteract(Interaction arg)
        {
            return !this.IsBusy;
        }

        private Task HandleInteract(Interaction arg)
        {
            throw new NotImplementedException();
        }
    }
}
