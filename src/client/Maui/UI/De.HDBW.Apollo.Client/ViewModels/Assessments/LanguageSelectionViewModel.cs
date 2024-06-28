// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Interactions;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    public partial class LanguageSelectionViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<SelectInteractionEntry> _languages = new ObservableCollection<SelectInteractionEntry>();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FlowDirection))]
        private CultureInfo _culture = new CultureInfo("de-DE");

        private string _selectedLanguage;
        private string[] _supportedLanguages = Array.Empty<string>();
        private NavigationParameters? _parameters;

        public LanguageSelectionViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<LanguageSelectionViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }

        public FlowDirection FlowDirection { get { return Culture?.TextInfo.IsRightToLeft ?? false ? FlowDirection.RightToLeft : FlowDirection.LeftToRight; } }

        public async override Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var items = new List<SelectInteractionEntry>();
                    foreach (var language in _supportedLanguages)
                    {
#if ANDROID
                        var name = new CultureInfo(language)?.DisplayName;
#elif IOS
                        var name = new CultureInfo(language)?.NativeName;
#endif
                        items.Add(SelectInteractionEntry.Import(name ?? language, language, _selectedLanguage == language, Navigate, CanNavigate, ToggleSelection, CanToggleSelection, HandleSelectionChanged));
                    }

                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(items), worker.Token).ConfigureAwait(false);
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
            foreach (var language in Languages)
            {
                language.ToggleSelectionStateCommand.NotifyCanExecuteChanged();
                language.NavigateCommand.NotifyCanExecuteChanged();
            }
        }

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            base.OnPrepare(navigationParameters);
            _parameters = navigationParameters;
            _selectedLanguage = navigationParameters.GetValue<string?>(NavigationParameter.Data) ?? string.Empty;
            _supportedLanguages = navigationParameters.GetValue<string?>(NavigationParameter.Result)?.Split(';') ?? Array.Empty<string>();
            Culture = new CultureInfo(_selectedLanguage);
            OnPropertyChanged(string.Empty);
        }

        private void LoadonUIThread(List<SelectInteractionEntry> items)
        {
            Languages = new ObservableCollection<SelectInteractionEntry>(items);
        }

        private void HandleSelectionChanged(SelectInteractionEntry entry)
        {
        }

        private bool CanToggleSelection(SelectInteractionEntry entry)
        {
            return !IsBusy && !entry.IsSelected;
        }

        private async Task ToggleSelection(SelectInteractionEntry entry)
        {
            using (var worker = ScheduleWork())
            {
                entry.IsSelected = !entry.IsSelected;
                foreach (var language in Languages)
                {
                    if (entry == language)
                    {
                        continue;
                    }

                    language.IsSelected = false;
                }

                try
                {
                    var parameters = _parameters ?? new NavigationParameters();
                    parameters.AddValue(NavigationParameter.Result, entry.Data);
                    await NavigationService.PopAsync(worker.Token, parameters);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(ToggleSelection)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(ToggleSelection)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(ToggleSelection)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanNavigate(InteractionEntry entry)
        {
            return false;
        }

        private Task Navigate(InteractionEntry entry)
        {
            return Task.CompletedTask;
        }
    }
}
