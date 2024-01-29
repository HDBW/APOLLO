// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile.LanguageEditors
{
    public partial class LanguageViewModel : AbstractProfileEditorViewModel<Language>
    {
        [ObservableProperty]
        [Required(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_PropertyRequired))]
        private string? _languageName;

        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _languageNiveaus = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        [Required(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_PropertyRequired))]
        private InteractionEntry? _selectedLanguageNiveau;

        private string? _savedState;

        private string? _selectionResult;

        private CultureInfo? _code;

        public LanguageViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<LanguageViewModel> logger,
            IUserService userService,
            IUserRepository userRepository)
            : base(dispatcherService, navigationService, dialogService, logger, userRepository, userService)
        {
        }

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            base.OnPrepare(navigationParameters);
            if (navigationParameters.ContainsKey(NavigationParameter.SavedState))
            {
                _savedState = navigationParameters.GetValue<string?>(NavigationParameter.SavedState);
            }

            _selectionResult = navigationParameters.ContainsKey(NavigationParameter.Result) ? navigationParameters.GetValue<string?>(NavigationParameter.Result) : null;
        }

        protected async override Task<Language?> LoadDataAsync(User user, string? enityId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var language = user?.Profile?.LanguageSkills.FirstOrDefault(x => x.Id == enityId);
            var niveau = language?.Niveau;
            var code = language?.Code;
#if ANDROID
            var name = code?.DisplayName;
#elif IOS
            var name = code?.NativeName;
#endif
            var isDirty = false;

            // Restore edit state
            if (!string.IsNullOrWhiteSpace(_savedState))
            {
                var currentState = _savedState.Deserialize<Language>();
                if (currentState != null)
                {
                    niveau = currentState.Niveau;
                    code = currentState.Name != null ? CultureInfo.GetCultures(CultureTypes.AllCultures).FirstOrDefault(c => c.Name == currentState.Name) : null;
                    name = currentState.Name;
                }
            }

            // Override language selection
            if (!string.IsNullOrWhiteSpace(_selectionResult))
            {
                code = CultureInfo.GetCultures(CultureTypes.AllCultures).FirstOrDefault(c => c.Name == _selectionResult);
#if ANDROID
                name = code?.DisplayName;
#elif IOS
                name = code?.NativeName;
#endif
            }

            isDirty = name != language?.Name || niveau != language?.Niveau;

            var languageNiveaus = new List<InteractionEntry>();
            languageNiveaus.Add(InteractionEntry.Import(Resources.Strings.Resources.LanguageNiveau_A1, LanguageNiveau.A1, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            languageNiveaus.Add(InteractionEntry.Import(Resources.Strings.Resources.LanguageNiveau_A2, LanguageNiveau.A2, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            languageNiveaus.Add(InteractionEntry.Import(Resources.Strings.Resources.LanguageNiveau_B1, LanguageNiveau.B1, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            languageNiveaus.Add(InteractionEntry.Import(Resources.Strings.Resources.LanguageNiveau_B2, LanguageNiveau.B2, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            languageNiveaus.Add(InteractionEntry.Import(Resources.Strings.Resources.LanguageNiveau_C1, LanguageNiveau.C1, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            languageNiveaus.Add(InteractionEntry.Import(Resources.Strings.Resources.LanguageNiveau_C2, LanguageNiveau.C2, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            var selectedNivau = languageNiveaus.FirstOrDefault(x => (LanguageNiveau?)x.Data == niveau);
            await ExecuteOnUIThreadAsync(() => LoadonUIThread(code, name, languageNiveaus.AsSortedList(), selectedNivau, isDirty), token).ConfigureAwait(false);
            return language;
        }

        protected override Language CreateNewEntry(User user)
        {
            var entry = new Language();
            user.Profile!.LanguageSkills.Add(entry);
            return entry;
        }

        protected override void DeleteEntry(User user, Language entry)
        {
            user.Profile!.LanguageSkills.Remove(entry);
        }

        protected override void ApplyChanges(Language entity)
        {
            entity.Code = _code!;
            entity.Name = entity.Code.Name;
#if ANDROID
            entity.Name = entity.Code.Name;
#elif IOS
            entity.Name = entity.Code.NativeName;
#endif
            entity.Niveau = (LanguageNiveau)SelectedLanguageNiveau!.Data!;
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            SearchLanguageCommand?.NotifyCanExecuteChanged();
        }

        partial void OnLanguageNameChanged(string? value)
        {
            ValidateProperty(value, nameof(LanguageName));
            IsDirty = true;
        }

        partial void OnSelectedLanguageNiveauChanged(InteractionEntry? value)
        {
            ValidateProperty(value, nameof(SelectedLanguageNiveau));
            IsDirty = true;
        }

        private void LoadonUIThread(CultureInfo? code, string? name, List<InteractionEntry> languageNiveaus, InteractionEntry? selectedNivau, bool isDirty)
        {
            _code = code;
            LanguageName = name;
            LanguageNiveaus = new ObservableCollection<InteractionEntry>(languageNiveaus);
            SelectedLanguageNiveau = selectedNivau ?? LanguageNiveaus.FirstOrDefault();
            IsDirty = isDirty;
            ValidateCommand?.Execute(null);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSearchLanguage))]
        private async Task SearchLanguage(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var language = new Language();
                    language.Id = EntryId;
                    language.Niveau = SelectedLanguageNiveau?.Data != null ? (LanguageNiveau)SelectedLanguageNiveau.Data : null;
                    language.Name = _code?.Name ?? string.Empty;

                    var data = language.Serialize();

                    // store state
                    _savedState = data;
                    var parameters = new NavigationParameters();
                    parameters.AddValue(NavigationParameter.SavedState, data);
                    await NavigationService.NavigateAsync(Routes.LanguageSearchView, token, parameters);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(SearchLanguage)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(SearchLanguage)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(SearchLanguage)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanSearchLanguage()
        {
            return !IsBusy;
        }
    }
}
