using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using Invite.Apollo.App.Graph.Assessment.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.Course.Enums;
using Invite.Apollo.App.Graph.Common.Models.Course;

namespace Invite.Apollo.App.Graph.Assessment.Data
{

    public class EduProvider
    {
        public EduProvider()
        {
            //course biwe setup tq lager
            Appointment appointment = Appointments[0];
            Course course = CourseList[0];
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
        }

        private static void SetCourseAppointmentRelations(Appointment appointment, Course course)
        {
            appointment.Course = course;
            appointment.CourseId = course.Id;
            course.Appointments.Add(appointment);
        }

        public static Dictionary<string, long> provider = new Dictionary<string, long>()
        {
            { "Biwe", 0 }, { "bbw", 1 }, { "TüV Rheinland Akademie",2 },
        };

        public Dictionary<long, Contact> Contacts = new Dictionary<long, Contact>()
        {
            { 0, new Contact() { Id = 0,
                ContactMail = "info-stuttgart@biwe.de",
                ContactName = "BBQ Stuttgart",
                ContactPhone = "0711 252875-19",
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Url = new Uri("https://www.biwe-bbq.de/ueber-uns/vor-ort/stuttgart-mittlerer-pfad")}},
            { 1, new Contact() { Id = 1,
                ContactMail = "",
                ContactName = "",
                ContactPhone = "",
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Url = new Uri("")}},
            { 2, new Contact() { Id = 2,
                ContactMail = "",
                ContactName = "",
                ContactPhone = "",
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Url = new Uri("")}},
            { 3, new Contact() {Id = 3,
                ContactMail = "",
                ContactName = "",
                ContactPhone = "",
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Url = new Uri("")}},
            { 4, new Contact() {Id = 4,
                ContactMail = "",
                ContactName = "",
                ContactPhone = "",
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Url = new Uri("")}},
            { 5, new Contact() {Id = 5,
                ContactMail = "",
                ContactName = "",
                ContactPhone = "",
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Url = new Uri("")}},
            { 6, new Contact() {Id = 6,
                ContactMail = "",
                ContactName = "",
                ContactPhone = "",
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Url = new Uri("")}},
            { 7, new Contact() {Id = 7,
                ContactMail = "",
                ContactName = "",
                ContactPhone = "",
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Url = new Uri("")}},
            { 8, new Contact() { Id = 8,
                ContactMail = "",
                ContactName = "",
                ContactPhone = "",
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Url = new Uri("")
            }},
            { 9, new Contact() { Id = 9,
                ContactMail = "",
                ContactName = "",
                ContactPhone = "",
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Url = new Uri("")
            }}



        };

