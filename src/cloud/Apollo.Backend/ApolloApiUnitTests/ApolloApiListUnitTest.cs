// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.


using Apollo.Common.Entities;
using Daenet.MongoDal;
using Daenet.MongoDal.Entitties;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;

namespace Apollo.Api.UnitTests
{
    /// <summary>
    /// Unit tests for the ApolloApi class.
    /// </summary>
    [TestClass]
    public class ApolloApiListUnitTests
    {
        private const string _cTestListType1 = "TestListType1";
        private const string _cTestListType2 = "TestListType2";
        private const string _cTestListType3 = "TestListType3";
        private const string _cTestListType4 = "TestListType4";

        /// <summary
        /// The list of ApolloList objects used for testing.
        /// </summary>
        private List<ApolloList> _testList = new List<ApolloList>()
        {
             new ApolloList()
             {
                 Id = "UT01",
                 ItemType = _cTestListType1,
                 Items = new List<ApolloListItem>()
                 {
                     new ApolloListItem()
                     {
                         ListItemId = 1,
                         Lng = "DE",
                         Value = "C# Entwickler"
                     },
                     new ApolloListItem()
                     {
                         ListItemId = 2,
                         Lng = "EN",
                         Value = "C# Developer"
                     }
                 }
             },
             new ApolloList()
             {
                 Id = "UT02",
                 ItemType = _cTestListType2,
                 Items = new List<ApolloListItem>()
                 {
                     new ApolloListItem()
                     {
                         ListItemId = 1,
                         Lng = "DE",
                         Value = "Python Entwickler"
                     },
                     new ApolloListItem()
                     {
                         ListItemId = 2,
                         Lng = "EN",
                         Value = "Python Developer"
                     }
                 }
             },
             new ApolloList()
             {
                 Id = "UT03",
                 ItemType = _cTestListType3,
                 Items = new List<ApolloListItem>()
                 {
                     new ApolloListItem()
                     {
                         ListItemId = 1,
                         Lng = "DE",
                         Value = "C++ Entwickler"
                     },
                     new ApolloListItem()
                     {
                         ListItemId= 2,
                         Lng = "EN",
                         Value = "C++ Developer"
                     }
                 }
             },
              new ApolloList()
             {
                 Id = "UT04",
                 ItemType = _cTestListType4,
                 Items = new List<ApolloListItem>()
                 {
                     new ApolloListItem()
                     {   ListItemId = 1,
                         Lng = "DE",
                         Value = "JAVA Entwickler"
                     },
                       new ApolloListItem()
                     {   ListItemId = 1,
                         Lng = "DE",
                         Value = "C# Entwickler"
                     },
                         new ApolloListItem()
                     {   ListItemId = 1,
                         Lng = "DE",
                         Value = "Python Hobbydeveloper"
                     },
                           new ApolloListItem()
                     {   ListItemId = 1,
                         Lng = "DE",
                         Value = "JAVA Script angelernter Programmierer"
                     },
                             new ApolloListItem()
                     {   ListItemId = 1,
                         Lng = "DE",
                         Value = "Indischer alleskönner Entwickler"
                     },
                     new ApolloListItem()
                     {   ListItemId= 2,
                         Lng = "EN",
                         Value = "C++ Developer"
                     }
                 }
             },
        };


        /// <summary>
        /// Makes sure that all test records are deleted after each test execution.
        /// </summary>
        /// <returns></returns>
        [TestCleanup]
        public async Task Cleanup()
        {
            var api = Helpers.GetApolloApi();

            var deleteResult = await api.DeleteListAsync(_testList.Select(l => l.Id).ToArray());

            Assert.AreEqual(_testList.Count, deleteResult);
        }


        private async Task InsertTestItems(ApolloApi api)
        {
            foreach (var item in _testList)
            {
                await api.CreateOrUpdateListAsync(item);
            }
        }


