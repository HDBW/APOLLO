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


namespace Apollo.ProfileImporter
{

    /// <summary>
    /// Imports profiles from blob storage, maps them to the the profile including skills property and upsert them into the
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
            _logger.LogInformation("Import of profiles started");

            try
            {
                var blobConnectionString = _configuration["BlobStorage:ConnectionString"];
                var containerName = _configuration["BlobStorage:ContainerName"];
                var maxConsumers = _configuration.GetValue<int>("MaxConsumers");

                await ImportProfilesFromBlobStorage(blobConnectionString, containerName, maxConsumers);

                _logger.LogInformation("Import of profiles completed");
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
        /// Handles the actual import of profiles from Azure Blob Storage by setting up a producer-consumer scenario
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

            var producerTask = Task.Run(async () =>
            {
                await foreach (var blobItem in blobItems)
                {
                    await fileChannel.Writer.WriteAsync(blobItem);
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
                            var blobClient = containerClient.GetBlobClient(blobItem.Name);
                            var blobProperties = await blobClient.GetPropertiesAsync();

                            if (blobProperties.Value.ContentLength <= 1024)
                            {
                                _logger.LogWarning($"Blob {blobItem.Name} is smaller than 1024 bytes and will be skipped.");
                                continue;
                            }

                            var downloadResponse = await blobClient.DownloadContentAsync();
                            var jsonData = downloadResponse.Value.Content.ToString();

                            var profile = MapJsonToProfile(jsonData);

                            var jsonDataSave = System.Text.Json.JsonSerializer.Serialize(profile);

                            var result = await _api.CreateOrUpdateBAProfileAsync(profile);

                            _logger.LogInformation($"Profile {profile.Id} imported successfully.");
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
        }



        // <summary>
        /// Maps JSON data to a Profile object. This involves parsing the JSON and converting it into an instance of a Profile.
        /// </summary>
        /// <param name="jsonData">The JSON string representing the profile data.</param>
        /// <returns>The mapped Profile object.</returns>
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
                Skills = jsonObject?.kenntnisse != null ? MapSkills(jsonObject.kenntnisse) : null,
                WebReferences = new List<WebReference> { new WebReference { Title = "BA-Test" } }
            };

            return apolloProfile;
        }


        /// <summary>
        /// Maps JSON data to a list of CareerInfo objects.
        /// Each CareerInfo is extracted from dynamic JSON objects representing various career details.
        /// </summary>
        /// <param name="items">Dynamic JSON objects representing career data.</param>
        /// <returns>A list of CareerInfo objects populated with the data from the JSON objects.</returns>
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
                    NonePreferedTerm = [],
                    TaxonomyInfo = Taxonomy.Unknown,
                    TaxonomieVersion = "",
                    CultureString = "de-DE",
                    BroaderConcepts = [],
                    NarrowerConcepts = [],
                    RelatedConcepts = [],
                    Skills = [],
                    EssentialSkills = [],
                    OptionalSkills = [],
                    EssentialKnowledge = [],
                    OptionalKnowledge = [],
                    Documents = [],
                    OccupationGroup = { },
                    DkzApprenticeship = true,
                    QualifiedProfessional = true,
                    NeedsUniversityDegree = true,
                    IsMilitaryApprenticeship = false,
                    IsGovernmentApprenticeship = false,
                    //ValidFrom = item?.Von,
                    //ValidTill = item?.EndDate,
                }
            }).ToList();
        }


        /// <summary>
        /// Maps JSON data to a list of Occupation objects.
        /// This method processes dynamic JSON objects that describe occupation details.
        /// </summary>
        /// <param name="items">Dynamic JSON objects representing occupation data.</param>
        /// <returns>A list of Occupation objects created from the JSON data.</returns>
        private List<Occupation> MapOccupation(IEnumerable<dynamic> items)
        {
            var data = items.Select(item => new Occupation
            {
                Description = item.ToString()
            }).ToList();
            return data;
        }


        /// <summary>
        /// Maps JSON data to a list of EducationInfo objects.
        /// Each EducationInfo is derived from dynamic JSON objects that detail educational backgrounds.
        /// </summary>
        /// <param name="items">Dynamic JSON objects representing education data.</param>
        /// <returns>A list of EducationInfo objects populated from the JSON data.</returns>
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
                CompletionState = item?.istAbgeschlossen != null ? new CompletionState { ListItemId = 1, Value = item.istAbgeschlossen } : null,
                Graduation = item?.schulAbschluss != null ? new SchoolGraduation { ListItemId = 1, Value = item.schulAbschluss } : null,
                UniversityDegree = item?.hochSchulAbschluss != null ? new UniversityDegree { ListItemId = 1, Value = item.hochSchulAbschluss } : null,
                TypeOfSchool = item?.schulart != null ? new TypeOfSchool { ListItemId = 1, Value = item.schulart } : null,
                NameOfInstitution = item?.nameArtEinrichtung,
                City = item?.ort,
                Recognition = item?.anerkennungAbschluss != null ? new RecognitionType { ListItemId = 1, Value = item.anerkennungAbschluss } : null
            }).ToList();

            return data;
        }


        /// <summary>
        /// Maps JSON data to a list of Qualification objects.
        /// Each Qualification object is created based on dynamic JSON data detailing specific qualifications.
        /// </summary>
        /// <param name="items">Dynamic JSON objects representing qualification data.</param>
        /// <returns>A list of Qualification objects created from the JSON data.</returns>
        private List<Qualification> MapQualification(IEnumerable<dynamic> items)
        {
            var data = items.Select(item => new Qualification
            {
                Name = item?.qualifikationsName,
                Description = item?.beschreibung,
                IssueDate = item?.ausstellDatum,
                ExpirationDate = item?.ablaufDatum,
                IssuingAuthority = null
            }).ToList();
            return data;
        }


        /// <summary>
        /// Maps JSON data to a Mobility object.
        /// This method processes dynamic JSON objects to extract mobility information, including willingness to travel and driver licenses.
        /// </summary>
        /// <param name="item">A dynamic JSON object representing mobility information.</param>
        /// <returns>A Mobility object populated from the JSON data.</returns>
        private Mobility MapMobilityInfo(dynamic item)
        {
            var data = new Mobility
            {
                WillingToTravel = item?.reisebereitschaft != null ? new Willing { ListItemId = 1, Value = item.reisebereitschaft } : null,
                DriverLicenses = item?.DriversLicense != null ? new List<DriversLicense> { new DriversLicense { ListItemId = 1, Value = item.fuehrerscheine } } : null,
                HasVehicle = item?.fahrzeugVorhanden
            };
            return data;
        }


        /// <summary>
        /// Maps JSON data to a list of LanguageSkill objects.
        /// Processes dynamic JSON arrays to extract language skill levels and names from the profile data.
        /// </summary>
        /// <param name="sprachkenntnisse">Dynamic JSON object containing language skill data.</param>
        /// <returns>A list of LanguageSkill objects populated based on the JSON data.</returns>
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


        /// <summary>
        /// Maps JSON data to a list of Skill objects.
        /// Extracts skills from dynamic JSON arrays detailing various competencies and expertise levels.
        /// </summary>
        /// <param name="kenntnisse">Dynamic JSON object containing detailed skills information.</param>
        /// <returns>A list of Skill objects created from the JSON data.</returns>
        private List<Skill> MapSkills(dynamic kenntnisse)
        {
            var skills = new List<Skill>();

            // Map Erweiterte Kenntnisse
            if (kenntnisse?.ErweiterteKenntnisse != null)
            {
                foreach (var skill in kenntnisse.ErweiterteKenntnisse)
                {
                    skills.Add(new Skill
                    {
                        Title = skill,
                        ScopeNote = "Erweiterte Kenntnisse",
                        TaxonomyInfo = Taxonomy.Unknown
                    });
                }
            }

            // Map Grundkenntnisse
            if (kenntnisse?.Grundkenntnisse != null)
            {
                foreach (var skill in kenntnisse.Grundkenntnisse)
                {
                    skills.Add(new Skill
                    {
                        Title = skill,
                        ScopeNote = "Grundkenntnisse",
                        TaxonomyInfo = Taxonomy.Unknown
                    });
                }
            }

            // Map Expertenkenntnisse
            if (kenntnisse?.Expertenkenntnisse != null)
            {
                foreach (var skill in kenntnisse.Expertenkenntnisse)
                {
                    skills.Add(new Skill
                    {
                        Title = skill,
                        ScopeNote = "Expertenkenntnisse",
                        TaxonomyInfo = Taxonomy.Unknown

                    });
                }
            }

            return skills;
        }



        string? GetCultureName(string language)
        {
            string? isoCode = GetISOCode(language);
            CultureInfo? cultureInfo = isoCode != null ? GetCultureInfoFromISOCode(isoCode) : null;
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
