// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Generic;
using De.HDBW.Apollo.Client.Models.Interactions;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public partial class ProfileViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<ObservableObject> _sections = new ObservableCollection<ObservableObject>();

        public ProfileViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<ProfileViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var sections = new List<ObservableObject>();
                    sections.Add(InteractionEntry.Import(Resources.Strings.Resources.PersonalInformationEditView_Title, new NavigationData(Routes.PersonalInformationEditView, null), NavigateToRoute, CanNavigateToRoute));
                    sections.Add(StringValue.Import(Resources.Strings.Resources.PersonalInformationEditView_UserName, "Fritz24"));
                    sections.Add(StringValue.Import(Resources.Strings.Resources.PersonalInformationEditView_Birthdate, DateTime.Today.ToString()));
                    sections.Add(SeperatorValue.Import());
                    sections.Add(InteractionEntry.Import(Resources.Strings.Resources.WebReferenceEditView_Title, new NavigationData(Routes.WebReferenceEditView, null), NavigateToRoute, CanNavigateToRoute));
                    sections.Add(StringValue.Import("LinkedIn", "https://www.linkedin.com/"));
                    sections.Add(StringValue.Import("Xing", "https://www.xing.com/"));
                    sections.Add(StringValue.Import("Facebook", "https://www.Facebook.com/"));
                    sections.Add(StringValue.Import("Youtube", "https://www.Youtube.com/"));
                    sections.Add(SeperatorValue.Import()); 
                    sections.Add(InteractionEntry.Import(Resources.Strings.Resources.MobilityEditView_Title, new NavigationData(Routes.MobilityEditView, null), NavigateToRoute, CanNavigateToRoute));
                    sections.Add(StringValue.Import(Resources.Strings.Resources.MobilityEditView_Willing, "nicht vorhanden"));
                    sections.Add(StringValue.Import(Resources.Strings.Resources.MobilityEditView_Vehicle, "ja"));
                    sections.Add(StringValue.Import(Resources.Strings.Resources.MobilityEditView_DriverLicenses, "A1, C1E, B96, Gabelstaplerschein"));
                    sections.Add(SeperatorValue.Import());
                    sections.Add(InteractionEntry.Import(Resources.Strings.Resources.LanguageSkillEditView_Title, new NavigationData(Routes.LanguageSkillEditView, null), NavigateToRoute, CanNavigateToRoute));
                    sections.Add(StringValue.Import("Deutsch", "C2"));
                    sections.Add(StringValue.Import("English", "B2"));
                    sections.Add(StringValue.Import("Französich", "A1"));
                    sections.Add(SeperatorValue.Import());
                    sections.Add(InteractionEntry.Import(Resources.Strings.Resources.LanguageSkillEditView_Title, new NavigationData(Routes.LanguageSkillEditView, null), NavigateToRoute, CanNavigateToRoute));
                    sections.Add(StringValue.Import("Deutsch", "C2"));
                    sections.Add(StringValue.Import("English", "B2"));
                    sections.Add(StringValue.Import("Französich", "A1"));
                    sections.Add(SeperatorValue.Import());
                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(sections), worker.Token);
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
            base.RefreshCommands();
            EditPersonalInformationsCommand?.NotifyCanExecuteChanged();
            foreach (var section in Sections?.OfType<InteractionEntry>() ?? Array.Empty<InteractionEntry>())
            {
                section.NavigateCommand?.NotifyCanExecuteChanged();
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanEditPersonalInformations))]
        private async Task EditPersonalInformations(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await NavigationService.NavigateAsync(Routes.PersonalInformationEditView, worker.Token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(EditPersonalInformations)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(EditPersonalInformations)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(EditPersonalInformations)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanEditPersonalInformations()
        {
            return !IsBusy;
        }

        private void LoadonUIThread(IEnumerable<ObservableObject> sections)
        {
            Sections = new ObservableCollection<ObservableObject>(sections);
        }

        private bool CanNavigateToRoute(InteractionEntry entry)
        {
            return !IsBusy && entry?.Data is NavigationData;
        }

        private async Task NavigateToRoute(InteractionEntry entry)
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var data = entry.Data as NavigationData;
                    var route = data?.route;
                    var parameters = data?.Parameters;
                    if (string.IsNullOrWhiteSpace(route))
                    {
                        return;
                    }
                    await NavigationService.NavigateAsync(route, worker.Token, parameters);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(NavigateToRoute)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(NavigateToRoute)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(NavigateToRoute)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }
    }
}
