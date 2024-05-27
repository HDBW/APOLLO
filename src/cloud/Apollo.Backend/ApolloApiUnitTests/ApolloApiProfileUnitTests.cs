// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.


using System.Collections;
using System.Globalization;
using System.Threading.Channels;
using Amazon.Runtime.Internal.Transform;
using Apollo.Api;
using Apollo.Api.UnitTests;
using Apollo.Common.Entities;
using Daenet.MongoDal;
using Daenet.MongoDal.Entitties;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using SharpCompress.Common;

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


        /// <summary>
        /// Test method to import profiles from JSON files using producer/consumer pattern.
        /// </summary>
        [TestMethod]
        public async Task ImportProfilesFromJsonFilesTest()
        {
            int MaxConsumers = 2;
            var jsonFolderPath = @"C:\Users\MukitKhan\Videos\DATA";
            var filePath = @"C:\Users\MukitKhan\Documents";
            var api = Helpers.GetApolloApi();
            var jsonFiles = Directory.GetFiles(jsonFolderPath, "*.json");

            // Create a bounded channel with a capacity of 1000
            var fileChannel = Channel.CreateBounded<string>(new BoundedChannelOptions(5)
            {
                FullMode = BoundedChannelFullMode.Wait
            });

            var consumerTasks = new List<Task>();

            // Producer Task: Writes file paths to the channel
            var producerTask = Task.Run(async () =>
            {
                foreach (var jsonFile in jsonFiles)
                {
                    await fileChannel.Writer.WriteAsync(jsonFile);
                }
                fileChannel.Writer.Complete();
            });

            // Consumer Tasks: Read file paths from the channel and process them
            for (int i = 0; i < MaxConsumers; i++)
            {
                consumerTasks.Add(Task.Run(async () =>
                {
                    await foreach (var jsonFile in fileChannel.Reader.ReadAllAsync())
                    {
                        try
                        {
                            var fileInfo = new FileInfo(jsonFile);
                            if (fileInfo.Length <= 1024)
                            {
                                // Log or handle small file scenario as needed
                                Console.WriteLine($"File {jsonFile} is smaller than 1024 bytes and will be skipped.");
                                continue;
                            }

                            var jsonData = await File.ReadAllTextAsync(jsonFile);
                            var profile = MapJsonToProfile(jsonData);
                            // var profile =  JsonConvert.DeserializeObject<Profile>(jsonData);

                            var jsonDataSave = JsonConvert.SerializeObject(profile);
                            File.WriteAllText(filePath, jsonDataSave);

                            var  result = await api.CreateOrUpdateBAProfileAsync(profile);

                        }
                        catch (Exception ex)
                        {
                            // Log the error
                            Console.WriteLine($"Error processing file {jsonFile}: {ex}");
                        }
                    }
                }));
            }

            // Wait for the producer and all consumers to complete
            await producerTask;
            await Task.WhenAll(consumerTasks);
        }

        private Profile MapJsonToProfile(string jsonData)
        {
            dynamic? jsonObject = JsonConvert.DeserializeObject(jsonData);

            var apolloProfile = new Profile
            {
                Id = null,
                CareerInfos = jsonObject?.werdegang != null ? MapCareerInfo((IEnumerable<dynamic>)jsonObject.werdegang) : null,
                Occupations = jsonObject?.berufe != null ? MapOccupation((IEnumerable<dynamic>)jsonObject.berufe) : null,
                EducationInfos = jsonObject?.bildung != null ? MapEducationInfo((IEnumerable<dynamic>)jsonObject.bildung) : null,
                Qualifications = jsonObject?.qualifikationen != null ? MapQualification((IEnumerable<dynamic>)jsonObject.qualifikationen) : null,
                MobilityInfo = jsonObject?.mobilitaet != null ? MapMobilityInfo(jsonObject.mobilitaet) : null,
                LanguageSkills = jsonObject?.sprachkenntnisse != null ? MapLanguageSkills(jsonObject.sprachkenntnisse) : null,
                Skills = jsonObject?.kenntnisse != null ? MapSkills(jsonObject.kenntnisse):null,
                WebReferences = new List<WebReference>{  new WebReference(){
                Title = "BA DataSet From Json File"
                    } }
            };

            return apolloProfile;
        }

        private List<CareerInfo> MapCareerInfo(IEnumerable<dynamic> items)
        {
            return items.Select(item => new CareerInfo
            {
                Description = item?.berufsbezeichnung,
                Start = item?.Von,
                End = item?.EndDate,
                Job = new Occupation()
                {
                    Description = item?.berufsbezeichnung,
                    UniqueIdentifier = null,
                    OccupationUri = null,
                    ClassificationCode = item?.lebenslaufart,
                    Identifier = item?.lebenslaufartenKategorie,
                    Concept = null,
                    RegulatoryAspect = "",
                    HasApprenticeShip = false,
                    IsUniversityOccupation = false,
                    IsUniversityDegree = true,
                    PreferedTerm = [],
                    NonePreferedTerm = [ ],
                    TaxonomyInfo = Taxonomy.Unknown,
                    TaxonomieVersion= "",
                    CultureString = "de-DE",
                    BroaderConcepts =  [ ],
                    NarrowerConcepts = [ ],
                    RelatedConcepts = [ ],
                    Skills = [ ],
                    EssentialSkills = [ ],
                    OptionalSkills = [ ],
                    EssentialKnowledge = [ ],
                    OptionalKnowledge = [ ],
                    Documents = [],
                    OccupationGroup = { },
                    DkzApprenticeship =true,
                    QualifiedProfessional = true,
                    NeedsUniversityDegree = true,
                    IsMilitaryApprenticeship = false,
                    IsGovernmentApprenticeship = false,
                    //ValidFrom = item?.Von,
                    //ValidTill = item?.EndDate,
                }
            }).ToList();
        }

        private List<Occupation> MapOccupation(IEnumerable<dynamic> items)
        {
            var data = items.Select(item => new Occupation
            {
                Description = item.ToString()
            }).ToList();
            return data;
        }


        private List<EducationInfo> MapEducationInfo(IEnumerable<dynamic> items)
        {
            var data = items.Select(item => new EducationInfo
            {
                Start = item?.von,
                End = item?.bis,
                Country = item?.land,
                Description = item?.beschreibung,
                ProfessionalTitle = new Occupation
                {
                    Description = item?.berufsbezeichnung
                },
                CompletionState = item?.istAbgeschlossen != null ?  new CompletionState { ListItemId = 1, Value = item.istAbgeschlossen }: null, 
                Graduation = item?.schulAbschluss != null ? new SchoolGraduation { ListItemId = 1, Value = item.schulAbschluss }: null, 
                UniversityDegree = item?.hochSchulAbschluss !=null ? new UniversityDegree { ListItemId = 1, Value = item.hochSchulAbschluss }: null,
                TypeOfSchool =  item?.schulart != null ? new TypeOfSchool { ListItemId =1 , Value = item.schulart } : null,
                NameOfInstitution = item?.nameArtEinrichtung,
                City = item?.ort,
                Recognition = item?.anerkennungAbschluss!=null ? new RecognitionType { ListItemId = 1, Value = item.anerkennungAbschluss }: null
            }).ToList();

            return data;
        }

        private List<Qualification> MapQualification(IEnumerable<dynamic> items)
        {
            var data =  items.Select(item => new Qualification
            {
                Name =  item?.qualifikationsName,
                Description = item?.beschreibung, 
                IssueDate = item?.ausstellDatum, 
                ExpirationDate = item?.ablaufDatum,
                IssuingAuthority = null
            }).ToList();
            return data;
        }


        private Mobility MapMobilityInfo(dynamic item)
        {
            var data = new Mobility
            {
                WillingToTravel = item?.reisebereitschaft!=null ? new Willing { ListItemId = 1, Value = item.reisebereitschaft }:null,
                DriverLicenses = item?.DriversLicense != null ? new List<DriversLicense>{new DriversLicense { ListItemId = 1, Value = item.fuehrerscheine }}:null,
                HasVehicle = item?.fahrzeugVorhanden 
            };
            return data;
        }


        private List<LanguageSkill> MapLanguageSkills(dynamic sprachkenntnisse)
        {
           
            var languageSkills = new List<LanguageSkill>();
            int listItemIdCounter = 1;

            // Map Verhandlungssicher languages
            if (sprachkenntnisse?.Verhandlungssicher != null)
            {
                foreach (var language in sprachkenntnisse.Verhandlungssicher)
                {
                    var p = language.Value;
                    languageSkills.Add(new LanguageSkill
                    {
                        Name = language.Value,
                        Code = GetCultureName(language.Value),
                        Niveau = new LanguageNiveau { ListItemId = listItemIdCounter++, Value = "Verhandlungssicher" },
                    });
                }
            }

            // Map Erweiterte Kenntnisse languages
            if (sprachkenntnisse?.ErweiterteKenntnisse != null)
            {
                foreach (var language in sprachkenntnisse.ErweiterteKenntnisse)
                {
                    languageSkills.Add(new LanguageSkill
                    {
                        Name = language.Value,
                        Code = GetCultureName(language.Value),
                        Niveau = new LanguageNiveau { ListItemId = listItemIdCounter++, Value = "Erweiterte Kenntnisse" },
                    });
                }
            }

            // Map Grundkenntnisse languages
            if (sprachkenntnisse?.Grundkenntnisse != null)
            {
                foreach (var language in sprachkenntnisse.Grundkenntnisse)
                {
                    languageSkills.Add(new LanguageSkill
                    {
                        Name = language.Value,
                        Code = GetCultureName(language.Value),
                        Niveau = new LanguageNiveau { ListItemId = listItemIdCounter++, Value = "Grundkenntnisse" },
                    });
                }
            }

            return languageSkills;
        }


        private List<Skill> MapSkills(dynamic items)
        {
            var skills = new List<Skill>();

            // Map Verhandlungssicher languages
            if (items?.ErweiterteKenntnisse != null)
            {
                foreach (var language in items.ErweiterteKenntnisse)
                {
                    skills.Add(new Skill
                    {
                        Title =  language.Value ,
                        ScopeNote = "ErweiterteKenntnisse"
                    });
                }
            }

            // Map Verhandlungssicher languages
            if (items?.Grundkenntnisse != null)
            {
                foreach (var language in items.Grundkenntnisse)
                {
                    skills.Add(new Skill
                    {
                        Title = language.Value,
                        ScopeNote = "Grundkenntnisse"
                    });
                }
            }

            // Map Verhandlungssicher languages
            if (items?.Expertenkenntnisse != null)
            {
                foreach (var language in items.Expertenkenntnisse)
                {
                    skills.Add(new Skill
                    {
                        Title = language.Value,
                        ScopeNote = "Expertenkenntnisse"
                    });
                }
            }
            return skills;


        }

        string? GetCultureName(string language)
        {
            string? isoCode = GetISOCode(language);
            CultureInfo? cultureInfo = isoCode!=null ? GetCultureInfoFromISOCode(isoCode) : null;
            return cultureInfo?.Name;

        }

        CultureInfo? GetCultureInfoFromISOCode(string isoCode)
        {
            try
            {
                return CultureInfo.GetCultureInfoByIetfLanguageTag(isoCode);
            }
            catch (CultureNotFoundException)
            {
                return null;
            }
        }

        string? GetISOCode(string languageName)
        {
            if (languageCodesIso6392.ContainsKey(languageName.ToLower()))
            {
                return languageCodesIso6392[languageName.ToLower()];
            }
            else
            {
                return null;
            }
        }

        Dictionary<string, string> languageCodesIso6392 = new Dictionary<string, string>
        {
            {"afrikaans", "afr"},
            {"akan/twi (westafrika)", "aka"},
            {"akzent", "deu"},
            {"albanisch", "alb"},
            {"amharisch (äthiopien)", "amh"},
            {"arabisch", "ara"},
            {"aramäisch/syrisch", "arc"},
            {"armenisch", "arm"},
            {"aserbaidschanisch", "aze"},
            {"bambara (westafrika, mali)", "bam"},
            {"bengali", "ben"},
            {"berberisch (tamazight)", "ber"},
            {"birmanisch", "bur"},
            {"bosnisch", "bos"},
            {"bulgarisch", "bul"},
            {"chinesisch", "chi"},
            {"deutsch", "deu"},
            // German dialects
            {"deutscher dialekt (alemannisch/schweizerdeutsch)", "gsw"},
            {"deutscher dialekt (allgäuisch)", "deu"},
            {"deutscher dialekt (badisch)", "deu"},
            {"deutscher dialekt (bayerisch)", "deu"},
            {"deutscher dialekt (bergisch)", "deu"},
            {"deutscher dialekt (berlinerisch)", "deu"},
            {"deutscher dialekt (böhmisch)", "deu"},
            {"deutscher dialekt (elsässisch)", "deu"},
            {"deutscher dialekt (erzgebirgisch)", "deu"},
            {"deutscher dialekt (fränkisch)", "deu"},
            {"deutscher dialekt (hallenser)", "deu"},
            {"deutscher dialekt (hamburgisch)", "deu"},
            {"deutscher dialekt (harzer)", "deu"},
            {"deutscher dialekt (hessisch)", "deu"},
            {"deutscher dialekt (hochdeutsch)",  "deu"},
            {"deutscher dialekt (jiddisch)",  "deu"},
            {"deutscher dialekt (koelsch)",  "deu"},
            {"deutscher dialekt (kärtnerisch)",  "deu"},
            {"deutscher dialekt (lausitzer)",  "deu"},
            {"deutscher dialekt (magdeburger)",  "deu"},
            {"deutscher dialekt (mannheimer)",  "deu"},
            {"deutscher dialekt (mecklenburgisch)",  "deu"},
            {"deutscher dialekt (norddeutsch)",  "deu"},
            {"deutscher dialekt (ostpreußisch)",  "deu"},
            {"deutscher dialekt (pfälzisch)", "deu"},
            {"deutscher dialekt (plattdeutsch)", "nds"},
            {"deutscher dialekt (pommerisch)", "pom"},
            {"deutscher dialekt (rheinisch)", "ksh"},
            {"deutscher dialekt (ruhrgebiet)", "ksh"},
            {"deutscher dialekt (saarländisch)", "sli"},
            {"deutscher dialekt (schlesisch)", "sli"},
            {"deutscher dialekt (schwäbisch)", "swg"},
            {"deutscher dialekt (steirisch)", "slv"},
            {"deutscher dialekt (sächsisch)", "sxu"},
            {"deutscher dialekt (thüringisch)", "gmh"},
            {"deutscher dialekt (tirolerisch)", "bar"},
            {"deutscher dialekt (vogtländisch)", "gmh"},
            {"deutscher dialekt (westfälisch)", "ksh"},
            {"deutscher dialekt (wienerisch)", "bar"},
            {"deutscher dialekt (österreich)", "bar"},
            {"dänisch", "dan"},
            {"englisch", "eng"},
            {"englisch (amerikanisches englisch)", "ena"},
            {"englisch (technisches englisch)", "eng"},
            {"englisch (wirtschafts-, businessenglisch)", "eng"},
            {"esperanto", "epo"},
            {"estnisch", "est"},
            {"filipino (tagalog)", "fil"},
            {"finnisch", "fin"},
            {"französisch", "fre"},
            {"friesisch", "fry"},
            {"färöisch", "fao"},
            {"gebärdensprache/unterstützte kommunikation (uk)", "sgn"},
            {"georgisch", "geo"},
            {"griechisch", "gre"},
            {"gujarati", "guj"},
            {"gälisch/irisch", "gla"},
            {"hausa (westafrika)", "hau"},
            {"hebräisch", "heb"},
            {"hindi", "hin"},
            {"hochdeutsch (dialektfrei)", "ger"},
            {"igbo", "ibo"},
            {"indonesisch", "ind"},
            {"isländisch", "ice"},
            {"italienisch", "ita"},
            {"jamaika-kreolisch (patois)", "jam"},
            {"japanisch", "jpn"},
            {"jiddisch", "yid"},
            {"kambodschanisch (khmer)", "khm"},
            {"kasachisch", "kaz"},
            {"katalanisch", "cat"},
            {"kirgisisch", "kir"},
            {"koreanisch", "kor"},
            {"kroatisch", "hrv"},
            {"kurdisch", "kur"},
            {"laotisch", "lao"},
            {"latein", "lat"},
            {"lettisch", "lav"},
            {"letzeburgisch (luxemburg)", "ltz"},
            {"litauisch", "lit"},
            {"malayalam", "mal"},
            {"malaysisch", "may"},
            {"marathi", "mar"},
            {"mazedonisch", "mac"}, 
            {"mongolisch", "mon"},
            {"nepalesisch", "nep"},
            {"niederdeutsch/plattdeutsch", "nds"},
            {"niederländisch/flämisch", "dut"},
            {"norwegisch", "nor"},
            {"oromo", "orm"},
            {"pakistanisch/urdu", "urd"},
            {"paschtu/paschto/pashto (afghanistan)", "pus"},
            {"persisch/dari/farsi", "per"},
            {"polnisch", "pol"},
            {"portugiesisch", "por"},
            {"punjabi", "pan"},
            {"rechtschreib- und grammatiksicherheit (deutsch)", "de"},
            {"romani", "rom"},
            {"rumänisch", "rum"},
            {"russisch", "rus"},
            {"schwedisch", "swe"},
            {"serbisch", "srp"},
            {"singhalesisch", "sin"},
            {"slowakisch", "slo"},
            {"slowenisch", "slv"},
            {"somali", "som"},
            {"sorbisch/wendisch", "wen"},
            {"spanisch", "spa"},
            {"suaheli (ostafrika)", "swa"},
            {"tadschikisch", "tgk"},
            {"taiwanisch", "tai"},
            {"tamil", "tam"},
            {"telugu", "tel"},
            {"thai", "tha"},
            {"tibetisch", "tib"},
            {"tigrinja (eritrea)", "tir"},
            {"tschechisch", "cze"},
            {"tschetschenisch", "che"},
            {"türkisch", "tur"},
            {"uigurisch", "uig"},
            {"ukrainisch", "ukr"},
            {"ungarisch", "hun"},
            {"usbekisch", "uzb"},
            {"vietnamesisch", "vie"},
            {"weißrussisch", "bel"},
            {"yoruba (westafrika)", "yor"}
        };


    }

}
