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



        // NOTE: METHOD CHANGED FOR ONLY FIRST 5 Blobs FOR TESTING FUNCTIONALITY

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
            int processedCount = 0;
            int skippedCount = 0;
            object lockObj = new object();

            var startTime = DateTime.Now; // Capture start time for duration calculation
            int blobCount = 0;

            var producerTask = Task.Run(async () =>
            {
                await foreach (var blobItem in blobItems)
                {
                    if (blobCount >= 5) break; // Process only the first 5 blobs
                    await fileChannel.Writer.WriteAsync(blobItem);
                    _logger.LogInformation($"Enqueued blob: {blobItem.Name} for processing.");
                    blobCount++;
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

                            _logger.LogInformation($"Profile object mapped: {System.Text.Json.JsonSerializer.Serialize(profile)}");

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

            _logger.LogInformation($"Import of profiles completed. Total processed: {processedCount}, Total skipped: {skippedCount}, Total time taken: {elapsedTime}");
        }


        // <summary>
        /// Maps JSON data to a Profile object. This involves parsing the JSON and converting it into an instance of a Profile.
        /// </summary>
        /// <param name="jsonData">The JSON string representing the profile data.</param>
        /// <returns>The mapped Profile object.</returns>
        private Profile MapJsonToProfile(string jsonData)
        {
            dynamic jsonObject = JsonConvert.DeserializeObject(jsonData);

            var apolloProfile = new Profile
            {
                Id = null,
                CareerInfos = jsonObject?.bewerber != null ? MapCareerInfo(jsonObject.bewerber) : new List<CareerInfo>(),
                Occupations = jsonObject?.bewerber != null ? MapOccupation(jsonObject.bewerber) : new List<Occupation>(),
                EducationInfos = jsonObject?.bewerber != null ? MapEducationInfo(jsonObject.bewerber) : new List<EducationInfo>(),
                Qualifications = jsonObject?.bewerber != null ? MapQualification(jsonObject.bewerber) : new List<Qualification>(),
                MobilityInfo = jsonObject?.bewerber != null ? MapMobilityInfo(jsonObject.bewerber) : new Mobility(),
                LanguageSkills = jsonObject?.bewerber != null ? MapLanguageSkills(jsonObject.bewerber) : new List<LanguageSkill>(),
                Skills = jsonObject?.bewerber != null ? MapSkills(jsonObject.bewerber) : new List<Skill>(),
                WebReferences = new List<WebReference> { new WebReference { Title = "BA-Test" } }
            };

            return apolloProfile;
        }

        private List<CareerInfo> MapCareerInfo(IEnumerable<dynamic> bewerbers)
        {
            var careerInfos = new List<CareerInfo>();

            foreach (var bewerber in bewerbers)
            {
                if (bewerber?.werdegang != null)
                {
                    foreach (var item in bewerber.werdegang)
                    {
                        careerInfos.Add(new CareerInfo
                        {
                            Description = item?.berufsbezeichnung,
                            Start = item?.Von,
                            End = item?.EndDate,
                            Job = new Occupation()
                            {
                                Description = item?.berufsbezeichnung,
                            }
                        });
                    }
                }
            }

            return careerInfos;
        }

        private List<Occupation> MapOccupation(IEnumerable<dynamic> bewerbers)
        {
            var occupations = new List<Occupation>();

            foreach (var bewerber in bewerbers)
            {
                if (bewerber?.berufe != null)
                {
                    foreach (var item in bewerber.berufe)
                    {
                        occupations.Add(new Occupation
                        {
                            Description = item.ToString()
                        });
                    }
                }
            }

            return occupations;
        }

        private List<EducationInfo> MapEducationInfo(IEnumerable<dynamic> bewerbers)
        {
            var educationInfos = new List<EducationInfo>();

            foreach (var bewerber in bewerbers)
            {
                if (bewerber?.bildung != null)
                {
                    foreach (var item in bewerber.bildung)
                    {
                        educationInfos.Add(new EducationInfo
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
                        });
                    }
                }
            }

            return educationInfos;
        }

        private List<Qualification> MapQualification(IEnumerable<dynamic> bewerbers)
        {
            var qualifications = new List<Qualification>();

            foreach (var bewerber in bewerbers)
            {
                if (bewerber?.qualifikationen != null)
                {
                    foreach (var item in bewerber.qualifikationen)
                    {
                        qualifications.Add(new Qualification
                        {
                            Name = item?.qualifikationsName,
                            Description = item?.beschreibung,
                            IssueDate = item?.ausstellDatum,
                            ExpirationDate = item?.ablaufDatum,
                            IssuingAuthority = null
                        });
                    }
                }
            }

            return qualifications;
        }

        private Mobility MapMobilityInfo(IEnumerable<dynamic> bewerbers)
        {
            var mobility = new Mobility();

            foreach (var bewerber in bewerbers)
            {
                if (bewerber?.mobilitaet != null)
                {
                    mobility = new Mobility
                    {
                        WillingToTravel = bewerber.mobilitaet?.reisebereitschaft != null ? new Willing { ListItemId = 1, Value = bewerber.mobilitaet.reisebereitschaft } : null,
                        DriverLicenses = bewerber.mobilitaet?.DriversLicense != null ? new List<DriversLicense> { new DriversLicense { ListItemId = 1, Value = bewerber.mobilitaet.fuehrerscheine } } : null,
                        HasVehicle = bewerber.mobilitaet?.fahrzeugVorhanden
                    };
                }
            }

            return mobility;
        }

        private List<LanguageSkill> MapLanguageSkills(IEnumerable<dynamic> bewerbers)
        {
            var languageSkills = new List<LanguageSkill>();
            int listItemIdCounter = 1;

            foreach (var bewerber in bewerbers)
            {
                // Map Verhandlungssicher languages
                if (bewerber?.sprachkenntnisse?.Verhandlungssicher != null)
                {
                    foreach (var language in bewerber.sprachkenntnisse.Verhandlungssicher)
                    {
                        languageSkills.Add(new LanguageSkill
                        {
                            Name = language.Value,
                            Code = GetCultureName(language.Value),
                            Niveau = new LanguageNiveau { ListItemId = listItemIdCounter++, Value = "Verhandlungssicher" },
                        });
                    }
                }

                // Map Erweiterte Kenntnisse languages
                if (bewerber?.sprachkenntnisse?.ErweiterteKenntnisse != null)
                {
                    foreach (var language in bewerber.sprachkenntnisse.ErweiterteKenntnisse)
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
                if (bewerber?.sprachkenntnisse?.Grundkenntnisse != null)
                {
                    foreach (var language in bewerber.sprachkenntnisse.Grundkenntnisse)
                    {
                        languageSkills.Add(new LanguageSkill
                        {
                            Name = language.Value,
                            Code = GetCultureName(language.Value),
                            Niveau = new LanguageNiveau { ListItemId = listItemIdCounter++, Value = "Grundkenntnisse" },
                        });
                    }
                }
            }

            return languageSkills;
        }

        private List<Skill> MapSkills(IEnumerable<dynamic> bewerbers)
        {
            var skills = new List<Skill>();

            foreach (var bewerber in bewerbers)
            {
                // Map Erweiterte Kenntnisse
                if (bewerber?.kenntnisse?.ErweiterteKenntnisse != null)
                {
                    foreach (var skill in bewerber.kenntnisse.ErweiterteKenntnisse)
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
                if (bewerber?.kenntnisse?.Grundkenntnisse != null)
                {
                    foreach (var skill in bewerber.kenntnisse.Grundkenntnisse)
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
                if (bewerber?.kenntnisse?.Expertenkenntnisse != null)
                {
                    foreach (var skill in bewerber.kenntnisse.Expertenkenntnisse)
                    {
                        skills.Add(new Skill
                        {
                            Title = skill,
                            ScopeNote = "Expertenkenntnisse",
                            TaxonomyInfo = Taxonomy.Unknown
                        });
                    }
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