        public Dictionary<long, Course> CourseList = new Dictionary<long, Course>()
        {
            {
                0, new Course() {
                                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                                Ticks = DateTime.Now.Ticks,
                                Availability = CourseAvailability.Available,
                                Appointments = new List<Appointment>(),
                                CourseContacts = new List<CourseHasContacts>(),
                                CourseProviderId = 0,
                                CourseTagType = CourseTagType.PartialQualification,
                                //Je nach Präferenz/Filter kann Angebot in Präsenz oder online stattfinden. (Wichtig: Falls Präsenz, dann Wohnort von Adrian entsprechend anpassen!)
                                CourseType = CourseType.InPerson,
                                CourseUrl = new Uri("https://www.biwe-bbq.de/weiterbildungen/anzeige/tq-lagerlogistik"),
                                Description = new StringBuilder()
                                    .AppendLine("Bei der Teilqualifizierung wird der anerkannte Ausbildungsberuf Fachkraft für Lagerlogistik (m/w/d) in folgende Module aufgegliedert.")
                                    .AppendLine("TQ-Modul 1 - Wareneingang (11 Wochen, davon 3 Wochen im Unternehmen) In diesem Modul werden folgende Inhalte vermittelt: Annehmen der Güter, Entladen und Kontrollieren der Lieferung, Prüfen der Lieferung anhand der Begleitpapiere, Abschluss Flurförderschein\r\n")
                                    .AppendLine("TQ-Modul 2 - Lagerung (11 Wochen, davon 3 Wochen im Unternehmen) In diesem Modul werden folgende Inhalte vermittelt: Auspacken, Sortieren und Lagern der Güter anforderungsgerecht und nach wirtschaftlichen Grundsätzen unter Beachtung der Lagerordnung , Transportieren und Zuleiten der Güter zum betrieblichen Bestimmungsort\r\n")
                                    .AppendLine("TQ-Modul 3 - Innerbetriebliche Logistik und Kontrolle (11 Wochen, davon 3 Wochen im Unternehmen) In diesem Modul werden folgende Inhalte vermittelt: Anwenden betrieblicher Informations- und Kommunikationssysteme, Standardsoftware und arbeitsplatzbezogener Software, Anwenden fachspezifischer Fremdsprachenkenntnisse, Durchführen von Bestandskontrollen und Maßnahmen der Bestandspflege\r\n")
                                    .AppendLine("TQ-Modul 4 - Kommissionierung und Endkontrolle (11 Wochen, davon 3 Wochen im Unternehmen) In diesem Modul werden folgende Inhalte vermittelt: Erstellen von Ladelisten/Beladeplänen unter Beachtung von Ladevorschriften, Kennzeichnen, Beschriften und Sichern von Sendungen nach gesetzlichen Vorgaben, Kommissionieren und Verpacken der Güter für Sendungen und Zusammenstellen zu Ladeeinheiten\r\n")
                                    .AppendLine("TQ-Modul 5 - Versand (11 Wochen, davon 3 Wochen im Unternehmen) In diesem Modul werden folgende Inhalte vermittelt: Kennzeichnen, Beschriften und Sichern von Sendungen nach gesetzlichen Vorgaben, Bearbeiten der Versand- und Begleitpapiere und Erstellen von Versandaufzeichnungen\r\n")
                                    .AppendLine("TQ-Modul 6 - Arbeitsorganisation und Qualitätssicherung (11 Wochen, davon 3 Wochen im Unternehmen) In diesem Modul werden folgende Inhalte vermittelt: Mitwirken bei logistischen Planungs- und Organisationsprozessen, Mitwirken bei qualitätssichernden Maßnahmen, Planen, Organisieren und Überwachen des Einsatzes von Arbeits- und Fördermitteln\r\n")
                                    .ToString(),
                                //320 Unterrichtseinheiten je 45 min pro Modul + 120 Unterrichtseinheiten je 60 min im Unternehmen
                                Duration = TimeSpan.FromMinutes(129600),//320 Unterrichtseinheiten je 45 min pro Modul + 120 Unterrichtseinheiten je 60 min im Unternehmen - 6 Module
                                ExternalId = "https://www.biwe-bbq.de/weiterbildungen/anzeige/tq-lagerlogistik",
                                InstructorId = 0,
                                KeyPhrases = "",
                                Language = "DE-DE",
                                Occurrence = OccurrenceType.FullTime,
                                PreRequisitesDescription = "Mindestens Sprachniveau B1 (wünschenswert B2), hohe Lernmotivation und Konzentrationsfähigkeit, Kommunikationsfähigkeit, Führerschein. Die Eignung wird in einem persönlichen Beratungsgespräch geprüft",
                                ShortDescription = "Mit der Teilqualifizierung können Sie Schritt für Schritt in sechs Etappen den Berufsabschluss Fachkraft für Lagerlogistik (m/w/d) erreichen. Bei erfolgreicher Kompetenzfeststellung erhalten Sie nach jeder Etappe ein Zertifikat inklusive Kompetenzfeststellungsergebnis, das bundesweit anerkannt ist. Sie haben die Möglichkeit, sich zur Externenprüfung bei der zuständigen Kammer anzumelden und damit den Berufsabschluss zu erwerben.",
                                TargetGroup = "Arbeitssuchende und Beschäftigte, die keinen oder einen fachfremden Berufsabschluss haben und sich weiter qualifizieren möchten und einen anerkannten Berufsabschluss anstreben.",
                                TrainingProviderId = EduProvider.provider["Biwe"],
                                Title = "Fachkraft für Lagerlogistik (m/w/d) - TQ/ETAPP",
                                LearningOutcomes = "Die TQ unterstützt Sie beim Einstieg in den Beruf Fachkraft für Lagerlogistik (m/w/d) und gibt Ihnen die Chance auf einen höher qualifizierten Arbeitsplatz. Überschaubare Lernphasen durch fachspezifische Ausrichtung der einzelnen Module, ermöglichen ein flexibles Lernen.",
                                Benefits = "Bei erfolgreicher Kompetenzfeststellung erhalten Sie nach jedem Modul ein Zertifikat inklusive Kompetenzfeststellungsergebnis, das bundesweit anerkannt ist.",
                                LoanOptions = "Förderfähig durch einen Bildungsgutschein, die Deutsche Rentenversicherung Bund und Land sowie das Qualifizierungschancengesetz",
                                Skills = "<Label TextType=\"Html\">\r\n    <![CDATA[\r\n       <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/ec66b111-d8d0-4516-88ba-ee0b6fe6f695\" target=\"_blank\">Kundenbestellungen bearbeiten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/27536739-c38b-45d2-9e96-1573b1d32fdd\" target=\"_blank\">Verpackungszubehör nutzen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/5d2e82cc-5943-4218-a459-a1956fad2b63\" target=\"_blank\">Lagerbestand verwalten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/d5fa1ed6-6cd8-41b9-8e78-2ba168ff3457\" target=\"_blank\">Bestellungen aus dem Online-Geschäft bearbeiten</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/7838de1e-d65e-4a3f-b60b-e2213026116f\" target=\"_blank\">Geräte für den Materialtransport bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/ab2bb44a-3956-4028-8715-8b70b1960b99\" target=\"_blank\">schwere Gewichte heben</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/e0ae0101-ab8f-47a2-938b-ab0cc367b3b5\" target=\"_blank\">Lagerdatenbank pflegen</a>.\r\n   \t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/5b91b6d4-345e-4195-a078-218514871e7b\" target=\"_blank\">effiziente Nutzung von Lagerraum sicherstellen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/28b7d7fb-0483-4877-9aaa-f990f10f16f5\" target=\"_blank\">Pick-by-Voice-Kommissionierungssysteme bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/b2b8ec41-d6d1-470d-9e78-4eee515aaa3d\" target=\"_blank\">Kettensäge bedienen</a>.\r\n\t   <a href=\"https://esco.ec.europa.eu/de/classification/skills?uri=http://data.europa.eu/esco/skill/23db1cab-e565-4a90-89f8-3a8685a20029\" target=\"_blank\">den physischen Zustand des Lagers pflegen und aufrechterhalten</a>.   \r\n    ]]>\r\n </Label>"
                }

            },
            { 1, new Course() {
                                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                                Ticks = DateTime.Now.Ticks,
                                Availability = CourseAvailability.Available,
                                Appointments = new List<Appointment>(),
                                CourseContacts = new List<CourseHasContacts>(),
                                CourseProviderId = 0,
                                CourseTagType = CourseTagType.PartialQualification,
                                //Vollzeit, Online am jew. Standort in Bayern mit Lernprozessbegleiter*in oder mit Zustimmung Kostenträger „von zu Hause“
                                CourseType = CourseType.InPerson,
                                CourseUrl = new Uri("https://www.bfz.de/kurs/eca-90531/fachlageristin-gueterbewegung-und-arbeitsschutz-modul-1"),
                                Description = "Mit der Teilqualifizierung können Sie Schritt für Schritt in sechs Etappen den Berufsabschluss Fachkraft für Lagerlogistik (m/w/d) erreichen. Bei erfolgreicher Kompetenzfeststellung erhalten Sie nach jeder Etappe ein Zertifikat inklusive Kompetenzfeststellungsergebnis, das bundesweit anerkannt ist. Sie haben die Möglichkeit, sich zur Externenprüfung bei der zuständigen Kammer anzumelden und damit den Berufsabschluss zu erwerben.",
                                //- Modul 1 - Fachlagerist*in – Güterbewegung und Arbeitsschutz: 13.02.2023 – 19.05.2023
                                //- Modul 3 - Fachlagerist*in – Lagerhaltung und Warenpflege: 06.02.2023 – 12.05.2023
                                //- Modul 4 - Fachlagerist*in – Kommissionierung und Verpackung: 23.01.2023 – 28.04.2023

                                Duration = TimeSpan.Zero,
                                ExternalId = "https://www.bfz.de/kurs/eca-90531/fachlageristin-gueterbewegung-und-arbeitsschutz-modul-1",
                                InstructorId = 0,
                                KeyPhrases = "",
                                Language = "DE-DE",
                                Occurrence = OccurrenceType.FullTime,
                                PreRequisitesDescription = "Mindestens Sprachniveau B1 (wünschenswert B2), hohe Lernmotivation und Konzentrationsfähigkeit, Kommunikationsfähigkeit, Führerschein. Die Eignung wird in einem persönlichen Beratungsgespräch geprüft",
                                ShortDescription = "Die TQdigital unterstützt Sie beim Einstieg in den Beruf Fachlagerist (m/d/w) und gibt Ihnen die Chance auf einen höher qualifizierten Arbeitsplatz. Überschaubare Lernphasen durch fachspezifische Ausrichtung der einzelnen Module, ermöglichen ein flexibles Lernen.\r\nOb einzelne Module oder Berufsabschluss – mit unseren Teilqualifizierungen (TQ) können Sie sich flexibel weiterbilden.\r\n\r\nJedes einzelne TQ-Modul qualifiziert Sie gezielt für einen spezifischen Einsatzbereich eines Ausbildungsberufs. Alle Module zusammen bilden in Theorie und Praxis das gesamte Berufsbild ab. Wenn Sie alle Einzelteile abgeschlossen haben, können Sie im Anschluss mit einer Externenprüfung der IHK den Berufsabschluss erreichen.\r\n",
                                TargetGroup = "Arbeitssuchende und Beschäftigte, die keinen oder einen fachfremden Berufsabschluss haben und sich weiter qualifizieren möchten und einen anerkannten Berufsabschluss anstreben.",
                                TrainingProviderId = EduProvider.provider["bbw"],
                                Title = "FACHLAGERIST*IN – TQdigital"
                            }}
        };

