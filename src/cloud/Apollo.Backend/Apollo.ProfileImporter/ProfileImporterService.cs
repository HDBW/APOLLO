using Apollo.Api;
using Apollo.Common.Entities;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json;
using System.Dynamic;
using System.Globalization;
using System.Threading;
using System.Threading.Channels;
using System.Diagnostics;


namespace Apollo.ProfileImporter
{

    /// <summary>
    /// Imports apolloprofiles from blob storage, maps them to the the profile including skills property and upsert them into the
    /// Apollo backend.
    /// </summary>
    /// <param name="args"></param>
    public class ProfileImporterService : IHostedService
    {
        private readonly ApolloApi _api;
        private readonly ILogger<ProfileImporterService> _logger;
        private readonly IConfiguration _configuration;


        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileImporterService"/> class.
        /// </summary>
        /// <param name="api">The instance of Apollo API to interact with the Apollo backend.</param>
        /// <param name="logger">The logger used for logging information, warnings, and errors.</param>
        /// <param name="configuration">The configuration holder for accessing configuration like connection strings and container names.</param>
        public ProfileImporterService(ApolloApi api, ILogger<ProfileImporterService> logger, IConfiguration configuration)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        /// <summary>
        /// Starts the profile import process as part of the hosted service lifecycle.
        /// </summary>
        /// <param name="cancellationToken">A token for stopping the service.</param>
        /// <returns>A Task that represents the asynchronous operation of starting the service.</returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Import of apolloprofiles started");

            try
            {
                var blobConnectionString = _configuration["BlobStorage:ConnectionString"];
                var containerName = _configuration["BlobStorage:ContainerName"];
                var maxConsumers = _configuration.GetValue<int>("MaxConsumers");

                await ImportProfilesFromBlobStorage(blobConnectionString, containerName, maxConsumers);

                _logger.LogInformation("Import of apolloprofiles completed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the import process");
            }
        }


        /// <summary>
        /// Stops the profile import service. This method is called when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">A token for stopping the service.</param>
        /// <returns>A Task that represents the completion of asynchronous operations.</returns>
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;


