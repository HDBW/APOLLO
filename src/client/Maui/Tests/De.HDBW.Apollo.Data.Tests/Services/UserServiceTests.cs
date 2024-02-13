using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.Data.Services;
using Invite.Apollo.App.Graph.Common.Backend.Api;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Graphics.Text;
using Xunit;
using Xunit.Abstractions;

namespace De.HDBW.Apollo.Data.Tests.Services
{
    public class UserServiceTests : AbstractServiceTestSetup<UserService>
    {
        public UserServiceTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public async Task CancellationTokenTests()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);
            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(TokenSource!.Token))
            {
                cts.Cancel();
                await Assert.ThrowsAnyAsync<OperationCanceledException>(() => Service.SaveAsync(null, cts.Token));
            }
        }

        [Fact]
        public async Task RegisterUserAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);
            string? userId = null;
            var testuser = new User() { ObjectId = "NewUser", Name = "NewUser" };
            User? createdUser = null;
            User? updatedUser = null;
            try
            {
                userId = await Service.SaveAsync(testuser, TokenSource!.Token);
                Assert.NotNull(userId);
                createdUser = await Service.GetUserAsync(userId!, TokenSource!.Token);
            }
            catch (ApolloApiException ex)
            {
                // Not existing ids return errorcode ErrorCodes.TrainingErrors.GetTrainingError;
                Assert.Equal(ErrorCodes.UserErrors.CreateOrUpdateUserError, ex.ErrorCode);
            }

            Assert.NotNull(userId);
            Assert.NotNull(createdUser);
            Assert.Equal(testuser.ObjectId, createdUser!.ObjectId);
            Assert.Equal(testuser.Name, createdUser.Name);

            testuser.Id = userId;
            testuser.Name = "Fritz";
            try
            {
                var updatedId = await Service.SaveAsync(testuser, TokenSource!.Token);
                updatedUser = await Service.GetUserAsync(userId!, TokenSource!.Token);
                createdUser = await Service.GetUserAsync(userId!, TokenSource!.Token);
            }
            catch (ApolloApiException ex)
            {
                // Not existing ids return errorcode ErrorCodes.TrainingErrors.GetTrainingError;
                Assert.Equal(ErrorCodes.UserErrors.CreateOrUpdateUserError, ex.ErrorCode);
            }

            Assert.NotNull(createdUser);
            Assert.NotNull(updatedUser);
            Assert.Equal(testuser.ObjectId, updatedUser!.ObjectId);
            Assert.Equal(testuser.Name, createdUser!.Name);
        }

        [Fact]
        public async Task GetUserAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);
            string userId = "User-5DE545AEF9974FD6826151725A9961F8";
            User? user = null;
            try
            {
                user = await Service.GetUserAsync(userId, TokenSource!.Token);
            }
            catch (ApolloApiException ex)
            {
                // Not existing ids return errorcode ErrorCodes.TrainingErrors.GetTrainingError;
                Assert.Equal(ErrorCodes.UserErrors.CreateOrUpdateUserError, ex.ErrorCode);
            }

            Assert.NotNull(user);
            Assert.Equal(userId, user!.Id);
        }

        [Fact]
        public async Task SaveUserAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);
            string? userId = null;
            try
            {
                var testuser = new User() { Id = "SER01", ObjectId = "Dummy", Name = "Dummy", IdentityProvicer = "Dummy" };
                userId = await Service.SaveAsync(testuser, TokenSource!.Token);
            }
            catch (ApolloApiException ex)
            {
                // Not existing ids return errorcode ErrorCodes.TrainingErrors.GetTrainingError;
                Assert.Equal(ErrorCodes.UserErrors.CreateOrUpdateUserError, ex.ErrorCode);
            }

            Assert.NotNull(userId);
        }

        [Fact]
        public async Task SaveProfileAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);
            string userId = "User-5DE545AEF9974FD6826151725A9961F8";
            User? user = null;
            var errors = 0;
            try
            {
                user = await Service.GetUserAsync(userId, TokenSource!.Token);
                Assert.NotNull(user);
                Assert.False(string.IsNullOrWhiteSpace(user.Id));
                var profile = user?.Profile ?? new Profile();
                profile.Licenses = null;
                profile.CareerInfos = null;
                profile.EducationInfos = null;
                profile.Qualifications = null;
                profile.WebReferences = null;
                profile.MobilityInfo = null;
                user!.Profile = profile;
                var savedUserId = await Service.SaveAsync(user, TokenSource!.Token);
                Assert.Equal(userId, savedUserId);
                user = await Service.GetUserAsync(userId, TokenSource!.Token);

                Assert.NotNull(user);
                Assert.Equal(userId, user!.Id);
                Assert.NotNull(user.Profile);
                //Assert.False(string.IsNullOrWhiteSpace(user!.Profile!.Id));
                Assert.Null(user.Profile.CareerInfos);
                Assert.Null(user.Profile.Licenses);
                Assert.Null(user.Profile.EducationInfos);
                Assert.Null(user.Profile.Qualifications);
                Assert.Null(user.Profile.CareerInfos);
                Assert.Null(user.Profile.WebReferences);
                try
                {
                    user.Profile!.CareerInfos = user.Profile!.CareerInfos ?? new List<CareerInfo>();
                    user = await CreateAndCheckCareerInfoAsync(user, userId, 0, CareerType.Homemaker, DateTime.Today.ToUniversalTime(), null);
                    user = await CreateAndCheckCareerInfoAsync(user, userId, 1, CareerType.Other, DateTime.Today.ToUniversalTime(), DateTime.Today.ToUniversalTime());
                    user = await CreateAndCheckCareerInfoAsync(user, userId, 2, CareerType.PersonCare, DateTime.Today.ToUniversalTime(), null);
                    user = await CreateAndCheckCareerInfoAsync(user, userId, 3, CareerType.Internship, DateTime.Today.ToUniversalTime(), null);
                    user = await CreateAndCheckCareerInfoAsync(user, userId, 4, CareerType.PersonCare, DateTime.Today.ToUniversalTime(), null);
                    user = await CreateAndCheckCareerInfoAsync(user, userId, 5, CareerType.CommunityService, DateTime.Today.ToUniversalTime(), null);
                    user = await CreateAndCheckCareerInfoAsync(user, userId, 6, CareerType.ExtraOccupationalExperience, DateTime.Today.ToUniversalTime(), null);
                    user = await CreateAndCheckCareerInfoAsync(user, userId, 7, CareerType.SelfEmployment, DateTime.Today.ToUniversalTime(), null);
                }
                catch (Exception ex)
                {
                    errors++;
                    user.Profile!.CareerInfos = null;
                    Logger.LogError(ex, "Error while creating CareerInfos");
                }

                try
                {
                    user.Profile!.EducationInfos = user.Profile!.EducationInfos ?? new List<EducationInfo>();
                    user = await CreateAndCheckEducationAsync(user, userId, 0, EducationType.Education, CompletionState.Failed);
                    user = await CreateAndCheckEducationAsync(user, userId, 1, EducationType.FurtherEducation, CompletionState.Ongoing);
                    user = await CreateAndCheckEducationAsync(user, userId, 2, EducationType.VocationalTraining, CompletionState.Completed);
                    user = await CreateAndCheckEducationAsync(user, userId, 3, EducationType.CompanyBasedVocationalTraining, CompletionState.Completed);
                }
                catch (Exception ex)
                {
                    errors++;
                    user.Profile!.EducationInfos = null;
                    Logger.LogError(ex, "Error while creating EductationInfo");
                }

                try
                {
                    user.Profile!.Licenses = user.Profile!.Licenses ?? new List<License>();
                    user = await CreateAndCheckLicenseAsync(user, userId, 0, "Test", DateTime.Today.ToUniversalTime(), null);
                    user = await CreateAndCheckLicenseAsync(user, userId, 1, "Test1", null, DateTime.Today.ToUniversalTime());
                    user = await CreateAndCheckLicenseAsync(user, userId, 2, "Test2", DateTime.Today.ToUniversalTime(), DateTime.Today.ToUniversalTime());
                }
                catch (Exception ex)
                {
                    errors++;
                    user.Profile!.Licenses = null;
                    Logger.LogError(ex, "Error while creating Licenses");
                }

                try
                {
                    user.Profile!.WebReferences = user.Profile!.WebReferences ?? new List<WebReference>();
                    user = await CreateAndCheckWebReferenceAsync(user, userId, 0, "Test", "http://heise.de");
                    user = await CreateAndCheckWebReferenceAsync(user, userId, 1, "Test1", "http://heise.de");
                    user = await CreateAndCheckWebReferenceAsync(user, userId, 2, "Test", "http://heise.de");
                }
                catch (Exception ex)
                {
                    errors++;
                    user.Profile!.WebReferences = null;
                    Logger.LogError(ex, "Error while creating WebReferences");
                }

                try
                {
                    user.Profile!.LanguageSkills = user.Profile!.LanguageSkills ?? new List<Language>();
                    user = await CreateAndCheckLanguageAsync(user, userId, 0, LanguageNiveau.B2, "de-DE");
                    user = await CreateAndCheckLanguageAsync(user, userId, 1, LanguageNiveau.A2, "en-US");
                    user = await CreateAndCheckLanguageAsync(user, userId, 2, LanguageNiveau.A1, "de-AT");
                }
                catch (Exception ex)
                {
                    errors++;
                    user.Profile!.LanguageSkills = null;
                    Logger.LogError(ex, "Error while creating LanguageSkills");
                }

                try
                {
                    user.Profile!.MobilityInfo = user.Profile!.MobilityInfo ?? new Mobility();
                    user = await CreateAndCheckMobilityInfoAsync(user, userId, false, Willing.No, new List<DriversLicense>() { DriversLicense.D, DriversLicense.BE, DriversLicense.D1 });
                    user = await CreateAndCheckMobilityInfoAsync(user, userId, true, null, new List<DriversLicense>() { });
                    user = await CreateAndCheckMobilityInfoAsync(user, userId, false, null, new List<DriversLicense>() { DriversLicense.A });
                }
                catch (Exception ex)
                {
                    errors++;
                    user.Profile!.MobilityInfo = null;
                    Logger.LogError(ex, "Error while creating MobilityInfo");
                }

                try
                {
                    user.Profile!.Qualifications = user.Profile!.Qualifications ?? new List<Qualification>();
                    user = await CreateAndCheckQualificationsAsync(user, userId, 0, "Test", "Test", DateTime.Today, null);
                    user = await CreateAndCheckQualificationsAsync(user, userId, 1, "Test1", "Test1", null, DateTime.Today);
                    user = await CreateAndCheckQualificationsAsync(user, userId, 2, "Test", null, null, null);
                }
                catch (Exception ex)
                {
                    errors++;
                    user.Profile!.Qualifications = null;
                    Logger.LogError(ex, "Error while creating Qualifications");
                }
            }
            catch (ApolloApiException ex)
            {
                // Not existing ids return errorcode ErrorCodes.TrainingErrors.GetTrainingError;
                Assert.Equal(ErrorCodes.UserErrors.CreateOrUpdateUserError, ex.ErrorCode);
            }

            Assert.True(errors == 0);
        }

        protected override UserService SetupService(string apiKey, string baseUri, ILogger<UserService> logger, HttpMessageHandler httpClientHandler)
        {
            return new UserService(logger, baseUri, apiKey, httpClientHandler);
        }

        protected override void CleanupAdditionalServices()
        {
        }

        protected override void SetupAdditionalServices(string apiKey, string baseUri, ILogger<UserService> logger, HttpMessageHandler httpClientHandler)
        {
        }

        private async Task<User> CreateAndCheckCareerInfoAsync(User existingUser, string userId, int index, CareerType careerType, DateTime start, DateTime? end)
        {
            string? savedUserId = null;
            if (existingUser.Profile!.CareerInfos!.Count() == index)
            {
                var careerInfo = new CareerInfo();
                careerInfo.CareerType = careerType.ToApolloListItem()!;
                careerInfo.Start = start;
                careerInfo.End = end;
                existingUser!.Profile!.CareerInfos!.Add(careerInfo);
                savedUserId = await Service.SaveAsync(existingUser, TokenSource!.Token);
                Assert.Equal(userId, savedUserId);
            }
            else
            {
                Assert.NotNull(existingUser.Profile!.CareerInfos);
                var careerInfo = existingUser.Profile.CareerInfos[index]!;
                careerInfo.CareerType = careerType.ToApolloListItem()!;
                careerInfo.Start = start;
                careerInfo.End = end;
                savedUserId = await Service.SaveAsync(existingUser, TokenSource!.Token);
                Assert.Equal(userId, savedUserId);
            }

            var savedUser = await Service.GetUserAsync(userId, TokenSource!.Token);
            Assert.Equal(userId, savedUser!.Id);
            Assert.NotNull(savedUser.Profile!.CareerInfos);
            Assert.True(savedUser.Profile!.CareerInfos!.Count()! >= index + 1);
            Assert.False(string.IsNullOrWhiteSpace(savedUser.Profile!.CareerInfos![index].Id));
            Assert.Equal(careerType, savedUser.Profile!.CareerInfos![index].CareerType.AsEnum<CareerType>());
            Assert.Equal(start, savedUser.Profile!.CareerInfos![index].Start);
            Assert.Equal(end, savedUser.Profile!.CareerInfos![index].End);
            return savedUser;
        }

        private async Task<User> CreateAndCheckQualificationsAsync(User existingUser, string userId, int index, string name, string? description, DateTime? granted, DateTime? expires)
        {
            string? savedUserId = null;
            if (existingUser.Profile!.Qualifications!.Count() == index)
            {
                var qualification = new Qualification();
                qualification.Name = name;
                qualification.Description = description;
                qualification.IssueDate = granted;
                qualification.ExpirationDate = expires;
                existingUser.Profile!.Qualifications!.Add(qualification);
                savedUserId = await Service.SaveAsync(existingUser, TokenSource!.Token);
                Assert.Equal(userId, savedUserId);
            }
            else
            {
                Assert.NotNull(existingUser.Profile!.Qualifications);
                var qualification = existingUser.Profile.Qualifications[index]!;
                qualification.Name = name;
                qualification.Description = description;
                qualification.IssueDate = granted;
                qualification.ExpirationDate = expires;
                savedUserId = await Service.SaveAsync(existingUser, TokenSource!.Token);
                Assert.Equal(userId, savedUserId);
            }

            var savedUser = await Service.GetUserAsync(userId, TokenSource!.Token);
            Assert.Equal(userId, savedUser!.Id);
            Assert.NotNull(savedUser.Profile!.Qualifications);
            Assert.Equal(index + 1, savedUser.Profile!.Qualifications!.Count()!);
            Assert.False(string.IsNullOrWhiteSpace(savedUser.Profile!.Qualifications![index].Id));
            Assert.Equal(name, savedUser.Profile!.Qualifications![index].Name);
            Assert.Equal(description, savedUser.Profile!.Qualifications![index].Description);
            Assert.Equal(granted, savedUser.Profile!.Qualifications![index].IssueDate);
            Assert.Equal(expires, savedUser.Profile!.Qualifications![index].ExpirationDate);
            return savedUser;
        }

        private async Task<User> CreateAndCheckMobilityInfoAsync(User existingUser, string userId, bool hasVehicle, Willing? willing, List<DriversLicense> driversLicenses)
        {
            string? savedUserId = null;
            var mobility = existingUser.Profile!.MobilityInfo;
            Assert.NotNull(mobility);
            mobility!.WillingToTravel = willing != null ? ((Willing)willing).ToApolloListItem() : null;
            mobility!.HasVehicle = hasVehicle;
            mobility.DriverLicenses = driversLicenses.Select(d => d.ToApolloListItem()!).ToList();
            savedUserId = await Service.SaveAsync(existingUser, TokenSource!.Token);
            Assert.Equal(userId, savedUserId);
            var savedUser = await Service.GetUserAsync(userId, TokenSource!.Token);
            Assert.Equal(userId, savedUser!.Id);
            Assert.NotNull(savedUser.Profile!.MobilityInfo);
            Assert.Equal(hasVehicle, savedUser.Profile!.MobilityInfo!.HasVehicle);
            Assert.Equal(willing, savedUser.Profile!.MobilityInfo!.WillingToTravel.AsEnum<Willing>());
            Assert.Equal(driversLicenses.Count, savedUser.Profile!.MobilityInfo!.DriverLicenses.Count());
            Assert.True(savedUser.Profile!.MobilityInfo!.DriverLicenses.TrueForAll(d =>
            {
                return driversLicenses!.Contains(d.AsEnum<DriversLicense>()!.Value);
            }));
            return savedUser;
        }

        private async Task<User> CreateAndCheckLanguageAsync(User existingUser, string userId, int index, LanguageNiveau niveau, string code)
        {
            string? savedUserId = null;
            if (existingUser.Profile!.LanguageSkills!.Count() == index)
            {
                var language = new Language();
                language.Niveau = niveau.ToApolloListItem();
                language.Name = new CultureInfo(code).DisplayName;
                language.Code = code;
                existingUser!.Profile!.LanguageSkills!.Add(language);
                savedUserId = await Service.SaveAsync(existingUser, TokenSource!.Token);
                Assert.Equal(userId, savedUserId);
            }
            else
            {
                Assert.NotNull(existingUser.Profile!.LanguageSkills);
                var language = existingUser.Profile!.LanguageSkills[index]!;
                language.Niveau = niveau.ToApolloListItem();
                language.Name = new CultureInfo(code).DisplayName;
                language.Code = code;
                savedUserId = await Service.SaveAsync(existingUser, TokenSource!.Token);
                Assert.Equal(userId, savedUserId);
            }

            var savedUser = await Service.GetUserAsync(userId, TokenSource!.Token);
            Assert.Equal(userId, savedUser!.Id);
            Assert.NotNull(savedUser.Profile!.LanguageSkills);
            Assert.Equal(index + 1, savedUser.Profile!.LanguageSkills!.Count()!);
            Assert.False(string.IsNullOrWhiteSpace(savedUser.Profile!.LanguageSkills![index].Id));
            Assert.Equal(code, savedUser.Profile!.LanguageSkills![index].Code);
            Assert.Equal(niveau, savedUser.Profile!.LanguageSkills![index].Niveau.AsEnum<LanguageNiveau>());
            Assert.Equal(new CultureInfo(code).DisplayName, savedUser.Profile!.LanguageSkills![index].Name); 
            Assert.Equal(code, savedUser.Profile!.LanguageSkills![index].Code);
            return savedUser;
        }

        private async Task<User> CreateAndCheckWebReferenceAsync(User existingUser, string userId, int index, string title, string uri)
        {
            string? savedUserId = null;
            if (existingUser.Profile!.WebReferences!.Count() == index)
            {
                var webReferences = new WebReference();
                webReferences.Title = title;
                webReferences.Url = new Uri(uri);
                existingUser.Profile!.WebReferences!.Add(webReferences);
                savedUserId = await Service.SaveAsync(existingUser, TokenSource!.Token);
                Assert.Equal(userId, savedUserId);
            }
            else
            {
                Assert.NotNull(existingUser.Profile!.WebReferences);
                var webReferences = existingUser.Profile!.WebReferences[index]!;
                webReferences.Title = title;
                webReferences.Url = new Uri(uri);
                savedUserId = await Service.SaveAsync(existingUser, TokenSource!.Token);
                Assert.Equal(userId, savedUserId);
            }

            var savedUser = await Service.GetUserAsync(userId, TokenSource!.Token);
            Assert.Equal(userId, savedUser!.Id);
            Assert.NotNull(savedUser.Profile!.WebReferences);
            Assert.Equal(index + 1, savedUser.Profile!.WebReferences!.Count()!);
            Assert.Equal(title, savedUser.Profile!.WebReferences![index].Title);
            Assert.False(string.IsNullOrWhiteSpace(savedUser.Profile!.WebReferences![index].Id));
            Assert.Equal(uri, savedUser.Profile!.WebReferences![index].Url.OriginalString);
            return savedUser;
        }

        private async Task<User> CreateAndCheckLicenseAsync(User existingUser, string userId, int index, string name, DateTime? granted, DateTime? expires)
        {
            string? savedUserId = null;
            if (existingUser.Profile!.Licenses!.Count() == index)
            {
                var license = new License();
                license.Name = name;
                license.Granted = granted;
                license.Expires = expires;
                existingUser.Profile!.Licenses!.Add(license);
                savedUserId = await Service.SaveAsync(existingUser, TokenSource!.Token);
                Assert.Equal(userId, savedUserId);
            }
            else
            {
                Assert.NotNull(existingUser.Profile!.Licenses);
                var license = existingUser.Profile!.Licenses[index]!;
                license.Name = name;
                license.Granted = granted;
                license.Expires = expires;
                savedUserId = await Service.SaveAsync(existingUser, TokenSource!.Token);
                Assert.Equal(userId, savedUserId);
            }

            var savedUser = await Service.GetUserAsync(userId, TokenSource!.Token);
            Assert.Equal(userId, savedUser!.Id);
            Assert.NotNull(savedUser.Profile!.Licenses);
            Assert.True(savedUser.Profile!.Licenses!.Count()! >= index + 1);
            Assert.Equal(name, savedUser.Profile!.Licenses![index].Name);
            Assert.False(string.IsNullOrWhiteSpace(savedUser.Profile!.Licenses![index].Id));
            Assert.Equal(granted, savedUser.Profile!.Licenses![index].Granted);
            Assert.Equal(expires, savedUser.Profile!.Licenses![index].Expires);
            return savedUser;
        }

        private async Task<User> CreateAndCheckEducationAsync(User existingUser, string userId, int index, EducationType educationType, CompletionState state)
        {
            string? savedUserId = null;
            if (existingUser.Profile!.EducationInfos!.Count() == index)
            {
                var educationInfo = new EducationInfo();
                educationInfo.EducationType = educationType.ToApolloListItem()!;
                educationInfo.CompletionState = state.ToApolloListItem()!;
                existingUser.Profile!.EducationInfos!.Add(educationInfo);
                savedUserId = await Service.SaveAsync(existingUser, TokenSource!.Token);
                Assert.Equal(userId, savedUserId);
            }
            else
            {
                Assert.NotNull(existingUser.Profile!.EducationInfos);
                var educationInfo = existingUser.Profile!.EducationInfos[index]!;
                educationInfo.EducationType = educationType.ToApolloListItem()!;
                educationInfo.CompletionState = state.ToApolloListItem()!;
                savedUserId = await Service.SaveAsync(existingUser, TokenSource!.Token);
                Assert.Equal(userId, savedUserId);
            }

            var savedUser = await Service.GetUserAsync(userId, TokenSource!.Token);
            Assert.Equal(userId, savedUser!.Id);
            Assert.NotNull(savedUser.Profile!.EducationInfos);
            Assert.True(savedUser.Profile!.EducationInfos!.Count()! >= index + 1);
            Assert.False(string.IsNullOrWhiteSpace(savedUser.Profile!.EducationInfos![index].Id));
            Assert.Equal(educationType, savedUser.Profile!.EducationInfos![index]!.EducationType.AsEnum<EducationType>());
            Assert.Equal(state, savedUser.Profile!.EducationInfos![index]!.EducationType.AsEnum<CompletionState>());
            return savedUser;
        }
    }
}