        /// <summary>
        /// Inserts test lists and then gets every of them.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task GetListInternalAsyncTest()
        {
            var api = Helpers.GetApolloApi();

            await InsertTestItems(api);

            foreach (var item in _testList)
            {
                var result = await api.GetListAsync(item.Items.First().Lng, item.ItemType);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(item.ItemType, result.ItemType);
                Assert.AreEqual(item.Items.Count, result.Items.Count);
                Assert.AreEqual(item.Items.First().Lng, result.Items[0].Lng);
                Assert.AreEqual(item.Items.First().Value, result.Items[0].Value);
            }
        }



        /// <summary>
        /// Tests querying Qualification List objects that match with the language.
        /// </summary>
        [TestMethod]
        public async Task QueryListAsyncTest()
        {
            var api = Helpers.GetApolloApi();

            await InsertTestItems(api);      

            // Get ll items of the given ItemType and language.
            var results = await api.QueryListItemsAsync(_testList?.First()?.Items?.First().Lng!, _testList?.First()?.ItemType!, null);

            // Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count == 1); // Single DE item is returned.

            results = await api.QueryListItemsAsync(_testList?.First()?.Items?.First().Lng!, _testList?.First()?.ItemType!, _testList?.First().Items.First().Value.Substring(1,3));
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count == 1); // all items are returned.
            Assert.IsTrue(results[0].Value.Contains("C# Entwickler"));
        }


        private static void AssertListId(string id)
        {
            Assert.IsNotNull(id);
            Assert.IsTrue(id.StartsWith(nameof(List)));
        }