        /// <summary>
        /// Handles the actual import of apolloprofiles from Azure Blob Storage by setting up a producer-consumer scenario
        /// where blobs are read and processed by multiple consumer tasks.
        /// </summary>
        /// <param name="blobConnectionString">The connection string to Azure Blob Storage.</param>
        /// <param name="containerName">The name of the Blob Storage container holding the profile data.</param>
        /// <param name="maxConsumers">The maximum number of consumer tasks to process blobs.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        private async Task ImportProfilesFromBlobStorage(string blobConnectionString, string containerName, int maxConsumers)
        {
            BlobContainerClient containerClient = new BlobContainerClient(blobConnectionString, containerName);
            var blobItems = containerClient.GetBlobsAsync();

            var fileChannel = Channel.CreateBounded<BlobItem>(new BoundedChannelOptions(1000)
            {
                FullMode = BoundedChannelFullMode.Wait
            });

            var consumerTasks = new List<Task>();
            int processedCount = 0;
            int skippedCount = 0;
            object lockObj = new object();

            var startTime = DateTime.Now; // Capture start time for duration calculation

            var producerTask = Task.Run(async () =>
            {
                await foreach (var blobItem in blobItems)
                {
                    await fileChannel.Writer.WriteAsync(blobItem);
                    _logger.LogInformation($"Enqueued blob: {blobItem.Name} for processing.");
                }
                fileChannel.Writer.Complete();
            });

            for (int i = 0; i < maxConsumers; i++)
            {
                consumerTasks.Add(Task.Run(async () =>
                {
                    await foreach (var blobItem in fileChannel.Reader.ReadAllAsync())
                    {
                        try
                        {
                            _logger.LogInformation($"Starting to process blob: {blobItem.Name}");

                            var blobClient = containerClient.GetBlobClient(blobItem.Name);
                            var blobProperties = await blobClient.GetPropertiesAsync();

                            if (blobProperties.Value.ContentLength <= 1024)
                            {
                                _logger.LogWarning($"Blob {blobItem.Name} is smaller than 1024 bytes and will be skipped.");
                                lock (lockObj)
                                {
                                    skippedCount++;
                                }
                                continue;
                            }

                            var downloadResponse = await blobClient.DownloadContentAsync();
                            var jsonData = downloadResponse.Value.Content.ToString();

                            _logger.LogInformation($"Downloaded blob: {blobItem.Name} successfully. Size: {blobProperties.Value.ContentLength} bytes");

                            var profile = MapJsonToProfile(jsonData);

                            //// Log the profile object with indented formatting for better readability
                            //_logger.LogInformation($"Profile object mapped: {JsonConvert.SerializeObject(profile, Formatting.Indented)}");

                            lock (lockObj)
                            {
                                processedCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Error processing blob {blobItem.Name}");
                        }
                    }
                }));
            }

            await producerTask;
            await Task.WhenAll(consumerTasks);

            var endTime = DateTime.Now; // Capture end time
            var elapsedTime = endTime - startTime; // Calculate the duration

            _logger.LogInformation($"Import of apolloprofiles completed. Total processed: {processedCount}, Total skipped: {skippedCount}, Total time taken: {elapsedTime}");
        }


        // <summary>
        /// Maps JSON data to a Profile object. This involves parsing the JSON and converting it into an instance of a Profile.
        /// </summary>
        /// <param name="jsonData">The JSON string representing the profile data.</param>
        /// <returns>The mapped Profile object.</returns>
        public List<Profile> MapJsonToProfile(string jsonData)
        {
            try
            {
                dynamic jsonObject = JsonConvert.DeserializeObject(jsonData);
                var apolloprofiles = new List<Profile>();

                if (jsonObject?.bewerber != null)
                {
                    foreach (var bewerber in jsonObject.bewerber)
                    {
                        var skills = MapSkills(bewerber);
                        var apolloProfile = new Profile
                        {
                            Id = bewerber?.refnr,
                            Skills = skills,
                           
                            CareerInfos = MapCareerInfo(bewerber),
                            Occupations = MapOccupations(bewerber),
                            EducationInfos = MapEducationInfo(bewerber),
                            Qualifications = MapQualification(bewerber),
                            MobilityInfo = MapMobilityInfo(bewerber),
                            LanguageSkills = MapLanguageSkills(bewerber),
                            WebReferences = MapWebReferences(bewerber)
                        };

                        apolloprofiles.Add(apolloProfile);
                    }
                }

                return apolloprofiles;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.Message);
                throw;  
            }
        }


