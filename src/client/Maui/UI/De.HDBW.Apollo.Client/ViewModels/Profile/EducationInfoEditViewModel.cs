// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Dialogs;
using De.HDBW.Apollo.Client.Models;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public partial class EducationInfoEditViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<EducationInfo> _educations = new ObservableCollection<EducationInfo>();

        public EducationInfoEditViewModel(
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
                    parameters.Add(NavigationParameter.Data, EducationType.Unkown);
                    var result = await DialogService.ShowPopupAsync<SelectOptionDialog, NavigationParameters, NavigationParameters>(parameters, worker.Token);
                    if (result?.GetValue<bool?>(NavigationParameter.Result) ?? false)
                    {
                        var route = string.Empty;
                        switch (result?.GetValue<EducationType?>(NavigationParameter.Data))
                        {
                            case EducationType.Education:
                                route = Routes.EducationInfoEducationView;
                                break;
                            case EducationType.CompanyBasedVocationalTraining:
                                route = Routes.EducationInfoCompanyBasedVocationalTrainingView;
                                break;
                            case EducationType.Study:
                                route = Routes.EducationInfoStudyView;
                                break;
                            case EducationType.VocationalTraining:
                                route = Routes.EducationInfoVocationalTrainingView;
                                break;
                            case EducationType.FurtherEducation:
                                route = Routes.EducationInfoFurtherEducationView;
                                break;
                            default:
                                break;
                        }

                        await NavigationService.NavigateAsync(route, worker.Token);
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
