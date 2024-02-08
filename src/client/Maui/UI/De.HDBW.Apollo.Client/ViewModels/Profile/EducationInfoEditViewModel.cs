// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Dialogs;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Profile;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public partial class EducationInfoEditViewModel : AbstractListViewModel<EducationInfoEntry, EducationInfo>
    {
        [ObservableProperty]
        private ObservableCollection<EducationInfo> _educations = new ObservableCollection<EducationInfo>();

        public EducationInfoEditViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<EducationInfoEditViewModel> logger,
            IUserRepository userRepository,
            IUserService userService)
            : base(dispatcherService, navigationService, dialogService, logger, userRepository, userService, Routes.EmptyView)
        {
        }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    User = await UserRepository.GetItemAsync(worker.Token).ConfigureAwait(false);
                    var items = new List<EducationInfo>();
                    items.AddRange(User?.Profile?.EducationInfos ?? new List<EducationInfo>());
                    items = items.AsSortedList();
                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(items.Select(x => EducationInfoEntry.Import(x, EditAsync, CanEdit, DeleteAsync, CanDelete))), worker.Token);
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

        protected override string? GetIdFromItem(AbstractProfileEntry<EducationInfo> entry)
        {
            return entry.Export().Id;
        }

        protected override void RemoveItemFromUser(User user, AbstractProfileEntry<EducationInfo> entry)
        {
            user.Profile!.EducationInfos!.Remove(entry.Export());
        }

        protected override async Task Add(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var parameters = new NavigationParameters();
                    parameters.Add(NavigationParameter.Data, EducationType.Unknown);
                    var result = await DialogService.ShowPopupAsync<SelectOptionDialog, NavigationParameters, NavigationParameters>(parameters, worker.Token);
                    if (result?.GetValue<bool?>(NavigationParameter.Result) ?? false)
                    {
                        var educationType = result?.GetValue<EducationType?>(NavigationParameter.Data);
                        parameters = new NavigationParameters();
                        parameters.Add(NavigationParameter.Type, educationType ?? EducationType.Unknown);
                        await NavigationService.NavigateAsync(GetRoute(educationType), worker.Token, parameters);
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

        protected override async Task EditAsync(AbstractProfileEntry<EducationInfo> entry)
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var id = GetIdFromItem(entry);
                    if (string.IsNullOrWhiteSpace(id))
                    {
                        return;
                    }

                    var careerType = entry.Export().EducationType;
                    NavigationParameters? editorParameters = new NavigationParameters();
                    editorParameters.AddValue<string>(NavigationParameter.Id, id);
                    editorParameters.Add(NavigationParameter.Type, careerType);
                    await NavigationService.NavigateAsync(GetRoute(careerType), worker.Token, editorParameters);
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

        private string GetRoute(EducationType? educationType)
        {
            var route = string.Empty;
            switch (educationType)
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

            return route;
        }
    }
}
