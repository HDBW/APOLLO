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
        /// Ensures a clean state before each test by deleting any existing test profiles.
        /// Then inserts test profiles into the database for testing.
        /// </summary>
        [TestInitialize]
        public async Task Initialize()
        {
            
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
            string createdProfileId = "User-LK-01";

            try
            {
                // Assuming we have a predefined userId to use for testing
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
                createdProfileId = jsonResponse.GetProperty("id").GetString();

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
            finally
            {
                if (!string.IsNullOrEmpty(createdProfileId))
                {
                    // Cleanup logic to delete the created profile
                    var deleteResponse = await httpClient.DeleteAsync($"{_cProfileController}/{createdProfileId}");
                    Assert.IsTrue(deleteResponse.IsSuccessStatusCode, "Failed to delete the created profile.");
                }
            }
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

            // Define your query object without sort expression
            var query = new Query
            {
                Fields = new List<string> { "NameOfInstitution"}, 
                Filter = new Filter
                {
                    IsOrOperator = false,
                    Fields = new List<FieldExpression>
            {
                new FieldExpression
                {
                    FieldName = "CareerInfos.NameOfInstitution",
                    Operator = QueryOperator.Contains,
                    Argument = new List<object> { "Tech Corporation" } 
                }
                // Add more filters as needed
            }
                },
                RequestCount = true,
                Top = 10,
                Skip = 0
            };

            // Serialize your query object to JSON
            var jsonQuery = JsonSerializer.Serialize(query, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Create StringContent from your serialized query
            var content = new StringContent(jsonQuery, Encoding.UTF8, "application/json");

            // Replace "{_cProfileController}" with your actual profile query API endpoint
            var response = await httpClient.PostAsync($"{_cProfileController}", content);
            Assert.IsTrue(response.IsSuccessStatusCode, "QueryProfiles should return a successful response.");

            // Read the response content as a string
            var responseContent = await response.Content.ReadAsStringAsync();

            // Manually parse the JSON response using JsonDocument
            using var doc = JsonDocument.Parse(responseContent);
            var profilesJson = doc.RootElement.GetProperty("profiles").GetRawText();

            // Deserialize the "profiles" part of the response into a List<Profile>
            var profiles = JsonSerializer.Deserialize<List<Profile>>(profilesJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assuming QueryProfilesResponse can be directly instantiated with a list of profiles
            var queryProfilesResponse = new QueryProfilesResponse(profiles);

            // Perform your assertions
            Assert.IsNotNull(queryProfilesResponse, "The response content should not be null.");
            Assert.IsTrue(queryProfilesResponse.Profiles.Any(), "The response should contain at least one profile.");
        }


        /// <summary>
        /// Tests creating or updating profiles.
        /// Verifies profiles can be created or updated successfully and checks the response content.
        /// </summary>
        [TestMethod]
        public async Task CreateOrUpdateProfileTest()
        {
            var httpClient = Helpers.GetHttpClient();

            // Create a sample profile for testing
            var sampleProfile = CreateSampleProfile();

            var userId = "User-LK-01";

            var createRequestObj = new
            {
                UserId = userId,
                Profile = sampleProfile,
                Filter = new { } 
            };

            // Serializing the request object to JSON for creation
            var createRequestJson = JsonSerializer.Serialize(createRequestObj);
            HttpContent createContent = new StringContent(createRequestJson, Encoding.UTF8, "application/json");

            // PUT to the CreateOrUpdate endpoint for creation
            // Ensure the endpoint URL matches your API's requirement for profile creation or update
            var createResponse = await httpClient.PutAsync($"{_cProfileController}", createContent);
            var createResponseContent = await createResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Create Profile Response Status Code: {createResponse.StatusCode}");
            Console.WriteLine($"Create Profile Response Content: {createResponseContent}");

            Assert.IsTrue(createResponse.IsSuccessStatusCode, "Creation of the profile failed.");

            // Logging response for debugging
            Console.WriteLine($"Create Profile Response Content: {createResponseContent}");

            // Modifying the profile object for update, assuming there's some property to modify
            sampleProfile.CareerInfos[0].Description = "Updated advanced level applications.";

            // Serializing the modified request object to JSON for update
            var updateRequestJson = JsonSerializer.Serialize(createRequestObj);
            HttpContent updateContent = new StringContent(updateRequestJson, Encoding.UTF8, "application/json");

            // PUT to the CreateOrUpdate endpoint for update
            var updateResponse = await httpClient.PutAsync($"{_cProfileController}", updateContent);
            Assert.IsTrue(updateResponse.IsSuccessStatusCode, "Update of the profile failed.");

            // Logging response for debugging
            var updateResponseContent = await updateResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Update Profile Response Content: {updateResponseContent}");

           // TODO: Deletetion
        }

    }
}