        /// <summary>
        /// Ids 0 - 19 biwe
        /// Ids 20 - 39 bbw
        /// Ids 40 - 59 tüv
        /// </summary>
        public Dictionary<long, Appointment> Appointments = new Dictionary<long, Appointment>()
        {
            {0,new Appointment() {
                Id = 0,
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                AppointmentType = AppointmentType.Unknow,
                AvailableSeats = -1,
                BookingContact = 0,
                BookingUrl = new Uri(""),
                CourseId = -1,
                Course = null,
                BookingCode = String.Empty,
                Location = "Stuttgart",
                OccurrenceType = OccurrenceType.FullTime,
                Type = CourseType.InPerson,
                StartDate = new DateTime(2023, 01, 02),
                Language = "DE-DE",
                Summary = "",
            }},
            {1,new Appointment() {
                Id = 1,
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                AppointmentType = AppointmentType.Unknow,
                AvailableSeats = -1,
                BookingContact = 0,
                BookingUrl = new Uri(""),
                CourseId = -1,
                Course = null,
                BookingCode = String.Empty,
                Location = "Stuttgart",
                OccurrenceType = OccurrenceType.FullTime,
                Type = CourseType.InPerson,
                StartDate = new DateTime(2023, 02, 06),
                Language = "DE-DE",
                Summary = "",
            }},
            {2,new Appointment() {
                Id = 2,
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                AppointmentType = AppointmentType.Unknow,
                AvailableSeats = -1,
                BookingContact = 0,
                BookingUrl = new Uri(""),
                CourseId = -1,
                Course = null,
                BookingCode = String.Empty,
                Location = "Stuttgart",
                OccurrenceType = OccurrenceType.FullTime,
                Type = CourseType.InPerson,
                StartDate = new DateTime(2023, 03, 06),
                Language = "DE-DE",
                Summary = "",
            }},
            {3,new Appointment() {
                Id = 3,
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                AppointmentType = AppointmentType.Unknow,
                AvailableSeats = -1,
                BookingContact = 0,
                BookingUrl = new Uri(""),
                CourseId = -1,
                Course = null,
                BookingCode = String.Empty,
                Location = "Stuttgart",
                OccurrenceType = OccurrenceType.FullTime,
                Type = CourseType.InPerson,
                StartDate = new DateTime(2023, 04, 03),
                Language = "DE-DE",
                Summary = "",
            }}
        };


        //Vielleicht so?
        /*
         * Usecase 1 - Adrian
         * VOR dem Skill-Assessment werden diese Kurse angezeigt: Abschlussorientierte TQ Biwe sowie bbw TQ Digital ebenfalls TQ 1
         *
         */

        public Dictionary<int, List<Course>> usecaseCourses = new Dictionary<int, List<Course>>();

    }
}
