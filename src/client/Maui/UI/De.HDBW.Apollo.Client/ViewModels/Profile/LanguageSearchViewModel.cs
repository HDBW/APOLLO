// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Interactions;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public partial class LanguageSearchViewModel : BaseViewModel
    {
        private string? _searchText;

        private InteractionEntry? _selectedItem;

        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _items = new ObservableCollection<InteractionEntry>();

        private List<string> _occupationNames = new List<string>();

        private IEnumerable<InteractionEntry>? _allCultures;

        private NavigationParameters? _parameters;

        public LanguageSearchViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<LanguageSearchViewModel> logger)
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
                    if (SearchCommand.CanExecute(SearchText))
                    {
                        SearchCommand.Execute(SearchText);
                    }
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
                    var allCultures = new List<InteractionEntry>();
                    foreach (var cultuer in CultureInfo.GetCultures(CultureTypes.AllCultures))
                    {
                        allCultures.Add(InteractionEntry.Import(cultuer.DisplayName, cultuer.Name, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    }

                    allCultures = allCultures.AsSortedList();
                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(allCultures), worker.Token);
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

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            _parameters = navigationParameters;
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            SearchCommand?.NotifyCanExecuteChanged();
        }

        private async void LoadonUIThread(IEnumerable<InteractionEntry> cultures)
        {
            _allCultures = cultures;
            await Search(null, CancellationToken.None);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSearch))]
        private Task Search(string? searchtext, CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(searchtext))
                    {
                        Items = new ObservableCollection<InteractionEntry>(_allCultures ?? Array.Empty<InteractionEntry>());
                    }
                    else
                    {
                        Items = new ObservableCollection<InteractionEntry>(_allCultures?.Where(x => x.Text?.Contains(searchtext, StringComparison.CurrentCultureIgnoreCase) ?? false) ?? Array.Empty<InteractionEntry>());
                    }
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

        private bool CanSearch(string? searchtext)
        {
            return !IsBusy && (searchtext?.Replace(" ", string.Empty).Length ?? 0) > 0;
        }

        private async void ApplyAndClose()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    _parameters = _parameters ?? new NavigationParameters();
                    _parameters.AddValue(NavigationParameter.Result, SelectedItem?.Data?.ToString());
                    await NavigationService.PopAsync(worker.Token, _parameters);
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
