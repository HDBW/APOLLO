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
using Invite.Apollo.App.Graph.Common.Models.Taxonomy;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile.CareerInfoEditors
{
    public partial class ServiceViewModel : BasicViewModel
    {
        [ObservableProperty]
        private string? _occupationName;

        [ObservableProperty]
        private string? _nameOfInstitution;

        [ObservableProperty]
        private string? _city;

        [ObservableProperty]
        private string? _country;

        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _serviceTypes = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private InteractionEntry? _selectedServiceType;

        private Occupation? _job;

        public ServiceViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<ServiceViewModel> logger,
            IUserRepository userRepository,
            IUserService userService)
            : base(dispatcherService, navigationService, dialogService, logger, userRepository, userService)
        {
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            SearchOccupationCommand?.NotifyCanExecuteChanged();
        }

        protected override async Task<CareerInfo?> LoadDataAsync(User user, string? entryId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var serviceTypes = new List<InteractionEntry>();
            serviceTypes.Add(InteractionEntry.Import(ServiceType.CivilianService.GetLocalizedString(), ServiceType.CivilianService, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            serviceTypes.Add(InteractionEntry.Import(ServiceType.MilitaryService.GetLocalizedString(), ServiceType.MilitaryService, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            serviceTypes.Add(InteractionEntry.Import(ServiceType.VoluntaryMilitaryService.GetLocalizedString(), ServiceType.VoluntaryMilitaryService, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            serviceTypes.Add(InteractionEntry.Import(ServiceType.MilitaryExercise.GetLocalizedString(), ServiceType.MilitaryExercise, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            var currentData = await base.LoadDataAsync(user, entryId, token).ConfigureAwait(false);
            var isDirty = IsDirty;
            var occupation = currentData?.Job;
            var nameOfInstitution = currentData?.NameOfInstitution;
            var city = currentData?.City;
            var country = currentData?.Country;
            var selectedServiceType = serviceTypes.FirstOrDefault(x => (x.Data as ServiceType?) == currentData?.ServiceType.AsEnum<ServiceType>()) ?? serviceTypes.FirstOrDefault();

            if (EditState != null)
            {
                occupation = EditState?.Job;
                nameOfInstitution = EditState?.NameOfInstitution;
                city = EditState?.City;
                country = EditState?.Country;
                selectedServiceType = serviceTypes.FirstOrDefault(x => (x.Data as ServiceType?) == EditState?.ServiceType.AsEnum<ServiceType>()) ?? serviceTypes.FirstOrDefault();
            }

            if (!string.IsNullOrWhiteSpace(SelectionResult))
            {
                occupation = SelectionResult.Deserialize<Occupation>();
                isDirty = true;
            }

            await ExecuteOnUIThreadAsync(() => LoadonUIThread(occupation, nameOfInstitution, city, country, serviceTypes.AsSortedList(), selectedServiceType, isDirty), token).ConfigureAwait(false);
            return currentData;
        }

        protected override void ApplyChanges(CareerInfo entry)
        {
            base.ApplyChanges(entry);
            entry.City = City;
            entry.Country = Country;
            entry.NameOfInstitution = NameOfInstitution;
            entry.ServiceType = ((SelectedServiceType?.Data as ServiceType?) ?? ServiceType.Unknown).ToApolloListItem();
            entry.Job = _job;
        }

        partial void OnCityChanged(string? value)
        {
            this.IsDirty = true;
        }

        partial void OnCountryChanged(string? value)
        {
            this.IsDirty = true;
        }

        partial void OnNameOfInstitutionChanged(string? value)
        {
            this.IsDirty = true;
        }

        partial void OnSelectedServiceTypeChanged(InteractionEntry? value)
        {
            this.IsDirty = true;
        }

        private void LoadonUIThread(Occupation? occupation, string? nameOfInstitution, string? city, string? country, List<InteractionEntry> serviceTypes, InteractionEntry? selectedServiceType, bool isDirty)
        {
            _job = occupation;
            ServiceTypes = new ObservableCollection<InteractionEntry>(serviceTypes);
            SelectedServiceType = selectedServiceType;
            OccupationName = _job?.PreferedTerm?.FirstOrDefault();
            NameOfInstitution = nameOfInstitution;
            City = city;
            Country = country;
            IsDirty = isDirty;
            ValidateCommand.Execute(null);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSearchOccupation))]
        private async Task SearchOccupation(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var parameter = new NavigationParameters();
                    parameter.AddValue(NavigationParameter.SavedState, GetCurrentState());
                    await NavigationService.NavigateAsync(Routes.OccupationSearchView, worker.Token, parameter);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(SearchOccupation)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(SearchOccupation)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(SearchOccupation)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanSearchOccupation()
        {
            return !IsBusy;
        }
    }
}
