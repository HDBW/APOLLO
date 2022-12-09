using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using Invite.Apollo.App.Graph.Assessment.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Course.Enums;
using Invite.Apollo.App.Graph.Common.Models.Course;

namespace Invite.Apollo.App.Graph.Assessment.Data
{

    public class UseCaseCourseData
    {

        public enum EduProviderId
        {
            Biwe = 0,
            Bbw = 1,
            Tuev = 2,
            Bfz
        }

        public static Dictionary<int, string > providerNames = new Dictionary<int, string>()
        {
            {(int)EduProviderId.Biwe, "Biwe"}, {(int)EduProviderId.Bbw, "bbw"}, {(int)EduProviderId.Tuev, "TüV Rheinland Akademie" },  {(int)EduProviderId.Bfz, "bfz" }
        };

        public static Dictionary<string, int> getProviderIdByName = new Dictionary<string, int>()
        {
            {"Biwe", (int)EduProviderId.Biwe}, {"bbw", (int)EduProviderId.Bbw}, {"TüV Rheinland Akademie", (int)EduProviderId.Tuev },  {"bfz", (int)EduProviderId.Bfz }
        };


        /// <summary>
        /// Get List of <c>Course</c> Objects by Usecasenumber int
        /// </summary>
        public Dictionary<int, List<CourseItem>> usecaseCourses = new Dictionary<int, List<CourseItem>>();

        /// <summary>
        /// Get List of <c>CourseContact</c> Objects by Usecasenumber int
        /// </summary>
        public Dictionary<int, List<CourseContact>> useCaseContacts = new Dictionary<int, List<CourseContact>>();

        /// <summary>
        /// Is a n:m mapper between courses and course contacts
        /// Ids 0 - 90 biwe
        /// Ids 0 - 29 biwe UseCase 1
        /// Ids 30 - 59 biwe UseCase 2
        /// Ids 60 - 90 biwe UseCase 3
        /// 
        /// Ids 100 - 190 bbw
        /// Ids 100 - 129 bbw UseCase 1
        /// Ids 130 - 159 bbw UseCase 2
        /// Ids 160 - 190 bbw UseCase 3
        /// 
        /// Ids 200 - 290 tüv
        /// Ids 200 - 229 bbw UseCase 1
        /// Ids 230 - 259 bbw UseCase 2
        /// Ids 260 - 290 bbw UseCase 3
        /// </summary>
        public Dictionary<long, CourseContactRelation> CourseContactRelations { get; set; }


        public List<EduProviderItem> ProviderList = new List<EduProviderItem>()
        {
            //TODO: Set Logo!
            new EduProviderItem()
            {
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Ticks = DateTime.Now.Ticks,
                Name = providerNames[(int)EduProviderId.Biwe],
                Description = "TODO",
                Logo = new Uri("https://invite-apollo.app/TODO"),
                Id =(int)EduProviderId.Biwe,
                Website = new Uri("https://www.biwe-bbq.de/")
            },
            new EduProviderItem()
            {
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Ticks = DateTime.Now.Ticks,
                Name = providerNames[(int)EduProviderId.Bbw],
                Description = "TODO",
                Logo = new Uri("https://invite-apollo.app/TODO"),
                Id = (int)EduProviderId.Bbw,
                Website = new Uri("https://www.bbw-weiterbildung.de/")
            },
            new EduProviderItem()
            {
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Ticks = DateTime.Now.Ticks,
                Name = providerNames[(int)EduProviderId.Tuev],
                Description = "TODO",
                Logo = new Uri("https://invite-apollo.app/TODO"),
                Id = (int)EduProviderId.Tuev,
                Website = new Uri("https://akademie.tuv.com/")
            },
            new EduProviderItem()
            {
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Ticks = DateTime.Now.Ticks,
                Name = providerNames[(int)EduProviderId.Bfz],
                Description = "TODO",
                Logo = new Uri("https://invite-apollo.app/TODO"),
                Id = (int)EduProviderId.Bfz,
                Website = new Uri("https://www.bfz.de/")
            }

        };

        /// <summary>
        /// Ids 0 - 90 biwe
        /// Ids 0 - 29 biwe UseCase 1
        /// Ids 30 - 59 biwe UseCase 2
        /// Ids 60 - 90 biwe UseCase 3
        /// 
        /// Ids 100 - 190 bbw
        /// Ids 100 - 129 bbw UseCase 1
        /// Ids 130 - 159 bbw UseCase 2
        /// Ids 160 - 190 bbw UseCase 3
        /// 
        /// Ids 200 - 290 tüv
        /// Ids 200 - 229 bbw UseCase 1
        /// Ids 230 - 259 bbw UseCase 2
        /// Ids 260 - 290 bbw UseCase 3
        /// </summary>
        public Dictionary<long, CourseContact> Contacts = new Dictionary<long, CourseContact>()
        {
            #region BIWE
            /*
             ***********************************BIWE******************************************
             *********************************************************************************             
             ************** | ID RANGE 0 - 90                                   |*************
             ************** | USECASE     | USERNAME      |   RANGE             |*************
             ************** ____________________________________________________|*************
             ************** |                                                   |*************
             ************** | 1           | ADRIAN        | 0 - 29              |*************
             ************** | 2           | KERSTIN       | 30 - 59             |*************
             ************** | 3           | ARWA          | 60 - 90             |*************
             ************** |___________________________________________________|*************
             ************************************BIWE*****************************************
             */

            #region USECASE 1 - ADRIAN ARBEITSSUCHEND 71634 LUDWIGSBURG Abschlussorientierte TQ LAGER
            { 0, new CourseContact() { Id = 0,
                ContactMail = "info-stuttgart@biwe.de",
                ContactName = "BBQ Stuttgart",
                ContactPhone = "0711 252875-19",
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Url = new Uri("https://www.biwe-bbq.de/ueber-uns/vor-ort/stuttgart-mittlerer-pfad")}},
            { 1, new CourseContact() { Id = 1,
                ContactMail = "TODO@todo.de",
                ContactName = "TODO",
                ContactPhone = "000000 1111111",
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Url = new Uri("https://www.biwe-bbq.de/ueber-uns/vor-ort/stuttgart-mittlerer-pfad")}},
            #endregion

            #region USECASE 2 - KERSTIN BESCHÄFTIGTE 81929 MÜNCHEN KAFFRAU EINZELHANDEL E-Commerce/Onlinehandel

            #endregion

            #region USECASE 3 - ARWA  BERUFLICHER AUFSTIEG AUGSBURG FACHINFORMATIKERIN IT Projektleitung / Führungsposition

            #endregion

            #endregion

            #region BBW
            /*
             ***********************************BBW*******************************************
             *********************************************************************************             
             ************** | ID RANGE 100 - 190                                |*************
             ************** | USECASE     | USERNAME      |   RANGE             |*************
             ************** ____________________________________________________|*************
             ************** |                                                   |*************
             ************** | 1           | ADRIAN        | 100 - 129           |*************
             ************** | 2           | KERSTIN       | 130 - 159           |*************
             ************** | 3           | ARWA          | 160 - 190           |*************
             ************** |___________________________________________________|*************
             ************************************BBW******************************************
             */

            #region USECASE 1 - ADRIAN ARBEITSSUCHEND 71634 LUDWIGSBURG Abschlussorientierte TQ LAGER
            //Fachlagerist (bfz Donauwörth * Ulm * Aalen - Hospitalgasse 16 - 73525 Schwäbisch Gmünd
            //https://www.bfz.de/kurs/eca-90527/pdf/fachlageristin.pdf
            //Algemein Fachlagerist?
            { 100, new CourseContact() { Id = 100,
                ContactMail = "Klaus Mildenberger",
                ContactName = "beratung@bfz.de",
                ContactPhone = "",
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Url = new Uri("https://www.bfz.de/donauwoerth-ulm-aalen")}},
            //Fachlagerist bfz Donauwörth Ulm Aalen Ansprechpartner
            { 101, new CourseContact() { Id = 100,
                ContactMail = "Christine Ziegler",
                ContactName = "christine.ziegler@bfz.de",
                ContactPhone = "07391 58751-13",
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Url = new Uri("https://www.bfz.de/donauwoerth-ulm-aalen")}},
            { 102, new CourseContact() { Id = 100,
                ContactMail = "info-dua@bfz.de",
                ContactName = "bfz Donauwörth · Ulm · Aalen",
                ContactPhone = "+49 906 70677-0",
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Url = new Uri("https://www.bfz.de/donauwoerth-ulm-aalen")}},
            #endregion

            #region USECASE 2 - KERSTIN BESCHÄFTIGTE 81929 MÜNCHEN KAFFRAU EINZELHANDEL E-Commerce/Onlinehandel

            #endregion

            #region USECASE 3 - ARWA  BERUFLICHER AUFSTIEG AUGSBURG FACHINFORMATIKERIN IT Projektleitung / Führungsposition

            #endregion

           
            #endregion

            #region TÜV
            /*
             ***********************************TÜV*******************************************
             *********************************************************************************             
             ************** | ID RANGE 200 - 290                                |*************
             ************** | USECASE     | USERNAME      |   RANGE             |*************
             ************** ____________________________________________________|*************
             ************** |                                                   |*************
             ************** | 1           | ADRIAN        | 200 - 229           |*************
             ************** | 2           | KERSTIN       | 230 - 259           |*************
             ************** | 3           | ARWA          | 260 - 290           |*************
             ************** |___________________________________________________|*************
             ************************************TÜV******************************************
             */
            #region USECASE 1 - ADRIAN ARBEITSSUCHEND 71634 LUDWIGSBURG Abschlussorientierte TQ LAGER
            
            #endregion

            #region USECASE 2 - KERSTIN BESCHÄFTIGTE 81929 MÜNCHEN KAFFRAU EINZELHANDEL E-Commerce/Onlinehandel

            #endregion

            #region USECASE 3 - ARWA  BERUFLICHER AUFSTIEG AUGSBURG FACHINFORMATIKERIN IT Projektleitung / Führungsposition

            #endregion


            #endregion


        };



