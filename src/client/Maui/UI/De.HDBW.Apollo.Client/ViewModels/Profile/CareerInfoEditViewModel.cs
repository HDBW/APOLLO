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
            ILogger<PersonalInformationEditViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
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
                        switch (result?.GetValue<CareerType?>(NavigationParameter.Data))
                        {
                            case CareerType.Other:
                                break;
                            case CareerType.WorkExperience:
                                break;
                            case CareerType.PartTimeWorkExperience:
                                break;
                            case CareerType.Internship:
                                break;
                            case CareerType.SelfEmployment:
                                break;
                            case CareerType.Service:
                                break;
                            case CareerType.CommunityService:
                                break;
                            case CareerType.VoluntaryService:
                                break;
                            case CareerType.ParentalLeave:
                                break;
                            case CareerType.Homemaker:
                                break;
                            case CareerType.ExtraOccupationalExperience:
                                break;
                            case CareerType.PersonCare:
                                break;
                            default:
                                break;
                        }
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
    }
}
