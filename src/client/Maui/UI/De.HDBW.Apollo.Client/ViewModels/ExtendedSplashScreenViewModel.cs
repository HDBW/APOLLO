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
            Instructions.Add(InstructionEntry.Import(null, "animation.json", Resources.Strings.Resource.ExtendedSplashScreenView_Instruction1));
            Instructions.Add(InstructionEntry.Import(null, "animation.json", Resources.Strings.Resource.ExtendedSplashScreenView_Instruction2));
            Instructions.Add(InstructionEntry.Import(null, "animation.json", Resources.Strings.Resource.ExtendedSplashScreenView_Instruction3));
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
            IsBusy = true;
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
                Logger?.LogDebug($"Canceled Skip in {GetType()}.");
            }
            catch (ObjectDisposedException)
            {
                Logger?.LogDebug($"Canceled Skip in {GetType()}.");
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown Error in Skip in {GetType()}.");
            }
            finally
            {
                if (!token.IsCancellationRequested)
                {
                    IsBusy = false;
                }
            }
        }

        private bool CanSkip()
        {
            return !IsBusy;
        }
    }
}
