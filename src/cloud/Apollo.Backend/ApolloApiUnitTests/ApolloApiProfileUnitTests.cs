// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.


using System.Collections;
using Apollo.Api;
using Apollo.Api.UnitTests;
using Apollo.Common.Entities;
using Daenet.MongoDal;
using Daenet.MongoDal.Entitties;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Apollo.Api.UnitTests
{

    /// <summary>
    /// Unit tests for the ApolloApi class.
    /// </summary>
    [TestClass]
    public class ApolloApiProfileUnitTests
    {

        private string UserId = "User-7a3751ab-d338-492c-a7a9-5607252e6eb3";
        private Profile CreateSampleProfile()
        {
            return new Profile
            {
                CareerInfos = new List<CareerInfo>
                {
                    new CareerInfo
                    {
                        Start = DateTime.Parse("2018-01-01"),
                        End = DateTime.Parse("2021-02-01"),
                        Description = "Developed advanced level applications.",
                        NameOfInstitution = "Tech Corporation",
                        City = "San Francisco",
                        Country = "USA",
                        ServiceType = new ServiceType {
                                        ListItemId = 1,
                                        Lng= "Invariant",
                                        Description= "Zivildienst",
                                        Value= "CivilianService"},
                        WorkingTimeModel = new WorkingTimeModel {
                                       ListItemId=1,
                                       Description = "dummy" ,
                                       Lng = "Invariant",
                                       Value = "PARTTIME" },
                        Job = new Occupation
                        {
                            UniqueIdentifier = "abcdet",
                            OccupationUri =  "https://example.com/occupation/1234",
                            ClassificationCode = "K1234",
                            Concept = "dummy Concept",
                            RegulatoryAspect = "dummy Regularity",
                            HasApprenticeShip = true,
                            IsUniversityOccupation = false,
                            IsUniversityDegree = true,
                            PreferedTerm = new List<string> { "Bäcker/in" },
                            NonePreferedTerm = new List<string> { "Bäckergeselle" },
                            BroaderConcepts = new List<string> { "Concept1", "Concept2" },
                            NarrowerConcepts = new List<string?> { "Concept3", "Concept4" },
                            RelatedConcepts = new List<string?> { "Concept5", "Concept4" },
                            Skills = new List<string> { "Skill1", "Skill2" },
                            EssentialSkills = new List<string> { "Skill1" },
                            OptionalSkills = new List<string> { "Skill2" },
                            EssentialKnowledge = new List<string> { "Knowledge1", "Knowledge2" },
                            OptionalKnowledge = new List<string> { "Knowledge3" },
                            Documents = new List<string> { "Document1", "Document2" },
                            OccupationGroup = new Dictionary<string, string>
                            {
                                { "Engineer", "Software Engineer" },
                                { "Doctor", "Medical Doctor" }
                            },
                            DkzApprenticeship = false,
                            QualifiedProfessional = true,
                            NeedsUniversityDegree = false,
                            IsMilitaryApprenticeship = true,
                            IsGovernmentApprenticeship = false,
                            ValidFrom = DateTime.Parse("2024-01-01"),
                            ValidTill = DateTime.Parse("2024-12-31T23:59:59Z")
                        }
                    }
                },
                EducationInfos = new List<EducationInfo>
                {
                    new EducationInfo
                    {
                        Start = DateTime.Parse("2015-09-01"),
                        End = DateTime.Parse("2019-06-30"),
                        City = "Berlin",
                        ProfessionalTitle = new Occupation
                        {
                            UniqueIdentifier = "abcde",
                            OccupationUri = "https://example.com/occupation/123",
                            ClassificationCode = "K123",
                            Concept = "dummy concept",
                            RegulatoryAspect = "dummy",
                            HasApprenticeShip = true,
                            IsUniversityOccupation = false,
                            IsUniversityDegree = true,
                            PreferedTerm = new List<string> { "Bäcker/in" },
                            NonePreferedTerm = new List<string> { "Bäckergeselle" },
                            BroaderConcepts = new List<string> { "Concept1", "Concept2" },
                            NarrowerConcepts = new List<string?> { "Concept3", "Concept4" },
                            RelatedConcepts = new List<string?> { "Concept5", "Concept4" },
                            Skills = new List<string> { "Skill1", "Skill2" },
                            EssentialSkills = new List<string> { "Skill1" },
                            OptionalSkills = new List<string> { "Skill2" },
                            EssentialKnowledge = new List<string> { "Knowledge1", "Knowledge2" },
                            OptionalKnowledge = new List<string> { "Knowledge3" },
                            Documents = new List<string> { "Document1", "Document2" },
                            OccupationGroup = new Dictionary<string, string>
                            {
                                { "Engineer", "Software Engineer" },
                                { "Doctor", "Medical Doctor" }
                            },
                            DkzApprenticeship = false,
                            QualifiedProfessional = true,
                            NeedsUniversityDegree = false,
                            IsMilitaryApprenticeship = true,
                            IsGovernmentApprenticeship = false,
                            ValidFrom = DateTime.Parse("2024-01-01"),
                            ValidTill = DateTime.Parse("2024-12-31T23:59:59Z")
                        },
                        Graduation = new SchoolGraduation { ListItemId = 11, Lng = "en", Description = "Bachelor's Degree", Value = "Bachelor" },
                        UniversityDegree = new UniversityDegree { ListItemId = 12, Lng = "en", Description = "Bachelor of Science", Value = "BSc" },
                        TypeOfSchool = new TypeOfSchool{ ListItemId = 13, Lng = "en", Description = "University", Value = "University" },
                        NameOfInstitution = "University XYZ",
                        EducationType = new EducationType { ListItemId = 13, Lng = "en", Description = "University", Value = "Unknown" },
                        Recognition = new RecognitionType { ListItemId = 0, Lng = "Invariant", Description = "Dummy description", Value = "Unknown" }

                    }
                },
                Qualifications = new List<Qualification>
                {
                    new Qualification
                    {
                        Name = "Bachelor's Degree in Computer Science",
                        Description = "A degree in computer science focusing on software development and algorithms.",
                        IssueDate = DateTime.Parse("2018-05-15"),
                        IssuingAuthority = "University XYZ"
                    },
                    new Qualification
                    {
                        Name = "Project Management Professional (PMP) Certification",
                        Description = "Certification demonstrating expertise in project management.",
                        IssueDate = DateTime.Parse("2020-02-28"),
                        ExpirationDate = DateTime.Parse("2023-02-28"),
                        IssuingAuthority = "Project Management Institute (PMI)"
                    }
                },
                MobilityInfo = new Mobility
                {
                    WillingToTravel = new Willing { ListItemId = 1, Lng = "Invariant", Description = "Willing to travel", Value = "Yes" },
                    DriverLicenses = new List<DriversLicense>
                    {
                        new DriversLicense { ListItemId = 1, Lng = "Invariant", Description = "Fahrerlaubnis BE", Value = "BE" },
                        new DriversLicense { ListItemId = 2, Lng = "Invariant", Description = "Seminarerlaubnis ASP", Value = "InstructorASF" }
                    },
                    HasVehicle = true
                },
                LanguageSkills = new List<LanguageSkill>
                {
                    new LanguageSkill { Name = "L1", Niveau = new LanguageNiveau { ListItemId = 7, Lng = "Invariant", Description = "Fluent", Value = "A1" }, Code = "en-US" },
                    new LanguageSkill { Name = "L2", Niveau = new LanguageNiveau { ListItemId = 8, Lng = "Invariant", Description = "Fluent", Value = "A2" }, Code = "en-US" }
                },
                Skills = null,
                Occupations = null,
                Knowledge = null,
                Apprenticeships = null,
                Licenses = new List<License>
                {
                    new License
                    {
                        ListItemId = 1,
                        Value = "Professional License",
                        Granted = DateTime.Parse("2022-01-01"),
                        Expires = DateTime.Parse("2023-12-31"),
                        IssuingAuthority = "License Authority A"
                    },
                    new License
                    {
                        ListItemId = 2,
                        Value = "Certification",
                        Granted = DateTime.Parse("2021-05-15"),
                        Expires = DateTime.Parse("2024-06-30"),
                        IssuingAuthority = "Certification Board"
                    }
                },
                LeadershipSkills = new LeadershipSkills
                {
                    PowerOfAttorney = true,
                    BudgetResponsibility = false,
                    YearsofLeadership = new YearRange { ListItemId = 3, Lng = "en", Description = "Leadership Experience", Value = "Experienced Leader" },
                    StaffResponsibility = new StaffResponsibility { ListItemId = 4, Lng = "en", Description = "Team Management", Value = "Team Manager" }

                },
                WebReferences = new List<WebReference>
                {
                    new WebReference { Url = new Uri("http://www.linkedin.com"), Title = "LinkedIn Profile" },
                    new WebReference { Url = new Uri("http://github.com/username"), Title = "Github Profile" }
                }
            };
        }



        /// <summary>
        /// Inserts test lists and then gets every of them that match with Id.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetProfileAsyncTestWithIdSpecified()
        {
          
            var api = Helpers.GetApolloApi();
            var sampleProfile = CreateSampleProfile();
            var userId = UserId;

            // Act
            var createdProfileId = await api.CreateOrUpdateProfileAsync(userId, sampleProfile);
            var retrievedProfile = await api.GetProfileAsync(createdProfileId);

            // Assert
            Assert.IsNotNull(retrievedProfile);
            Assert.AreEqual(createdProfileId, retrievedProfile.Id);
           
            // Clean up
            await api.DeleteProfileAsync(createdProfileId);

        }

        /// <summary>
        /// Unit test for updating a user profile using the ApolloApi.
        /// </summary>
        [TestMethod]
        public async Task UpdateProfileAsyncTest()
        {
            // Arrange
            var api = Helpers.GetApolloApi();
            var sampleProfile = CreateSampleProfile();
            var userId = UserId;

            // Act
            var createdProfileId = await api.CreateOrUpdateProfileAsync(userId, sampleProfile);

            // Update the sample profile (you can modify any property you want to test)

            if (sampleProfile.CareerInfos != null && sampleProfile.CareerInfos.Count > 0)
            {
                sampleProfile.CareerInfos[0].Description = "Updated description";
                sampleProfile.CareerInfos[0].Country = "Germany";
            }

            await api.CreateOrUpdateProfileAsync(userId, sampleProfile);

            // Retrieve the updated profile
            var updatedProfile = await api.GetProfileAsync(createdProfileId);

            // Assert
            Assert.IsNotNull(updatedProfile);
            Assert.AreEqual(createdProfileId, updatedProfile.Id);
            Assert.AreEqual(sampleProfile.CareerInfos?[0]?.Description, updatedProfile.CareerInfos?[0]?.Description);
            Assert.AreEqual(sampleProfile.CareerInfos?[0]?.Country, updatedProfile.CareerInfos?[0]?.Country);
            // Add more assertions for other properties...

            // Clean up
            await api.DeleteProfileAsync(createdProfileId);
       }

        /// <summary>
        /// Test creating a profile with null or incomplete data.
        /// </summary>
        [TestMethod]
        public async Task CreateProfileWithNullDataTest()
        {
            var api = Helpers.GetApolloApi();
            var invalidProfile = new Profile();

            // Attempt to create the profile with invalid data
            var result = await api.CreateOrUpdateProfileAsync(UserId, invalidProfile);

            // Assert
            Assert.IsNotNull(result);
            
        }

        /// <summary>
        /// Test ApolloApiException in case a Profile not found
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApolloApiException), "Profile with ID 'NonExistentProfileId1234' not found.")]
        public async Task ProfileNotFoundExceptionTest()
        {
            // Arrange
            var api = Helpers.GetApolloApi();
            var nonExistentProfileId = "NonExistentProfileId1234";

            // Act: Attempt to retrieve a non-existent profile
            await api.GetProfileAsync(nonExistentProfileId);
        }
    }
}
