// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
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
        private string? _savedState;
        private string? _selectionResult;

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
            var editState = _savedState.Deserialize<CareerInfo?>();
            var serviceTypes = new List<InteractionEntry>();
            serviceTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.ServiceType_CivilianService, ServiceType.CivilianService, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            serviceTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.ServiceType_MilitaryService, ServiceType.MilitaryService, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            serviceTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.ServiceType_VoluntaryMilitaryService, ServiceType.VoluntaryMilitaryService, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            serviceTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.ServiceType_MilitaryExercise, ServiceType.MilitaryExercise, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            var currentData = await base.LoadDataAsync(user, entryId, token).ConfigureAwait(false);
            await ExecuteOnUIThreadAsync(() => LoadonUIThread(currentData, serviceTypes), token).ConfigureAwait(false);
            return currentData;
        }

        protected override void ApplyChanges(CareerInfo entity)
        {
            base.ApplyChanges(entity);
            entity.City = City;
            entity.Country = Country;
            entity.NameOfInstitution = NameOfInstitution;
            entity.ServiceType = (SelectedServiceType?.Data as ServiceType?) ?? ServiceType.Unknown;
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

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            base.OnPrepare(navigationParameters);
            _savedState = navigationParameters.GetValue<string>(NavigationParameter.Data);
            _selectionResult = navigationParameters.GetValue<string>(NavigationParameter.Result);
        }

        private void LoadonUIThread(CareerInfo? careerInfo, List<InteractionEntry> serviceTypes)
        {
            ServiceTypes = new ObservableCollection<InteractionEntry>(serviceTypes);
            SelectedServiceType = ServiceTypes.FirstOrDefault(x => (x.Data as ServiceType?) == careerInfo?.ServiceType) ?? ServiceTypes.FirstOrDefault();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSearchOccupation))]
        private async Task SearchOccupation(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var parameter = new NavigationParameters();
                    parameter.AddValue(NavigationParameter.Data, GetCurrentState());
                    await NavigationService.NavigateAsync(Routes.OccupationSearchView, token, parameter);
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