        /// <summary>
        /// Maps career information from a dynamic object representing a single bewerber.
        /// This method extracts career-related data and transforms it into a list of CareerInfo objects.
        /// </summary>
        /// <param name="bewerber">Dynamic object containing career data for a profile.</param>
        /// <returns>List of CareerInfo objects.</returns>
        private List<CareerInfo> MapCareerInfo(dynamic bewerber)
        {
            var careerInfos = new List<CareerInfo>();

            if (bewerber?.letzteTaetigkeit != null)
            {
                var letzteTaetigkeit = bewerber.letzteTaetigkeit;

                var careerInfo = new CareerInfo
                {
                    Description = (string)letzteTaetigkeit["bezeichnung"],
                    Start = bewerber?.verfuegbarkeitVon != null
                        ? ParseDate((string)bewerber.verfuegbarkeitVon.ToString()) // Ensure it's a string
                        : (DateTime?)null,
                    End = null,

                    Job = new Occupation
                    {
                        Description = (string)letzteTaetigkeit["bezeichnung"],
                        ClassificationCode = bewerber?.arbeitszeitModelle != null
                            ? string.Join(", ", bewerber.arbeitszeitModelle.ToObject<List<string>>())
                            : null,
                        Identifier = bewerber?.stellenart?.ToString(),
                        ValidFrom = bewerber?.veroeffentlichungsdatum != null
                            ? ParseDate((string)bewerber.veroeffentlichungsdatum.ToString()) // Ensure it's a string
                            : (DateTime?)null,
                        ValidTill = bewerber?.aktualisierungsdatum != null
                            ? ParseDate((string)bewerber.aktualisierungsdatum.ToString()) // Ensure it's a string
                            : (DateTime?)null,
                        UniqueIdentifier = null,
                        OccupationUri = null,
                        Concept = null,
                        RegulatoryAspect = "",
                        HasApprenticeShip = false,
                        IsUniversityOccupation = false,
                        IsUniversityDegree = false,
                        PreferedTerm = new List<string>(),
                        NonePreferedTerm = new List<string>(),
                        TaxonomyInfo = Taxonomy.Unknown,
                        TaxonomieVersion = "",
                        CultureString = "de-DE",
                        BroaderConcepts = new List<string>(),
                        NarrowerConcepts = new List<string>(),
                        RelatedConcepts = new List<string>(),
                        Skills = new List<string>(),
                        EssentialSkills = new List<string>(),
                        OptionalSkills = new List<string>(),
                        EssentialKnowledge = new List<string>(),
                        OptionalKnowledge = new List<string>(),
                        Documents = new List<string>(),
                        OccupationGroup = new Dictionary<string, string>(),
                        DkzApprenticeship = false,
                        QualifiedProfessional = false,
                        NeedsUniversityDegree = false,
                        IsMilitaryApprenticeship = false,
                        IsGovernmentApprenticeship = false,
                    },

                    City = bewerber.lokation?.ort != null ? bewerber.lokation.ort.ToString() : null,
                    Country = bewerber.lokation?.land != null ? bewerber.lokation.land.ToString() : null,
                };

                careerInfos.Add(careerInfo);
            }

            // Mapping Field Experiences
            if (bewerber?.erfahrung?.berufsfeldErfahrung != null)
            {
                foreach (var fieldExperience in bewerber.erfahrung.berufsfeldErfahrung)
                {
                    careerInfos.Add(new CareerInfo
                    {
                        Description = fieldExperience.berufsfeld?.ToString(),
                        Start = null, // Assuming no start date available
                        End = null,
                        Job = new Occupation
                        {
                            Description = fieldExperience.berufsfeld?.ToString(),
                            Identifier = "FieldExperience",
                        },
                        City = null,
                        Country = null
                    });
                }
            }

            return careerInfos;
        }


        /// <summary>
        /// Parses a string representation of a date into a DateTime object.
        /// </summary>
        /// <param name="dateString">String containing the date to be parsed.</param>
        private DateTime? ParseDate(string dateString)
        {
            if (string.IsNullOrEmpty(dateString))
                return null;

            string[] formats = new[] {
                "MM/dd/yyyy HH:mm:ss",  // Format: 12/31/2023 23:59:59
                "yyyy-MM-ddTHH:mm:ss.fff",  // Format: 2023-04-03T14:00:06.782
                "yyyy-MM-dd",  // Format: 2023-04-03
                "yyyy-MM-ddTHH:mm:ss",  // Format: 2023-04-03T14:00:06
                "MM/dd/yyyy",  // Format: 12/31/2023
                "yyyy/MM/dd",  // Format: 2023/12/31
            };

            if (DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateValue))
            {
                return dateValue;
            }

