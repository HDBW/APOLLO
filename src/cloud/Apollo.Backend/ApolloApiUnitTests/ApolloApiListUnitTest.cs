// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.


using System.Collections.Immutable;
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
        //[TestCleanup]
        //public async Task Cleanup()
        //{
        //    var api = Helpers.GetApolloApi();

        //    var deleteResult = await api.DeleteListAsync(_testList.Select(l => l.Id).ToArray());

        //    Assert.AreEqual(_testList.Count, deleteResult);
        //}


        private async Task InsertTestItems(ApolloApi api)
        {
            foreach (var item in _testList)
            {
                await api.CreateOrUpdateListAsync(item);
            }
        }



        /// <summary>
        /// Inserts test lists and then gets every of them
        /// </summary>
        /// <returns></returns>

        [TestMethod]
        public async Task GetAllListsAsyncTest()
        {
            var api = Helpers.GetApolloApi();

            //
            // Get all lists
            var result = await api.GetAllListsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }




        /// <summary>
        /// Inserts test lists and then gets every of them that match with Id.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetListAsyncTestWithIdSpecified()
        {
            var api = Helpers.GetApolloApi();

            await InsertTestItems(api);

            foreach (var item in _testList)
            {
                //
                // With Id specified only

                var result = await api.GetListAsync(id: item.Id);
                Assert.IsNotNull(result);
                Assert.AreEqual(item.Id, result.Id);
                Assert.AreEqual(item.Items.Count, result.Items.Count);

            }

        }



        /// <summary>
        /// Filter test with both ItemType only
        /// Test will Inserts  lists and then gets all item that match with given itemType.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetListAsyncTestOnlyWithItemType()
        {
            var api = Helpers.GetApolloApi();

            await InsertTestItems(api);

            foreach (var item in _testList)
            {
                var result = await api.GetListAsync(itemType: item.ItemType);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(item.ItemType, result.ItemType);
                Assert.AreEqual(item.Items.Count, result.Items.Count);
                Assert.AreEqual(item.Items.First().Lng, result.Items[0].Lng);
                Assert.AreEqual(item.Items.First().Value, result.Items[0].Value);
            }
        }

        /// <summary>
        /// Inserts test lists and then tests the case when an item is not found.
        /// </summary>
        [TestMethod]
        public async Task GetListAsyncTestItemNotFoundException()
        {
            var api = Helpers.GetApolloApi();
            await InsertTestItems(api);

            //
            // The following mnagauge does not exist in the DB
            var nonExistentItemType = "NonExistentItemType";

            //
            //Check  for having ItemNotFoundException
            await Assert.ThrowsExceptionAsync<ApolloApiException>(() => api.GetListAsync(itemType: nonExistentItemType, throwIfNotFound: true));

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
            var results = await api.QueryListItemsAsync(_testList?.Last()?.Items?.First().Lng!, _testList?.Last()?.ItemType!, null);

            //
            //Check count of items with matching language
            var matchingtemsCountInTestList = _testList?.Last()?.Items.Count(expectedItem => expectedItem.Lng == _testList?.Last()?.Items.First().Lng);

            // Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count == matchingtemsCountInTestList);
            Assert.IsTrue(results.All(expectedItem => expectedItem.Lng == _testList?.Last()?.Items?.First().Lng));

            results = await api.QueryListItemsAsync(_testList?.First()?.Items?.First().Lng!, _testList?.First()?.ItemType!, _testList?.First().Items.First().Value.Substring(1, 3));
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count == 1); // all items are returned.
            Assert.IsTrue(results[0].Value.Contains(_testList?.First().Items.First().Value!));
        }




        /// <summary>
        /// Test for inserting lists with autogenerated IDs, verifying successful insertion,
        /// </summary>
        [TestMethod]
        public async Task InsertListsWithAutogenIdTest()
        {
            var api = Helpers.GetApolloApi();

            //
            // Create an ApolloList without specifying the ID
            var apolloListWithoutId = new ApolloList
            {
                Id = null,
                ItemType = _cTestListType1,
                Items = _testList.First().Items
            };

            //
            // Insert the list and obtain the autogenerated ID
            var id = await api.CreateOrUpdateListAsync(apolloListWithoutId);

            Assert.AreEqual(_testList?.First().Id, id);

            // 
            // Delete the list using the obtained ID
            var deleteResult = await api.DeleteListAsync(new[] { id });

            // 
            // Verify that the delete operation was successful
            Assert.AreEqual(1, deleteResult);
        }



        /// <summary>
        /// Tests creating or updating a Qualification List object and then cleaning up by deleting it.
        /// </summary>
        [TestMethod]
        public async Task UpdateExistingItemsIntheListTest()
        {
            var api = Helpers.GetApolloApi();
            await InsertTestItems(api);

            var existingApolloList = _testList.FirstOrDefault();
            Assert.IsNotNull(existingApolloList, "Empty item _testList.");

            //
            //Update the existing item in Apollo list

            existingApolloList.Items[1].Lng = "EN Updated";
            existingApolloList.Items[1].Value = "C# Developer Updated";

            var existingApolloListId = await api.CreateOrUpdateListAsync(existingApolloList);
            Assert.IsNotNull(existingApolloListId);

            //
            //Retrieve the updated list
            var updatedList = await api.GetListAsync(itemType: _testList[0].ItemType);

            //
            // Assert that the existing list is updated
            Assert.IsNotNull(updatedList);
            Assert.AreEqual(existingApolloList.Items[1].Lng, updatedList.Items[1].Lng);
            Assert.AreEqual(existingApolloList.Items[1].Value, updatedList.Items[1].Value);
            //await Cleanup();
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
            await PopulateKnownLicense();
        }

        private async Task PopulateContactType()
        {
            var api = Helpers.GetApolloApi();

            ApolloList apolloList = new ApolloList()
            {
                Description = "This Enum represents the Type of the Contact.It indicates if the Contact is a Professional or Private Contact",
                ItemType = nameof(ContactType),
                Items = new List<ApolloListItem>
                {
                    new ApolloListItem { ListItemId = 0, Value = "Unknown", Lng = null, Description = "Unknown" },
                    new ApolloListItem { ListItemId = 1, Value = "Professional", Lng = null, Description = "Professional" },
                    new ApolloListItem { ListItemId = 2, Value = "Private", Lng = null, Description = "Private" },
                    new ApolloListItem { ListItemId = 3, Value = "Trainer", Lng = null, Description = null },
                    new ApolloListItem { ListItemId = 4, Value = "Training", Lng = null, Description = null },
                    new ApolloListItem { ListItemId = 5, Value = "TrainingLocation", Lng = null, Description = null }
                }
            };

            var id = await api.CreateOrUpdateListAsync(apolloList);

            var dbItem = await api.GetListAsync(id: id);

            Assert.IsNotNull(dbItem);
            Assert.IsTrue(dbItem.ItemType == nameof(ContactType));
            Assert.IsTrue(dbItem.Items.Count == 6);
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
                    new ApolloListItem { ListItemId = 0, Value = "Unknown", Lng = null, Description = null },
                    new ApolloListItem { ListItemId = 1, Value = "Other", Lng = null, Description = "Sonstiges" },
                    new ApolloListItem { ListItemId = 2, Value = "WorkExperience", Lng = null, Description = "Berufspraxis" },
                    new ApolloListItem { ListItemId = 3, Value = "PartTimeWorkExperience", Lng = null, Description = "Berufspraxis (Nebenbeschäftigung)" },
                    new ApolloListItem { ListItemId = 4, Value = "Internship", Lng = null, Description = "Praktikum" },
                    new ApolloListItem { ListItemId = 5, Value = "SelfEmployment", Lng = null, Description = "Selbständigkeit" },
                    new ApolloListItem { ListItemId = 6, Value = "Service", Lng = null, Description = "Wehrdienst/-übung/Zivildienst" },
                    new ApolloListItem { ListItemId = 7, Value = "CommunityService", Lng = null, Description = "Gemeinnützige Arbeit" },
                    new ApolloListItem { ListItemId = 8, Value = "VoluntaryService", Lng = null, Description = "Freiwilligendienst" },
                    new ApolloListItem { ListItemId = 9, Value = "ParentalLeave", Lng = null, Description = "Mutterschutz / Elternzeit" },
                    new ApolloListItem { ListItemId = 10, Value = "Homemaker", Lng = null, Description = "Hausfrau/mann" },
                    new ApolloListItem { ListItemId = 11, Value = "ExtraOccupationalExperience", Lng = null, Description = "Außerberufliche Erfahrungen" },
                    new ApolloListItem { ListItemId = 12, Value = "PersonCare", Lng = null, Description = "Betr. pflegebed. Person" }
                }
            };

            var id = await api.CreateOrUpdateListAsync(apolloList);

            var dbItem = await api.GetListAsync(id: id);

            Assert.IsNotNull(dbItem);
            Assert.IsTrue(dbItem.ItemType == nameof(CareerType));
            Assert.IsTrue(dbItem.Items.Count == 13);
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
                    new ApolloListItem() { ListItemId = 0, Lng = null, Description = null,  Value = "None" },
                    new ApolloListItem() { ListItemId = 1, Lng = null, Description = null,  Value = "Completed" },
                    new ApolloListItem() { ListItemId = 2, Lng = null, Description = null,  Value = "Failed" },
                    new ApolloListItem() { ListItemId = 3, Lng = null, Description = null,  Value = "Ongoing" }
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

            ApolloList apolloList = new ApolloList
            {
                Description = "Is Culture specific and prop a List sync",
                ItemType = nameof(DriversLicense),
                Items = new List<ApolloListItem>
                {
                    new ApolloListItem { ListItemId = 0, Value = "Unknown", Lng = null, Description = null },
                    new ApolloListItem { ListItemId = 1, Value = "B", Lng = null, Description = "Fahrerlaubnis B" },
                    new ApolloListItem { ListItemId = 2, Value = "BE", Lng = null, Description = "Fahrerlaubnis BE" },
                    new ApolloListItem { ListItemId = 3, Value = "Forklift", Lng = null, Description = "Gabelstaplerschein" },
                    new ApolloListItem { ListItemId = 4, Value = "C1E", Lng = null, Description = "Fahrerlaubnis C1E" },
                    new ApolloListItem { ListItemId = 5, Value = "C1", Lng = null, Description = "Fahrerlaubnis C1" },
                    new ApolloListItem { ListItemId = 6, Value = "L", Lng = null, Description = "Fahrerlaubnis L" },
                    new ApolloListItem { ListItemId = 7, Value = "AM", Lng = null, Description = "Fahrerlaubnis AM" },
                    new ApolloListItem { ListItemId = 8, Value = "A", Lng = null, Description = "Fahrerlaubnis A" },
                    new ApolloListItem { ListItemId = 9, Value = "CE", Lng = null, Description = "Fahrerlaubnis CE" },
                    new ApolloListItem { ListItemId = 10, Value = "C", Lng = null, Description = "Fahrerlaubnis C" },
                    new ApolloListItem { ListItemId = 11, Value = "A1", Lng = null, Description = "Fahrerlaubnis A1" },
                    new ApolloListItem { ListItemId = 12, Value = "B96", Lng = null, Description = "Fahrerlaubnis B96" },
                    new ApolloListItem { ListItemId = 13, Value = "T", Lng = null, Description = "Fahrerlaubnis T" },
                    new ApolloListItem { ListItemId = 14, Value = "A2", Lng = null, Description = "Fahrerlaubnis A2" },
                    new ApolloListItem { ListItemId = 15, Value = "Moped", Lng = null, Description = "Fahrerlaubnis Mofa und Krankenfahrstühle" },
                    new ApolloListItem { ListItemId = 16, Value = "Drivercard", Lng = null, Description = "Fahrerkarte" },
                    new ApolloListItem { ListItemId = 17, Value = "PassengerTransport", Lng = null, Description = "Fahrerlaubnis Fahrgastbeförderung" },
                    new ApolloListItem { ListItemId = 18, Value = "D", Lng = null, Description = "Fahrerlaubnis D" },
                    new ApolloListItem { ListItemId = 19, Value = "InstructorBE", Lng = null, Description = "Führerschein Baumaschinen" },
                    new ApolloListItem { ListItemId = 20, Value = "ConstructionMachines", Lng = null, Description = "Führerschein Baumaschinen" },
                    new ApolloListItem { ListItemId = 21, Value = "DE", Lng = null, Description = "Fahrerlaubnis DE" },
                    new ApolloListItem { ListItemId = 22, Value = "D1", Lng = null, Description = "Fahrerlaubnis D1" },
                    new ApolloListItem { ListItemId = 23, Value = "D1E", Lng = null, Description = "Fahrerlaubnis D1E" },
                    new ApolloListItem { ListItemId = 24, Value = "InstructorA", Lng = null, Description = "Fahrlehrerlaubnis Klasse A" },
                    new ApolloListItem { ListItemId = 25, Value = "InstructorCE", Lng = null, Description = "Fahrlehrerlaubnis Klasse CE" },
                    new ApolloListItem { ListItemId = 26, Value = "TrailerDriving", Lng = null, Description = "Gespannführerschein" },
                    new ApolloListItem { ListItemId = 27, Value = "InstructorDE", Lng = null, Description = "Fahrlehrerlaubnis Klasse DE" },
                    new ApolloListItem { ListItemId = 28, Value = "Class1", Lng = null, Description = "Lokomotiv-/Triebfahrzeugführerschein Klasse 1" },
                    new ApolloListItem { ListItemId = 29, Value = "Class3", Lng = null, Description = "Lokomotiv-/Triebfahrzeugführerschein Klasse 3" },
                    new ApolloListItem { ListItemId = 30, Value = "Class2", Lng = null, Description = "Lokomotiv-/Triebfahrzeugführerschein Klasse 2" },
                    new ApolloListItem { ListItemId = 31, Value = "InstructorASF", Lng = null, Description = "Seminarerlaubnis ASF" },
                    new ApolloListItem { ListItemId = 32, Value = "InstructorASP", Lng = null, Description = "Seminarerlaubnis ASP" }
                }
            };


            var id = await api.CreateOrUpdateListAsync(apolloList);

            var dbItem = await api.GetListAsync(id: id);

            Assert.IsNotNull(dbItem);
            Assert.IsTrue(dbItem.ItemType == nameof(DriversLicense));
            Assert.IsTrue(dbItem.Items.Count == 33);
        }


        private async Task PopulateEducationType()
        {
            var api = Helpers.GetApolloApi();

            ApolloList apolloList = new ApolloList()
            {
                Description = "Populate Education Type",
                ItemType = nameof(EducationType),
                Items = new List<ApolloListItem>
                {
                    new ApolloListItem { ListItemId = 0, Value = "Unknown", Lng = null, Description = null },
                    new ApolloListItem { ListItemId = 1, Value = "Education", Lng = null, Description = "Schulbildung" },
                    new ApolloListItem { ListItemId = 2, Value = "CompanyBasedVocationalTraining", Lng = null, Description = "Berufsausbildung (betr./außerbetr.)" },
                    new ApolloListItem { ListItemId = 3, Value = "Study", Lng = null, Description = "Studium" },
                    new ApolloListItem { ListItemId = 4, Value = "VocationalTraining", Lng = null, Description = "Berufsausbildung (schulisch)" },
                    new ApolloListItem { ListItemId = 5, Value = "FurtherEducation", Lng = null, Description = "Weiterbildung" }
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
                Items = new List<ApolloListItem>
                {
                    new ApolloListItem { ListItemId = 0, Value = "Unknown", Lng = null, Description = null },
                    new ApolloListItem { ListItemId = 1, Value = "A1", Lng = null, Description = null },
                    new ApolloListItem { ListItemId = 2, Value = "A2", Lng = null, Description = null },
                    new ApolloListItem { ListItemId = 3, Value = "B1", Lng = null, Description = null },
                    new ApolloListItem { ListItemId = 4, Value = "B2", Lng = null, Description = null },
                    new ApolloListItem { ListItemId = 5, Value = "C1", Lng = null, Description = null },
                    new ApolloListItem { ListItemId = 6, Value = "C2", Lng = null, Description = null }
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
                Items = new List<ApolloListItem>
                {
                    new ApolloListItem { ListItemId = 0, Value = "Unknown", Lng = null, Description = null },
                    new ApolloListItem { ListItemId = 1, Value = "UnregulatedNotRecognized", Lng = null, Description = "Nicht reglementierter, nicht anerkannter Abschluss" },
                    new ApolloListItem { ListItemId = 2, Value = "RegulatedNotRecognized", Lng = null, Description = "Reglementierter und nicht anerkannter Abschluss" },
                    new ApolloListItem { ListItemId = 3, Value = "Recognized", Lng = null, Description = "Anerkannter Abschluss" },
                    new ApolloListItem { ListItemId = 4, Value = "Pending", Lng = null, Description = "Anerkennung des Abschlusses wird geprüft" },
                    new ApolloListItem { ListItemId = 5, Value = "PartialRecognized", Lng = null, Description = "Teilweise anerkannter Abschluss" }
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
                Items = new List<ApolloListItem>
                {
                    new ApolloListItem { ListItemId = 0, Value = "Unknown", Lng = null, Description = null },
                    new ApolloListItem { ListItemId = 1, Value = "SecondarySchoolCertificate", Lng = null, Description = "Hauptschulabschluss" },
                    new ApolloListItem { ListItemId = 2, Value = "AdvancedTechnicalCollegeCertificate", Lng = null, Description = "Fachhochschulreife - allows to study at a university of applied sciences" },
                    new ApolloListItem { ListItemId = 3, Value = "HigherEducationEntranceQualificationALevel", Lng = null, Description = "Allgemeine Hochschulreife - allows to study at a university" },
                    new ApolloListItem { ListItemId = 4, Value = "IntermediateSchoolCertificate", Lng = null, Description = "Mittlere Reife / Mittlerer Bildungsabschluss" },
                    new ApolloListItem { ListItemId = 5, Value = "ExtendedSecondarySchoolLeavingCertificate", Lng = null, Description = "Qualifizierender / erweiterter Hauptschulabschluss" },
                    new ApolloListItem { ListItemId = 6, Value = "NoSchoolLeavingCertificate", Lng = null, Description = "kein Schulabschluss" },
                    new ApolloListItem { ListItemId = 7, Value = "SpecialSchoolLeavingCertificate", Lng = null, Description = "Schulabschluss der Förderschule" },
                    new ApolloListItem { ListItemId = 8, Value = "SubjectRelatedEntranceQualification", Lng = null, Description = "Fachgebundene Hochschulreife - allows to study at a university of applied sciences or in a specific subject area in a university" },
                    new ApolloListItem { ListItemId = 9, Value = "AdvancedTechnicalCollegeWithoutCertificate", Lng = null, Description = "Abgänger Klasse 11-13 ohne Abschluss" }
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
                Items = new List<ApolloListItem>
                {
                    new ApolloListItem { ListItemId = 0, Value = "Unknown", Lng = null, Description = null },
                    new ApolloListItem { ListItemId = 1, Value = "CivilianService", Lng = null, Description = "Zivildienst" },
                    new ApolloListItem { ListItemId = 2, Value = "MilitaryService", Lng = null, Description = "Grundwehrdienst" },
                    new ApolloListItem { ListItemId = 3, Value = "VoluntaryMilitaryService", Lng = null, Description = "Freiwilliger Wehrdienst" },
                    new ApolloListItem { ListItemId = 4, Value = "MilitaryExercise", Lng = null, Description = "Wehrübung" }
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
                Items = new List<ApolloListItem>
                {
                    new ApolloListItem { ListItemId = 0, Value = "Unknown", Lng = null, Description = null },
                    new ApolloListItem { ListItemId = 1, Value = "LessThan10", Lng = null, Description = "bis 9 Mitarbeiter/innen" },
                    new ApolloListItem { ListItemId = 2, Value = "Between10And49", Lng = null, Description = "10 - 49 Mitarbeiter/innen" },
                    new ApolloListItem { ListItemId = 3, Value = "Between50And499", Lng = null, Description = "50 - 499 Mitarbeiter/innen" },
                    new ApolloListItem { ListItemId = 4, Value = "MoreThan499", Lng = null, Description = "500 und mehr Mitarbeiter/innen" }
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

                    new ApolloListItem { ListItemId = 0, Value = "Unknown", Lng = null, Description = null },
                    new ApolloListItem { ListItemId = 1, Value = "Other", Lng = null, Description = "Sonstige Schule" },
                    new ApolloListItem { ListItemId = 2, Value = "HighSchool", Lng = null, Description = "Gymnasium" },
                    new ApolloListItem { ListItemId = 3, Value = "SecondarySchool", Lng = null, Description = "Realschule" },
                    new ApolloListItem { ListItemId = 4, Value = "VocationalCollege", Lng = null, Description = "Berufsfachschule" },
                    new ApolloListItem { ListItemId = 5, Value = "MainSchool", Lng = null, Description = "Hauptschule" },
                    new ApolloListItem { ListItemId = 6, Value = "VocationalHighSchool", Lng = null, Description = "Berufsoberschule/ Technische Oberschule" },
                    new ApolloListItem { ListItemId = 7, Value = "VocationalSchool", Lng = null, Description = "Berufsschule" },
                    new ApolloListItem { ListItemId = 8, Value = "SpecialSchool", Lng = null, Description = "Förderschule" },
                    new ApolloListItem { ListItemId = 9, Value = "IntegratedComprehensiveSchool", Lng = null, Description = "Integrierte Gesamtschule" },
                    new ApolloListItem { ListItemId = 10, Value = "SchoolWithMultipleCourses", Lng = null, Description = "Schulart mit mehreren Bildungsgängen" },
                    new ApolloListItem { ListItemId = 11, Value = "TechnicalCollege", Lng = null, Description = "Fachoberschule" },
                    new ApolloListItem { ListItemId = 12, Value = "TechnicalHighSchool", Lng = null, Description = "Fachgymnasium" },
                    new ApolloListItem { ListItemId = 13, Value = "TechnicalSchool", Lng = null, Description = "Fachschule" },
                    new ApolloListItem { ListItemId = 14, Value = "Colleague", Lng = null, Description = "Kolleg" },
                    new ApolloListItem { ListItemId = 15, Value = "EveningHighSchool", Lng = null, Description = "Abendgymnasium" },
                    new ApolloListItem { ListItemId = 16, Value = "VocationalTrainingSchool", Lng = null, Description = "Berufsaufbauschule" },
                    new ApolloListItem { ListItemId = 17, Value = "NightSchool", Lng = null, Description = "Abendrealschule" },
                    new ApolloListItem { ListItemId = 18, Value = "EveningSchool", Lng = null, Description = "Abendhauptschule" },
                    new ApolloListItem { ListItemId = 19, Value = "WaldorfSchool", Lng = null, Description = "Freie Waldorfschule" },
                    new ApolloListItem { ListItemId = 20, Value = "TechnicalAcademy", Lng = null, Description = "Fachakademie" },
                    new ApolloListItem { ListItemId = 21, Value = "UniversityOfAppliedScience", Lng = null, Description = "Hochschule für angewandte Wissenschaften" }
                }
            };

            var id = await api.CreateOrUpdateListAsync(apolloList);

            var dbItem = await api.GetListAsync(id: id);

            Assert.IsNotNull(dbItem);
            Assert.IsTrue(dbItem.ItemType == nameof(TypeOfSchool));
            Assert.IsTrue(dbItem.Items.Count == 22);
        }


        private async Task PopulateUniversityDegree()
        {
            var api = Helpers.GetApolloApi();

            ApolloList apolloList = new ApolloList()
            {
                Description = "International",
                ItemType = nameof(UniversityDegree),
                Items = new List<ApolloListItem>
                {
                   new ApolloListItem { ListItemId = 0, Value = "Unknown", Lng = null, Description = null },
                   new ApolloListItem { ListItemId = 1, Value = "Master", Lng = null, Description = "Master / Diplom / Magister" },
                   new ApolloListItem { ListItemId = 2, Value = "Bachelor", Lng = null, Description = "Bachelor" },
                   new ApolloListItem { ListItemId = 3, Value = "Pending", Lng = null, Description = "Anerkennung des Abschlusses wird geprüft" },
                   new ApolloListItem { ListItemId = 4, Value = "Doctorate", Lng = null, Description = "Promotion" },
                   new ApolloListItem { ListItemId = 5, Value = "StateExam", Lng = null, Description = "Staatsexamen" },
                   new ApolloListItem { ListItemId = 6, Value = "UnregulatedUnrecognized", Lng = null, Description = "Nicht reglementierter, nicht anerkannter Abschluss" },
                   new ApolloListItem { ListItemId = 7, Value = "RegulatedUnrecognized", Lng = null, Description = "Reglementierter und nicht anerkannter Abschluss" },
                   new ApolloListItem { ListItemId = 8, Value = "PartialRecognized", Lng = null, Description = "Teilweise anerkannter Abschluss" },
                   new ApolloListItem { ListItemId = 9, Value = "EcclesiasticalExam", Lng = null, Description = "Kirchliches Examen" },
                   new ApolloListItem { ListItemId = 10, Value = "Other", Lng = null, Description = "Nicht relevant" }
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
                Items = new List<ApolloListItem>
                {
                    new ApolloListItem { ListItemId = 0, Value = "Unknown", Lng = null, Description = null },
                    new ApolloListItem { ListItemId = 1, Value = "Other", Lng = null, Description = "Sonstiges" },
                    new ApolloListItem { ListItemId = 2, Value = "VoluntarySocialYear", Lng = null, Description = "Freiwilliges Soziales Jahr (FSJ)" },
                    new ApolloListItem { ListItemId = 3, Value = "FederalVolunteerService", Lng = null, Description = "Bundesfreiwilligendienst" },
                    new ApolloListItem { ListItemId = 4, Value = "VoluntaryEcologicalYear", Lng = null, Description = "Freiwilliges Ökologisches Jahr (FÖJ)" },
                    new ApolloListItem { ListItemId = 5, Value = "VoluntarySocialTrainingYear", Lng = null, Description = "Freiwilliges Soziales Trainingsjahr (FSTJ)" },
                    new ApolloListItem { ListItemId = 6, Value = "VoluntaryCulturalYear", Lng = null, Description = "Freiwilliges Kulturelles Jahr (FKJ)" },
                    new ApolloListItem { ListItemId = 7, Value = "VoluntarySocialYearInSport", Lng = null, Description = "Freiwilliges Soziales Jahr im Sport" },
                    new ApolloListItem { ListItemId = 8, Value = "VoluntaryYearInMonumentConservation", Lng = null, Description = "Freiwilliges Jahr in der Denkmalpflege (FJD)" }
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
                    new ApolloListItem { ListItemId = 0, Value = "Unknown", Lng = null, Description = null },
                    new ApolloListItem { ListItemId = 1, Value = "Yes", Lng = null, Description = "proved" },
                    new ApolloListItem { ListItemId = 2, Value = "No", Lng = null, Description = "not present" },
                    new ApolloListItem { ListItemId = 3, Value = "Partly", Lng = null, Description = "partial" }
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
                Items = new List<ApolloListItem>
                {
                    new ApolloListItem { ListItemId = 0, Value = "Unknown", Lng = null, Description = null },
                    new ApolloListItem { ListItemId = 1, Value = "LessThan2", Lng = null, Description = "bis 2 Jahre" },
                    new ApolloListItem { ListItemId = 2, Value = "Between2And5", Lng = null, Description = "2 bis 5 Jahre" },
                    new ApolloListItem { ListItemId = 3, Value = "MoreThan5", Lng = null, Description = "mehr als 5 Jahre" }
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
                Items = new List<ApolloListItem>
                {
                    new ApolloListItem { ListItemId = 0, Value = "Unknown", Lng = null, Description = null },
                    new ApolloListItem { ListItemId = 1, Value = "FULLTIME", Lng = null, Description = null },
                    new ApolloListItem { ListItemId = 2, Value = "PARTTIME", Lng = null, Description = null },
                    new ApolloListItem { ListItemId = 3, Value = "SHIFT_NIGHT_WORK_WEEKEND", Lng = null, Description = null },
                    new ApolloListItem { ListItemId = 4, Value = "MINIJOB", Lng = null, Description = null },
                    new ApolloListItem { ListItemId = 5, Value = "HOME_TELEWORK", Lng = null, Description = null }
                }
            };

            var id = await api.CreateOrUpdateListAsync(apolloList);

            var dbItem = await api.GetListAsync(id: id);
            Assert.IsNotNull(dbItem);
            Assert.IsTrue(dbItem.ItemType == nameof(WorkingTimeModel));
            Assert.IsTrue(dbItem.Items.Count == 6);
        }


        private async Task PopulateKnownLicense()
        {
            var api = Helpers.GetApolloApi();

            ApolloList apolloList = new ApolloList
            {
                Description = "Is Culture specific and prop a List sync",
                ItemType = nameof(License),
                Items = new List<ApolloListItem>
                {
                    new ApolloListItem { ListItemId = 0, Value = "Verwaltungsfachprüfung", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 1, Value = "Verwaltungsfachprüfung I", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 2, Value = "A-Patent mit Diplom", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 3, Value = "A-Patent ohne Diplom", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 5, Value = "Allg. Sprechfunkzeugnis für den Flugfunkdienst (AZF)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 6, Value = "Allgemeines Betriebszeugnis für Funker (GOC)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 7, Value = "Approbation", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 8, Value = "Arbeitsmedizinische Vorsorgeuntersuchung G 25 (Fahr-, Steuer- und Überwachungstätigkeiten)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 9, Value = "Arbeitsmedizinische Vorsorgeuntersuchung G 26.3 (Tragen von schwerem Atemschutzgerät)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 10, Value = "Arbeitsmedizinische Vorsorgeuntersuchung G 41 (Arbeiten mit Absturzgefahr)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 11, Value = "Arbeitssicherheit an Bahnanlagen (KoRil 406)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 12, Value = "Arbeitszugführung, Prüfung (Eisenbahn, Gleisbau)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 13, Value = "ATPL-A (Airline Transport Pilot Licence - Aircraft)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 15, Value = "Ausbildereignungsprüfung", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 16, Value = "Ausbildung von Luftfahrzeugführern im Instrumentenflug", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 17, Value = "Ausbildung zum Fluglehrer", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 18, Value = "Ausbildung zum Fluglehrer", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 19, Value = "B1-Lehrgang (Feuerwehr)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 20, Value = "Bahnübergangsposten, Prüfung (Eisenbahn, Gleisbau)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 21, Value = "Basic safety training (Seefahrt)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 22, Value = "Bedienerausweis für Hebebühnen/Hubarbeitsbühnen", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 23, Value = "Befähigung für Arbeiten unter Spannung (AuS)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 24, Value = "Befähigung nach §20 SprengG", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 25, Value = "Befähigung nach Berufskraftfahrer-Qualifikations-Gesetz (BKrFQG)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 26, Value = "Berechtigung Motorsäge/Freischneider", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 27, Value = "Beschränkt gültiges Betriebszeugnis für Funker (ROC)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 28, Value = "Beschränkt gültiges Sprechfunkzeugnis für den Flugfunkdienst (BZF I)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 29, Value = "Betriebsmittelprüfung gemäß DGUV", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 30, Value = "BF3-Berechtigung (Begleitfahrzeuge)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 31, Value = "Bodenseeschifferpatent", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 32, Value = "Bootsmannbrief", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 33, Value = "BOSIET - Basic Offshore Safety Induction Emergency Training", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 34, Value = "Brandschutzbeauftragte/r (Lehrgang)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 35, Value = "Bremsprobenberechtigung (Eisenbahn)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 36, Value = "Bridge Team Management", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 37, Value = "Brücken-Wachbefähigung (Wachgänger)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 38, Value = "C-Patent mit Diplom", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 40, Value = "C1-Lehrgang (Feuerwehr)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 41, Value = "C2-Lehrgang (Feuerwehr)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 43, Value = "CPL-A (Commercial Pilot Licence - Airplane)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 44, Value = "CPL-H (Commercial Pilot Licence - Helicopter)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 45, Value = "D-Patent mit Diplom", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 46, Value = "D1-Lehrgang (Feuerwehr)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 47, Value = "D2-Lehrgang (Feuerwehr)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 48, Value = "DGQ-Schein", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 49, Value = "Digitale Lernzertifikate", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 50, Value = "Drohnenführerschein (Kenntnisnachw. g. §§ 21a, 21b LuftVO)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 51, Value = "Elektro-Blechschweißer-Prüfung", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 52, Value = "Elektro-Schweißen Basisqualifikation", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 53, Value = "Elektronikpass", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 54, Value = "Emspatent", Lng = "de-de", Description = null },

                    // New Items: Mukit
                    new ApolloListItem { ListItemId = 55, Value = "Extended certificate of good conduct", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 56, Value = "Certified Specialist Lawyer (Examination)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 57, Value = "Specialist recognition", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 58, Value = "Expertise in airbags and belt tensioners", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 59, Value = "Specialist knowledge in occupational safety tree work", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 60, Value = "Specialist qualification in technical sterilization assistance", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 61, Value = "Certificate of expertise in ultrasound (NiSV)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 62, Value = "Specialist examination for road haulage (§ 5 GBZugV)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 63, Value = "Specialist examination for taxis and rental car traffic (§ 4 PBZugV)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 64, Value = "Fire Protection Certificate (Advanced Fire-Fighting)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 65, Value = "Fisherman's exam (fishing, fishing license)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 66, Value = "Air traffic licences/authorisations according to JAR-FCL", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 67, Value = "Release authorization CAT A", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 68, Value = "Release authorization CAT B1", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 69, Value = "CAT C Sharing Authorization", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 70, Value = "Certificate of good conduct", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 71, Value = "Radio Communications Certificate Inland Navigation", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 72, Value = "Gas Plant Welder Testing", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 73, Value = "Gas Sheet Welder Testing", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 74, Value = "Gas Pipe Welder Testing", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 75, Value = "Gas Welding Basic Qualification", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 76, Value = "Gastanker Certificate I", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 77, Value = "Gas Anchor Certificate II", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 78, Value = "Hazard voucher explosive substances/class 1 (ADR)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 79, Value = "Hazard voucher IATA-DGR, PK1 (sender)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 80, Value = "Hazard voucher IATA-DGR, PK10 (flight crew)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 81, Value = "Hazard voucher IATA-DGR, PK11 (flight attendant)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 82, Value = "Hazard voucher IATA-DGR, PK12 (Safetyh.contr. Passengers)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 83, Value = "Hazard voucher IATA-DGR, PK2 (packer)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 84, Value = "Hazard voucher IATA-DGR, PK3 (freight forwarding, dangerous goods)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 85, Value = "Hazard voucher IATA-DGR, PK4 (forwarding personnel, freight)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 86, Value = "Hazard voucher IATA-DGR, PK5 (Sped., load., warehouse, etc.)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 87, Value = "Hazard voucher IATA-DGR, PK6 (Vollschul., Luftverk.pers.)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 88, Value = "Hazard voucher IATA-DGR, PK7 (Luftverk.pers. Freight Departure.)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 89, Value = "Hazard voucher IATA-DGR, PK8 (air transport, lag., loading)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 90, Value = "Hazard voucher IATA-DGR, PK9 (passenger handling)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 91, Value = "Hazard voucher for radioactive substances/class 7 (ADR)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 92, Value = "Hazard voucher for general cargo (ADR)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 93, Value = "Hazard voucher for tankers (ADR)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 94, Value = "Guild Recognition", Lng = "de-de", Description = null },

                    new ApolloListItem { ListItemId = 95, Value = "GPL-TMG (Motor Glider Guidance)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 96, Value = "Valid U.S. visa", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 97, Value = "Hamburger Hafenpatent", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 98, Value = "HAZMAT (Dangerous Goods for Maritime Shipping)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 99, Value = "HUET - Helicopter Underwater Escape Training", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 100, Value = "IFR (Instrument Rating)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 101, Value = "Chamber of Industry and Commerce (IHK) Plant Security Specialist Examination", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 102, Value = "Intervention force (course according to VdS)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 103, Value = "Hunter exam (hunting license)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 104, Value = "Youth Leader Card (Juleica)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 105, Value = "Captain (ships of all sizes/worldwide)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 106, Value = "Cash Register Pass", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 107, Value = "Concert Exam", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 108, Value = "Crane licence", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 109, Value = "Aerobatic rating", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 110, Value = "Food lawl. knows. according to §4, Abs.1,4 Gaststättenges.", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 111, Value = "Apprentice welder, master welder (examination)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 112, Value = "Head of the machine system (any drive power)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 113, Value = "Long Range", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 114, Value = "MAG Plant Welder Testing", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 115, Value = "MAG Sheet Metal Welder Testing", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 116, Value = "MAG Pipe Welder Testing", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 117, Value = "MAG Welding Basic Qualification", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 118, Value = "Märkische Waterways", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 119, Value = "Machine certificate for woodworking machines (§ 12 ArbSchG)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 120, Value = "Machine Guard Qualification (Watchman)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 121, Value = "Machinist training (fire brigade)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 122, Value = "Matrosenbrief", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 123, Value = "Medical Care Certificate (Seefahrt)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 124, Value = "MIG Sheet Metal Welder Testing", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 125, Value = "MIG Pipe Welder Testing", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 126, Value = "MIG Welding Basic Qualification", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 127, Value = "Proof of tests according to DGZfP", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 128, Value = "Nautical Watch Officer (Ships of All Sizes/Worldwide)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 129, Value = "NK - Captain", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 130, Value = "North German Canals and Waterways", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 131, Value = "NVFR (Night Flight Rating)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 132, Value = "NVM - Full Sailor Deck", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 133, Value = "Patent AN/A-500 BRZ", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 134, Value = "Patent CNaut/C-750 kW", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 135, Value = "PE plastic welder test GW 330", Lng = "de-de", Description = null },

                    new ApolloListItem { ListItemId = 136, Value = "Personal certification DIN EN ISO/IEC 17024", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 137, Value = "Nursing permit according to § 43 SGB VIII", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 138, Value = "PPL-A/PPL-N (Private Aircraft Control)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 139, Value = "PPL-C/GPL (Glider Guidance)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 140, Value = "PPL-D (Free Balloon Guidance)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 141, Value = "PPL-H (Private Helicopter Guidance)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 142, Value = "private dance training", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 143, Value = "Proficiency in survival craft and fast rescue boats", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 144, Value = "Project management according to IPMA (GPM)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 145, Value = "Project management according to PMI", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 146, Value = "Project management according to PRINCE2", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 147, Value = "Inspection authorization Safety Testing (SP) Truck", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 148, Value = "Testing Licence Fachr. Electronics", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 149, Value = "Testing in accordance with the German Ordinance on Industrial Safety and Health (BetrSichV)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 150, Value = "Qualification for medical treatment care according to SGB V/XI", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 151, Value = "Radarpatent", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 152, Value = "Radar Simulator Course (ARPA)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 153, Value = "Shunting supervision, testing (railway, track construction)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 154, Value = "REGALPRÜFUNG DIN EN 15635", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 155, Value = "Lifeboatman's licence", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 156, Value = "Rescue Service - Instructor Course", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 157, Value = "Ro-Ro passenger ship safety training", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 158, Value = "X-ray certificate (proof of knowledge according to § 145 StrlSchV)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 159, Value = "Proof of expertise in accordance with § 83 of the Medical Device Law Implementation Act", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 160, Value = "Sachkenntnisnachweis nach Arzneimittelgesetz (§§ 75,76 AMG)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 161, Value = "Sachkunde Ladungssicherung auf Straßenfahrzeugen (VDI 2700)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 162, Value = "Sachkunde Laserschutz (Laserschutzbeauftragte, §6, DGUV)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 163, Value = "Sachkunde nach §§ 15, 16 BetrSichV (Aufzugwärter)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 164, Value = "Sachkunde zur Prüfung von Flüssiggasanlagen (DVGW G 607)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 165, Value = "Sachkundenachweis FELASA - Versuchstierkunde/Tierexperimente", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 166, Value = "Sachkundenachweis freiverkäufliche Arzneimittel (§ 50 AMG)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 167, Value = "Sachkundenachweis für Sicherheit und Gesundheitsschutz nach DGUV", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 168, Value = "Sachkundenachweis nach § 11 Chemikalien-Verbotsverordnung", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 169, Value = "Sachkundenachweis nach § 11 Tierschutzgesetz", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 170, Value = "Sachkundenachweis nach § 28 Gentechnik-Sicherheitsverordnung", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 171, Value = "Sachkundenachweis nach § 5 Chemikalien-Klimaschutzverordnung", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 172, Value = "Sachkundenachweis nach Pflanzenschutz-Sachkundeverordnung", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 173, Value = "Sachkundenachweis TRGS 519 (Asbestentsorgung)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 174, Value = "Sachkundeprüfung Hundeverordnung", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 175, Value = "Sachkundeprüfung nach § 34a GewO (Bewachung)", Lng = "de-de", Description = null },

                    new ApolloListItem { ListItemId = 176, Value = "Sachkundeprüfung nach § 34d GewO (Versicherungsvermittlung)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 177, Value = "Sachkundeprüfung nach § 34i GewO (Immobiliardarlehensvermittlung)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 178, Value = "Sachkundeprüfung nach § 7 Waffengesetz", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 179, Value = "SCC-Zertifikat (Sicherheits-Certifikat-Contractoren)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 180, Value = "Schaltberechtigung (Mittelspannung bis 30 kV)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 181, Value = "Schiffe aller Größen/weltweit", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 182, Value = "Schifferpatent A", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 183, Value = "Schifferpatent B", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 184, Value = "Schifferpatent F (Fährschein)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 185, Value = "Schiffsmaschinist (Antriebsleistung bis zu 750 KW)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 186, Value = "Schiffsmechaniker-Brief (Seefahrt)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 187, Value = "Schiffssicherheit (SSO)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 188, Value = "Schleppberechtigung (Flugverkehr)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 189, Value = "Schusswaffenlizenz (Waffenschein)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 190, Value = "Schweißerprüfung DIN EN 287-1, DIN EN ISO 9606-1 (Stähle)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 191, Value = "Schweißerprüfung DIN EN 287-2, DIN EN ISO 9606-2 (Aluminium)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 192, Value = "Schweißfachingenieur/in (Prüfung)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 193, Value = "Schweißfachmann/-frau (Prüfung)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 194, Value = "Schweißtechniker/in (Prüfung)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 195, Value = "Security Related Training", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 196, Value = "Seediensttauglichkeit", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 197, Value = "Seefunkzeugnis 1. Klasse", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 198, Value = "Seefunkzeugnis 2. Klasse", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 199, Value = "Shiphandling-Certificate (SUSAN)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 200, Value = "Sicherheitsbestimmungen/ADNR", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 201, Value = "Sicherheitskontrollen nach Luftsicherheitsgesetz (LuftSiG)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 202, Value = "Sicherheitslehrgang", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 203, Value = "Sicherungsaufsicht, Prüfung (Eisenbahn, Gleisbau)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 204, Value = "Sicherungsposten, Prüfung (Eisenbahn, Gleisbau)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 205, Value = "SIVV-Schein (Sichern, Instandsetzen, Verbinden, Verstärken)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 206, Value = "Sportbootführerschein Binnen", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 207, Value = "Sportbootführerschein See", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 208, Value = "Sporthochseeschifferschein", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 209, Value = "Sportküstenschifferschein", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 210, Value = "Sportseeschifferschein", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 211, Value = "Sprachlehrzertifikat TESOL", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 212, Value = "Sprechfunkzeugnis BZF I (englisch, deutsch)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 213, Value = "Sprechfunkzeugnis BZF II (deutsch)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 214, Value = "Staatliche Anerkennung", Lng = "de-de", Description = null },

                    new ApolloListItem { ListItemId = 215, Value = "Staatliche Prüfung", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 216, Value = "Strahlenschutz - Lehrgang", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 217, Value = "Streu- und Sprühberechtigung (Flugzeug)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 218, Value = "Tankergrundkurs (Dienst auf Tankschiffen - Grundausbildung)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 219, Value = "Tanzpädagogikzertifikat (Privatschule)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 220, Value = "Technischer Wachoffizier", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 221, Value = "Tischler-/Schreiner-Maschinenlehrgang (TSM)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 222, Value = "TLM - Leiter der Maschinenanlage", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 223, Value = "Trainer-/Moderatorenberechtigung Fahrsicherheitstraining", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 224, Value = "Trainings-, Einweisungsberechtigung (Flugverkehr)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 225, Value = "TWO - Technischer Wachoffizier", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 226, Value = "Unterrichtungsnachweis der IHK (§ 34a GewO) (Bewachung)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 227, Value = "Vorlageberechtigung", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 228, Value = "Wagenprüfer, Prüfung (Eisenbahn)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 229, Value = "Wägerprüfung", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 230, Value = "WIG-Anlagenschweißer-Prüfung", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 231, Value = "WIG-Blechschweißer-Prüfung", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 232, Value = "WIG-Rohrschweißer-Prüfung", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 233, Value = "WIG-Schweißen Basisqualifikation", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 234, Value = "Zuverlässigkeitsüberprüfung nach § 7 Luftsicherheitsgesetz (LuftSiG)", Lng = "de-de", Description = null },
                    new ApolloListItem { ListItemId = 235, Value = "Zweiter technischer Offizier", Lng = "de-de", Description = null }
        }
    };

            var id = await api.CreateOrUpdateListAsync(apolloList);

            var dbItem = await api.GetListAsync(id: id);
            Assert.IsNotNull(dbItem);
            Assert.IsTrue(dbItem.ItemType == nameof(License));
          // Assert.IsTrue(dbItem.Items.Count == 6);
        }
        #endregion
    }
}

