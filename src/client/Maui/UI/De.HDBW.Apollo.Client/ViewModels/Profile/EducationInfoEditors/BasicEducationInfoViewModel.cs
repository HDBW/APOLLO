// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
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

namespace De.HDBW.Apollo.Client.ViewModels.Profile.EducationInfoEditors
{
    public partial class BasicEducationInfoViewModel : AbstractProfileEditorViewModel<EducationInfo>
    {
        [ObservableProperty]
        private DateTime _start = DateTime.Today;

        [ObservableProperty]
        private DateTime? _end;

        [ObservableProperty]
        private string? _nameOfInstitution;

        [ObservableProperty]
        private string? _city;

        [ObservableProperty]
        private string? _country;

        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _completionStates = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private InteractionEntry? _selectedCompletionState;

        private EducationType? _type;

        private string? _savedState;

        private string? _selectionResult;

        public BasicEducationInfoViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<BasicEducationInfoViewModel> logger,
            IUserRepository userRepository,
            IUserService userService)
            : base(dispatcherService, navigationService, dialogService, logger, userRepository, userService)
        {
        }

        public string Title
        {
            get
            {
                return _type.GetLocalizedString();
            }
        }

        public bool HasEnd
        {
            get
            {
                return End.HasValue;
            }
        }

        protected string? SelectionResult
        {
            get
            {
                return _selectionResult;
            }
        }

        protected EducationInfo? EditState { get; private set; }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            ClearEndCommand?.NotifyCanExecuteChanged();
        }

        protected override async Task<EducationInfo?> LoadDataAsync(User user, string? entryId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var completionStates = new List<InteractionEntry>();
            completionStates.Add(InteractionEntry.Import(CompletionState.Completed.GetLocalizedString(), CompletionState.Completed, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            completionStates.Add(InteractionEntry.Import(CompletionState.Ongoning.GetLocalizedString(), CompletionState.Ongoning, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            completionStates.Add(InteractionEntry.Import(CompletionState.Failed.GetLocalizedString(), CompletionState.Failed, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

            var currentData = user.Profile?.EducationInfos?.FirstOrDefault(x => x.Id == entryId);
            EditState = _savedState.Deserialize<EducationInfo?>();
            var currentState = currentData.Serialize<EducationInfo?>();
            var isDirty = !string.Equals(currentState, _savedState);

            var start = currentData?.Start;
            var end = currentData?.End;
            var nameOfInstitution = currentData?.NameOfInstitution;
            var city = currentData?.City;
            var country = currentData?.Country;
            var selectedCompletionState = completionStates.FirstOrDefault(x => (x.Data as CompletionState?) == currentData?.CompletionState) ?? CompletionStates.FirstOrDefault();

            if (EditState != null)
            {
                start = EditState.Start;
                end = EditState.End;
                nameOfInstitution = EditState.NameOfInstitution;
                city = EditState.City;
                country = EditState.Country;
                selectedCompletionState = completionStates.FirstOrDefault(x => (x.Data as CompletionState?) == EditState.CompletionState);
            }

            await ExecuteOnUIThreadAsync(() => LoadonUIThread(start, end, nameOfInstitution, city, country, selectedCompletionState, completionStates.AsSortedList(), isDirty), token).ConfigureAwait(false);
            return currentData;
        }

        protected override EducationInfo CreateNewEntry(User user)
        {
            var entry = new EducationInfo();
            entry.EducationType = _type ?? EducationType.Unkown;
            user.Profile!.EducationInfos = user.Profile!.EducationInfos ?? new List<EducationInfo>();
            user.Profile!.EducationInfos.Add(entry);
            return entry;
        }

        protected override void DeleteEntry(User user, EducationInfo entry)
        {
            user.Profile!.EducationInfos!.Remove(entry);
        }

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            base.OnPrepare(navigationParameters);
            _type = navigationParameters.GetValue<EducationType?>(NavigationParameter.Type);
            _savedState = navigationParameters.ContainsKey(NavigationParameter.SavedState) ? navigationParameters.GetValue<string>(NavigationParameter.SavedState) : null;
            _selectionResult = navigationParameters.ContainsKey(NavigationParameter.Result) ? navigationParameters.GetValue<string>(NavigationParameter.Result) : null;
        }

        protected override void ApplyChanges(EducationInfo entry)
        {
            entry.Start = Start.ToDTODate();
            entry.End = End.ToDTODate();
            entry.NameOfInstitution = NameOfInstitution;
            entry.City = City;
            entry.Country = Country;
            entry.CompletionState = (SelectedCompletionState?.Data as CompletionState?) ?? CompletionState.Failed;
        }

        partial void OnEndChanged(DateTime? value)
        {
            IsDirty = true;
            OnPropertyChanged(nameof(HasEnd));
            RefreshCommands();
        }

        partial void OnStartChanged(DateTime value)
        {
            IsDirty = true;
        }

        partial void OnSelectedCompletionStateChanged(InteractionEntry? value)
        {
            IsDirty = true;
        }

        partial void OnCityChanged(string? value)
        {
            IsDirty = true;
        }

        partial void OnCountryChanged(string? value)
        {
            IsDirty = true;
        }

        partial void OnNameOfInstitutionChanged(string? value)
        {
            IsDirty = true;
        }

        private void LoadonUIThread(DateTime? start, DateTime? end, string? nameOfInstitution, string? city, string? country, InteractionEntry? completionState, List<InteractionEntry> completionStates, bool isDirty)
        {
            Start = start.ToUIDate() ?? Start;
            End = end.ToUIDate();
            NameOfInstitution = nameOfInstitution;
            City = city;
            Country = country;
            CompletionStates = new ObservableCollection<InteractionEntry>(completionStates);
            SelectedCompletionState = completionState;
            IsDirty = isDirty;
            ValidateCommand.Execute(null);
        }

        [RelayCommand(CanExecute = nameof(CanClearEnd))]
        private void ClearEnd()
        {
            End = null;
        }

        private bool CanClearEnd()
        {
            return !IsBusy && HasEnd;
        }
    }
}
