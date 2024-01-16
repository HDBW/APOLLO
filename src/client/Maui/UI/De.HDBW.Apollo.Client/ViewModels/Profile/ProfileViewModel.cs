// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Generic;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public partial class ProfileViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<ObservableObject> _sections = new ObservableCollection<ObservableObject>();

        [ObservableProperty]
        private bool _isRegistered;

        public ProfileViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<ProfileViewModel> logger,
            IProfileRepository profileRepository,
            ISessionService sessionService)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(profileRepository);
            ArgumentNullException.ThrowIfNull(sessionService);
            ProfileRepository = profileRepository;
            SessionService = sessionService;
        }

        private IProfileRepository ProfileRepository { get; }

        private ISessionService SessionService { get; }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var sections = new List<ObservableObject>();
                    if (!SessionService.HasRegisteredUser)
                    {
                        await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(sections, SessionService.HasRegisteredUser), worker.Token);
                        return;
                    }

                    var interactions = new List<InteractionEntry>
                    {
                        InteractionEntry.Import(Resources.Strings.Resources.QualificationEditView_Title, new NavigationData(Routes.QualificationEditView, null), NavigateToRoute, CanNavigateToRoute),
                        InteractionEntry.Import(Resources.Strings.Resources.ContactInfoEditView_Title, new NavigationData(Routes.ContactInfoEditView, null), NavigateToRoute, CanNavigateToRoute),
                    };

                    sections.Add(RecommendationValue.Import("Hier fehlt noch was.", "Je besser das Profil gefüllt ist, desto bessere Vorschläge liefern wir dir.", 0.75, interactions));
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
                    sections.Add(InteractionEntry.Import(Resources.Strings.Resources.LanguageEditView_Title, new NavigationData(Routes.LanguageEditView, null), NavigateToRoute, CanNavigateToRoute));
                    sections.Add(StringValue.Import("Deutsch", "C2"));
                    sections.Add(StringValue.Import("English", "B2"));
                    sections.Add(StringValue.Import("Französich", "A1"));
                    sections.Add(SeperatorValue.Import());
                    sections.Add(InteractionEntry.Import(Resources.Strings.Resources.CareerInfoEditView_Title, new NavigationData(Routes.CareerInfoEditView, null), NavigateToRoute, CanNavigateToRoute));
                    sections.Add(StringValue.Import("Seit 01.08.2023", "Laufend"));
                    sections.Add(StringValue.Import("Kaufmännische Fachkraft (Nebenbeschäftigung)", string.Empty));
                    sections.Add(StringValue.Import(string.Empty, "Aachen"));
                    sections.Add(StringValue.Import(string.Empty, "Deutschland"));
                    sections.Add(StringValue.Import("01.11.2017 - 30.06.2023", string.Empty));
                    sections.Add(StringValue.Import("Kaufmännische Fachkraft", string.Empty));
                    sections.Add(StringValue.Import(string.Empty, "vorbereitende Buchhaltung, Rechnungserstellung, Personalwesen + Funker"));
                    sections.Add(StringValue.Import("01.12.2001 - 30.10.2017", string.Empty));
                    sections.Add(StringValue.Import("Funker/in", string.Empty));
                    sections.Add(StringValue.Import(string.Empty, "Funker, Telefonist"));
                    sections.Add(SeperatorValue.Import());
                    sections.Add(InteractionEntry.Import(Resources.Strings.Resources.EducationInfoEditView_Title, new NavigationData(Routes.EducationInfoEditView, null), NavigateToRoute, CanNavigateToRoute));
                    sections.Add(StringValue.Import("01.08.2000 - 30.10.2000", "Ohne Abschluss"));
                    sections.Add(StringValue.Import("Kaufmann/-frau - Kurier-, Express- u. Postdienstleistungen", string.Empty));
                    sections.Add(StringValue.Import("01.09.1987 - 31.08.1997", string.Empty));
                    sections.Add(StringValue.Import("Mittlere Reife / Mittlerer Bildungsabschluss", string.Empty));
                    sections.Add(StringValue.Import(string.Empty, "Integrierte Gesamtschule"));
                    sections.Add(SeperatorValue.Import());
                    sections.Add(InteractionEntry.Import(Resources.Strings.Resources.LicenseEditView_Title, new NavigationData(Routes.LicenseEditView, null), NavigateToRoute, CanNavigateToRoute));
                    sections.Add(StringValue.Import("WIG - Rohrschweißer - Prüfung", string.Empty));
                    sections.Add(StringValue.Import(string.Empty, "Erworben: 15.08.2012 Gültig bis: 14.08.2014"));
                    sections.Add(StringValue.Import("SCC-Zertifikat (Sicherheits-Certifikat-Contractoren)", string.Empty));
                    sections.Add(StringValue.Import(string.Empty, "Erworben: 01.09.2007"));
                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(sections, SessionService.HasRegisteredUser), worker.Token);
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
            RegisterCommand?.NotifyCanExecuteChanged();
            foreach (var section in Sections?.OfType<InteractionEntry>() ?? Array.Empty<InteractionEntry>())
            {
                section.NavigateCommand?.NotifyCanExecuteChanged();
            }

            foreach (var section in Sections?.OfType<RecommendationValue>() ?? Array.Empty<RecommendationValue>())
            {
                foreach (var interaction in section.Recommendations)
                {
                    interaction.NavigateCommand?.NotifyCanExecuteChanged();
                }
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

        private bool CanRegister()
        {
            return !IsBusy && !IsRegistered;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanRegister))]
        private async Task Register(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await NavigationService.NavigateAsync(Routes.RegistrationView, worker.Token);
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
            return !IsBusy && IsRegistered;
        }

        private void LoadonUIThread(IEnumerable<ObservableObject> sections, bool isRegistered)
        {
            Sections = new ObservableCollection<ObservableObject>(sections);
            IsRegistered = isRegistered;
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
