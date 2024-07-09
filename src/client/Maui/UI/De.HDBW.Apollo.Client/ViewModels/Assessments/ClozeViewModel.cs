// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Messages;
using De.HDBW.Apollo.Client.Models.Assessment;
using De.HDBW.Apollo.SharedContracts.Questions;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    public partial class ClozeViewModel : AbstractQuestionViewModel<Cloze, ClozeEntry>
    {
        [ObservableProperty]
        private List<string> _ids = new List<string>();

        public ClozeViewModel(
            ISheetService sheetService,
            IAssessmentService service,
            IRawDataCacheRepository repository,
            IUserSecretsService userSecretsService,
            IAudioPlayerService audioPlayerService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<ClozeViewModel> logger)
            : base(service, repository, userSecretsService, audioPlayerService, dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(sheetService);
            SheetService = sheetService;
        }

        public bool IsWebViewLoaded { get; set; } = false;

        private ISheetService SheetService { get; }

        public override Task OnNavigatedToAsync()
        {
            WeakReferenceMessenger.Default.Register<SetValueMessage>(this, OnValueSet);
            return base.OnNavigatedToAsync();
        }

        public override Task OnNavigatingFromAsync()
        {
            WeakReferenceMessenger.Default.Unregister<SetValueMessage>(this);
            return base.OnNavigatingFromAsync();
        }

        protected override ClozeEntry CreateEntry(Cloze data)
        {
            return ClozeEntry.Import(data);
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            NavigateBackCommand?.NotifyCanExecuteChanged();
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName != nameof(Question))
            {
                return;
            }

            foreach (var id in Question?.Ids ?? new List<string>())
            {
                OnValueSet(this, new SetValueMessage(id, null));
            }

            WeakReferenceMessenger.Default.Send(new ReloadMessage());
        }

        private void OnValueSet(object recipient, SetValueMessage message)
        {
            Question?.OnSetValue(message.Id, message.Value);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanNavigateBack))]
        private async Task NavigateBack(CancellationToken token)
        {
            Logger.LogInformation($"Invoked {nameof(NavigateBackCommand)} in {GetType().Name}.");
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    if (SheetService.IsShowingSheet)
                    {
                        await SheetService.CloseAsync<SelectionSheetViewModel>();
                    }

                    await NavigationService.PopAsync(worker.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(NavigateBack)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(NavigateBack)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(NavigateBack)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanNavigateBack()
        {
            return !IsBusy;
        }
    }
}