        /// <summary>
        /// Creates the test lists in the database.
        /// </summary>
        [TestMethod]
        public async Task InsertListsWithPredefinedIdTest()
        {
            var api = Helpers.GetApolloApi();

            // Insert each test list and accumulate qualification IDs
            foreach (var apolloList in _testList)
            {
                var id = await api.CreateOrUpdateListAsync(apolloList);

                Assert.IsNotNull(id);
                Assert.IsTrue(id == apolloList.Id);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public async Task InsertListsWithAutogenIdTest()
        {
            var api = Helpers.GetApolloApi();

            // Insert each test list and accumulate qualification IDs
            foreach (var apolloList in _testList)
            {
                apolloList.Id = null;

                var id = await api.CreateOrUpdateListAsync(apolloList);

                AssertListId(id);
            }
        }


        /// <summary>
        /// Tests creating or updating a Qualification List object and then cleaning up by deleting it.
        /// </summary>
        [TestMethod]
        public async Task UpdateExistingItemsIntheListTest()
        {
            var api = Helpers.GetApolloApi();

            // Update an existing Apollo list
            var existingApolloList = _testList.FirstOrDefault();
            Assert.IsNotNull(existingApolloList, "Empty item _testList.");

            // Update the existing item in Apollo list
            existingApolloList.Items[1].Lng = "EN Updated";
            existingApolloList.Items[1].Value = "C# Developer Updated";

            var existingApolloListId = await api.CreateOrUpdateListAsync(existingApolloList);
            Assert.IsNotNull(existingApolloListId);

            // Retrieve the updated list
            var updatedList = await api.GetListAsync(_testList[0].Items[0].Lng, _testList[0].ItemType);

            // Assert that the existing list is updated
            Assert.IsNotNull(updatedList);
            Assert.AreEqual(existingApolloList.Items[1].Lng, updatedList.Items[1].Lng);
            Assert.AreEqual(existingApolloList.Items[1].Value, updatedList.Items[1].Value);

            // Add a new Item to the existing List
            var newQualificationList = new ApolloList
            {
                Id = "UT01",
                ItemType = _cTestListType1,
                Items = new List<ApolloListItem>
                {
                    new ApolloListItem
                    {
                        ListItemId = 3,
                        Lng = "DE",
                        Value = "New Value Item"
                    }
                }
            };

            throw new NotImplementedException();

            //var newQualificationIds = await api.CreateOrUpdateQualificationAsync(newQualificationList);
            //Assert.IsNotNull(newQualificationIds);

            //// Retrieve the list after adding the new item
            //var newListWithNewItem = await api.GetListAsync(_testList[0].Items[0].Lng, _testList[0].ItemType);

            //// Assert that the new item is present in the list
            //Assert.IsNotNull(newListWithNewItem);
            //Assert.IsTrue(newListWithNewItem.Items.Any(item => item.ListItemId == 3));
        }

        #region List Population
        /// <summary>
        /// Creates the test lists in the database.
        /// </summary>
        [TestMethod]
        public async Task PopulateListsTest()
        {
            await PopulateContactType();
            await PopulateCarreerType();
            await PopulateCompletionState();
            await PopulateDriversLicense();
            await PopulateEducationType();
            await PopulateLanguageNiveau();
            await PopulateRecognitionType();
            await PopulateSchoolGraduation();
            await PopulateServiceType();
            await PopulateStaffResponsibility();
            await PopulateUniversityDegree();
            await PopulateTypeOfSchool();
            await PopulateVoluntaryServiceType();
            await PopulateWilling();
            await PopulateYearRange();
            await PopulateWorkingTimeModel();
            await PopulateCompletionState();
       
        }

        private async Task PopulateContactType()
        {
            var api = Helpers.GetApolloApi();

            ApolloList apolloList = new ApolloList()
            {
                Description = "This Enum represents the Type of the Contact.It indicates if the Contact is a Professional or Private Contact",
                ItemType = nameof(ContactType),
                Items = new List<ApolloListItem>()
                 {
                     new ApolloListItem()
                     {
                         ListItemId = 0,
                         Value = "Unknown"
                     },
                     new ApolloListItem()
                     {
                         ListItemId = 1,
                         Value = "Professional"
                     },
                     new ApolloListItem()
                     {
                         ListItemId = 2,
                         Value = "Private"
                     }
                 }
            };

            var id = await api.CreateOrUpdateListAsync(apolloList);

            var dbItem = await api.GetListAsync(id:id);

            Assert.IsNotNull(dbItem);
            Assert.IsTrue(dbItem.ItemType == nameof(ContactType));
            Assert.IsTrue(dbItem.Items.Count == 3);
        }

        private async Task PopulateCarreerType()
        {
            var api = Helpers.GetApolloApi();

            ApolloList apolloList = new ApolloList()
            {
                Description = "Could be specific for countries",
                ItemType = nameof(CareerType),
                Items = new List<ApolloListItem>()
                 {

                    new ApolloListItem() { ListItemId = 0, Value = "Unknown" },
                    new ApolloListItem() { ListItemId = 1, Value = "Other" },
                    new ApolloListItem() { ListItemId = 2, Value = "WorkExperience" },
                    new ApolloListItem() { ListItemId = 3, Value = "PartTimeWorkExperience" },
                    new ApolloListItem() { ListItemId = 4, Value = "Internship" },
                    new ApolloListItem() { ListItemId = 5, Value = "SelfEmployment" },
                    new ApolloListItem() { ListItemId = 6, Value = "Service" },
                    new ApolloListItem() { ListItemId = 7, Value = "CommunityService" },
                    new ApolloListItem() { ListItemId = 8, Value = "VoluntaryService" },
                    new ApolloListItem() { ListItemId = 9, Value = "ParentalLeave" },
                    new ApolloListItem() { ListItemId = 10, Value = "Homemaker" },
                    new ApolloListItem() { ListItemId = 11, Value = "ExtraOccupationalExperience" },

                }
            };

            var id = await api.CreateOrUpdateListAsync(apolloList);

            var dbItem = await api.GetListAsync(id: id);

            Assert.IsNotNull(dbItem);
            Assert.IsTrue(dbItem.ItemType == nameof(CareerType));
            Assert.IsTrue(dbItem.Items.Count == 12);
        }

        private async Task PopulateCompletionState()
        {
            var api = Helpers.GetApolloApi();

            ApolloList apolloList = new ApolloList()
            {
                Description = "CompletionState Description",
                ItemType = nameof(CompletionState),
                Items = new List<ApolloListItem>()
                 {
                    new ApolloListItem() { ListItemId = 0, Value = "None" },
                    new ApolloListItem() { ListItemId = 1, Value = "Completed" },
                    new ApolloListItem() { ListItemId = 2, Value = "Failed" },
                    new ApolloListItem() { ListItemId = 3, Value = "Ongoing" }
                 }
            };

            var id = await api.CreateOrUpdateListAsync(apolloList);

            var dbItem = await api.GetListAsync(id: id);

            Assert.IsNotNull(dbItem);
            Assert.IsTrue(dbItem.ItemType == nameof(CompletionState));
            Assert.IsTrue(dbItem.Items.Count == 4);
        }


        private async Task PopulateDriversLicense()
        {
            var api = Helpers.GetApolloApi();

            ApolloList apolloList = new ApolloList()
            {
                Description = "Is Culture specific and prop a List sync",
                ItemType = nameof(DriversLicense),
                Items = new List<ApolloListItem>()
                 {
                    new ApolloListItem() { ListItemId = 0, Value = "Unknown" },
                    new ApolloListItem() { ListItemId = 1, Value = "B" },
                    new ApolloListItem() { ListItemId = 2, Value = "BE" },
                    new ApolloListItem() { ListItemId = 3, Value = "Forklift" },
                    new ApolloListItem() { ListItemId = 4, Value = "C1E" }
                 }
            };

            var id = await api.CreateOrUpdateListAsync(apolloList);

            var dbItem = await api.GetListAsync(id: id);

            Assert.IsNotNull(dbItem);
            Assert.IsTrue(dbItem.ItemType == nameof(DriversLicense));
            Assert.IsTrue(dbItem.Items.Count == 5);
        }


        private async Task PopulateEducationType()
        {
            var api = Helpers.GetApolloApi();

            ApolloList apolloList = new ApolloList()
            {
                Description = "Populate Education Type",
                ItemType = nameof(EducationType),
                Items = new List<ApolloListItem>()
                 {
                    new ApolloListItem() { ListItemId = 0, Value = "Unknown" },
                    new ApolloListItem() { ListItemId = 1, Value = "Education" },
                    new ApolloListItem() { ListItemId = 2, Value = "CompanyBasedVocationalTraining" },
                    new ApolloListItem() { ListItemId = 3, Value = "Study" },
                    new ApolloListItem() { ListItemId = 4, Value = "VocationalTraining" },
                    new ApolloListItem() { ListItemId = 5, Value = "FurtherEducation" }
                 }
            };

            var id = await api.CreateOrUpdateListAsync(apolloList);

            var dbItem = await api.GetListAsync(id: id);

            Assert.IsNotNull(dbItem);
            Assert.IsTrue(dbItem.ItemType == nameof(EducationType));
            Assert.IsTrue(dbItem.Items.Count == 6);
        }

        private async Task PopulateLanguageNiveau()
        {
            var api = Helpers.GetApolloApi();

            ApolloList apolloList = new ApolloList()
            {
                Description = "International Standard",
                ItemType = nameof(LanguageNiveau),
                Items = new List<ApolloListItem>()
                 {
                    new ApolloListItem() { ListItemId = 0, Value = "Unknown" },
                    new ApolloListItem() { ListItemId = 1, Value = "A1" },
                    new ApolloListItem() { ListItemId = 2, Value = "A2" },
                    new ApolloListItem() { ListItemId = 3, Value = "B1" },
                    new ApolloListItem() { ListItemId = 4, Value = "B2" },
                    new ApolloListItem() { ListItemId = 5, Value = "C1" },
                    new ApolloListItem() { ListItemId = 6, Value = "C2" }
                 }
            };

            var id = await api.CreateOrUpdateListAsync(apolloList);

            var dbItem = await api.GetListAsync(id: id);

            Assert.IsNotNull(dbItem);
            Assert.IsTrue(dbItem.ItemType == nameof(LanguageNiveau));
            Assert.IsTrue(dbItem.Items.Count == 7);
        }


        private async Task PopulateRecognitionType()
        {
            var api = Helpers.GetApolloApi();

            ApolloList apolloList = new ApolloList()
            {
                Description = "German Specific",
                ItemType = nameof(RecognitionType),
                Items = new List<ApolloListItem>()
                 {
                   new ApolloListItem() { ListItemId = 0, Value = "Unknown" },
                   new ApolloListItem() { ListItemId = 1, Value = "UnregulatedNotRecognized" },
                   new ApolloListItem() { ListItemId = 2, Value = "RegulatedNotRecognized" },
                   new ApolloListItem() { ListItemId = 3, Value = "Recognized" },
                   new ApolloListItem() { ListItemId = 4, Value = "Pending" },
                   new ApolloListItem() { ListItemId = 5, Value = "PartialRecognized" }
                 }
            };

            var id = await api.CreateOrUpdateListAsync(apolloList);

            var dbItem = await api.GetListAsync(id: id);

            Assert.IsNotNull(dbItem);
            Assert.IsTrue(dbItem.ItemType == nameof(RecognitionType));
            Assert.IsTrue(dbItem.Items.Count == 6);
        }


        private async Task PopulateSchoolGraduation()
        {
            var api = Helpers.GetApolloApi();

            ApolloList apolloList = new ApolloList()
            {
                Description = "German Specific",
                ItemType = nameof(SchoolGraduation),
                Items = new List<ApolloListItem>()
                 {
                   new ApolloListItem() { ListItemId = 0, Value = "Unknown" },
                   new ApolloListItem() { ListItemId = 1, Value = "SecondarySchoolCertificate" },
                   new ApolloListItem() { ListItemId = 2, Value = "AdvancedTechnicalCollegeCertificate" },
                   new ApolloListItem() { ListItemId = 3, Value = "HigherEducationEntranceQualificationALevel" },
                   new ApolloListItem() { ListItemId = 4, Value = "IntermediateSchoolCertificate" },
                   new ApolloListItem() { ListItemId = 5, Value = "ExtendedSecondarySchoolLeavingCertificate" },
                   new ApolloListItem() { ListItemId = 6, Value = "NoSchoolLeavingCertificate" },
                   new ApolloListItem() { ListItemId = 7, Value = "SpecialSchoolLeavingCertificate" },
                   new ApolloListItem() { ListItemId = 8, Value = "SubjectRelatedEntranceQualification" },
                   new ApolloListItem() { ListItemId = 9, Value = "AdvancedTechnicalCollegeWithoutCertificate" }

                 }
            };

            var id = await api.CreateOrUpdateListAsync(apolloList);

            var dbItem = await api.GetListAsync(id: id);

            Assert.IsNotNull(dbItem);
            Assert.IsTrue(dbItem.ItemType == nameof(SchoolGraduation));
            Assert.IsTrue(dbItem.Items.Count == 10);
        }


        private async Task PopulateServiceType()
        {
            var api = Helpers.GetApolloApi();

            ApolloList apolloList = new ApolloList()
            {
                Description = "German Specific",
                ItemType = nameof(ServiceType),
                Items = new List<ApolloListItem>()
                 {
                  new ApolloListItem() { ListItemId = 0, Value = "Unknown" },
                  new ApolloListItem() { ListItemId = 1, Value = "CivilianService" },
                  new ApolloListItem() { ListItemId = 2, Value = "MilitaryService" },
                  new ApolloListItem() { ListItemId = 3, Value = "VoluntaryMilitaryService" },
                  new ApolloListItem() { ListItemId = 4, Value = "MilitaryExercise" }
                 }
            };

            var id = await api.CreateOrUpdateListAsync(apolloList);

            var dbItem = await api.GetListAsync(id: id);

            Assert.IsNotNull(dbItem);
            Assert.IsTrue(dbItem.ItemType == nameof(ServiceType));
            Assert.IsTrue(dbItem.Items.Count == 5);
        }


        private async Task PopulateStaffResponsibility()
        {
            var api = Helpers.GetApolloApi();

            ApolloList apolloList = new ApolloList()
            {
                Description = "German Specific",
                ItemType = nameof(StaffResponsibility),
                Items = new List<ApolloListItem>()
                 {
                     new ApolloListItem() { ListItemId = 0, Value = "Unknown" },
                     new ApolloListItem() { ListItemId = 1, Value = "LessThan10" },
                     new ApolloListItem() { ListItemId = 2, Value = "Between10And49" },
                     new ApolloListItem() { ListItemId = 3, Value = "Between50And499" },
                     new ApolloListItem() { ListItemId = 4, Value = "MoreThan499" }
                 }
            };

            var id = await api.CreateOrUpdateListAsync(apolloList);

            var dbItem = await api.GetListAsync(id: id);

            Assert.IsNotNull(dbItem);
            Assert.IsTrue(dbItem.ItemType == nameof(StaffResponsibility));
            Assert.IsTrue(dbItem.Items.Count == 5);
        }


        private async Task PopulateTypeOfSchool()
        {
            var api = Helpers.GetApolloApi();

            ApolloList apolloList = new ApolloList()
            {
                Description = "German Specific enum",
                ItemType = nameof(TypeOfSchool),
                Items = new List<ApolloListItem>()
                 {
                     new ApolloListItem() { ListItemId = 0, Value = "Unknown" },
                     new ApolloListItem() { ListItemId = 1, Value = "Other" },
                     new ApolloListItem() { ListItemId = 2, Value = "HighSchool" },
                     new ApolloListItem() { ListItemId = 3, Value = "SecondarySchool" },
                     new ApolloListItem() { ListItemId = 4, Value = "VocationalCollege" }
                 }
            };

            var id = await api.CreateOrUpdateListAsync(apolloList);

            var dbItem = await api.GetListAsync(id: id);

            Assert.IsNotNull(dbItem);
            Assert.IsTrue(dbItem.ItemType == nameof(TypeOfSchool));
            Assert.IsTrue(dbItem.Items.Count == 5);
        }


        private async Task PopulateUniversityDegree()
        {
            var api = Helpers.GetApolloApi();

            ApolloList apolloList = new ApolloList()
            {
                Description = "International",
                ItemType = nameof(UniversityDegree),
                Items = new List<ApolloListItem>()
                 {
                    new ApolloListItem() { ListItemId = 0, Value = "Unknown" },
                    new ApolloListItem() { ListItemId = 1, Value = "Master" },
                    new ApolloListItem() { ListItemId = 2, Value = "Bachelor" },
                    new ApolloListItem() { ListItemId = 3, Value = "Pending" },
                    new ApolloListItem() { ListItemId = 4, Value = "Doctorate" },
                    new ApolloListItem() { ListItemId = 5, Value = "StateExam" },
                    new ApolloListItem() { ListItemId = 6, Value = "UnregulatedUnrecognized" },
                    new ApolloListItem() { ListItemId = 7, Value = "RegulatedUnrecognized" },
                    new ApolloListItem() { ListItemId = 8, Value = "PartialRecognized" },
                    new ApolloListItem() { ListItemId = 9, Value = "EcclesiasticalExam" },
                    new ApolloListItem() { ListItemId = 10, Value = "Other" }
                 }
            };

            var id = await api.CreateOrUpdateListAsync(apolloList);

            var dbItem = await api.GetListAsync(id: id);

            Assert.IsNotNull(dbItem);
            Assert.IsTrue(dbItem.ItemType == nameof(UniversityDegree));
            Assert.IsTrue(dbItem.Items.Count == 11);
        }


        private async Task PopulateVoluntaryServiceType()
        {
            var api = Helpers.GetApolloApi();

            ApolloList apolloList = new ApolloList()
            {
                Description = "VoluntaryServiceType in German",
                ItemType = nameof(VoluntaryServiceType),
                Items = new List<ApolloListItem>()
                 {
                    new ApolloListItem() { ListItemId = 0, Value = "Unknown" },
                    new ApolloListItem() { ListItemId = 1, Value = "Other" },
                    new ApolloListItem() { ListItemId = 2, Value = "VoluntarySocialYear" },
                    new ApolloListItem() { ListItemId = 3, Value = "FederalVolunteerService" },
                    new ApolloListItem() { ListItemId = 4, Value = "VoluntaryEcologicalYear" },
                    new ApolloListItem() { ListItemId = 5, Value = "VoluntarySocialTrainingYear" },
                    new ApolloListItem() { ListItemId = 6, Value = "VoluntaryCulturalYear" },
                    new ApolloListItem() { ListItemId = 7, Value = "VoluntarySocialYearInSport" },
                    new ApolloListItem() { ListItemId = 8, Value = "VoluntaryYearInMonumentConservation" }
                 }
            };

            var id = await api.CreateOrUpdateListAsync(apolloList);

            var dbItem = await api.GetListAsync(id: id);

            Assert.IsNotNull(dbItem);
            Assert.IsTrue(dbItem.ItemType == nameof(VoluntaryServiceType));
            Assert.IsTrue(dbItem.Items.Count == 9);
        }

        private async Task PopulateWilling()
        {
            var api = Helpers.GetApolloApi();

            ApolloList apolloList = new ApolloList()
            {
                Description = "Willing German / BA",
                ItemType = nameof(Willing),
                Items = new List<ApolloListItem>()
                 {
                    new ApolloListItem() { ListItemId = 0, Value = "Unknown" },
                    new ApolloListItem() { ListItemId = 1, Value = "Yes" },
                    new ApolloListItem() { ListItemId = 2, Value = "No" },
                    new ApolloListItem() { ListItemId = 3, Value = "Partly" }
                 }
            };

            var id = await api.CreateOrUpdateListAsync(apolloList);

            var dbItem = await api.GetListAsync(id: id);
            Assert.IsNotNull(dbItem);
            Assert.IsTrue(dbItem.ItemType == nameof(Willing));
            Assert.IsTrue(dbItem.Items.Count == 4);
        }



        private async Task PopulateYearRange()
        {
            var api = Helpers.GetApolloApi();

            ApolloList apolloList = new ApolloList()
            {
                Description = "YearRange BA/German",
                ItemType = nameof(YearRange),
                Items = new List<ApolloListItem>()
                 {
                   new ApolloListItem() { ListItemId = 0, Value = "Unknown" },
                   new ApolloListItem() { ListItemId = 1, Value = "LessThan2" },
                   new ApolloListItem() { ListItemId = 2, Value = "Between2And5" },
                   new ApolloListItem() { ListItemId = 3, Value = "MoreThan5" }
                 }
            };

            var id = await api.CreateOrUpdateListAsync(apolloList);

            var dbItem = await api.GetListAsync(id: id);
            Assert.IsNotNull(dbItem);
            Assert.IsTrue(dbItem.ItemType == nameof(YearRange));
            Assert.IsTrue(dbItem.Items.Count == 4);
        }



        private async Task PopulateWorkingTimeModel()
        {
            var api = Helpers.GetApolloApi();

            ApolloList apolloList = new ApolloList()
            {
                Description = "WorkingTimeModel BA German specific",
                ItemType = nameof(WorkingTimeModel),
                Items = new List<ApolloListItem>()
                 {
                       new ApolloListItem() { ListItemId = 0, Value = "Unknown" },
                       new ApolloListItem() { ListItemId = 1, Value = "FULLTIME" },
                       new ApolloListItem() { ListItemId = 2, Value = "PARTTIME" },
                       new ApolloListItem() { ListItemId = 3, Value = "SHIFT_NIGHT_WORK_WEEKEND" },
                       new ApolloListItem() { ListItemId = 4, Value = "MINIJOB" },
                       new ApolloListItem() { ListItemId = 5, Value = "HOME_TELEWORK" }
                 }
            };

            var id = await api.CreateOrUpdateListAsync(apolloList);

            var dbItem = await api.GetListAsync(id: id);
            Assert.IsNotNull(dbItem);
            Assert.IsTrue(dbItem.ItemType == nameof(WorkingTimeModel));
            Assert.IsTrue(dbItem.Items.Count == 6);
        }
        #endregion

    }
}

