namespace De.HDBW.Apollo.Client.ViewModels
{
    using System.Collections.ObjectModel;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using De.HDBW.Apollo.Client.Contracts;
    using De.HDBW.Apollo.Client.Models;
    using Microsoft.Extensions.Logging;

    public partial class ExtendedSplashScreenViewModel : BaseViewModel
    {
        private readonly ObservableCollection<Instruction> instructions = new ObservableCollection<Instruction>();

        public ExtendedSplashScreenViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<ExtendedSplashScreenViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            this.Instructions.Add(Instruction.Import(null, Resources.Strings.Resource.ExtendedSplashScreenView_Instruction1));
            this.Instructions.Add(Instruction.Import(null, Resources.Strings.Resource.ExtendedSplashScreenView_Instruction2));
            this.Instructions.Add(Instruction.Import(null, Resources.Strings.Resource.ExtendedSplashScreenView_Instruction3));
        }

        public ObservableCollection<Instruction> Instructions
        {
            get { return this.instructions; }
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSkip))]
        private async Task Skip(CancellationToken token)
        {
            this.IsBusy = true;
            try
            {
                await this.NavigationService.PushToRootAsnc(Routes.Shell, token);
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
