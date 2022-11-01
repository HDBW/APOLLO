// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class ExtendedSplashScreenViewModel : BaseViewModel
    {
        private readonly ObservableCollection<InstructionEntry> _instructions = new ObservableCollection<InstructionEntry>();

        public ExtendedSplashScreenViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            IPreferenceService preferenceService,
            ISessionService sessionService,
            ILogger<ExtendedSplashScreenViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            PreferenceService = preferenceService;
            SessionService = sessionService;
            Instructions.Add(InstructionEntry.Import("splashdeco1.png", null, Resources.Strings.Resource.ExtendedSplashScreenView_Instruction1, Resources.Strings.Resource.ExtendedSplashScreenView_Instruction1Detail));
            Instructions.Add(InstructionEntry.Import(null, null, Resources.Strings.Resource.ExtendedSplashScreenView_Instruction2, Resources.Strings.Resource.ExtendedSplashScreenView_Instruction2Detail));
            Instructions.Add(InstructionEntry.Import(null, null, Resources.Strings.Resource.ExtendedSplashScreenView_Instruction3, Resources.Strings.Resource.ExtendedSplashScreenView_Instruction3Detail));
        }

        public ObservableCollection<InstructionEntry> Instructions
        {
            get { return _instructions; }
        }

        private IPreferenceService PreferenceService { get; }

        private ISessionService SessionService { get; }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            SkipCommand?.NotifyCanExecuteChanged();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSkip))]
        private async Task Skip(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    if (SessionService.HasRegisteredUser)
                    {
                        await NavigationService.PushToRootAsnc(Routes.StartView, token);
                    }
                    else
                    {
                        await NavigationService.PushToRootAsnc(Routes.RegistrationView, token);
                    }
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Skip)} in {GetType()}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Skip)} in {GetType()}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(Skip)} in {GetType()}.");
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
    }
}