            return null;
        }


        /// <summary>
        /// Maps occupation data from the profile's dynamic data structure to a list of Occupation objects.
        /// </summary>
        /// <param name="bewerber">Dynamic object containing the individual's occupation information.</param>
        /// <returns>List of Occupation objects.</returns>
        private List<Occupation> MapOccupations(dynamic bewerber)
        {
            var occupations = new List<Occupation>();

            if (bewerber?.berufe != null)
            {
                foreach (var item in bewerber.berufe)
                {
                    occupations.Add(new Occupation
                    {
                        Description = item.ToString(),
                        ClassificationCode = bewerber?.arbeitszeitModelle != null
                            ? string.Join(", ", bewerber.arbeitszeitModelle.ToObject<List<string>>())
                            : null,
                        Identifier = bewerber?.stellenart?.ToString()
                    });
                }
            }

            return occupations;
        }


        /// <summary>
        /// Maps education information from the dynamic data structure of a profile.
        /// It transforms the education data into a structured list of EducationInfo objects.
        /// </summary>
        /// <param name="bewerber">Dynamic object representing the individual profile.</param>
        /// <returns>List of EducationInfo objects.</returns>
        private List<EducationInfo> MapEducationInfo(dynamic bewerber)
        {
            var educationInfos = new List<EducationInfo>();

            if (bewerber?.ausbildungen != null)
            {
                foreach (var item in bewerber.ausbildungen)
                {
                    int year = item?.jahr != null ? (int)item.jahr : 0;

                    educationInfos.Add(new EducationInfo
                    {
                        
                        // Map the end date to the year provided
                        End = year > 0 ? new DateTime(year, 1, 1) : (DateTime?)null,

                        // Map the city and country from location info, if available.
                        City = bewerber.lokation?.ort != null ? bewerber.lokation.ort.ToString() : null,
                        Country = bewerber.lokation?.land != null ? bewerber.lokation.land.ToString() : null,

                        // Map the description from 'art'.
                        Description = item?.art != null ? item.art.ToString() : null,

                        // Map the professional title from 'art'.
                        ProfessionalTitle = new Occupation
                        {
                            Description = item?.art != null ? item.art.ToString() : null
                        },


                        // Conditionally map other fields
                        CompletionState = item?.completionState != null ? item.completionState.ToString() : null,
                        Graduation = item?.graduation != null ? item.graduation.ToString() : null,
                        UniversityDegree = item?.universityDegree != null ? item.universityDegree.ToString() : null,
                        TypeOfSchool = item?.typeOfSchool != null ? item.typeOfSchool.ToString() : null,
                        NameOfInstitution = item?.nameOfInstitution != null ? item.nameOfInstitution.ToString() : null,
                        EducationType = item?.educationType != null ? item.educationType.ToString() : null,
                        Recognition = item?.recognition != null ? item.recognition.ToString() : null
                    });
                }
            }

            return educationInfos;
        }


        /// <summary>
        /// Maps qualifications data from a profile's dynamic data structure to a list of Qualification objects.
        /// </summary>
        /// <param name="bewerber">Dynamic object containing profile details.</param>
        /// <returns>List of Qualification objects.</returns>
        private List<Qualification> MapQualification(dynamic bewerber)
        {
            var qualifications = new List<Qualification>();

            if (bewerber?.qualifikationen != null)
            {
                foreach (var item in bewerber.qualifikationen)
                {
                    qualifications.Add(new Qualification
                    {
                        Name = item?.qualifikationsName,
                        Description = item?.beschreibung,
                        IssueDate = item?.ausstellDatum != null ? DateTime.Parse(item.ausstellDatum.ToString()) : (DateTime?)null,
                        ExpirationDate = item?.ablaufDatum != null ? DateTime.Parse(item.ablaufDatum.ToString()) : (DateTime?)null,
                        IssuingAuthority = item?.ausstellendeInstitution
                    });
                }
            }

            // Map licenses as qualifications if they exist
            if (bewerber?.lizenzen != null)
            {
                foreach (var item in bewerber.lizenzen)
                {
                    qualifications.Add(new Qualification
                    {
                        Name = item?.lizenz,
                        Description = "License",
                        IssueDate = item?.erteilt != null ? DateTime.Parse(item.erteilt.ToString()) : (DateTime?)null,
                        ExpirationDate = item?.ablaufDatum != null ? DateTime.Parse(item.ablaufDatum.ToString()) : (DateTime?)null,
                        IssuingAuthority = item?.ausstellendeInstitution
                    });
                }
            }

            return qualifications;
        }


        /// <summary>
        /// Maps mobility information from a profile's dynamic data structure.
        /// </summary>
        /// <param name="bewerber">Dynamic object representing the individual's profile.</param>
        /// <returns>Mobility object containing travel willingness and driver license details.</returns>
        private Mobility MapMobilityInfo(dynamic bewerber)
        {
            var mobility = new Mobility();

            if (bewerber?.mobilitaet != null)
            {
                mobility.WillingToTravel = bewerber.mobilitaet?.reisebereitschaft != null ? new Willing { ListItemId = 1, Value = bewerber.mobilitaet.reisebereitschaft } : null;
                mobility.DriverLicenses = bewerber.mobilitaet?.DriversLicense != null
                    ? ((IEnumerable<dynamic>)bewerber.mobilitaet.DriversLicense).Select(dl => new DriversLicense { ListItemId = 1, Value = dl.ToString() }).ToList()
                    : null;
                mobility.HasVehicle = bewerber.mobilitaet?.fahrzeugVorhanden;
            }

            return mobility;
        }


        /// <summary>
        /// Maps language skills from the profile's dynamic data.
        /// </summary>
        /// <param name="bewerber">Dynamic object containing the profile's language skills.</param>
        /// <returns>List of LanguageSkill objects.</returns>
        private List<LanguageSkill> MapLanguageSkills(dynamic bewerber)
        {
            var languageSkills = new List<LanguageSkill>();
            int listItemIdCounter = 1;

            if (bewerber?.sprachkenntnisse != null)
            {
                // Map Verhandlungssicher languages
                if (bewerber.sprachkenntnisse.Verhandlungssicher != null)
                {
                    foreach (var language in bewerber.sprachkenntnisse.Verhandlungssicher)
                    {
                        languageSkills.Add(new LanguageSkill
                        {
                            Name = language.Value.ToString(),
                            Code = GetCultureName(language.Value.ToString()),
                            Niveau = new LanguageNiveau { ListItemId = listItemIdCounter++, Value = "Verhandlungssicher" },
                        });
                    }
                }

                // Map Erweiterte Kenntnisse languages
                if (bewerber.sprachkenntnisse.ErweiterteKenntnisse != null)
                {
                    foreach (var language in bewerber.sprachkenntnisse.ErweiterteKenntnisse)
                    {
                        languageSkills.Add(new LanguageSkill
                        {
                            Name = language.Value.ToString(),
                            Code = GetCultureName(language.Value.ToString()),
                            Niveau = new LanguageNiveau { ListItemId = listItemIdCounter++, Value = "Erweiterte Kenntnisse" },
                        });
                    }
                }

                // Map Grundkenntnisse languages
                if (bewerber.sprachkenntnisse.Grundkenntnisse != null)
                {
                    foreach (var language in bewerber.sprachkenntnisse.Grundkenntnisse)
                    {
                        languageSkills.Add(new LanguageSkill
                        {
                            Name = language.Value.ToString(),
                            Code = GetCultureName(language.Value.ToString()),
                            Niveau = new LanguageNiveau { ListItemId = listItemIdCounter++, Value = "Grundkenntnisse" },
                        });
                    }
                }
            }

            return languageSkills;
        }


        /// <summary>
        /// Maps skills from the dynamic profile data, particularly from designated skill fields.
        /// </summary>
        /// <param name="kenntnisse"></param>
        /// <returns></returns>
        private List<Skill> MapSkills(dynamic bewerber)
        {
            var skills = new List<Skill>();
            int skillIdCounter = 1;

            // Map Expertenkenntnisse
            if (bewerber?.kenntnisse?.expertenKenntnisse != null)
            {
                foreach (var skill in bewerber.kenntnisse.expertenKenntnisse)
                {
                    skills.Add(new Skill
                    {
                        SkillId = skillIdCounter++, // Assign a unique SkillId
                        Title = new ApolloList
                        {
                            Items = new List<ApolloListItem>
                    {
                        new ApolloListItem { ListItemId = skillIdCounter++, Value = skill.ToString() }
                    },
                            ItemType = nameof(Skill.Title)
                        },
                        ScopeNote = "Expertenkenntnisse",
                        TaxonomyInfo = Taxonomy.Unknown
                    });
                }
            }
            else if (bewerber?.expertenKenntnisse != null)
            {
                foreach (var skill in bewerber.expertenKenntnisse)
                {
                    skills.Add(new Skill
                    {
                        SkillId = skillIdCounter++, // Assign a unique SkillId
                        Title = new ApolloList
                        {
                            Items = new List<ApolloListItem>
                    {
                        new ApolloListItem { ListItemId = skillIdCounter++, Value = skill.ToString() }
                    },
                            ItemType = nameof(Skill.Title)
                        },
                        ScopeNote = "Expertenkenntnisse",
                        TaxonomyInfo = Taxonomy.Unknown
                    });
                }
            }

            // Map Erweiterte Kenntnisse
            if (bewerber?.kenntnisse?.ErweiterteKenntnisse != null)
            {
                foreach (var skill in bewerber.kenntnisse.ErweiterteKenntnisse)
                {
                    skills.Add(new Skill
                    {
                        SkillId = skillIdCounter++, // Assign a unique SkillId
                        Title = new ApolloList
                        {
                            Items = new List<ApolloListItem>
                    {
                        new ApolloListItem { ListItemId = skillIdCounter++, Value = skill.ToString() }
                    },
                            ItemType = nameof(Skill.Title)
                        },
                        ScopeNote = "Erweiterte Kenntnisse",
                        TaxonomyInfo = Taxonomy.Unknown
                    });
                }
            }
            else if (bewerber?.ErweiterteKenntnisse != null)
            {
                foreach (var skill in bewerber.ErweiterteKenntnisse)
                {
                    skills.Add(new Skill
                    {
                        SkillId = skillIdCounter++, // Assign a unique SkillId
                        Title = new ApolloList
                        {
                            Items = new List<ApolloListItem>
                    {
                        new ApolloListItem { ListItemId = skillIdCounter++, Value = skill.ToString() }
                    },
                            ItemType = nameof(Skill.Title)
                        },
                        ScopeNote = "Erweiterte Kenntnisse",
                        TaxonomyInfo = Taxonomy.Unknown
                    });
                }
            }

            // Map Grundkenntnisse
            if (bewerber?.kenntnisse?.Grundkenntnisse != null)
            {
                foreach (var skill in bewerber.kenntnisse.Grundkenntnisse)
                {
                    skills.Add(new Skill
                    {
                        SkillId = skillIdCounter++, // Assign a unique SkillId
                        Title = new ApolloList
                        {
                            Items = new List<ApolloListItem>
                    {
                        new ApolloListItem { ListItemId = skillIdCounter++, Value = skill.ToString() }
                    },
                            ItemType = nameof(Skill.Title)
                        },
                        ScopeNote = "Grundkenntnisse",
                        TaxonomyInfo = Taxonomy.Unknown
                    });
                }
            }
            else if (bewerber?.Grundkenntnisse != null)
            {
                foreach (var skill in bewerber.Grundkenntnisse)
                {
                    skills.Add(new Skill
                    {
                        SkillId = skillIdCounter++, // Assign a unique SkillId
                        Title = new ApolloList
                        {
                            Items = new List<ApolloListItem>
                    {
                        new ApolloListItem { ListItemId = skillIdCounter++, Value = skill.ToString() }
                    },
                            ItemType = nameof(Skill.Title)
                        },
                        ScopeNote = "Grundkenntnisse",
                        TaxonomyInfo = Taxonomy.Unknown
                    });
                }
            }

            return skills;
        }


        /// <summary>
        /// Maps web references from the profile's dynamic data to a list of WebReference objects.
        /// </summary>
        /// <param name="bewerber">Dynamic object representing the profile's web references.</param>
        /// <returns>List of WebReference objects.</returns>
        private List<WebReference> MapWebReferences(dynamic bewerber)
        {
            var webReferences = new List<WebReference>();

            if (bewerber?.webReferenzen != null)
            {
                foreach (var item in bewerber.webReferenzen)
                {
                    webReferences.Add(new WebReference
                    {
                        Url = item?.url != null ? new Uri(item.url.ToString()) : null,
                        Title = item?.titel
                    });
                }
            }

            return webReferences;
        }


        private string? GetCultureName(string language)
        {
            string? isoCode = GetISOCode(language);
            CultureInfo? cultureInfo = isoCode != null ? GetCultureInfoFromISOCode(isoCode) : null;
            return cultureInfo?.Name;
        }


        private CultureInfo? GetCultureInfoFromISOCode(string isoCode)
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


        private string? GetISOCode(string languageName)
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
