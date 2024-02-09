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
        #endregion

    }
}

