// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Interactions;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public partial class OccupationSearchViewModel : BaseViewModel
    {
        private string? _searchText;

        private InteractionEntry? _selectedItem;

        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _items = new ObservableCollection<InteractionEntry>();

        private List<string> _occupationNames = new List<string>();

        public OccupationSearchViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<OccupationSearchViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }

        public string? SearchText
        {
            get
            {
                return _searchText;
            }

            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    RefreshCommands();
                }
            }
        }

        public InteractionEntry? SelectedItem
        {
            get
            {
                return _selectedItem;
            }

            set
            {
                if (SetProperty(ref _selectedItem, value))
                {
                    RefreshCommands();
                    if (_selectedItem != null)
                    {
                        ApplyAndClose();
                    }
                }
            }
        }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    using (var stream = await FileSystem.OpenAppPackageFileAsync("CareerInfo_JobTtitle_filtered.txt"))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            _occupationNames = ReadLines(reader).Order().ToList();
                        }
                    }

                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(), worker.Token);
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
            base.RefreshCommands();
            SearchCommand?.NotifyCanExecuteChanged();
        }

        private void LoadonUIThread()
        {
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSearch))]
        private Task Search(string searchtext, CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var items = _occupationNames.Where(x => x.Contains(searchtext, StringComparison.InvariantCultureIgnoreCase)).Select(x => InteractionEntry.Import(x, x, (x) => { return Task.CompletedTask; }, (x) => { return true; })).ToList();
                    if (!items.Any())
                    {
                        items.Add(InteractionEntry.Import(searchtext, searchtext, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    }

                    Items = new ObservableCollection<InteractionEntry>(items);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Search)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Search)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(Search)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }

                return Task.CompletedTask;
            }
        }

        private bool CanSearch(string searchtext)
        {
            return !IsBusy && (searchtext?.Replace(" ", string.Empty).Length ?? 0) > 3;
        }

        private IEnumerable<string> ReadLines(StreamReader reader)
        {
            string line;
            while ((line = reader?.ReadLine()) != null)
            {
                yield return line;
            }
        }

        private async void ApplyAndClose()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    await NavigationService.PopAsync(worker.Token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(ApplyAndClose)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(ApplyAndClose)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(ApplyAndClose)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }
    }
}
