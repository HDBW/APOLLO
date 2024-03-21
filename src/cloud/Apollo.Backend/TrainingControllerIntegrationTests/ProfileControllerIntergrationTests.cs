// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Auth.AccessControlPolicy;
using Apollo.Common.Entities;
using Apollo.Service.Controllers;
using Google.Protobuf.WellKnownTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace Apollo.RestService.IntergrationTests
{
    /// <summary>
    /// Integration tests for the ProfileController class.
    /// </summary>
    [TestClass]
    public class ProfileControllerIntergrationTests
    {

        private const string _cProfileController = "Profile";

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

        public ProfileControllerIntergrationTests()
        {
            // Constructor can be used to set up configuration if needed
        }


        /// <summary>
        /// Cleans up test profiles from the database after each test to ensure a clean state.
        /// </summary>
        [TestCleanup]
        public async Task Cleanup()
        {
            //var httpClient = Helpers.GetHttpClient();
            //foreach (var profile in _testProfiles)
            //{
            //    await httpClient.DeleteAsync($"{_cProfileController}/{profile.Id}");
            //}
        }


        /// <summary>
        /// Ensures a clean state before each test by deleting any existing test profiles.
        /// Then inserts test profiles into the database for testing.
        /// </summary>
        [TestInitialize]
        public async Task Initialize()
        {
            //await Cleanup(); // Ensure clean state before each test
            //await InsertTestProfiles(); // Insert test data
        }


        /// <summary>
        /// Inserts test profiles into the system and verifies if the insertion was successful.
        /// This method serializes the list of test profiles to JSON, posts it to the profile insert API endpoint,
        /// and checks the response to ensure that all profiles were inserted as expected. It performs cleanup before
        /// and after insertion to ensure a clean state for the test environment.
        /// </summary>

        //[TestMethod]
        //public async Task InsertTestProfiles()
        //{


        //    var httpClient = Helpers.GetHttpClient();
        //    var json = JsonSerializer.Serialize(_testProfiles);
        //    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");


        //    var response = await httpClient.PostAsync($"{_cProfileController}/insert", content);
        //    Assert.IsTrue(response.IsSuccessStatusCode);

        //    var responseJson = await response.Content.ReadAsStringAsync();
        //    var insertedIds = JsonSerializer.Deserialize<List<string>>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        //    Assert.IsNotNull(insertedIds, "The response should include IDs of inserted profiles.");
        //    Assert.AreEqual(_testProfiles.Length, insertedIds.Count, "The number of inserted profiles should match the input.");

        //}


        /// <summary>
        /// Tests retrieving a specific profile by ID.
        /// Verifies the retrieved profile matches the expected profile data.
        /// </summary>
        [TestMethod]
        public async Task GetProfileTest()
        {
            var httpClient = Helpers.GetHttpClient();

            // Assuming you have a predefined userId to use for testing
            var userId = "User-LK-01";

            // Create a sample profile for testing
            var sampleProfile = CreateSampleProfile();

            // Encapsulate the profile and userId in an object as expected by the API
            var createRequestObj = new
            {
                UserId = userId,
                Profile = sampleProfile
            };

            // Serialize the request object to JSON
            var createRequestJson = JsonSerializer.Serialize(createRequestObj);
            var createContent = new StringContent(createRequestJson, Encoding.UTF8, "application/json");

            // PUT to the profile creation endpoint
            var createResponse = await httpClient.PutAsync($"{_cProfileController}", createContent);

            // Ensure the creation was successful before proceeding
            Assert.IsTrue(createResponse.IsSuccessStatusCode, "Profile creation failed.");

            // Log the status code and response content for debugging
            var createResponseContent = await createResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Create Profile Request Status Code: {createResponse.StatusCode}");
            Console.WriteLine($"Create Profile Response Content: {createResponseContent}");

            // Deserialize the response to get the ID of the created profile
            var jsonResponse = JsonSerializer.Deserialize<JsonElement>(createResponseContent);
            var createdProfileId = jsonResponse.GetProperty("id").GetString();

            // Ensure the ID was successfully retrieved
            Assert.IsFalse(string.IsNullOrEmpty(createdProfileId), "Failed to obtain a valid profile ID from the creation response.");

            // Retrieve the profile using the ID obtained from the creation response
            var retrievalResponse = await httpClient.GetAsync($"{_cProfileController}/{createdProfileId}");

            // Ensure the retrieval was successful
            Assert.IsTrue(retrievalResponse.IsSuccessStatusCode, "Failed to retrieve the created profile.");

            // Log the retrieval attempt details
            var retrievedProfileJson = await retrievalResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Retrieved Profile Content: {retrievedProfileJson}");

            // Deserialize the retrieved profile to access the ID correctly
            var retrievedProfile = JsonSerializer.Deserialize<JsonElement>(retrievedProfileJson);
            var retrievedProfileId = retrievedProfile.GetProperty("profile").GetProperty("id").GetString();

            // Assert the retrieved profile ID matches the expected ID
            Assert.AreEqual(createdProfileId, retrievedProfileId, "Retrieved profile ID does not match.");
        }


        public class CreateProfileResponse
        {
            public string Id { get; set; }
        }


        /// <summary>
        /// Tests querying profiles based on specified criteria.
        /// Verifies the query returns profiles that match the criteria.
        /// </summary>
        [TestMethod]
        public async Task QueryProfilesTest()
        {
            var httpClient = Helpers.GetHttpClient();
            var query = new
            {
                Fields = new string[] { }, // Assuming you're querying for all fields, or specify the fields you need
                Filter = new
                {
                    IsOrOperator = false,
                    Fields = new[]
                    {
                new
                {
                    FieldName = "CareerInfos._id",
                    Operator = 1, // Assuming '1' signifies the equality operator in your system
                    Argument = new[] { "CareerInfo-ApolloList-7373440846634EF7B29ECD1832A3B536" },
                    Distinct = true
                }
            }
                },
                RequestCount = true,
                Top = 10,
                Skip = 0,
                SortExpression = new
                {
                    FieldName = "trainingName",
                    Order = 1 // Assuming '1' signifies ascending order
                }
            };

            var jsonQuery = JsonSerializer.Serialize(query, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var content = new StringContent(jsonQuery, Encoding.UTF8, "application/json");

            try
            {
                var response = await httpClient.PostAsync($"{_cProfileController}", content); // Ensure the URL matches your query endpoint
                Console.WriteLine($"Response Status Code: {response.StatusCode}");

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Content: {responseContent}");

                Assert.IsTrue(response.IsSuccessStatusCode, "QueryProfiles should return a successful response.");

                // Adjust the deserialization type according to the expected response structure
                var profiles = JsonSerializer.Deserialize<List<Profile>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                Assert.IsNotNull(profiles, "The response content should not be null.");
                Assert.IsTrue(profiles.Any(), "The response should contain at least one profile.");

                // Further detailed assertions based on expected data
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException caught: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");

                throw; // Rethrow the exception to fail the test with the captured exception details
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected exception occurred: {ex.Message}");
                throw; // Rethrow for any other unexpected exceptions
            }
        }




        ///// <summary>
        ///// Tests creating or updating profiles.
        ///// Verifies profiles can be created or updated successfully and checks the response content.
        ///// </summary>
        //[TestMethod]
        //public async Task CreateOrUpdateProfileTest()
        //{
        //    var httpClient = Helpers.GetHttpClient();

        //    foreach (var testProfile in _testProfiles)
        //    {
        //        // Serialize the individual profile object to JSON
        //        var profileJson = JsonSerializer.Serialize(testProfile);
        //        HttpContent content = new StringContent(profileJson, Encoding.UTF8, "application/json");

        //        // Send the create or update request
        //        HttpResponseMessage response;

        //        // Check if the profile already has an ID to determine if it should be an update or insert
        //        if (string.IsNullOrEmpty(testProfile.Id))
        //        {
        //            // No ID means it's a new profile, so use the POST endpoint
        //            response = await httpClient.PostAsync($"{_cProfileController}", content);
        //        }
        //        else
        //        {
        //            // An ID is present, use the PUT endpoint to update
        //            response = await httpClient.PutAsync($"{_cProfileController}", content);
        //        }

        //        // Assert that the response is successful
        //        Assert.IsTrue(response.IsSuccessStatusCode, "The response should be successful.");

        //        // Deserialize the response content to get the ID of the created or updated profile
        //        var responseContent = await response.Content.ReadAsStringAsync();
        //        var createdOrUpdatedId = JsonSerializer.Deserialize<string>(responseContent);
        //        Assert.IsNotNull(createdOrUpdatedId, "The response should contain the ID of the created or updated profile.");

        //        // Additional assertions to check the response content can be added here
        //        // For example, we might want to retrieve the profile again using the ID and verify some of its properties
        //    }
        //}

    }
}
