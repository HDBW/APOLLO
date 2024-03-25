// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Dialogs;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Profile;
using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public partial class CareerInfoEditViewModel : AbstractListViewModel<CareerInfoEntry, CareerInfo>
    {
        public CareerInfoEditViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<CareerInfoEditViewModel> logger,
            IUserRepository userRepository,
            IUserService userService)
            : base(dispatcherService, navigationService, dialogService, logger, userRepository, userService, Routes.EmptyView)
        {
        }

        public override async Task OnNavigatedToAsync()
        {
            if (IsShowingDialog)
            {
                return;
            }

            using (var worker = ScheduleWork())
            {
                try
                {
                    User = await UserRepository.GetItemAsync(worker.Token).ConfigureAwait(false);
                    var items = new List<CareerInfo>();
                    items.AddRange(User?.Profile?.CareerInfos ?? new List<CareerInfo>());
                    items = items.AsSortedList();
                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(items.Select(x => CareerInfoEntry.Import(x, EditAsync, CanEdit, DeleteAsync, CanDelete))), worker.Token);
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

        protected override string? GetIdFromItem(AbstractProfileEntry<CareerInfo> entry)
        {
            return entry.Export().Id;
        }

        protected override void RemoveItemFromUser(User user, AbstractProfileEntry<CareerInfo> entry)
        {
            user.Profile!.CareerInfos!.Remove(entry.Export());
        }

        protected override async Task Add(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    IsShowingDialog = true;

                    var parameters = new NavigationParameters();
                    parameters.Add(NavigationParameter.Data, CareerType.WorkExperience);
                    var result = await DialogService.ShowPopupAsync<SelectOptionDialog, NavigationParameters, NavigationParameters>(parameters, worker.Token);
                    if (result?.GetValue<bool?>(NavigationParameter.Result) ?? false)
                    {
                        var route = string.Empty;
                        NavigationParameters? editorParameters = new NavigationParameters();
                        var careerType = result.GetValue<CareerType>(NavigationParameter.Data);
                        editorParameters.Add(NavigationParameter.Type, careerType);

                        switch (careerType)
                        {
                            case CareerType.Other:
                                route = Routes.CareerInfoOtherView;
                                break;
                            case CareerType.WorkExperience:
                                route = Routes.CareerInfoOccupationView;
                                break;
                            case CareerType.PartTimeWorkExperience:
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
                    IsShowingDialog = false;
                    UnscheduleWork(worker);
                }
            }
        }

        protected override async Task EditAsync(AbstractProfileEntry<CareerInfo> entry)
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

                    var careerType = entry.Export().CareerType.AsEnum<CareerType>()!;
                    NavigationParameters? editorParameters = new NavigationParameters();
                    editorParameters.AddValue<string>(NavigationParameter.Id, id);
                    editorParameters.Add(NavigationParameter.Type, careerType);
                    var route = string.Empty;
                    switch (careerType)
                    {
                        case CareerType.Other:
                            route = Routes.CareerInfoOtherView;
                            break;
                        case CareerType.WorkExperience:
                            route = Routes.CareerInfoOccupationView;
                            break;
                        case CareerType.PartTimeWorkExperience:
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
    }
}
