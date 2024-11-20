// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Messages;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Assessment;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    public partial class SelectionSheetViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<SelectableTextEntry> _items = new ObservableCollection<SelectableTextEntry>();
        private List<string> _selectionsValues = new List<string>();
        private string _id = string.Empty;

        public SelectionSheetViewModel(
            ISheetService sheetService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<SelectionSheetViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(sheetService);
            SheetService = sheetService;
        }

        private ISheetService SheetService { get; }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var items = _selectionsValues.Select(x => SelectableTextEntry.Import(x, () => { }, ToggleSelection, CanToggleSelection));
                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(items), worker.Token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
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
            CloseCommand?.NotifyCanExecuteChanged();
            foreach (var item in Items)
            {
                item.ToggleSelectionCommand?.NotifyCanExecuteChanged();
            }
        }

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            _id = navigationParameters.GetValue<string>(NavigationParameter.Id) ?? string.Empty;
            var selections = navigationParameters.GetValue<string>(NavigationParameter.Data);
            _selectionsValues = (selections ?? string.Empty).Split(";").ToList();
        }

        private void LoadonUIThread(IEnumerable<SelectableTextEntry> selections)
        {
            Items.Clear();
            foreach (var selection in selections)
            {
                Items.Add(selection);
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanClose), FlowExceptionsToTaskScheduler = false, IncludeCancelCommand = false)]
        private async Task Close(CancellationToken token)
        {
            Logger.LogInformation($"Invoked {nameof(CloseCommand)} in {GetType().Name}.");
            using (var worker = ScheduleWork())
            {
                try
                {
                    var text = Items.FirstOrDefault(x => x.IsSelected)?.Text ?? string.Empty;
                    await SheetService.CloseAsync(this);
                    WeakReferenceMessenger.Default.Send(new SelectionMessage(_id, text));
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Close)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Close)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(Close)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanClose()
        {
            return true;
        }

        private void ToggleSelection(SelectableTextEntry entry)
        {
            foreach (var item in Items)
            {
                if (item == entry)
                {
                    entry.IsSelected = !entry.IsSelected;
                }
                else
                {
                    item.IsSelected = false;
                }
            }
        }

        private bool CanToggleSelection(SelectableTextEntry entry)
        {
            return !IsBusy;
        }
    }
}
