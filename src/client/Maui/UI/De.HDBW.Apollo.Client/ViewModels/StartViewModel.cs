﻿using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Dialogs;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    // TODO: We need a way to navigate back to UseCaseSelection. PushToRoot
    public partial class StartViewModel : BaseViewModel
    {
        private readonly ObservableCollection<InteractionEntry> _interactions = new ObservableCollection<InteractionEntry>();

        public StartViewModel(
            IPreferenceService preferenceService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<StartViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            PreferenceService = preferenceService;
            Interactions.Add(InteractionEntry.Import(Resources.Strings.Resource.StartViewModel_InteractionProfile, HandleInteract, CanHandleInteract));
            Interactions.Add(InteractionEntry.Import(Resources.Strings.Resource.StartViewModel_InteractionCareer, HandleInteract, CanHandleInteract));
            Interactions.Add(InteractionEntry.Import(Resources.Strings.Resource.StartViewModel_InteractionRetraining, HandleInteract, CanHandleInteract));
            Interactions.Add(InteractionEntry.Import(Resources.Strings.Resource.StartViewModel_InteractionSkills, HandleInteract, CanHandleInteract));
            Interactions.Add(InteractionEntry.Import(Resources.Strings.Resource.StartViewModel_InteractionCV, HandleInteract, CanHandleInteract));
        }

        public ObservableCollection<InteractionEntry> Interactions
        {
            get
            {
                return _interactions;
            }
        }

        private IPreferenceService PreferenceService { get; }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            OpenSettingsCommand?.NotifyCanExecuteChanged();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanOpenSettings), FlowExceptionsToTaskScheduler = false, IncludeCancelCommand = false)]
        private async Task OpenSettings(CancellationToken token)
        {
            try
            {
                IsBusy = true;
                var parameters = new NavigationParameters();
                parameters.AddValue(NavigationParameter.Id, 0);
                parameters.AddValue(NavigationParameter.Unknown, "Test");
                await NavigationService.NavigateAsnc(Routes.EmptyView, token, parameters);
            }
            catch (OperationCanceledException)
            {
                Logger?.LogDebug($"Canceled OpenSettings in {GetType()}.");
            }
            catch (ObjectDisposedException)
            {
                Logger?.LogDebug($"Canceled OpenSettings in {GetType()}.");
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown Error in OpenSettings in {GetType()}.");
            }
            finally
            {
                if (!token.IsCancellationRequested)
                {
                    IsBusy = false;
                }
            }
        }

        private bool CanOpenSettings()
        {
            return !IsBusy;
        }

        [RelayCommand(AllowConcurrentExecutions = false, FlowExceptionsToTaskScheduler = false, IncludeCancelCommand = true)]
        private async Task LoadData(CancellationToken token)
        {
            try
            {
                var taskList = new List<Task>();
                Task<NavigationParameters?>? dialogTask = null;
                var isFirstTime = PreferenceService.GetValue(Preference.IsFirstTime, true);
                if (isFirstTime)
                {
                    PreferenceService.SetValue(Preference.IsFirstTime, false);
                    dialogTask = DialogService.ShowPopupAsync<FirstTimeDialog, NavigationParameters>(token);
                    taskList.Add(dialogTask);
                }

                if (taskList.Any())
                {
                    await Task.WhenAll(taskList).ConfigureAwait(false);
                }

                var selection = dialogTask?.Result?.GetValue<bool>(NavigationParameter.Result) ?? false;
                if (selection)
                {
                    await NavigationService.NavigateAsnc(Routes.TutorialView, token);
                }
            }
            catch (OperationCanceledException)
            {
                Logger?.LogDebug($"Canceled LoadData in {GetType()}.");
            }
            catch (ObjectDisposedException)
            {
                Logger?.LogDebug($"Canceled LoadData in {GetType()}.");
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown Error in LoadData in {GetType()}.");
            }
        }

        private bool CanHandleInteract(InteractionEntry arg)
        {
            return !IsBusy;
        }

        private Task HandleInteract(InteractionEntry arg)
        {
            throw new NotImplementedException();
        }
    }
}