        /// <summary>
        /// Ids 0 - 90 biwe
        /// Ids 0 - 29 biwe UseCase 1
        /// Ids 30 - 59 biwe UseCase 2
        /// Ids 60 - 90 biwe UseCase 3
        /// 
        /// Ids 100 - 190 bbw
        /// Ids 100 - 129 bbw UseCase 1
        /// Ids 130 - 159 bbw UseCase 2
        /// Ids 160 - 190 bbw UseCase 3
        /// 
        /// Ids 200 - 290 tüv
        /// Ids 200 - 229 bbw UseCase 1
        /// Ids 230 - 259 bbw UseCase 2
        /// Ids 260 - 290 bbw UseCase 3
        /// </summary>
        public Dictionary<long, CourseItem> CourseList = new Dictionary<long, CourseItem>()
        {
            //TODO: CourseProvider vs TrainingProvider vs InstructorId für Dezember?
            //TODO: AppendLine bzq \r\n doppelt gemoppelt????

            #region BIWE
            /*
             ***********************************BIWE******************************************
             *********************************************************************************             
             ************** | ID RANGE 0 - 90                                   |*************
             ************** | USECASE     | USERNAME      |   RANGE             |*************
             ************** ____________________________________________________|*************
             ************** |                                                   |*************
             ************** | 1           | ADRIAN        | 0 - 29              |*************
             ************** | 2           | KERSTIN       | 30 - 59             |*************
             ************** | 3           | ARWA          | 60 - 90             |*************
             ************** |___________________________________________________|*************
             ************************************BIWE*****************************************
             */
            #region USECASE 1 - ADRIAN ARBEITSSUCHEND 71634 LUDWIGSBURG Abschlussorientierte TQ LAGER

            #region FACHLAGERIST*IN
            //Biwe: TQ Lager: https://www.biwe-bbq.de/weiterbildungsportal/themen/coaching-qualifizierung/einzelansicht/reha-coaching
            { 0, new CourseItem() {
                                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                                Ticks = DateTime.Now.Ticks,
                                Availability = CourseAvailability.Available,
                                CourseProviderId = (int) EduProviderId.Biwe,
                                TrainingProviderId = (int)EduProviderId.Biwe,
                                InstructorId = (int)EduProviderId.Biwe,
                                CourseTagType = CourseTagType.PartialQualification,
                                //Je nach Präferenz/Filter kann Angebot in Präsenz oder online stattfinden. (Wichtig: Falls Präsenz, dann Wohnort von Adrian entsprechend anpassen!)
                                CourseType = CourseType.InPerson,
                                Occurrence = OccurrenceType.FullTime,
                                //320 Unterrichtseinheiten je 45 min pro Modul + 120 Unterrichtseinheiten je 60 min im Unternehmen
                                Duration = "11 Wochen",//320 Unterrichtseinheiten je 45 min pro Modul + 120 Unterrichtseinheiten je 60 min im Unternehmen - 6 Module
                                CourseUrl = new Uri("https://www.biwe-bbq.de/weiterbildungen/anzeige/tq-lagerlogistik"),
                                ExternalId = "https://www.biwe-bbq.de/weiterbildungen/anzeige/tq-lagerlogistik",
                                Language = "DE-DE",
                                KeyPhrases = "",
                                Title = "Fachkraft für Lagerlogistik (m/w/d) - TQ/ETAPP",
                                ShortDescription = "Mit der Teilqualifizierung können Sie Schritt für Schritt in sechs Etappen den Berufsabschluss Fachkraft für Lagerlogistik (m/w/d) erreichen. Bei erfolgreicher Kompetenzfeststellung erhalten Sie nach jeder Etappe ein Zertifikat inklusive Kompetenzfeststellungsergebnis, das bundesweit anerkannt ist. Sie haben die Möglichkeit, sich zur Externenprüfung bei der zuständigen Kammer anzumelden und damit den Berufsabschluss zu erwerben.",
                                Description = new StringBuilder()
                                    .AppendLine("Bei der Teilqualifizierung wird der anerkannte Ausbildungsberuf Fachkraft für Lagerlogistik (m/w/d) in folgende Module aufgegliedert.")
                                    .AppendLine("TQ-Modul 1 - Wareneingang (11 Wochen, davon 3 Wochen im Unternehmen) In diesem Modul werden folgende Inhalte vermittelt: Annehmen der Güter, Entladen und Kontrollieren der Lieferung, Prüfen der Lieferung anhand der Begleitpapiere, Abschluss Flurförderschein\r\n")
                                    .AppendLine("TQ-Modul 2 - Lagerung (11 Wochen, davon 3 Wochen im Unternehmen) In diesem Modul werden folgende Inhalte vermittelt: Auspacken, Sortieren und Lagern der Güter anforderungsgerecht und nach wirtschaftlichen Grundsätzen unter Beachtung der Lagerordnung , Transportieren und Zuleiten der Güter zum betrieblichen Bestimmungsort\r\n")
                                    .AppendLine("TQ-Modul 3 - Innerbetriebliche Logistik und Kontrolle (11 Wochen, davon 3 Wochen im Unternehmen) In diesem Modul werden folgende Inhalte vermittelt: Anwenden betrieblicher Informations- und Kommunikationssysteme, Standardsoftware und arbeitsplatzbezogener Software, Anwenden fachspezifischer Fremdsprachenkenntnisse, Durchführen von Bestandskontrollen und Maßnahmen der Bestandspflege\r\n")
                                    .AppendLine("TQ-Modul 4 - Kommissionierung und Endkontrolle (11 Wochen, davon 3 Wochen im Unternehmen) In diesem Modul werden folgende Inhalte vermittelt: Erstellen von Ladelisten/Beladeplänen unter Beachtung von Ladevorschriften, Kennzeichnen, Beschriften und Sichern von Sendungen nach gesetzlichen Vorgaben, Kommissionieren und Verpacken der Güter für Sendungen und Zusammenstellen zu Ladeeinheiten\r\n")
                                    .AppendLine("TQ-Modul 5 - Versand (11 Wochen, davon 3 Wochen im Unternehmen) In diesem Modul werden folgende Inhalte vermittelt: Kennzeichnen, Beschriften und Sichern von Sendungen nach gesetzlichen Vorgaben, Bearbeiten der Versand- und Begleitpapiere und Erstellen von Versandaufzeichnungen\r\n")
                                    .AppendLine("TQ-Modul 6 - Arbeitsorganisation und Qualitätssicherung (11 Wochen, davon 3 Wochen im Unternehmen) In diesem Modul werden folgende Inhalte vermittelt: Mitwirken bei logistischen Planungs- und Organisationsprozessen, Mitwirken bei qualitätssichernden Maßnahmen, Planen, Organisieren und Überwachen des Einsatzes von Arbeits- und Fördermitteln\r\n")
                                    .ToString(),
                                TargetGroup = "Arbeitssuchende und Beschäftigte, die keinen oder einen fachfremden Berufsabschluss haben und sich weiter qualifizieren möchten und einen anerkannten Berufsabschluss anstreben.",
                                PreRequisitesDescription = "Mindestens Sprachniveau B1 (wünschenswert B2), hohe Lernmotivation und Konzentrationsfähigkeit, Kommunikationsfähigkeit, Führerschein. Die Eignung wird in einem persönlichen Beratungsgespräch geprüft",
                                LearningOutcomes = "Die TQ unterstützt Sie beim Einstieg in den Beruf Fachkraft für Lagerlogistik (m/w/d) und gibt Ihnen die Chance auf einen höher qualifizierten Arbeitsplatz. Überschaubare Lernphasen durch fachspezifische Ausrichtung der einzelnen Module, ermöglichen ein flexibles Lernen.",
                                Benefits = "Bei erfolgreicher Kompetenzfeststellung erhalten Sie nach jedem Modul ein Zertifikat inklusive Kompetenzfeststellungsergebnis, das bundesweit anerkannt ist.",
                                LoanOptions = "Förderfähig durch einen Bildungsgutschein, die Deutsche Rentenversicherung Bund und Land sowie das Qualifizierungschancengesetz",
                                Skills = "<Label TextType=\"Html\">\r\n    <![CDATA[\r\n       <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/ec66b111-d8d0-4516-88ba-ee0b6fe6f695\" target=\"_blank\">Kundenbestellungen bearbeiten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/27536739-c38b-45d2-9e96-1573b1d32fdd\" target=\"_blank\">Verpackungszubehör nutzen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/5d2e82cc-5943-4218-a459-a1956fad2b63\" target=\"_blank\">Lagerbestand verwalten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/d5fa1ed6-6cd8-41b9-8e78-2ba168ff3457\" target=\"_blank\">Bestellungen aus dem Online-Geschäft bearbeiten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/7838de1e-d65e-4a3f-b60b-e2213026116f\" target=\"_blank\">Geräte für den Materialtransport bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/ab2bb44a-3956-4028-8715-8b70b1960b99\" target=\"_blank\">schwere Gewichte heben</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/e0ae0101-ab8f-47a2-938b-ab0cc367b3b5\" target=\"_blank\">Lagerdatenbank pflegen</a>.\r\n   \t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/5b91b6d4-345e-4195-a078-218514871e7b\" target=\"_blank\">effiziente Nutzung von Lagerraum sicherstellen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/28b7d7fb-0483-4877-9aaa-f990f10f16f5\" target=\"_blank\">Pick-by-Voice-Kommissionierungssysteme bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/b2b8ec41-d6d1-470d-9e78-4eee515aaa3d\" target=\"_blank\">Kettensäge bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/23db1cab-e565-4a90-89f8-3a8685a20029\" target=\"_blank\">den physischen Zustand des Lagers pflegen und aufrechterhalten</a>.   \r\n    ]]>\r\n </Label>"
                                
            } },
            //{ 1, new CourseItem() {
            //                    Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
            //                    Ticks = DateTime.Now.Ticks,
            //                    Availability = CourseAvailability.Available,
            //                    CourseProviderId = (int) EduProviderId.Biwe,
            //                    TrainingProviderId = (int)EduProviderId.Biwe,
            //                    InstructorId = (int)EduProviderId.Biwe,
            //                    CourseTagType = CourseTagType.PartialQualification,
            //                    //Je nach Präferenz/Filter kann Angebot in Präsenz oder online stattfinden. (Wichtig: Falls Präsenz, dann Wohnort von Adrian entsprechend anpassen!)
            //                    CourseType = CourseType.InPerson,
            //                    Occurrence = OccurrenceType.FullTime,
            //                    //320 Unterrichtseinheiten je 45 min pro Modul + 120 Unterrichtseinheiten je 60 min im Unternehmen
            //                    Duration = TimeSpan.FromMinutes(129600),//320 Unterrichtseinheiten je 45 min pro Modul + 120 Unterrichtseinheiten je 60 min im Unternehmen - 6 Module
            //                    CourseUrl = new Uri("https://www.biwe-bbq.de/weiterbildungen/anzeige/tq-lagerlogistik"),
            //                    ExternalId = "https://www.biwe-bbq.de/weiterbildungen/anzeige/tq-lagerlogistik",
            //                    Language = "DE-DE",
            //                    KeyPhrases = "",
            //                    Title = "Modul 1 Wareneingang: Fachkraft für Lagerlogistik (m/w/d) - TQ/ETAPP",
            //                    ShortDescription = "Mit der Teilqualifizierung können Sie Schritt für Schritt in sechs Etappen den Berufsabschluss Fachkraft für Lagerlogistik (m/w/d) erreichen. Bei erfolgreicher Kompetenzfeststellung erhalten Sie nach jeder Etappe ein Zertifikat inklusive Kompetenzfeststellungsergebnis, das bundesweit anerkannt ist. Sie haben die Möglichkeit, sich zur Externenprüfung bei der zuständigen Kammer anzumelden und damit den Berufsabschluss zu erwerben.",
            //                    Description = new StringBuilder()
            //                        .AppendLine("Modul 1 -  Wareneingang\r\n")
            //                        .AppendLine("Annehmen der Güter\r\n")
            //                        .AppendLine("Entladen und Kontrollieren der Lieferung\r\n")
            //                        .AppendLine("Prüfen der Lieferung anhand der Begleitpapiere\r\n")
            //                        .AppendLine("Abschluss Flurförderschein\r\n")
            //                        .AppendLine("Dauer:\r\n )")
            //                        .AppendLine("11 Wochen\r\n(320 UE* und\r\n120 h**im Unternehmen)\r\n")
            //                        .AppendLine("1 Der Unterricht erfolgt in Präsenz an einem unserer Standorte.\r\n")
            //                        .AppendLine("2 Die Dauer verlängert sich ggf., sofern Feiertage in den Modulzeitraum fallen bzw. Urlaub geplant wird.\r\n")
            //                        .AppendLine("*UE = Unterrichtseinheit á 45 Minuten während der Theoriephase.\r\n")
            //                        .AppendLine("**h = Zeitstunde á 60 Minuten während der betrieblichen Qualifizierungsphase.\r\n")
            //                        .ToString(),
            //                    TargetGroup = "Arbeitssuchende und Beschäftigte, die keinen oder einen fachfremden Berufsabschluss haben und sich weiter qualifizieren möchten und einen anerkannten Berufsabschluss anstreben.",
            //                    PreRequisitesDescription = "Mindestens Sprachniveau B1 (wünschenswert B2), hohe Lernmotivation und Konzentrationsfähigkeit, Kommunikationsfähigkeit, Führerschein. Die Eignung wird in einem persönlichen Beratungsgespräch geprüft",
            //                    LearningOutcomes = "Die TQ unterstützt Sie beim Einstieg in den Beruf Fachkraft für Lagerlogistik (m/w/d) und gibt Ihnen die Chance auf einen höher qualifizierten Arbeitsplatz. Überschaubare Lernphasen durch fachspezifische Ausrichtung der einzelnen Module, ermöglichen ein flexibles Lernen.",
            //                    Benefits = "Bei erfolgreicher Kompetenzfeststellung erhalten Sie nach jedem Modul ein Zertifikat inklusive Kompetenzfeststellungsergebnis, das bundesweit anerkannt ist.",
            //                    LoanOptions = "Förderfähig durch einen Bildungsgutschein, die Deutsche Rentenversicherung Bund und Land sowie das Qualifizierungschancengesetz",
            //                    Skills = "<Label TextType=\"Html\">\r\n    <![CDATA[\r\n       <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/ec66b111-d8d0-4516-88ba-ee0b6fe6f695\" target=\"_blank\">Kundenbestellungen bearbeiten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/27536739-c38b-45d2-9e96-1573b1d32fdd\" target=\"_blank\">Verpackungszubehör nutzen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/5d2e82cc-5943-4218-a459-a1956fad2b63\" target=\"_blank\">Lagerbestand verwalten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/d5fa1ed6-6cd8-41b9-8e78-2ba168ff3457\" target=\"_blank\">Bestellungen aus dem Online-Geschäft bearbeiten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/7838de1e-d65e-4a3f-b60b-e2213026116f\" target=\"_blank\">Geräte für den Materialtransport bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/ab2bb44a-3956-4028-8715-8b70b1960b99\" target=\"_blank\">schwere Gewichte heben</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/e0ae0101-ab8f-47a2-938b-ab0cc367b3b5\" target=\"_blank\">Lagerdatenbank pflegen</a>.\r\n   \t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/5b91b6d4-345e-4195-a078-218514871e7b\" target=\"_blank\">effiziente Nutzung von Lagerraum sicherstellen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/28b7d7fb-0483-4877-9aaa-f990f10f16f5\" target=\"_blank\">Pick-by-Voice-Kommissionierungssysteme bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/b2b8ec41-d6d1-470d-9e78-4eee515aaa3d\" target=\"_blank\">Kettensäge bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/23db1cab-e565-4a90-89f8-3a8685a20029\" target=\"_blank\">den physischen Zustand des Lagers pflegen und aufrechterhalten</a>.   \r\n    ]]>\r\n </Label>"
            //    } },
            #endregion

            #endregion

            #region USECASE 2 - KERSTIN BESCHÄFTIGTE 81929 MÜNCHEN KAFFRAU EINZELHANDEL E-Commerce/Onlinehandel

            #endregion

            #region USECASE 3 - ARWA  BERUFLICHER AUFSTIEG AUGSBURG FACHINFORMATIKERIN IT Projektleitung / Führungsposition

            #endregion

            #endregion

            #region BBW
            /*
             ***********************************BBW*******************************************
             *********************************************************************************             
             ************** | ID RANGE 100 - 190                                |*************
             ************** | USECASE     | USERNAME      |   RANGE             |*************
             ************** ____________________________________________________|*************
             ************** |                                                   |*************
             ************** | 1           | ADRIAN        | 100 - 129           |*************
             ************** | 2           | KERSTIN       | 130 - 159           |*************
             ************** | 3           | ARWA          | 160 - 190           |*************
             ************** |___________________________________________________|*************
             ************************************BBW******************************************
             */

            #region USECASE 1 - ADRIAN ARBEITSSUCHEND 71634 LUDWIGSBURG Abschlussorientierte TQ LAGER

            #region FACHLAGERIST*IN
            //https://www.bfz.de/kurs/eca-90527/fachlageristin
            { 100, new CourseItem() {
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Ticks = DateTime.Now.Ticks,
                Availability = CourseAvailability.Available,
                CourseProviderId = (int) EduProviderId.Bbw,
                TrainingProviderId = (int)EduProviderId.Bbw,
                InstructorId = (int)EduProviderId.Bbw,
                CourseTagType = CourseTagType.PartialQualification,
                CourseType = CourseType.Online,
                Occurrence = OccurrenceType.FullTime,
                Duration = "11 Wochen",
                CourseUrl = new Uri("https://www.bfz.de/kurs/eca-90527/fachlageristin"),
                ExternalId = "https://www.bfz.de/kurs/eca-90527/fachlageristin",
                Language = "DE-DE",
                KeyPhrases = "",
                Title = "FACHLAGERIST*IN – TQdigital",
                ShortDescription = "Mit der Teilqualifizierung können Sie Schritt für Schritt in sechs Etappen den Berufsabschluss Fachkraft für Lagerlogistik (m/w/d) erreichen. Bei erfolgreicher Kompetenzfeststellung erhalten Sie nach jeder Etappe ein Zertifikat inklusive Kompetenzfeststellungsergebnis, das bundesweit anerkannt ist. Sie haben die Möglichkeit, sich zur Externenprüfung bei der zuständigen Kammer anzumelden und damit den Berufsabschluss zu erwerben.",
                Description = new StringBuilder()
                    .AppendLine("Neben den Fachinhalten werden auch Qualifikationen in den Feldern Kommunikation, Arbeitssicherheit und Umweltschutz vermittelt.\r\n")
                    .AppendLine("Arbeitsorganisation im Betreib\r\n")
                    .AppendLine("Einsatz von Arbeitsmitteln (Wiegen und Messen)\r\n")
                    .AppendLine("Wareneingang - Annahme von Gütern\r\n")
                    .AppendLine("Lagerung von Gütern\r\n")
                    .AppendLine("Kommissionierung und Verpackung\r\n")
                    .AppendLine("Versand von Gütern\r\n")
                    .AppendLine("Sicherheits- und Gesundheitsschutz, Umweltschutz\r\n")
                    .AppendLine("Logistische Prozesse und Qualitätssicherung\r\n")
                    .AppendLine("Die vermittelten Unterrichtsinhalte werden in der Praxis vertieft und in ausbildungsberechtigten Betrieben in der Region erweitert.")
                    .AppendLine("Hinweis zu unseren Lernmethoden\r\n")
                    .AppendLine("Live-​Online-Unterricht")
                    .AppendLine("Der Unterricht findet ausschließlich online statt – mit einem*r Dozent*in im virtuellen Klassenzimmer (Adobe Connect, Vitero o. Ä.).\r\n")
                    .AppendLine("Unsere Lernprozessbegleiter*innen unterstützen Sie im kompletten Schulungszeitraum.\r\n")
                    .AppendLine("Sie wollen die Qualifizierung von zu Hause aus machen? Hierzu benötigen Sie die Zustimmung des Kostenträgers. Bei digitalen Umschulungen ist zusätzlich das Einverständnis der regionalen Kammer erforderlich.")
                    .ToString(),
                TargetGroup =  new StringBuilder()
                    .AppendLine("Arbeitssuchende\r\n")
                    .AppendLine("Menschen ohne Berufsabschluss\r\n")
                    .AppendLine("Beschäftigte\r\n")
                    .AppendLine("Migrant*innen/Asylbewerber*innen\r\n")
                    .AppendLine("Menschen mit Behinderung\r\n")
                    .AppendLine("Soldat*innen")
                    .ToString(),
                PreRequisitesDescription =  new StringBuilder()
                    .AppendLine("Hauptschulabschluss, technisches Grundverständnis\r\n")
                    .AppendLine("Berufsabschluss oder mindestens dreijährige berufliche Praxis\r\n")
                    .AppendLine("gute Deutschkenntnisse in Wort und Schrift\r\n")
                    .AppendLine("Beratungsgespräch und bei Bedarf ein Eignungstest")
                    .ToString(),
                LearningOutcomes = "Nach erfolgreicher Teilnahme an der Umschulung sind Sie Fachlagerist*in.",
                Benefits = new StringBuilder()
                    .AppendLine("IHK-Abschluss\r\n")
                    .AppendLine("Prüfung vor der zuständigen Kammer\r\n")
                    .AppendLine("Träger-Zertifikat")
                    .ToString(),
                LoanOptions =new StringBuilder()
                    .AppendLine("Agentur für Arbeit\r\n")
                    .AppendLine("Berufsförderungsdienst der Bundeswehr\r\n")
                    .AppendLine("Bildungsgutschein (BGS)\r\n")
                    .AppendLine("Jobcenter\r\n")
                    .AppendLine("Knappschaft-Bahn-See\r\n")
                    .AppendLine("Qualifizierungschancengesetz\r\n")
                    .AppendLine("Renten- und Unfallversicherungsträger\r\n")
                    .AppendLine("Selbstzahler - individuelle Fördermöglichkeiten\r\n")
                    .AppendLine("Transfergesellschaften\r\n")
                    .ToString(),
                Skills = "<Label TextType=\"Html\">\r\n    <![CDATA[\r\n       <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/ec66b111-d8d0-4516-88ba-ee0b6fe6f695\" target=\"_blank\">Kundenbestellungen bearbeiten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/27536739-c38b-45d2-9e96-1573b1d32fdd\" target=\"_blank\">Verpackungszubehör nutzen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/5d2e82cc-5943-4218-a459-a1956fad2b63\" target=\"_blank\">Lagerbestand verwalten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/d5fa1ed6-6cd8-41b9-8e78-2ba168ff3457\" target=\"_blank\">Bestellungen aus dem Online-Geschäft bearbeiten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/7838de1e-d65e-4a3f-b60b-e2213026116f\" target=\"_blank\">Geräte für den Materialtransport bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/ab2bb44a-3956-4028-8715-8b70b1960b99\" target=\"_blank\">schwere Gewichte heben</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/e0ae0101-ab8f-47a2-938b-ab0cc367b3b5\" target=\"_blank\">Lagerdatenbank pflegen</a>.\r\n   \t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/5b91b6d4-345e-4195-a078-218514871e7b\" target=\"_blank\">effiziente Nutzung von Lagerraum sicherstellen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/28b7d7fb-0483-4877-9aaa-f990f10f16f5\" target=\"_blank\">Pick-by-Voice-Kommissionierungssysteme bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/b2b8ec41-d6d1-470d-9e78-4eee515aaa3d\" target=\"_blank\">Kettensäge bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/23db1cab-e565-4a90-89f8-3a8685a20029\" target=\"_blank\">den physischen Zustand des Lagers pflegen und aufrechterhalten</a>.   \r\n    ]]>\r\n </Label>"
            } },
            //https://www.bfz.de/kurs/eca-90531/fachlageristin-gueterbewegung-und-arbeitsschutz-modul-1?r%5Bl%5D%5Bd%5D=50&r%5Bl%5D%5Bl%5D=71634%20Ludwigsburg
            //13 Wochen, Vollzeit
            //MODUL 1
            { 101, new CourseItem() {
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Ticks = DateTime.Now.Ticks,
                Availability = CourseAvailability.Available,
                CourseProviderId = (int) EduProviderId.Bbw,
                TrainingProviderId = (int)EduProviderId.Bbw,
                InstructorId = (int)EduProviderId.Bbw,
                CourseTagType = CourseTagType.PartialQualification,
                CourseType = CourseType.Online,
                Occurrence = OccurrenceType.FullTime,
                Duration = "11 Wochen",
                CourseUrl = new Uri("https://www.bfz.de/kurs/eca-90531/fachlageristin-gueterbewegung-und-arbeitsschutz-modul-1"),
                ExternalId = "https://www.bfz.de/kurs/eca-90531/fachlageristin-gueterbewegung-und-arbeitsschutz-modul-1",
                Language = "DE-DE",
                KeyPhrases = "",
                Title = "FACHLAGERIST*IN – GÜTERBEWEGUNG UND ARBEITSSCHUTZ (MODUL 1)",
                ShortDescription = new StringBuilder()
                    .AppendLine("Teilqualifizierung zur schrittweisen Qualifizierung bis zum Berufsabschluss Fachlagerist*in\r\n")
                    .AppendLine("Als Fachlagerist*in sind Sie unter anderem in Bereichen wie der Warenannahme oder dem Lager tätig. Im Modul 1 Güterbewegung und Arbeitsschutz lernen Sie Grundlagen über den Arbeitsschutz, Umweltschutz und rechtliche Grundlagen kennen. Mit dieser Teilqualifizierung machen Sie den ersten Schritt, hin zu einem anerkannten Berufsabschluss, zu dem Sie sich mit weiteren Modulen qualifizieren können.")
                    .ToString(),
                Description = new StringBuilder()
                    .AppendLine("Die betriebliche Qualifizierungsphase von 4 Wochen erfolgt bei Betrieben in der Region\r\n")
                    .AppendLine("Inhalte\r\n")
                    .AppendLine("Einführung in das Berufsfeld und Vermittlung von Grundlagenwissen\r\n")
                    .AppendLine("Arbeitsschutz\r\n")
                    .AppendLine("Umweltschutz und rechtliche Grundlagen\r\n")
                    .AppendLine("Güter im Betrieb transportieren\r\n")
                    .AppendLine("Flurförderschein\r\n")
                    .AppendLine("Das Gütesiegel „Eine TQ besser!\" der ARBEITGEBERINITIATIVE TEILQUALIFIZIERUNG garantiert die Durchführung von Teilqualifizierungen nach festgelegten Standards.\r\n")
                    .AppendLine("Nach erfolgreicher Kompetenzfeststellung erhalten Sie das bfz vbw Zertifikat Teilqualifizierung Fachlagerist*in, Modul 1: Güterbewegung und Arbeitsschutz.")
                    .AppendLine("Hinweis zu unseren Lernmethoden\r\n")
                    .AppendLine("Live-​Online-Unterricht\r\n")
                    .AppendLine("Der Unterricht findet ausschließlich online statt – mit einem*r Dozent*in im virtuellen Klassenzimmer (Adobe Connect, Vitero o. Ä.).\r\n")
                    .AppendLine("Unsere Lernprozessbegleiter*innen unterstützen Sie im kompletten Schulungszeitraum.\r\n")
                    .AppendLine("Sie wollen die Qualifizierung von zu Hause aus machen? Hierzu benötigen Sie die Zustimmung des Kostenträgers. Bei digitalen Umschulungen ist zusätzlich das Einverständnis der regionalen Kammer erforderlich.")
                    .ToString(),
                TargetGroup =  new StringBuilder()
                    .AppendLine("Arbeitssuchende\r\n")
                    .AppendLine("Beschäftigte\r\n")
                    .AppendLine("Menschen ohne Berufsabschluss\r\n")
                    .ToString(),
                PreRequisitesDescription =  new StringBuilder()
                    .AppendLine("Folgende Voraussetzungen müssen Sie für die Teilqualifizierung mitbringen:\r\n")
                    .AppendLine("ausreichendes Sprachniveau (mind. B1, B2 in der digitalen Lernform),\r\n")
                    .AppendLine("hohe Lernmotivation,\r\n")
                    .AppendLine("sowohl technisches wie auch kaufmännisches Grundverständnis\r\n")
                    .AppendLine("Interesse an einer Arbeitsaufnahme im Lagerbereich\r\n")
                    .AppendLine("Ihre Eignung prüfen wir gemeinsam mit Ihnen in einem persönlichen Beratungsgespräch.")
                    .ToString(),
                LearningOutcomes = "Nach erfolgreicher Teilnahme an diesem Modul beherrschen Sie die Güterbewegung und den Arbeitsschutz der Teilqualifizierung \"Fachlagerist*in\".",
                Benefits = new StringBuilder()
                    .AppendLine("Teilqualifizierungen bieten Ihnen die Möglichkeit, in einzelnen Abschnitten Fachkenntnisse zu erwerben und sich diese Leistungen zertifizieren zu lassen. Wenn alle Module eines Berufs erfolgreich absolviert werden, ist eine Externenprüfung vor der zuständigen Kammer möglich.\r\n")
                    .AppendLine("Mit der Teilqualifizierung können Sie sich Schritt für Schritt in fünf Modulen zum*zur Fachlagerist*in mit IHK-Kammerprüfung qualifizieren.\r\n")
                    .ToString(),
                LoanOptions =new StringBuilder()
                    .AppendLine("Agentur für Arbeit\r\n")
                    .AppendLine("Berufsförderungsdienst der Bundeswehr\r\n")
                    .AppendLine("Berufsgenossenschaften")
                    .AppendLine("Bildungsgutschein (BGS)\r\n")
                    .AppendLine("Jobcenter\r\n")
                    .AppendLine("Knappschaft-Bahn-See\r\n")
                    .AppendLine("Qualifizierungschancengesetz\r\n")
                    .AppendLine("Renten- und Unfallversicherungsträger\r\n")
                    .AppendLine("Selbstzahler - individuelle Fördermöglichkeiten\r\n")
                    .AppendLine("Transfergesellschaften\r\n")
                    .ToString(),
                Skills = "<Label TextType=\"Html\">\r\n    <![CDATA[\r\n       <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/ec66b111-d8d0-4516-88ba-ee0b6fe6f695\" target=\"_blank\">Kundenbestellungen bearbeiten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/27536739-c38b-45d2-9e96-1573b1d32fdd\" target=\"_blank\">Verpackungszubehör nutzen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/5d2e82cc-5943-4218-a459-a1956fad2b63\" target=\"_blank\">Lagerbestand verwalten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/d5fa1ed6-6cd8-41b9-8e78-2ba168ff3457\" target=\"_blank\">Bestellungen aus dem Online-Geschäft bearbeiten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/7838de1e-d65e-4a3f-b60b-e2213026116f\" target=\"_blank\">Geräte für den Materialtransport bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/ab2bb44a-3956-4028-8715-8b70b1960b99\" target=\"_blank\">schwere Gewichte heben</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/e0ae0101-ab8f-47a2-938b-ab0cc367b3b5\" target=\"_blank\">Lagerdatenbank pflegen</a>.\r\n   \t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/5b91b6d4-345e-4195-a078-218514871e7b\" target=\"_blank\">effiziente Nutzung von Lagerraum sicherstellen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/28b7d7fb-0483-4877-9aaa-f990f10f16f5\" target=\"_blank\">Pick-by-Voice-Kommissionierungssysteme bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/b2b8ec41-d6d1-470d-9e78-4eee515aaa3d\" target=\"_blank\">Kettensäge bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/23db1cab-e565-4a90-89f8-3a8685a20029\" target=\"_blank\">den physischen Zustand des Lagers pflegen und aufrechterhalten</a>.   \r\n    ]]>\r\n </Label>"
            } },
            //https://www.bfz.de/kurs/eca-90580/fachlageristin-wareneingang-modul-2?r%5Bl%5D%5Bd%5D=50&r%5Bl%5D%5Bl%5D=71634%20Ludwigsburg#box_eventlist
            //12 Wochen, Vollzeit
            //MODUL 2
            { 102, new CourseItem() {
                                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                                Ticks = DateTime.Now.Ticks,
                                Availability = CourseAvailability.Available,
                                CourseProviderId = (int) EduProviderId.Bbw,
                                TrainingProviderId = (int)EduProviderId.Bbw,
                                InstructorId = (int)EduProviderId.Bbw,
                                CourseTagType = CourseTagType.PartialQualification,
                                CourseType = CourseType.Online,
                                Occurrence = OccurrenceType.FullTime,
                                Duration = "11 Wochen",
                                CourseUrl = new Uri("https://www.bfz.de/kurs/eca-90580/fachlageristin-wareneingang-modul-2"),
                                ExternalId = "https://www.bfz.de/kurs/eca-90580/fachlageristin-wareneingang-modul-2",
                                Language = "DE-DE",
                                KeyPhrases = "",
                                Title = "FACHLAGERIST*IN – WARENEINGANG (MODUL 2)",
                                ShortDescription = new StringBuilder()
                                    .AppendLine("Teilqualifizierung zur schrittweisen Qualifizierung bis zum Berufsabschluss Fachlagerist*in\r\n")
                                    .AppendLine("Als Fachlagerist*in nehmen Sie unter anderem Waren an und lagern diese dann sachgerecht. Außerdem machen Sie Lieferungen versandfertig oder leiten Güter an Empfänger im Betrieb weiter. Im Modul 2 Wareneingang wird Ihnen Wissen über die Güterannahme und Güterkontrolle, sowie das Tabellenkalkulations- und Lagerhaltungsprogramm vermittelt. Mit dieser Teilqualifizierung stellen Sie die Weichen hin zu einem anerkannten Berufsabschluss, für den Sie sich mit weiteren Modulen qualifizieren können.")
                                    .ToString(),
                                Description = new StringBuilder()
                                    .AppendLine("Die betriebliche Qualifizierungsphase von 4 Wochen erfolgt bei Betrieben in der Region\r\n")
                                    .AppendLine("Inhalte\r\n")
                                    .AppendLine("Grundlagen Beschaffung\r\n")
                                    .AppendLine("Güter annehmen und kontrollieren\r\n")
                                    .AppendLine("Tabellenkalkulations- und Lagerhaltungsprogramm\r\n")
                                    .AppendLine("Erfolgsunterstützung, Kompetenzfeststellung praktisch und theoretisch\r\n")

                                    .AppendLine("Das Gütesiegel „Eine TQ besser!\" der ARBEITGEBERINITIATIVE TEILQUALIFIZIERUNG garantiert die Durchführung von Teilqualifizierungen nach festgelegten Standards.\r\n")
                                    .AppendLine("Nach erfolgreicher Kompetenzfeststellung erhalten Sie das bfz vbw Zertifikat Teilqualifizierung Fachlagerist*in, Modul 2: Wareneingang.")
                                    .AppendLine("Hinweis zu unseren Lernmethoden\r\n")
                                    .AppendLine("Live-​Online-Unterricht\r\n")
                                    .AppendLine("Der Unterricht findet ausschließlich online statt – mit einem*r Dozent*in im virtuellen Klassenzimmer (Adobe Connect, Vitero o. Ä.).\r\n")
                                    .AppendLine("Unsere Lernprozessbegleiter*innen unterstützen Sie im kompletten Schulungszeitraum.\r\n")
                                    .AppendLine("Sie wollen die Qualifizierung von zu Hause aus machen? Hierzu benötigen Sie die Zustimmung des Kostenträgers. Bei digitalen Umschulungen ist zusätzlich das Einverständnis der regionalen Kammer erforderlich.")
                                    .ToString(),
                                TargetGroup =  new StringBuilder()
                                    .AppendLine("Arbeitssuchende\r\n")
                                    .AppendLine("Beschäftigte\r\n")
                                    .AppendLine("Menschen ohne Berufsabschluss\r\n")
                                    .ToString(),
                                PreRequisitesDescription =  new StringBuilder()
                                    .AppendLine("Folgende Voraussetzungen müssen Sie für die Teilqualifizierung mitbringen:\r\n")
                                    .AppendLine("ausreichendes Sprachniveau (mind. B1, B2 in der digitalen Lernform),\r\n")
                                    .AppendLine("hohe Lernmotivation,\r\n")
                                    .AppendLine("sowohl technisches wie auch kaufmännisches Grundverständnis\r\n")
                                    .AppendLine("Interesse an einer Arbeitsaufnahme im Lagerbereich\r\n")
                                    .AppendLine("Ihre Eignung prüfen wir gemeinsam mit Ihnen in einem persönlichen Beratungsgespräch.")
                                    .ToString(),
                                LearningOutcomes = "Nach erfolgreicher Teilnahme an diesem Modul beherrschen Sie den Wareneingang der Teilqualifizierung \"Fachlagerist*in\".",
                                Benefits = new StringBuilder()
                                    .AppendLine("Teilqualifizierungen bieten Ihnen die Möglichkeit, in einzelnen Abschnitten Fachkenntnisse zu erwerben und sich diese Leistungen zertifizieren zu lassen. Wenn alle Module eines Berufs erfolgreich absolviert werden, ist eine Externenprüfung vor der zuständigen Kammer möglich.\r\n")
                                    .AppendLine("Mit der Teilqualifizierung können Sie sich Schritt für Schritt in fünf Modulen zum*zur Fachlagerist*in mit IHK-Kammerprüfung qualifizieren.\r\n")
                                    .ToString(),
                                LoanOptions =new StringBuilder()
                                    .AppendLine("Agentur für Arbeit\r\n")
                                    .AppendLine("Berufsförderungsdienst der Bundeswehr\r\n")
                                    .AppendLine("Berufsgenossenschaften")
                                    .AppendLine("Bildungsgutschein (BGS)\r\n")
                                    .AppendLine("Jobcenter\r\n")
                                    .AppendLine("Knappschaft-Bahn-See\r\n")
                                    .AppendLine("Qualifizierungschancengesetz\r\n")
                                    .AppendLine("Renten- und Unfallversicherungsträger\r\n")
                                    .AppendLine("Selbstzahler - individuelle Fördermöglichkeiten\r\n")
                                    .AppendLine("Transfergesellschaften\r\n")
                                    .ToString(),
                                Skills = "<Label TextType=\"Html\">\r\n    <![CDATA[\r\n       <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/ec66b111-d8d0-4516-88ba-ee0b6fe6f695\" target=\"_blank\">Kundenbestellungen bearbeiten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/27536739-c38b-45d2-9e96-1573b1d32fdd\" target=\"_blank\">Verpackungszubehör nutzen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/5d2e82cc-5943-4218-a459-a1956fad2b63\" target=\"_blank\">Lagerbestand verwalten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/d5fa1ed6-6cd8-41b9-8e78-2ba168ff3457\" target=\"_blank\">Bestellungen aus dem Online-Geschäft bearbeiten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/7838de1e-d65e-4a3f-b60b-e2213026116f\" target=\"_blank\">Geräte für den Materialtransport bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/ab2bb44a-3956-4028-8715-8b70b1960b99\" target=\"_blank\">schwere Gewichte heben</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/e0ae0101-ab8f-47a2-938b-ab0cc367b3b5\" target=\"_blank\">Lagerdatenbank pflegen</a>.\r\n   \t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/5b91b6d4-345e-4195-a078-218514871e7b\" target=\"_blank\">effiziente Nutzung von Lagerraum sicherstellen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/28b7d7fb-0483-4877-9aaa-f990f10f16f5\" target=\"_blank\">Pick-by-Voice-Kommissionierungssysteme bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/b2b8ec41-d6d1-470d-9e78-4eee515aaa3d\" target=\"_blank\">Kettensäge bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/23db1cab-e565-4a90-89f8-3a8685a20029\" target=\"_blank\">den physischen Zustand des Lagers pflegen und aufrechterhalten</a>.   \r\n    ]]>\r\n </Label>"
            } },
             //https://www.bfz.de/kurs/eca-90041/fachlageristin-lagerhaltung-und-warenpflege-modul-3?r%5Bl%5D%5Bd%5D=50&r%5Bl%5D%5Bl%5D=71634%20Ludwigsburg%20#box_eventlist
            //MODUL 3
            { 103, new CourseItem() {
                                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                                Ticks = DateTime.Now.Ticks,
                                Availability = CourseAvailability.Available,
                                CourseProviderId = (int) EduProviderId.Bbw,
                                TrainingProviderId = (int)EduProviderId.Bbw,
                                InstructorId = (int)EduProviderId.Bbw,
                                CourseTagType = CourseTagType.PartialQualification,
                                CourseType = CourseType.Online,
                                Occurrence = OccurrenceType.FullTime,
                                Duration = "11 Wochen", 
                                CourseUrl = new Uri("https://www.bfz.de/kurs/eca-90041/fachlageristin-lagerhaltung-und-warenpflege-modul-3"),
                                ExternalId = "https://www.bfz.de/kurs/eca-90041/fachlageristin-lagerhaltung-und-warenpflege-modul-3",
                                Language = "DE-DE",
                                KeyPhrases = "",
                                Title = "FACHLAGERIST*IN – LAGERHALTUNG UND WARENPFLEGE (MODUL 3)",
                                ShortDescription = new StringBuilder()
                                    .AppendLine("Teilqualifizierung zur schrittweisen Qualifizierung bis zum Berufsabschluss Fachlagerist*in\r\n")
                                    .AppendLine("Als Fachlagerist*in sind Sie für die Warenannahmen und deren sachgerechte Lagerung verantwortlich. Zudem stellen Sie zum Beispiel Lieferungen zum weiteren Versand zusammen oder leiten Güter an Empfänger im Betrieb weiter. In diesem Modul lernen Sie die Grundlagen über die Optimierung von logistischen Prozesse und die Güterbearbeitung kennen. Mit dieser Teilqualifizierung gehen Sie einen wichtigen Schritt hin zu einem anerkannten Berufsabschluss, zu dem Sie sich mit weiteren Modulen qualifizieren können.")
                                    .ToString(),
                                Description = new StringBuilder()
                                    .AppendLine("Die betriebliche Qualifizierungsphase von 4 Wochen erfolgt bei Betrieben in der Region\r\n")
                                    .AppendLine("Inhalte\r\n")
                                    .AppendLine("Güter lagern\r\n")
                                    .AppendLine("Güter bearbeiten inkl. Inventur\r\n")
                                    .AppendLine("Logistische Prozesse optimieren (Grundlagen)\r\n")
                                    .AppendLine("Kennzahlen ermitteln und auswerten (Grundlagen)\r\n")

                                    .AppendLine("Das Gütesiegel „Eine TQ besser!\" der ARBEITGEBERINITIATIVE TEILQUALIFIZIERUNG garantiert die Durchführung von Teilqualifizierungen nach festgelegten Standards.\r\n")
                                    .AppendLine("Nach erfolgreicher Kompetenzfeststellung erhalten Sie das bfz vbw Zertifikat Teilqualifizierung Fachlagerist*in, Modul 3: Lagerhaltung und Warenpflege.")
                                    .AppendLine("Hinweis zu unseren Lernmethoden\r\n")
                                    .AppendLine("Live-​Online-Unterricht\r\n")
                                    .AppendLine("Der Unterricht findet ausschließlich online statt – mit einem*r Dozent*in im virtuellen Klassenzimmer (Adobe Connect, Vitero o. Ä.).\r\n")
                                    .AppendLine("Unsere Lernprozessbegleiter*innen unterstützen Sie im kompletten Schulungszeitraum.\r\n")
                                    .AppendLine("Sie wollen die Qualifizierung von zu Hause aus machen? Hierzu benötigen Sie die Zustimmung des Kostenträgers. Bei digitalen Umschulungen ist zusätzlich das Einverständnis der regionalen Kammer erforderlich.")
                                    .ToString(),
                                TargetGroup =  new StringBuilder()
                                    .AppendLine("Arbeitssuchende\r\n")
                                    .AppendLine("Beschäftigte\r\n")
                                    .AppendLine("Menschen ohne Berufsabschluss\r\n")
                                    .ToString(),
                                PreRequisitesDescription =  new StringBuilder()
                                    .AppendLine("Folgende Voraussetzungen müssen Sie für die Teilqualifizierung mitbringen:\r\n")
                                    .AppendLine("ausreichendes Sprachniveau (mind. B1, B2 in der digitalen Lernform),\r\n")
                                    .AppendLine("hohe Lernmotivation,\r\n")
                                    .AppendLine("sowohl technisches wie auch kaufmännisches Grundverständnis\r\n")
                                    .AppendLine("Interesse an einer Arbeitsaufnahme im Lagerbereich\r\n")
                                    .AppendLine("Ihre Eignung prüfen wir gemeinsam mit Ihnen in einem persönlichen Beratungsgespräch.")
                                    .ToString(),
                                LearningOutcomes = "Nach erfolgreicher Teilnahme an diesem Modul beherrschen Sie den Wareneingang der Teilqualifizierung \"Fachlagerist*in\".",
                                Benefits = new StringBuilder()
                                    .AppendLine("Teilqualifizierungen bieten Ihnen die Möglichkeit, in einzelnen Abschnitten Fachkenntnisse zu erwerben und sich diese Leistungen zertifizieren zu lassen. Wenn alle Module eines Berufs erfolgreich absolviert werden, ist eine Externenprüfung vor der zuständigen Kammer möglich.\r\n")
                                    .AppendLine("Mit der Teilqualifizierung können Sie sich Schritt für Schritt in fünf Modulen zum*zur Fachlagerist*in mit IHK-Kammerprüfung qualifizieren.\r\n")
                                    .ToString(),
                                LoanOptions =new StringBuilder()
                                    .AppendLine("Agentur für Arbeit\r\n")
                                    .AppendLine("Berufsförderungsdienst der Bundeswehr\r\n")
                                    .AppendLine("Berufsgenossenschaften")
                                    .AppendLine("Bildungsgutschein (BGS)\r\n")
                                    .AppendLine("Jobcenter\r\n")
                                    .AppendLine("Knappschaft-Bahn-See\r\n")
                                    .AppendLine("Qualifizierungschancengesetz\r\n")
                                    .AppendLine("Renten- und Unfallversicherungsträger\r\n")
                                    .AppendLine("Selbstzahler - individuelle Fördermöglichkeiten\r\n")
                                    .AppendLine("Transfergesellschaften\r\n")
                                    .ToString(),
                                Skills = "<Label TextType=\"Html\">\r\n    <![CDATA[\r\n       <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/ec66b111-d8d0-4516-88ba-ee0b6fe6f695\" target=\"_blank\">Kundenbestellungen bearbeiten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/27536739-c38b-45d2-9e96-1573b1d32fdd\" target=\"_blank\">Verpackungszubehör nutzen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/5d2e82cc-5943-4218-a459-a1956fad2b63\" target=\"_blank\">Lagerbestand verwalten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/d5fa1ed6-6cd8-41b9-8e78-2ba168ff3457\" target=\"_blank\">Bestellungen aus dem Online-Geschäft bearbeiten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/7838de1e-d65e-4a3f-b60b-e2213026116f\" target=\"_blank\">Geräte für den Materialtransport bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/ab2bb44a-3956-4028-8715-8b70b1960b99\" target=\"_blank\">schwere Gewichte heben</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/e0ae0101-ab8f-47a2-938b-ab0cc367b3b5\" target=\"_blank\">Lagerdatenbank pflegen</a>.\r\n   \t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/5b91b6d4-345e-4195-a078-218514871e7b\" target=\"_blank\">effiziente Nutzung von Lagerraum sicherstellen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/28b7d7fb-0483-4877-9aaa-f990f10f16f5\" target=\"_blank\">Pick-by-Voice-Kommissionierungssysteme bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/b2b8ec41-d6d1-470d-9e78-4eee515aaa3d\" target=\"_blank\">Kettensäge bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/23db1cab-e565-4a90-89f8-3a8685a20029\" target=\"_blank\">den physischen Zustand des Lagers pflegen und aufrechterhalten</a>.   \r\n    ]]>\r\n </Label>"
            } },
            //https://www.bfz.de/kurs/eca-90405/fachlageristin-kommissionierung-und-verpackung-modul-4?r%5Bl%5D%5Bd%5D=50&r%5Bl%5D%5Bl%5D=71634%20Ludwigsburg#box_eventlist
            //MODUL 4
            { 104, new CourseItem() {
                    Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                    Ticks = DateTime.Now.Ticks,
                    Availability = CourseAvailability.Available,
                    CourseProviderId = (int) EduProviderId.Bbw,
                    TrainingProviderId = (int)EduProviderId.Bbw,
                    InstructorId = (int)EduProviderId.Bbw,
                    CourseTagType = CourseTagType.PartialQualification,
                    CourseType = CourseType.Online,
                    Occurrence = OccurrenceType.FullTime,
                    Duration = "11 Wochen",
                    CourseUrl = new Uri("https://www.bfz.de/kurs/eca-90041/fachlageristin-lagerhaltung-und-warenpflege-modul-3"),
                    ExternalId = "https://www.bfz.de/kurs/eca-90041/fachlageristin-lagerhaltung-und-warenpflege-modul-3",
                    Language = "DE-DE",
                    KeyPhrases = "",
                    Title = "FACHLAGERIST*IN – KOMMISSIONIERUNG UND VERPACKUNG (MODUL 4)",
                    ShortDescription = new StringBuilder()
                        .AppendLine("Teilqualifizierung zur schrittweisen Qualifizierung bis zum Berufsabschluss Fachlagerist*in\r\n")
                        .AppendLine("Als Fachlagerist*in sind Sie unter anderem für die Annahme von Waren an und die sachgerechte Lagerung zuständig. Des Weiteren stellen Sie Lieferungen für den bereit oder leiten Güter an Empfänger im Betrieb weiter. Dieses Modul vermittelt Ihnen Kenntnisse über die Kommissionierung und die Verpackung. Mit dieser Teilqualifizierung bestreiten Sie einen wichtigen Teil des Weges zu einem anerkannten Berufsabschluss, zu dem Sie sich mit weiteren Modulen qualifizieren können.\r\n")
                        .ToString(),
                    Description = new StringBuilder()
                        .AppendLine("Die betriebliche Qualifizierungsphase von 4 Wochen erfolgt bei Betrieben in der Region\r\n")
                        .AppendLine("Inhalte\r\n")
                        .AppendLine("Güter kommissionieren\r\n")
                        .AppendLine("Güter verpacken\r\n")
                        
                        .AppendLine("Das Gütesiegel „Eine TQ besser!\" der ARBEITGEBERINITIATIVE TEILQUALIFIZIERUNG garantiert die Durchführung von Teilqualifizierungen nach festgelegten Standards.\r\n")
                        .AppendLine("Nach erfolgreicher Kompetenzfeststellung erhalten Sie das bfz vbw Zertifikat Teilqualifizierung Fachlagerist*in, Modul 4: Kommissionierung und Verpackung.")
                        .AppendLine("Hinweis zu unseren Lernmethoden\r\n")
                        .AppendLine("Live-​Online-Unterricht\r\n")
                        .AppendLine("Der Unterricht findet ausschließlich online statt – mit einem*r Dozent*in im virtuellen Klassenzimmer (Adobe Connect, Vitero o. Ä.).\r\n")
                        .AppendLine("Unsere Lernprozessbegleiter*innen unterstützen Sie im kompletten Schulungszeitraum.\r\n")
                        .AppendLine("Sie wollen die Qualifizierung von zu Hause aus machen? Hierzu benötigen Sie die Zustimmung des Kostenträgers. Bei digitalen Umschulungen ist zusätzlich das Einverständnis der regionalen Kammer erforderlich.")
                        .ToString(),
                    TargetGroup =  new StringBuilder()
                        .AppendLine("Arbeitssuchende\r\n")
                        .AppendLine("Beschäftigte\r\n")
                        .AppendLine("Menschen ohne Berufsabschluss\r\n")
                        .ToString(),
                    PreRequisitesDescription =  new StringBuilder()
                        .AppendLine("Folgende Voraussetzungen müssen Sie für die Teilqualifizierung mitbringen:\r\n")
                        .AppendLine("ausreichendes Sprachniveau (mind. B1, B2 in der digitalen Lernform),\r\n")
                        .AppendLine("hohe Lernmotivation,\r\n")
                        .AppendLine("sowohl technisches wie auch kaufmännisches Grundverständnis\r\n")
                        .AppendLine("Interesse an einer Arbeitsaufnahme im Lagerbereich\r\n")
                        .AppendLine("Ihre Eignung prüfen wir gemeinsam mit Ihnen in einem persönlichen Beratungsgespräch.")
                        .ToString(),
                    LearningOutcomes = "Nach erfolgreicher Teilnahme an diesem Modul beherrschen Sie den Wareneingang der Teilqualifizierung \"Fachlagerist*in\".",
                    Benefits = new StringBuilder()
                        .AppendLine("Teilqualifizierungen bieten Ihnen die Möglichkeit, in einzelnen Abschnitten Fachkenntnisse zu erwerben und sich diese Leistungen zertifizieren zu lassen. Wenn alle Module eines Berufs erfolgreich absolviert werden, ist eine Externenprüfung vor der zuständigen Kammer möglich.\r\n")
                        .AppendLine("Mit der Teilqualifizierung können Sie sich Schritt für Schritt in fünf Modulen zum*zur Fachlagerist*in mit IHK-Kammerprüfung qualifizieren.\r\n")
                        .ToString(),
                    LoanOptions =new StringBuilder()
                        .AppendLine("Agentur für Arbeit\r\n")
                        .AppendLine("Berufsförderungsdienst der Bundeswehr\r\n")
                        .AppendLine("Berufsgenossenschaften")
                        .AppendLine("Bildungsgutschein (BGS)\r\n")
                        .AppendLine("Jobcenter\r\n")
                        .AppendLine("Knappschaft-Bahn-See\r\n")
                        .AppendLine("Qualifizierungschancengesetz\r\n")
                        .AppendLine("Renten- und Unfallversicherungsträger\r\n")
                        .AppendLine("Selbstzahler - individuelle Fördermöglichkeiten\r\n")
                        .AppendLine("Transfergesellschaften\r\n")
                        .ToString(),
                    Skills = "<Label TextType=\"Html\">\r\n    <![CDATA[\r\n       <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/ec66b111-d8d0-4516-88ba-ee0b6fe6f695\" target=\"_blank\">Kundenbestellungen bearbeiten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/27536739-c38b-45d2-9e96-1573b1d32fdd\" target=\"_blank\">Verpackungszubehör nutzen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/5d2e82cc-5943-4218-a459-a1956fad2b63\" target=\"_blank\">Lagerbestand verwalten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/d5fa1ed6-6cd8-41b9-8e78-2ba168ff3457\" target=\"_blank\">Bestellungen aus dem Online-Geschäft bearbeiten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/7838de1e-d65e-4a3f-b60b-e2213026116f\" target=\"_blank\">Geräte für den Materialtransport bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/ab2bb44a-3956-4028-8715-8b70b1960b99\" target=\"_blank\">schwere Gewichte heben</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/e0ae0101-ab8f-47a2-938b-ab0cc367b3b5\" target=\"_blank\">Lagerdatenbank pflegen</a>.\r\n   \t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/5b91b6d4-345e-4195-a078-218514871e7b\" target=\"_blank\">effiziente Nutzung von Lagerraum sicherstellen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/28b7d7fb-0483-4877-9aaa-f990f10f16f5\" target=\"_blank\">Pick-by-Voice-Kommissionierungssysteme bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/b2b8ec41-d6d1-470d-9e78-4eee515aaa3d\" target=\"_blank\">Kettensäge bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/23db1cab-e565-4a90-89f8-3a8685a20029\" target=\"_blank\">den physischen Zustand des Lagers pflegen und aufrechterhalten</a>.   \r\n    ]]>\r\n </Label>"
            } },
             //https://www.bfz.de/kurs/eca-90871/fachlageristin-gueterverladung-und-versand-modul-5?r%5Bl%5D%5Bd%5D=50&r%5Bl%5D%5Bl%5D=71634%20Ludwigsburg%20#box_eventlist
            //MODUL 5
            { 105, new CourseItem() {
                    Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                    Ticks = DateTime.Now.Ticks,
                    Availability = CourseAvailability.Available,
                    CourseProviderId = (int) EduProviderId.Bbw,
                    TrainingProviderId = (int)EduProviderId.Bbw,
                    InstructorId = (int)EduProviderId.Bbw,
                    CourseTagType = CourseTagType.PartialQualification,
                    CourseType = CourseType.Online,
                    Occurrence = OccurrenceType.FullTime,
                    Duration = "11 Wochen",
                    CourseUrl = new Uri("https://www.bfz.de/kurs/eca-90041/fachlageristin-lagerhaltung-und-warenpflege-modul-3"),
                    ExternalId = "https://www.bfz.de/kurs/eca-90041/fachlageristin-lagerhaltung-und-warenpflege-modul-3",
                    Language = "DE-DE",
                    KeyPhrases = "",
                    Title = "FACHLAGERIST*IN – GÜTERVERLADUNG UND VERSAND (MODUL 5)",
                    ShortDescription = new StringBuilder()
                        .AppendLine("Teilqualifizierung zur schrittweisen Qualifizierung bis zum Berufsabschluss Fachlagerist*in\r\n")
                        .AppendLine("Als Fachlagerist*in nehmen Sie unter anderem Waren an und lagern diese sachgerecht. Sie machen des Weiteren Lieferungen versandfertig oder leiten verschiedene Güter an Empfänger im Betrieb weiter. Güter verladen und versenden sind die Hauptinhalte von diesem Modul 5. Mit dieser Teilqualifizierung stellen Sie die Weichen für einen anerkannten Berufsabschluss, für dem Sie sich mit weiteren Modulen qualifizieren können.\r\n")
                        .ToString(),
                    Description = new StringBuilder()
                        .AppendLine("Die betriebliche Qualifizierungsphase von 4 Wochen erfolgt bei Betrieben in der Region\r\n")
                        .AppendLine("Inhalte\r\n")
                        .AppendLine("Güter verladen inkl. Ladungssicherung\r\n")
                        .AppendLine("Güter versenden\r\n")

                        .AppendLine("Das Gütesiegel „Eine TQ besser!\" der ARBEITGEBERINITIATIVE TEILQUALIFIZIERUNG garantiert die Durchführung von Teilqualifizierungen nach festgelegten Standards.\r\n")
                        .AppendLine("Nach erfolgreicher Kompetenzfeststellung erhalten Sie das bfz vbw Zertifikat Teilqualifizierung Fachlagerist*in, Modul 5: Güterverladung und Versand.")
                        .AppendLine("Hinweis zu unseren Lernmethoden\r\n")
                        .AppendLine("Live-​Online-Unterricht\r\n")
                        .AppendLine("Der Unterricht findet ausschließlich online statt – mit einem*r Dozent*in im virtuellen Klassenzimmer (Adobe Connect, Vitero o. Ä.).\r\n")
                        .AppendLine("Unsere Lernprozessbegleiter*innen unterstützen Sie im kompletten Schulungszeitraum.\r\n")
                        .AppendLine("Sie wollen die Qualifizierung von zu Hause aus machen? Hierzu benötigen Sie die Zustimmung des Kostenträgers. Bei digitalen Umschulungen ist zusätzlich das Einverständnis der regionalen Kammer erforderlich.")
                        .ToString(),
                    TargetGroup =  new StringBuilder()
                        .AppendLine("Arbeitssuchende\r\n")
                        .AppendLine("Beschäftigte\r\n")
                        .AppendLine("Menschen ohne Berufsabschluss\r\n")
                        .ToString(),
                    PreRequisitesDescription =  new StringBuilder()
                        .AppendLine("Folgende Voraussetzungen müssen Sie für die Teilqualifizierung mitbringen:\r\n")
                        .AppendLine("ausreichendes Sprachniveau (mind. B1, B2 in der digitalen Lernform),\r\n")
                        .AppendLine("hohe Lernmotivation,\r\n")
                        .AppendLine("sowohl technisches wie auch kaufmännisches Grundverständnis\r\n")
                        .AppendLine("Interesse an einer Arbeitsaufnahme im Lagerbereich\r\n")
                        .AppendLine("Ihre Eignung prüfen wir gemeinsam mit Ihnen in einem persönlichen Beratungsgespräch.")
                        .ToString(),
                    LearningOutcomes = "Nach erfolgreicher Teilnahme an diesem Modul beherrschen Sie den Wareneingang der Teilqualifizierung \"Fachlagerist*in\".",
                    Benefits = new StringBuilder()
                        .AppendLine("Teilqualifizierungen bieten Ihnen die Möglichkeit, in einzelnen Abschnitten Fachkenntnisse zu erwerben und sich diese Leistungen zertifizieren zu lassen. Wenn alle Module eines Berufs erfolgreich absolviert werden, ist eine Externenprüfung vor der zuständigen Kammer möglich.\r\n")
                        .AppendLine("Mit der Teilqualifizierung können Sie sich Schritt für Schritt in fünf Modulen zum*zur Fachlagerist*in mit IHK-Kammerprüfung qualifizieren.\r\n")
                        .ToString(),
                    LoanOptions =new StringBuilder()
                        .AppendLine("Agentur für Arbeit\r\n")
                        .AppendLine("Berufsförderungsdienst der Bundeswehr\r\n")
                        .AppendLine("Berufsgenossenschaften")
                        .AppendLine("Bildungsgutschein (BGS)\r\n")
                        .AppendLine("Jobcenter\r\n")
                        .AppendLine("Knappschaft-Bahn-See\r\n")
                        .AppendLine("Qualifizierungschancengesetz\r\n")
                        .AppendLine("Renten- und Unfallversicherungsträger\r\n")
                        .AppendLine("Selbstzahler - individuelle Fördermöglichkeiten\r\n")
                        .AppendLine("Transfergesellschaften\r\n")
                        .ToString(),
                    Skills = "<Label TextType=\"Html\">\r\n    <![CDATA[\r\n       <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/ec66b111-d8d0-4516-88ba-ee0b6fe6f695\" target=\"_blank\">Kundenbestellungen bearbeiten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/27536739-c38b-45d2-9e96-1573b1d32fdd\" target=\"_blank\">Verpackungszubehör nutzen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/5d2e82cc-5943-4218-a459-a1956fad2b63\" target=\"_blank\">Lagerbestand verwalten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/d5fa1ed6-6cd8-41b9-8e78-2ba168ff3457\" target=\"_blank\">Bestellungen aus dem Online-Geschäft bearbeiten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/7838de1e-d65e-4a3f-b60b-e2213026116f\" target=\"_blank\">Geräte für den Materialtransport bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/ab2bb44a-3956-4028-8715-8b70b1960b99\" target=\"_blank\">schwere Gewichte heben</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/e0ae0101-ab8f-47a2-938b-ab0cc367b3b5\" target=\"_blank\">Lagerdatenbank pflegen</a>.\r\n   \t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/5b91b6d4-345e-4195-a078-218514871e7b\" target=\"_blank\">effiziente Nutzung von Lagerraum sicherstellen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/28b7d7fb-0483-4877-9aaa-f990f10f16f5\" target=\"_blank\">Pick-by-Voice-Kommissionierungssysteme bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/b2b8ec41-d6d1-470d-9e78-4eee515aaa3d\" target=\"_blank\">Kettensäge bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/23db1cab-e565-4a90-89f8-3a8685a20029\" target=\"_blank\">den physischen Zustand des Lagers pflegen und aufrechterhalten</a>.   \r\n    ]]>\r\n </Label>"
            } },
            #endregion

            #region FACHKRAFT FÜR LAGERLOGISTIK (IHK)
            //FACHKRAFT FÜR LAGERLOGISTIK (IHK)
            //ONLINE - LINK geht nicht!!
            //https://www.bbw-seminare.de/seminarinfos/eca/22134/fachkraft-fuer-lagerlogistik-ihk-o/
            //https://cc.bingj.com/cache.aspx?q=bbw+Fachkraft+f%c3%bcr+Lagerlogistik+(IHK)+Online&d=4611669763630311&mkt=de-DE&setlang=en-US&w=qKHRQsAhyfQxJV_NDE-Zm_QpHUUXbwqh
            //https://www.bfz.de/kurs/eca-90871/fachlageristin-gueterverladung-und-versand-modul-5?r%5Bl%5D%5Bd%5D=50&r%5Bl%5D%5Bl%5D=71634%20Ludwigsburg%20#box_eventlist
            //{ 106, new CourseItem() {
            //        Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
            //        Ticks = DateTime.Now.Ticks,
            //        Availability = CourseAvailability.Available,
            //        CourseProviderId = (int) EduProviderId.Bbw,
            //        TrainingProviderId = (int)EduProviderId.Bbw,
            //        InstructorId = (int)EduProviderId.Bbw,
            //        CourseTagType = CourseTagType.PartialQualification,
            //        CourseType = CourseType.Online,
            //        Occurrence = OccurrenceType.FullTime,
            //        Duration = TimeSpan.FromMinutes(14400),
            //        CourseUrl = new Uri("https://www.bfz.de/kurs/eca-90041/fachlageristin-lagerhaltung-und-warenpflege-modul-3"),
            //        ExternalId = "https://www.bfz.de/kurs/eca-90041/fachlageristin-lagerhaltung-und-warenpflege-modul-3",
            //        Language = "DE-DE",
            //        KeyPhrases = "",
            //        Title = "FACHKRAFT FÜR LAGERLOGISTIK (IHK)",
            //        ShortDescription = new StringBuilder()
            //            .AppendLine("Umschulung im digitalen Lernformat in Vollzeit\r\n")
            //            .AppendLine("Die Logistik ist als elementarer Bestandteil in allen Bereichen des wirtschaftlichen Lebens zu finden: Handel, Industrie, Verkehr, etc. Deshalb haben ausgebildete Fachkräfte für Lagerlogistik vielfältige Integrationsmöglichkeiten auf dem Arbeitsmarkt. Sie sind sind verantwortlich für Annahme und Versand von Gütern sowie deren Kontrolle und Lagerung. Sie erstellen Begleitpapiere für den Güterversand, optimieren logistische Prozesse und berechnen Lagerkapazitäten.\r\n")
            //            .ToString(),
            //        Description = new StringBuilder()
            //            .AppendLine("Die betriebliche Qualifizierungsphase von 4 Wochen erfolgt bei Betrieben in der Region\r\n")
            //            .AppendLine("Inhalte\r\n")
            //            .AppendLine("Güter verladen inkl. Ladungssicherung\r\n")
            //            .AppendLine("Güter versenden\r\n")

            //            .AppendLine("Das Gütesiegel „Eine TQ besser!\" der ARBEITGEBERINITIATIVE TEILQUALIFIZIERUNG garantiert die Durchführung von Teilqualifizierungen nach festgelegten Standards.\r\n")
            //            .AppendLine("Nach erfolgreicher Kompetenzfeststellung erhalten Sie das bfz vbw Zertifikat Teilqualifizierung Fachlagerist*in, Modul 5: Güterverladung und Versand.")
            //            .AppendLine("Hinweis zu unseren Lernmethoden\r\n")
            //            .AppendLine("Live-​Online-Unterricht\r\n")
            //            .AppendLine("Der Unterricht findet ausschließlich online statt – mit einem*r Dozent*in im virtuellen Klassenzimmer (Adobe Connect, Vitero o. Ä.).\r\n")
            //            .AppendLine("Unsere Lernprozessbegleiter*innen unterstützen Sie im kompletten Schulungszeitraum.\r\n")
            //            .AppendLine("Sie wollen die Qualifizierung von zu Hause aus machen? Hierzu benötigen Sie die Zustimmung des Kostenträgers. Bei digitalen Umschulungen ist zusätzlich das Einverständnis der regionalen Kammer erforderlich.")
            //            .ToString(),
            //        TargetGroup =  new StringBuilder()
            //            .AppendLine("Arbeitssuchende\r\n")
            //            .AppendLine("Beschäftigte\r\n")
            //            .AppendLine("Menschen ohne Berufsabschluss\r\n")
            //            .ToString(),
            //        PreRequisitesDescription =  new StringBuilder()
            //            .AppendLine("Folgende Voraussetzungen müssen Sie für die Teilqualifizierung mitbringen:\r\n")
            //            .AppendLine("ausreichendes Sprachniveau (mind. B1, B2 in der digitalen Lernform),\r\n")
            //            .AppendLine("hohe Lernmotivation,\r\n")
            //            .AppendLine("sowohl technisches wie auch kaufmännisches Grundverständnis\r\n")
            //            .AppendLine("Interesse an einer Arbeitsaufnahme im Lagerbereich\r\n")
            //            .AppendLine("Ihre Eignung prüfen wir gemeinsam mit Ihnen in einem persönlichen Beratungsgespräch.")
            //            .ToString(),
            //        LearningOutcomes = "Nach erfolgreicher Teilnahme an diesem Modul beherrschen Sie den Wareneingang der Teilqualifizierung \"Fachlagerist*in\".",
            //        Benefits = new StringBuilder()
            //            .AppendLine("Teilqualifizierungen bieten Ihnen die Möglichkeit, in einzelnen Abschnitten Fachkenntnisse zu erwerben und sich diese Leistungen zertifizieren zu lassen. Wenn alle Module eines Berufs erfolgreich absolviert werden, ist eine Externenprüfung vor der zuständigen Kammer möglich.\r\n")
            //            .AppendLine("Mit der Teilqualifizierung können Sie sich Schritt für Schritt in fünf Modulen zum*zur Fachlagerist*in mit IHK-Kammerprüfung qualifizieren.\r\n")
            //            .ToString(),
            //        LoanOptions =new StringBuilder()
            //            .AppendLine("Agentur für Arbeit\r\n")
            //            .AppendLine("Berufsförderungsdienst der Bundeswehr\r\n")
            //            .AppendLine("Berufsgenossenschaften")
            //            .AppendLine("Bildungsgutschein (BGS)\r\n")
            //            .AppendLine("Jobcenter\r\n")
            //            .AppendLine("Knappschaft-Bahn-See\r\n")
            //            .AppendLine("Qualifizierungschancengesetz\r\n")
            //            .AppendLine("Renten- und Unfallversicherungsträger\r\n")
            //            .AppendLine("Selbstzahler - individuelle Fördermöglichkeiten\r\n")
            //            .AppendLine("Transfergesellschaften\r\n")
            //            .ToString(),
            //        Skills = "<Label TextType=\"Html\">\r\n    <![CDATA[\r\n       <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/ec66b111-d8d0-4516-88ba-ee0b6fe6f695\" target=\"_blank\">Kundenbestellungen bearbeiten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/27536739-c38b-45d2-9e96-1573b1d32fdd\" target=\"_blank\">Verpackungszubehör nutzen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/5d2e82cc-5943-4218-a459-a1956fad2b63\" target=\"_blank\">Lagerbestand verwalten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/d5fa1ed6-6cd8-41b9-8e78-2ba168ff3457\" target=\"_blank\">Bestellungen aus dem Online-Geschäft bearbeiten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/7838de1e-d65e-4a3f-b60b-e2213026116f\" target=\"_blank\">Geräte für den Materialtransport bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/ab2bb44a-3956-4028-8715-8b70b1960b99\" target=\"_blank\">schwere Gewichte heben</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/e0ae0101-ab8f-47a2-938b-ab0cc367b3b5\" target=\"_blank\">Lagerdatenbank pflegen</a>.\r\n   \t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/5b91b6d4-345e-4195-a078-218514871e7b\" target=\"_blank\">effiziente Nutzung von Lagerraum sicherstellen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/28b7d7fb-0483-4877-9aaa-f990f10f16f5\" target=\"_blank\">Pick-by-Voice-Kommissionierungssysteme bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/b2b8ec41-d6d1-470d-9e78-4eee515aaa3d\" target=\"_blank\">Kettensäge bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/23db1cab-e565-4a90-89f8-3a8685a20029\" target=\"_blank\">den physischen Zustand des Lagers pflegen und aufrechterhalten</a>.   \r\n    ]]>\r\n </Label>"
            //} }
            #endregion

            #endregion

            #region USECASE 2 - KERSTIN BESCHÄFTIGTE 81929 MÜNCHEN KAFFRAU EINZELHANDEL E-Commerce/Onlinehandel

            #endregion

            #region USECASE 3 - ARWA  BERUFLICHER AUFSTIEG AUGSBURG FACHINFORMATIKERIN IT Projektleitung / Führungsposition

            #endregion

            #endregion

            #region TÜV
            /*
             ***********************************TÜV*******************************************
             *********************************************************************************             
             ************** | ID RANGE 200 - 290                                |*************
             ************** | USECASE     | USERNAME      |   RANGE             |*************
             ************** ____________________________________________________|*************
             ************** |                                                   |*************
             ************** | 1           | ADRIAN        | 200 - 229           |*************
             ************** | 2           | KERSTIN       | 230 - 259           |*************
             ************** | 3           | ARWA          | 260 - 290           |*************
             ************** |___________________________________________________|*************
             ************************************TÜV******************************************
             */
            #region USECASE 1 - ADRIAN ARBEITSSUCHEND 71634 LUDWIGSBURG Abschlussorientierte TQ LAGER

            //{ 200, new CourseItem() {
            //        Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
            //        Ticks = DateTime.Now.Ticks,
            //        Availability = CourseAvailability.Available,
            //        CourseProviderId = (int) EduProviderId.Bbw,
            //        TrainingProviderId = (int)EduProviderId.Bbw,
            //        InstructorId = (int)EduProviderId.Bbw,
            //        CourseTagType = CourseTagType.PartialQualification,
            //        CourseType = CourseType.Online,
            //        Occurrence = OccurrenceType.FullTime,
            //        Duration = TimeSpan.FromMinutes(14400),
            //        CourseUrl = new Uri("https://akademie.tuv.com/weiterbildungen/grundlagen-der-lagerwirtschaft-kompakt-557431"),
            //        ExternalId = "https://akademie.tuv.com/weiterbildungen/grundlagen-der-lagerwirtschaft-kompakt-557431",
            //        Language = "DE-DE",
            //        KeyPhrases = "",
            //        Title = "Grundlagen der Lagerwirtschaft kompakt",
            //        ShortDescription = new StringBuilder()
            //            .AppendLine("Lager - Wareneingang - Warenumschlag - Lagertechnik - Lagerprozesse - Lagerkennzahlen.")
            //            .ToString(),
            //        Description = new StringBuilder()
            //            .AppendLine("Im Lager werden Produkte gelagert und umgeschlagen, kommissioniert, konfektioniert, verpackt, zum Versand vorbereitet oder auch Retouren bearbeitet. Mit diesem Seminar erhalten Sie einen fundierten und praxisorientierten Einblick in die Aufgaben der Lager- und Materialwirtschaft bei der betrieblichen Leistungserstellung und die wichtigsten Abwicklungsprozesse in der Lagerlogistik. Damit verfügen Sie über das notwendige \"Handwerkszeug\" für Ihre täglichen, operativen Aufgaben in einer modernen betrieblichen Lagerlogistik.\r\n")
            //            .AppendLine("Inhalte\r\n")
            //            .AppendLine("Güter verladen inkl. Ladungssicherung\r\n")
            //            .AppendLine("Güter versenden\r\n")

            //            .AppendLine("Das Gütesiegel „Eine TQ besser!\" der ARBEITGEBERINITIATIVE TEILQUALIFIZIERUNG garantiert die Durchführung von Teilqualifizierungen nach festgelegten Standards.\r\n")
            //            .AppendLine("Nach erfolgreicher Kompetenzfeststellung erhalten Sie das bfz vbw Zertifikat Teilqualifizierung Fachlagerist*in, Modul 5: Güterverladung und Versand.")
            //            .AppendLine("Hinweis zu unseren Lernmethoden\r\n")
            //            .AppendLine("Live-​Online-Unterricht\r\n")
            //            .AppendLine("Der Unterricht findet ausschließlich online statt – mit einem*r Dozent*in im virtuellen Klassenzimmer (Adobe Connect, Vitero o. Ä.).\r\n")
            //            .AppendLine("Unsere Lernprozessbegleiter*innen unterstützen Sie im kompletten Schulungszeitraum.\r\n")
            //            .AppendLine("Sie wollen die Qualifizierung von zu Hause aus machen? Hierzu benötigen Sie die Zustimmung des Kostenträgers. Bei digitalen Umschulungen ist zusätzlich das Einverständnis der regionalen Kammer erforderlich.")
            //            .ToString(),
            //        TargetGroup =  new StringBuilder()
            //            .AppendLine("Arbeitssuchende\r\n")
            //            .AppendLine("Beschäftigte\r\n")
            //            .AppendLine("Menschen ohne Berufsabschluss\r\n")
            //            .ToString(),
            //        PreRequisitesDescription =  new StringBuilder()
            //            .AppendLine("Folgende Voraussetzungen müssen Sie für die Teilqualifizierung mitbringen:\r\n")
            //            .AppendLine("ausreichendes Sprachniveau (mind. B1, B2 in der digitalen Lernform),\r\n")
            //            .AppendLine("hohe Lernmotivation,\r\n")
            //            .AppendLine("sowohl technisches wie auch kaufmännisches Grundverständnis\r\n")
            //            .AppendLine("Interesse an einer Arbeitsaufnahme im Lagerbereich\r\n")
            //            .AppendLine("Ihre Eignung prüfen wir gemeinsam mit Ihnen in einem persönlichen Beratungsgespräch.")
            //            .ToString(),
            //        LearningOutcomes = "Nach erfolgreicher Teilnahme an diesem Modul beherrschen Sie den Wareneingang der Teilqualifizierung \"Fachlagerist*in\".",
            //        Benefits = new StringBuilder()
            //            .AppendLine("Teilqualifizierungen bieten Ihnen die Möglichkeit, in einzelnen Abschnitten Fachkenntnisse zu erwerben und sich diese Leistungen zertifizieren zu lassen. Wenn alle Module eines Berufs erfolgreich absolviert werden, ist eine Externenprüfung vor der zuständigen Kammer möglich.\r\n")
            //            .AppendLine("Mit der Teilqualifizierung können Sie sich Schritt für Schritt in fünf Modulen zum*zur Fachlagerist*in mit IHK-Kammerprüfung qualifizieren.\r\n")
            //            .ToString(),
            //        LoanOptions =new StringBuilder()
            //            .AppendLine("Agentur für Arbeit\r\n")
            //            .AppendLine("Berufsförderungsdienst der Bundeswehr\r\n")
            //            .AppendLine("Berufsgenossenschaften")
            //            .AppendLine("Bildungsgutschein (BGS)\r\n")
            //            .AppendLine("Jobcenter\r\n")
            //            .AppendLine("Knappschaft-Bahn-See\r\n")
            //            .AppendLine("Qualifizierungschancengesetz\r\n")
            //            .AppendLine("Renten- und Unfallversicherungsträger\r\n")
            //            .AppendLine("Selbstzahler - individuelle Fördermöglichkeiten\r\n")
            //            .AppendLine("Transfergesellschaften\r\n")
            //            .ToString(),
            //        Skills = "<Label TextType=\"Html\">\r\n    <![CDATA[\r\n       <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/ec66b111-d8d0-4516-88ba-ee0b6fe6f695\" target=\"_blank\">Kundenbestellungen bearbeiten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/27536739-c38b-45d2-9e96-1573b1d32fdd\" target=\"_blank\">Verpackungszubehör nutzen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/5d2e82cc-5943-4218-a459-a1956fad2b63\" target=\"_blank\">Lagerbestand verwalten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/d5fa1ed6-6cd8-41b9-8e78-2ba168ff3457\" target=\"_blank\">Bestellungen aus dem Online-Geschäft bearbeiten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/7838de1e-d65e-4a3f-b60b-e2213026116f\" target=\"_blank\">Geräte für den Materialtransport bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/ab2bb44a-3956-4028-8715-8b70b1960b99\" target=\"_blank\">schwere Gewichte heben</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/e0ae0101-ab8f-47a2-938b-ab0cc367b3b5\" target=\"_blank\">Lagerdatenbank pflegen</a>.\r\n   \t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/5b91b6d4-345e-4195-a078-218514871e7b\" target=\"_blank\">effiziente Nutzung von Lagerraum sicherstellen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/28b7d7fb-0483-4877-9aaa-f990f10f16f5\" target=\"_blank\">Pick-by-Voice-Kommissionierungssysteme bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/b2b8ec41-d6d1-470d-9e78-4eee515aaa3d\" target=\"_blank\">Kettensäge bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/23db1cab-e565-4a90-89f8-3a8685a20029\" target=\"_blank\">den physischen Zustand des Lagers pflegen und aufrechterhalten</a>.   \r\n    ]]>\r\n </Label>"
            //} }

            #endregion

            #region USECASE 2 - KERSTIN BESCHÄFTIGTE 81929 MÜNCHEN KAFFRAU EINZELHANDEL E-Commerce/Onlinehandel

            #endregion

            #region USECASE 3 - ARWA  BERUFLICHER AUFSTIEG AUGSBURG FACHINFORMATIKERIN IT Projektleitung / Führungsposition

            #endregion

            
            #endregion
        };

