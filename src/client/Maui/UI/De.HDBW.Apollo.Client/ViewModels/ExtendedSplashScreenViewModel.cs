namespace De.HDBW.Apollo.Client.ViewModels
{
    using System.Collections.ObjectModel;
    using CommunityToolkit.Mvvm.Input;
    using De.HDBW.Apollo.Client.Contracts;
    using De.HDBW.Apollo.Client.Models;
    using De.HDBW.Apollo.SharedContracts.Services;
    using Microsoft.Extensions.Logging;

    public partial class ExtendedSplashScreenViewModel : BaseViewModel
    {
        private readonly ObservableCollection<InstructionEntry> instructions = new ObservableCollection<InstructionEntry>();

        public ExtendedSplashScreenViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            IPreferenceService preferenceService,
            ISessionService sessionService,
            ILogger<ExtendedSplashScreenViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            this.PreferenceService = preferenceService;
            this.SessionService = sessionService;
            this.Instructions.Add(InstructionEntry.Import(null, "animation.json", Resources.Strings.Resource.ExtendedSplashScreenView_Instruction1));
            this.Instructions.Add(InstructionEntry.Import(null, "animation.json", Resources.Strings.Resource.ExtendedSplashScreenView_Instruction2));
            this.Instructions.Add(InstructionEntry.Import(null, "animation.json", Resources.Strings.Resource.ExtendedSplashScreenView_Instruction3));
        }

        public ObservableCollection<InstructionEntry> Instructions
        {
            get { return this.instructions; }
        }

        private IPreferenceService PreferenceService { get; }

        private ISessionService SessionService { get; }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            this.SkipCommand?.NotifyCanExecuteChanged();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSkip))]
        private async Task Skip(CancellationToken token)
        {
            this.IsBusy = true;
            try
            {
                if (this.SessionService.HasRegisteredUser)
                {
                    await this.NavigationService.PushToRootAsnc(Routes.StartView, token);
                }
                else
                {
                    await this.NavigationService.PushToRootAsnc(Routes.RegistrationView, token);
                }
            }
            catch (OperationCanceledException)
            {
                this.Logger?.LogDebug($"Canceled Skip in {this.GetType()}.");
            }
            catch (ObjectDisposedException)
            {
                this.Logger?.LogDebug($"Canceled Skip in {this.GetType()}.");
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex, $"Unknown Error in Skip in {this.GetType()}.");
            }
            finally
            {
                if (!token.IsCancellationRequested)
                {
                    this.IsBusy = false;
                }
            }
        }

        private bool CanSkip()
        {
            return !this.IsBusy;
        }
    }
}
