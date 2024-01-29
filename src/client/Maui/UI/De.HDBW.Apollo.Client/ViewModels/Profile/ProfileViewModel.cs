// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Generic;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.Client.Models.Profile;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;
using Microsoft.Extensions.Logging;
using static Java.Util.Jar.Attributes;
using UserProfile = Invite.Apollo.App.Graph.Common.Models.UserProfile.Profile;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public partial class ProfileViewModel : BaseViewModel
    {
        private const string PersonalInfoIcon = "profilepersonalinfo.png";
        private const string ContactInfoIcon = "profilecontactinfo.png";
        private const string QualificationInfoIcon = "profilequalificationinfo.png";
        private const string LicensInfoIcon = "profilelicensinfo.png";
        private const string CareerInfoIcon = "profilecareerinfo.png";
        private const string EducationInfoIcon = "profileeducationinfo.png";
        private const string LanguageIcon = "profilelanguage.png";
        private const string WebReferenceIcon = "profilewebreference.png";
        private const string MobilityInfoIcon = "profilemobilityinfo.png";

        [ObservableProperty]
        private ObservableCollection<ObservableObject> _sections = new ObservableCollection<ObservableObject>();

        [ObservableProperty]
        private bool _isRegistered;

        public ProfileViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<ProfileViewModel> logger,
            IUserRepository userRepository,
            IUserService userService,
            ISessionService sessionService)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(userRepository);
            ArgumentNullException.ThrowIfNull(userService);
            ArgumentNullException.ThrowIfNull(sessionService);
            UserRepository = userRepository;
            UserService = userService;
            SessionService = sessionService;
        }

        private IUserRepository UserRepository { get; }

        private IUserService UserService { get; }

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
                        await ExecuteOnUIThreadAsync(() => LoadonUIThread(sections, SessionService.HasRegisteredUser), worker.Token);
                        return;
                    }

                    var user = await UserRepository.GetItemAsync(worker.Token).ConfigureAwait(false);
                    if (user == null)
                    {
                        await ExecuteOnUIThreadAsync(() => LoadonUIThread(sections, SessionService.HasRegisteredUser), worker.Token);
                        return;
                    }

                    var recommendationValue = CreateRecommendationValue(user);
                    if (recommendationValue != null)
                    {
                        sections.Add(recommendationValue);
                    }

                    var missingParts = recommendationValue?.Recommendations?.ToList() ?? new List<InteractionEntry>();
                    var missingRoutes = missingParts.Select(x => x.Data).OfType<NavigationData>().Select(x => x.Route).ToList();
                    if (!missingRoutes.Contains(Routes.PersonalInformationEditView))
                    {
                        sections.AddRange(CreatePersonalInformations(user));
                    }

                    if (!missingRoutes.Contains(Routes.ContactInfoEditView))
                    {
                        sections.AddRange(CreateContactInformations(user));
                    }

                    if (!missingRoutes.Contains(Routes.QualificationEditView))
                    {
                        sections.AddRange(CreateQualificationInformations(user.Profile!));
                    }

                    if (!missingRoutes.Contains(Routes.LicenseEditView))
                    {
                        sections.AddRange(CreateLicenseInformations(user.Profile!));
                    }

                    if (!missingRoutes.Contains(Routes.CareerInfoEditView))
                    {
                        sections.AddRange(CreateCareerInformations(user.Profile!));
                    }

                    if (!missingRoutes.Contains(Routes.EducationInfoEditView))
                    {
                        sections.AddRange(CreateEducationInformations(user.Profile!));
                    }

                    if (!missingRoutes.Contains(Routes.LanguageEditView))
                    {
                        sections.AddRange(CreateLanguageInformations(user.Profile!));
                    }

                    if (!missingRoutes.Contains(Routes.WebReferenceEditView))
                    {
                        sections.AddRange(CreateWebReferenceInformations(user.Profile!));
                    }

                    if (!missingRoutes.Contains(Routes.MobilityEditView))
                    {
                        sections.AddRange(CreateMobilityInformations(user.Profile!));
                    }

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

        private IEnumerable<ObservableObject> CreatePersonalInformations(User user)
        {
            var informations = new List<ObservableObject>();
            informations.Add(InteractionEntry.Import(Resources.Strings.Resources.PersonalInformationEditView_Title, new NavigationData(Routes.PersonalInformationEditView, null), NavigateToRoute, CanNavigateToRoute, PersonalInfoIcon));
            if (!string.IsNullOrEmpty(user.Name))
            {
                informations.Add(StringValue.Import(Resources.Strings.Resources.PersonalInformationEditView_UserName, user.Name));
            }

            if (user.Birthdate.HasValue)
            {
                informations.Add(StringValue.Import(Resources.Strings.Resources.PersonalInformationEditView_Birthdate, user.Birthdate.Value.ToLocalTime().ToString("d")));
            }

            informations.Add(SeperatorValue.Import());
            return informations;
        }

        private IEnumerable<ObservableObject> CreateContactInformations(User user)
        {
            var informations = new List<ObservableObject>();
            informations.Add(InteractionEntry.Import(Resources.Strings.Resources.ContactInfoEditView_Title, new NavigationData(Routes.ContactInfoEditView, null), NavigateToRoute, CanNavigateToRoute, ContactInfoIcon));
            var moreThanThree = (user.ContactInfos?.Count ?? 0) > 3;
            var contactInfos = user.ContactInfos.AsSortedList().Take(3);
            var i = 0;
            foreach (var contactInfo in contactInfos)
            {
                var entry = ContactEntry.Import(contactInfo, (x) => { return Task.CompletedTask; }, (x) => { return false; }, (x) => { return Task.CompletedTask; }, (x) => { return false; });
                foreach (var line in entry.AllLines)
                {
                    informations.Add(line);
                }

                if (!moreThanThree || i < 2)
                {
                    informations.Add(SeperatorValue.Import());
                }

                i++;
            }

            if (moreThanThree)
            {
                informations.Add(StringValue.Import("...", null));
            }

            return informations;
        }

        private IEnumerable<ObservableObject> CreateQualificationInformations(UserProfile profile)
        {
            var informations = new List<ObservableObject>();
            informations.Add(InteractionEntry.Import(Resources.Strings.Resources.QualificationEditView_Title, new NavigationData(Routes.QualificationEditView, null), NavigateToRoute, CanNavigateToRoute, QualificationInfoIcon));
            var moreThanThree = (profile.Qualifications?.Count ?? 0) > 3;
            var qualifications = profile.Qualifications.AsSortedList().Take(3);
            var i = 0;
            foreach (var qualification in qualifications)
            {
                var entry = QualificationEntry.Import(qualification, (x) => { return Task.CompletedTask; }, (x) => { return false; }, (x) => { return Task.CompletedTask; }, (x) => { return false; });
                foreach (var line in entry.AllLines)
                {
                    informations.Add(line);
                }

                if (!moreThanThree || i < 2)
                {
                    informations.Add(SeperatorValue.Import());
                }

                i++;
            }

            if (moreThanThree)
            {
                informations.Add(StringValue.Import("...", null));
            }

            return informations;
        }

        private IEnumerable<ObservableObject> CreateEducationInformations(UserProfile profile)
        {
            var informations = new List<ObservableObject>();
            informations.Add(InteractionEntry.Import(Resources.Strings.Resources.EducationInfoEditView_Title, new NavigationData(Routes.EducationInfoEditView, null), NavigateToRoute, CanNavigateToRoute, EducationInfoIcon));
            var moreThanThree = (profile.EducationInfos?.Count ?? 0) > 3;
            var educationInfos = profile.EducationInfos.AsSortedList().Take(3);
            var i = 0;
            foreach (var educationInfo in educationInfos)
            {
                var entry = EducationInfoEntry.Import(educationInfo, (x) => { return Task.CompletedTask; }, (x) => { return false; }, (x) => { return Task.CompletedTask; }, (x) => { return false; });
                foreach (var line in entry.AllLines)
                {
                    informations.Add(line);
                }

                if (!moreThanThree || i < 2)
                {
                    informations.Add(SeperatorValue.Import());
                }

                i++;
            }

            if (moreThanThree)
            {
                informations.Add(StringValue.Import("...", null));
            }

            return informations;
        }

        private IEnumerable<ObservableObject> CreateLicenseInformations(UserProfile profile)
        {
            var informations = new List<ObservableObject>();
            informations.Add(InteractionEntry.Import(Resources.Strings.Resources.LicenseEditView_Title, new NavigationData(Routes.LicenseEditView, null), NavigateToRoute, CanNavigateToRoute, LicensInfoIcon));
            var moreThanThree = (profile.Licenses?.Count ?? 0) > 3;
            var licenses = profile.Licenses.AsSortedList().Take(3);
            var i = 0;
            foreach (var license in licenses)
            {
                var entry = LicenseEntry.Import(license, (x) => { return Task.CompletedTask; }, (x) => { return false; }, (x) => { return Task.CompletedTask; }, (x) => { return false; });
                foreach (var line in entry.AllLines)
                {
                    informations.Add(line);
                }

                if (!moreThanThree || i < 2)
                {
                    informations.Add(SeperatorValue.Import());
                }

                i++;
            }

            if (moreThanThree)
            {
                informations.Add(StringValue.Import("...", null));
            }

            return informations;
        }

        private IEnumerable<ObservableObject> CreateCareerInformations(UserProfile profile)
        {
            var informations = new List<ObservableObject>();
            informations.Add(InteractionEntry.Import(Resources.Strings.Resources.CareerInfoEditView_Title, new NavigationData(Routes.CareerInfoEditView, null), NavigateToRoute, CanNavigateToRoute, CareerInfoIcon));
            var moreThanThree = (profile.CareerInfos?.Count ?? 0) > 3;
            var careerInfos = profile.CareerInfos.AsSortedList().Take(3);
            var i = 0;
            foreach (var careerInfo in careerInfos)
            {
                var entry = CareerInfoEntry.Import(careerInfo, (x) => { return Task.CompletedTask; }, (x) => { return false; }, (x) => { return Task.CompletedTask; }, (x) => { return false; });
                foreach (var line in entry.AllLines)
                {
                    informations.Add(line);
                }

                if (!moreThanThree || i < 2)
                {
                    informations.Add(SeperatorValue.Import());
                }

                i++;
            }

            if (moreThanThree)
            {
                informations.Add(StringValue.Import("...", null));
            }

            return informations;
        }

        private IEnumerable<ObservableObject> CreateLanguageInformations(UserProfile profile)
        {
            var informations = new List<ObservableObject>();
            informations.Add(InteractionEntry.Import(Resources.Strings.Resources.LanguageEditView_Title, new NavigationData(Routes.LanguageEditView, null), NavigateToRoute, CanNavigateToRoute, LanguageIcon));
            var moreThanThree = (profile.LanguageSkills?.Count ?? 0) > 3;
            var languageSkills = profile.LanguageSkills.AsSortedList().Take(3);
            var i = 0;
            foreach (var languageSkill in languageSkills)
            {
                var entry = LanguageEntry.Import(languageSkill, (x) => { return Task.CompletedTask; }, (x) => { return false; }, (x) => { return Task.CompletedTask; }, (x) => { return false; });
                foreach (var line in entry.AllLines)
                {
                    informations.Add(line);
                }

                if (!moreThanThree || i < 2)
                {
                    informations.Add(SeperatorValue.Import());
                }

                i++;
            }

            if (moreThanThree)
            {
                informations.Add(StringValue.Import("...", null));
            }

            return informations;
        }

        private IEnumerable<ObservableObject> CreateWebReferenceInformations(UserProfile profile)
        {
            var informations = new List<ObservableObject>();
            informations.Add(InteractionEntry.Import(Resources.Strings.Resources.WebReferenceEditView_Title, new NavigationData(Routes.WebReferenceEditView, null), NavigateToRoute, CanNavigateToRoute, WebReferenceIcon));
            var moreThanThree = (profile.WebReferences?.Count ?? 0) > 3;
            var webReferences = profile.WebReferences.AsSortedList().Take(3);
            var i = 0;
            foreach (var webReference in webReferences)
            {
                var entry = WebReferenceEntry.Import(webReference, (x) => { return Task.CompletedTask; }, (x) => { return false; }, (x) => { return Task.CompletedTask; }, (x) => { return false; });
                foreach (var line in entry.AllLines)
                {
                    informations.Add(line);
                }

                if (!moreThanThree || i < 2)
                {
                    informations.Add(SeperatorValue.Import());
                }

                i++;
            }

            if (moreThanThree)
            {
                informations.Add(StringValue.Import("...", null));
            }

            return informations;
        }

        private IEnumerable<ObservableObject> CreateMobilityInformations(UserProfile profile)
        {
            var informations = new List<ObservableObject>();
            informations.Add(InteractionEntry.Import(Resources.Strings.Resources.MobilityEditView_Title, new NavigationData(Routes.MobilityEditView, null), NavigateToRoute, CanNavigateToRoute, MobilityInfoIcon));
            informations.Add(StringValue.Import(Resources.Strings.Resources.MobilityEditView_Willing, profile.MobilityInfo.WillingToTravel?.GetLocalizedString()));
            informations.Add(StringValue.Import(Resources.Strings.Resources.MobilityEditView_Vehicle, profile.MobilityInfo.HasVehicle ? Resources.Strings.Resources.Global_Yes : Resources.Strings.Resources.Global_No));
            informations.Add(StringValue.Import(Resources.Strings.Resources.MobilityEditView_DriverLicenses, string.Join(", ", profile.MobilityInfo.DriverLicenses.Select(x => Resources.Strings.Resources.ResourceManager.GetString($"DriversLicense_{Enum.GetName(x)}")) ?? new List<string>())));
            return informations;
        }

        private RecommendationValue? CreateRecommendationValue(User user)
        {
            var weights = new Dictionary<string, double>();
            weights.Add(Routes.PersonalInformationEditView, 2);
            weights.Add(Routes.ContactInfoEditView, 1);
            weights.Add(Routes.QualificationEditView, 2);
            weights.Add(Routes.LicenseEditView, 2);
            weights.Add(Routes.CareerInfoEditView, 2);
            weights.Add(Routes.EducationInfoEditView, 1);
            weights.Add(Routes.LanguageEditView, 2);
            weights.Add(Routes.WebReferenceEditView, 1);
            weights.Add(Routes.MobilityEditView, 0);

            var interactions = new List<InteractionEntry>();
            if (string.IsNullOrWhiteSpace(user.Name))
            {
                interactions.Add(InteractionEntry.Import(Resources.Strings.Resources.PersonalInformationEditView_Title, new NavigationData(Routes.PersonalInformationEditView, null), NavigateToRoute, CanNavigateToRoute, PersonalInfoIcon));
            }

            if (!(user.ContactInfos?.Any() ?? false))
            {
                interactions.Add(InteractionEntry.Import(Resources.Strings.Resources.ContactInfoEditView_Title, new NavigationData(Routes.ContactInfoEditView, null), NavigateToRoute, CanNavigateToRoute, ContactInfoIcon));
            }

            if (!(user.Profile?.Qualifications.Any() ?? false))
            {
                interactions.Add(InteractionEntry.Import(Resources.Strings.Resources.QualificationEditView_Title, new NavigationData(Routes.QualificationEditView, null), NavigateToRoute, CanNavigateToRoute, QualificationInfoIcon));
            }

            if (!(user.Profile?.Licenses.Any() ?? false))
            {
                interactions.Add(InteractionEntry.Import(Resources.Strings.Resources.LicenseEditView_Title, new NavigationData(Routes.LicenseEditView, null), NavigateToRoute, CanNavigateToRoute, LicensInfoIcon));
            }

            if (!(user.Profile?.CareerInfos.Any() ?? false))
            {
                interactions.Add(InteractionEntry.Import(Resources.Strings.Resources.CareerInfoEditView_Title, new NavigationData(Routes.CareerInfoEditView, null), NavigateToRoute, CanNavigateToRoute, CareerInfoIcon));
            }

            if (!(user.Profile?.EducationInfos.Any() ?? false))
            {
                interactions.Add(InteractionEntry.Import(Resources.Strings.Resources.EducationInfoEditView_Title, new NavigationData(Routes.EducationInfoEditView, null), NavigateToRoute, CanNavigateToRoute, EducationInfoIcon));
            }

            if (!(user.Profile?.LanguageSkills.Any() ?? false))
            {
                interactions.Add(InteractionEntry.Import(Resources.Strings.Resources.LanguageEditView_Title, new NavigationData(Routes.LanguageEditView, null), NavigateToRoute, CanNavigateToRoute, LanguageIcon));
            }

            if (!(user.Profile?.WebReferences.Any() ?? false))
            {
                interactions.Add(InteractionEntry.Import(Resources.Strings.Resources.WebReferenceEditView_Title, new NavigationData(Routes.WebReferenceEditView, null), NavigateToRoute, CanNavigateToRoute, WebReferenceIcon));
            }

            if (user.Profile?.MobilityInfo?.WillingToTravel == null)
            {
                interactions.Add(InteractionEntry.Import(Resources.Strings.Resources.MobilityEditView_Title, new NavigationData(Routes.MobilityEditView, null), NavigateToRoute, CanNavigateToRoute, MobilityInfoIcon));
            }

            if (!interactions.Any())
            {
                return null;
            }

            var totalWeight = weights.Sum(x => x.Value);
            var currentWeight = interactions.Select(x => x.Data).OfType<NavigationData>().Sum(i => weights[i.Route]);
            var score = (totalWeight - currentWeight) / totalWeight;
            return RecommendationValue.Import(Resources.Strings.Resources.ProfileView_RecommendationSection_Title, Resources.Strings.Resources.ProfileView_RecommendationSection_SubTitle, score, interactions);
        }
    }
}