        /// <summary>
        /// Ids 0 - 90 biwe
        /// Ids 0 - 29 biwe UseCase 1
        /// Ids 30 - 59 biwe UseCase 2
        /// Ids 60 - 90 biwe UseCase 3
        /// 
        /// Ids 100 - 190 bbw
        /// Ids 100 - 129 bbw UseCase 1
        /// Ids 130 - 159 bbw UseCase 2
        /// Ids 160 - 190 bbw UseCase 3
        /// 
        /// Ids 200 - 290 tüv
        /// Ids 200 - 229 bbw UseCase 1
        /// Ids 230 - 259 bbw UseCase 2
        /// Ids 260 - 290 bbw UseCase 3
        /// </summary>
        public Dictionary<long, CourseAppointment> Appointments = new Dictionary<long, CourseAppointment>()
        {


            #region BIWE
            /*
             ***********************************BIWE******************************************
             *********************************************************************************             
             ************** | ID RANGE 0 - 90                                   |*************
             ************** | USECASE     | USERNAME      |   RANGE             |*************
             ************** ____________________________________________________|*************
             ************** |                                                   |*************
             ************** | 1           | ADRIAN        | 0 - 29              |*************
             ************** | 2           | KERSTIN       | 30 - 59             |*************
             ************** | 3           | ARWA          | 60 - 90             |*************
             ************** |___________________________________________________|*************
             ************************************BIWE*****************************************
             */

            #region USECASE 1 - ADRIAN ARBEITSSUCHEND 71634 LUDWIGSBURG Abschlussorientierte TQ LAGER

            {0,new CourseAppointment() {
                Id = 0,
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                //AppointmentType = AppointmentType.Unknow,
                //AvailableSeats = -1,
                //BookingContact = 0,
                //BookingUrl = new Uri("https://invite-apollo.app/TODO"),
                CourseId = -1,
                BookingCode = String.Empty,
                //Location = "Stuttgart",
                //OccurrenceType = OccurrenceType.FullTime,
                //Type = CourseType.InPerson,
                //StartDate = new DateTime(2023, 01, 02),
                //Language = "DE-DE",
                Summary = "",
            }},
            {1,new CourseAppointment() {
                Id = 1,
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                //AppointmentType = AppointmentType.Unknow,
                //AvailableSeats = -1,
                //BookingContact = 0,
                //BookingUrl = new Uri("https://invite-apollo.app/TODO"),
                CourseId = -1,
                BookingCode = String.Empty,
                //Location = "Stuttgart",
                //OccurrenceType = OccurrenceType.FullTime,
                //Type = CourseType.InPerson,
                //StartDate = new DateTime(2023, 02, 06),
                //Language = "DE-DE",
                Summary = "",
            }},
            {2,new CourseAppointment() {
                Id = 2,
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                //AppointmentType = AppointmentType.Unknow,
                //AvailableSeats = -1,
                //BookingContact = 0,
                //BookingUrl = new Uri("https://invite-apollo.app/TODO"),
                CourseId = -1,
                BookingCode = String.Empty,
                //Location = "Stuttgart",
                //OccurrenceType = OccurrenceType.FullTime,
                //Type = CourseType.InPerson,
                //StartDate = new DateTime(2023, 03, 06),
                //Language = "DE-DE",
                Summary = "",
            }},
            {3,new CourseAppointment() {
                Id = 3,
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                //AppointmentType = AppointmentType.Unknow,
                //AvailableSeats = -1,
                //BookingContact = 0,
                //BookingUrl = new Uri("https://invite-apollo.app/TODO"),
                CourseId = -1,
                BookingCode = String.Empty,
                //Location = "Stuttgart",
                //OccurrenceType = OccurrenceType.FullTime,
                //Type = CourseType.InPerson,
                //StartDate = new DateTime(2023, 04, 03),
                //Language = "DE-DE",
                Summary = "",
            }},

            #endregion

            #region USECASE 2 - KERSTIN BESCHÄFTIGTE 81929 MÜNCHEN KAFFRAU EINZELHANDEL E-Commerce/Onlinehandel

            #endregion

            #region USECASE 3 - ARWA  BERUFLICHER AUFSTIEG AUGSBURG FACHINFORMATIKERIN IT Projektleitung / Führungsposition

            #endregion

            #endregion

            #region BBW
            /*
             ***********************************BBW*******************************************
             *********************************************************************************             
             ************** | ID RANGE 100 - 190                                |*************
             ************** | USECASE     | USERNAME      |   RANGE             |*************
             ************** ____________________________________________________|*************
             ************** |                                                   |*************
             ************** | 1           | ADRIAN        | 100 - 129           |*************
             ************** | 2           | KERSTIN       | 130 - 159           |*************
             ************** | 3           | ARWA          | 160 - 190           |*************
             ************** |___________________________________________________|*************
             ************************************BBW******************************************
             */
            #region USECASE 1 - ADRIAN ARBEITSSUCHEND 71634 LUDWIGSBURG Abschlussorientierte TQ LAGER

            {100,new CourseAppointment() {
                Id = 100,
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                //AppointmentType = AppointmentType.Unknow,
                //AvailableSeats = -1,
                //BookingContact = 0,
                //BookingUrl = new Uri("https://invite-apollo.app/TODO"),
                CourseId = -1,
                BookingCode = String.Empty,
                //Location = "Stuttgart",
                //OccurrenceType = OccurrenceType.FullTime,
                //Type = CourseType.InPerson,
                //StartDate = new DateTime(2023, 01, 02),
                //Language = "DE-DE",
                Summary = "",
            }},


            #endregion

            #region USECASE 2 - KERSTIN BESCHÄFTIGTE 81929 MÜNCHEN KAFFRAU EINZELHANDEL E-Commerce/Onlinehandel

            #endregion

            #region USECASE 3 - ARWA  BERUFLICHER AUFSTIEG AUGSBURG FACHINFORMATIKERIN IT Projektleitung / Führungsposition

            #endregion

            #endregion

            #region TÜV
            /*
             ***********************************TÜV*******************************************
             *********************************************************************************             
             ************** | ID RANGE 200 - 290                                |*************
             ************** | USECASE     | USERNAME      |   RANGE             |*************
             ************** ____________________________________________________|*************
             ************** |                                                   |*************
             ************** | 1           | ADRIAN        | 200 - 229           |*************
             ************** | 2           | KERSTIN       | 230 - 259           |*************
             ************** | 3           | ARWA          | 260 - 290           |*************
             ************** |___________________________________________________|*************
             ************************************TÜV******************************************
             */
            #region USECASE 1 - ADRIAN ARBEITSSUCHEND 71634 LUDWIGSBURG Abschlussorientierte TQ LAGER

            {200,new CourseAppointment() {
                Id = 200,
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                //AppointmentType = AppointmentType.Unknow,
                //AvailableSeats = -1,
                //BookingContact = 0,
                //BookingUrl = new Uri("https://invite-apollo.app/TODO"),
                CourseId = -1,
                BookingCode = String.Empty,
                //Location = "Stuttgart",
                //OccurrenceType = OccurrenceType.FullTime,
                //Type = CourseType.InPerson,
                //StartDate = new DateTime(2023, 01, 02),
                //Language = "DE-DE",
                Summary = "",
            }},


            #endregion

            #region USECASE 2 - KERSTIN BESCHÄFTIGTE 81929 MÜNCHEN KAFFRAU EINZELHANDEL E-Commerce/Onlinehandel

            #endregion

            #region USECASE 3 - ARWA  BERUFLICHER AUFSTIEG AUGSBURG FACHINFORMATIKERIN IT Projektleitung / Führungsposition

            #endregion

            #endregion



        };


