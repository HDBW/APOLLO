// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Dialogs;
using De.HDBW.Apollo.Client.Models;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public partial class CareerInfoEditViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<CareerInfo> _careers = new ObservableCollection<CareerInfo>();

        public CareerInfoEditViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<CareerInfoEditViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var careeres = new List<CareerInfo>();
                    careeres.Add(new CareerInfo());
                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(careeres), worker.Token);
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
            AddCommand?.NotifyCanExecuteChanged();
            base.RefreshCommands();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanAdd))]
        private async Task Add(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var parameters = new NavigationParameters();
                    parameters.Add(NavigationParameter.Data, CareerType.WorkExperience);
                    var result = await DialogService.ShowPopupAsync<SelectOptionDialog, NavigationParameters, NavigationParameters>(parameters, worker.Token);
                    if (result?.GetValue<bool?>(NavigationParameter.Result) ?? false)
                    {
                        var route = string.Empty;
                        NavigationParameters? editorParameters = null;
                        switch (result?.GetValue<CareerType?>(NavigationParameter.Data))
                        {
                            case CareerType.Other:
                                route = Routes.CareerInfoOtherView;
                                break;
                            case CareerType.WorkExperience:
                                route = Routes.CareerInfoOccupationView;
                                break;
                            case CareerType.PartTimeWorkExperience:
                                editorParameters = new NavigationParameters();
                                editorParameters.Add(NavigationParameter.Data, WorkingTimeModel.MINIJOB);
                                route = Routes.CareerInfoOccupationView;
                                break;
                            case CareerType.Internship:
                                route = Routes.CareerInfoOtherView;
                                break;
                            case CareerType.SelfEmployment:
                                route = Routes.CareerInfoOccupationView;
                                break;
                            case CareerType.Service:
                                route = Routes.CareerInfoServiceView;
                                break;
                            case CareerType.CommunityService:
                                route = Routes.CareerInfoOtherView;
                                break;
                            case CareerType.VoluntaryService:
                                route = Routes.CareerInfoVoluntaryServiceView;
                                break;
                            case CareerType.ParentalLeave:
                                route = Routes.CareerInfoBasicView;
                                break;
                            case CareerType.Homemaker:
                                route = Routes.CareerInfoBasicView;
                                break;
                            case CareerType.ExtraOccupationalExperience:
                                route = Routes.CareerInfoBasicView;
                                break;
                            case CareerType.PersonCare:
                                route = Routes.CareerInfoBasicView;
                                break;
                            default:
                                break;
                        }

                        await NavigationService.NavigateAsync(route, worker.Token, editorParameters);
                    }
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Add)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Add)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(Add)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanAdd()
        {
            return !IsBusy;
        }

        private void LoadonUIThread(List<CareerInfo> careeres)
        {
            Careers = new ObservableCollection<CareerInfo>(careeres);
        }
    }
}
