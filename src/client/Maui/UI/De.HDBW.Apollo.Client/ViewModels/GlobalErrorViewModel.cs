using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Messages;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class GlobalErrorViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string? _message;

        public GlobalErrorViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<GlobalErrorViewModel> logger,
            INetworkService networkService,
            IMessenger messenger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(networkService);
            ArgumentNullException.ThrowIfNull(messenger);
            NetworkService = networkService;
            Messenger = messenger;
            Messenger.Register<NetworkStatusChangeMessage>(this, OnNetworkStatusChange);
        }

        public bool HasError
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Message);
            }
        }

        private INetworkService NetworkService { get; }

        private IMessenger Messenger { get; }

        public override Task OnNavigatedToAsync()
        {
            UpdateErrorState();
            return Task.CompletedTask;
        }

        private void OnNetworkStatusChange(object recipient, NetworkStatusChangeMessage message)
        {
            Logger.LogDebug($"Received {nameof(NetworkStatusChangeMessage)} in {GetType().Name}.");
            UpdateErrorState();
        }

        private async void UpdateErrorState()
        {
            var message = string.Empty;
            if (!NetworkService.HasNetworkConnection)
            {
                message = Resources.Strings.Resources.GlobalError_NoInternet;
            }

            await ExecuteOnUIThreadAsync(() => LoadOnUIThread(message), CancellationToken.None);
        }

        private void LoadOnUIThread(string message)
        {
            Message = message;
            OnPropertyChanged(nameof(HasError));
        }
    }
}
