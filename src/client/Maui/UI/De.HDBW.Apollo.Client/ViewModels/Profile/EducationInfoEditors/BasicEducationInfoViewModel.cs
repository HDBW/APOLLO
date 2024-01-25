// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Interactions;
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
                switch (_type)
                {
                    case EducationType.CompanyBasedVocationalTraining:
                        return Resources.Strings.Resources.SelectOptionsDialog_EducationType_CompanyBasedVocationalTraining;
                    case EducationType.Education:
                        return Resources.Strings.Resources.SelectOptionsDialog_EducationType_Education;
                    case EducationType.FurtherEducation:
                        return Resources.Strings.Resources.SelectOptionsDialog_EducationType_FurtherEducation;
                    case EducationType.Study:
                        return Resources.Strings.Resources.SelectOptionsDialog_EducationType_Study;
                    case EducationType.VocationalTraining:
                        return Resources.Strings.Resources.SelectOptionsDialog_EducationType_VocationalTraining;
                    default:
                        return string.Empty;
                }
            }
        }

        public bool HasEnd
        {
            get
            {
                return End.HasValue;
            }
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            ClearEndCommand?.NotifyCanExecuteChanged();
        }

        protected override async Task<EducationInfo?> LoadDataAsync(User user, string? entryId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var currentData = user.Profile?.EducationInfos.FirstOrDefault(x => x.Id == entryId);

            var completionStates = new List<InteractionEntry>();
            completionStates.Add(InteractionEntry.Import(Resources.Strings.Resources.CompletionState_Completed, CompletionState.Completed, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            completionStates.Add(InteractionEntry.Import(Resources.Strings.Resources.CompletionState_Failed, CompletionState.Failed, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            completionStates.Add(InteractionEntry.Import(Resources.Strings.Resources.CompletionState_Ongoning, CompletionState.Ongoning, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

            await ExecuteOnUIThreadAsync(() => LoadonUIThread(currentData, completionStates), token).ConfigureAwait(false);
            return currentData;
        }

        protected override EducationInfo CreateNewEntry(User user)
        {
            var entry = new EducationInfo();
            entry.EducationType = _type ?? EducationType.Unkown;
            user.Profile!.EducationInfos.Add(entry);
            return entry;
        }

        protected override void DeleteEntry(User user, EducationInfo entry)
        {
            user.Profile!.EducationInfos.Remove(entry);
        }

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            base.OnPrepare(navigationParameters);
            _type = navigationParameters.GetValue<EducationType?>(NavigationParameter.Type);
        }

        protected override void ApplyChanges(EducationInfo entry)
        {
            entry.Start = Start.ToDTODate();
            entry.End = End.ToDTODate();
            entry.NameOfInstitution = NameOfInstitution;
            entry.City = City;
            entry.Country = Country;
            entry.EducationType = _type ?? EducationType.Unkown;
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

        private void LoadonUIThread(EducationInfo? educationInfo, List<InteractionEntry> completionStates)
        {
            Start = educationInfo?.Start.ToUIDate() ?? Start;
            End = educationInfo?.End.ToUIDate();
            NameOfInstitution = educationInfo?.NameOfInstitution ?? string.Empty;
            City = educationInfo?.City ?? string.Empty;
            Country = educationInfo?.Country ?? string.Empty;
            CompletionStates = new ObservableCollection<InteractionEntry>(completionStates);
            SelectedCompletionState = CompletionStates.FirstOrDefault(x => (x.Data as CompletionState?) == educationInfo?.CompletionState) ?? CompletionStates.FirstOrDefault();
            IsDirty = false;
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