        public UseCaseCourseData()
        {
            CourseContactRelations = new();

            //course biwe setup tq lager
            CourseAppointment appointment = Appointments[0];
            CourseItem course = CourseList[0];
            SetCourseAppointmentRelations(appointment, course);

            appointment = Appointments[1];
            course = CourseList[0];
            SetCourseAppointmentRelations(appointment, course);

            appointment = Appointments[2];
            course = CourseList[0];
            SetCourseAppointmentRelations(appointment, course);

            appointment = Appointments[3];
            course = CourseList[0];
            SetCourseAppointmentRelations(appointment, course);


            List<CourseItem> usecaseCourseLists = new List<CourseItem>();


            for (int j = 0; j < 2; j++)
                SetContactCourseRelations(course, Contacts[j]);


            //TODO: Add other UseCases and Courses
            //Usecase 1 
            for (int i = 0; i < 20; i++)
            {
                if(CourseList.ContainsKey(i))
                    usecaseCourseLists.Add(CourseList[i]);
            }

            for (int i = 60; i < 80; i++)
            {
                if (CourseList.ContainsKey(i))
                    usecaseCourseLists.Add(CourseList[i]);
            }

            usecaseCourses.Add(0,usecaseCourseLists);

            System.Console.WriteLine(ProviderList.Count);


        }

        private void AddBiweCourse(int usecase)
        {

        }

        private void AddBbwCourse(int usecase)
        {

        }


        private void AddTuevCourse(int usecase)
        {

        }

        private void AddApointment(int usecase, Course course)
        {

        }

        private void SetCourseAppointmentRelations(CourseAppointment appointment, CourseItem course)
        {
            appointment.CourseId = course.Id;
        }

        private void SetContactCourseRelations(CourseItem course, CourseContact contact)
        {
            int index = CourseContactRelations.Count;
            CourseContactRelations.Add(index, new CourseContactRelation{Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"), Ticks = DateTime.Now.Ticks, Id = index, CourseContactId = contact.Id, CourseId = course.Id});
        }


    }
}
