// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Interactions;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class SelectOptionDialogViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _options = new ObservableCollection<InteractionEntry>();

        private InteractionEntry? _selectedOption;

        private object? _selectionType;

        public SelectOptionDialogViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<SelectOptionDialogViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }

        public InteractionEntry? SelectedOption
        {
            get
            {
                return _selectedOption;
            }

            set
            {
                if (SetProperty(ref _selectedOption, value))
                {
                    RefreshCommands();
                }
            }
        }

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            base.OnPrepare(navigationParameters);
            _selectionType = navigationParameters.GetValue<object?>(NavigationParameter.Data);
            var selections = new List<InteractionEntry>();
            switch (_selectionType)
            {
                case CareerType _:
                    selections.Add(InteractionEntry.Import(Resources.Strings.Resources.SelectOptionsDialog_CareerType_Other, CareerType.Other, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    selections.Add(InteractionEntry.Import(Resources.Strings.Resources.SelectOptionsDialog_CareerType_WorkExperience, CareerType.WorkExperience, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    selections.Add(InteractionEntry.Import(Resources.Strings.Resources.SelectOptionsDialog_CareerType_PartTimeWorkExperience, CareerType.PartTimeWorkExperience, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    selections.Add(InteractionEntry.Import(Resources.Strings.Resources.SelectOptionsDialog_CareerType_Internship, CareerType.Internship, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    selections.Add(InteractionEntry.Import(Resources.Strings.Resources.SelectOptionsDialog_CareerType_SelfEmployment, CareerType.SelfEmployment, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    selections.Add(InteractionEntry.Import(Resources.Strings.Resources.SelectOptionsDialog_CareerType_MilitaryService, CareerType.Service, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    selections.Add(InteractionEntry.Import(Resources.Strings.Resources.SelectOptionsDialog_CareerType_CommunityService, CareerType.CommunityService, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    selections.Add(InteractionEntry.Import(Resources.Strings.Resources.SelectOptionsDialog_CareerType_VoluntaryService, CareerType.VoluntaryService, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    selections.Add(InteractionEntry.Import(Resources.Strings.Resources.SelectOptionsDialog_CareerType_ParentalLeave, CareerType.ParentalLeave, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    selections.Add(InteractionEntry.Import(Resources.Strings.Resources.SelectOptionsDialog_CareerType_Homemaker, CareerType.Homemaker, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    selections.Add(InteractionEntry.Import(Resources.Strings.Resources.SelectOptionsDialog_CareerType_ExtraOccupationalExperience, CareerType.ExtraOccupationalExperience, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    selections.Add(InteractionEntry.Import(Resources.Strings.Resources.SelectOptionsDialog_CareerType_PersonCare, CareerType.PersonCare, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    break;
                case EducationType _:
                    selections.Add(InteractionEntry.Import(Resources.Strings.Resources.SelectOptionsDialog_EducationType_Education, EducationType.Education, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    selections.Add(InteractionEntry.Import(Resources.Strings.Resources.SelectOptionsDialog_EducationType_CompanyBasedVocationalTraining, EducationType.CompanyBasedVocationalTraining, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    selections.Add(InteractionEntry.Import(Resources.Strings.Resources.SelectOptionsDialog_EducationType_Study, EducationType.Study, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    selections.Add(InteractionEntry.Import(Resources.Strings.Resources.SelectOptionsDialog_EducationType_VocationalTraining, EducationType.VocationalTraining, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    selections.Add(InteractionEntry.Import(Resources.Strings.Resources.SelectOptionsDialog_EducationType_FurtherEducation, EducationType.FurtherEducation, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    break;
            }

            LoadonUIThread(selections);
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            CancelCommand?.NotifyCanExecuteChanged();
            ContinueCommand?.NotifyCanExecuteChanged();
        }

        private bool CanContinue()
        {
            return !IsBusy && SelectedOption != null;
        }

        [CommunityToolkit.Mvvm.Input.RelayCommand(CanExecute = nameof(CanContinue))]
        private void Continue()
        {
            var result = new NavigationParameters();
            result.AddValue(NavigationParameter.Result, true);
            result.AddValue(NavigationParameter.Data, SelectedOption?.Data);
            DialogService.ClosePopup(this, result);
        }

        private bool CanCancel()
        {
            return !IsBusy;
        }

        [CommunityToolkit.Mvvm.Input.RelayCommand(CanExecute = nameof(CanCancel))]
        private void Cancel()
        {
            var result = new NavigationParameters();
            result.AddValue(NavigationParameter.Result, false);
            DialogService.ClosePopup(this, result);
        }

        private void LoadonUIThread(List<InteractionEntry> options)
        {
            Options = new ObservableCollection<InteractionEntry>(options);
        }
    }
}
