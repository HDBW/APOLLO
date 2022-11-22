using System.Globalization;
using Invite.Apollo.App.Graph.Assessment.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.Course.Enums;
using Invite.Apollo.App.Graph.Common.Models.Course;

namespace Invite.Apollo.App.Graph.Assessment.Data
{

    public static class EduProvider
    {
        public static Dictionary<string, long> provider = new Dictionary<string, long>()
        {
            { "Biwe", 0 }, { "bbw", 1 }, { "TüV Rheinland Akademie",2 },
        };

        public static Dictionary<long, Course> CourseList = new Dictionary<long, Course>()
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
                                Description = "Mit der Teilqualifizierung können Sie Schritt für Schritt in sechs Etappen den Berufsabschluss Fachkraft für Lagerlogistik (m/w/d) erreichen. Bei erfolgreicher Kompetenzfeststellung erhalten Sie nach jeder Etappe ein Zertifikat inklusive Kompetenzfeststellungsergebnis, das bundesweit anerkannt ist. Sie haben die Möglichkeit, sich zur Externenprüfung bei der zuständigen Kammer anzumelden und damit den Berufsabschluss zu erwerben.",
                                //320 Unterrichtseinheiten je 45 min pro Modul + 120 Unterrichtseinheiten je 60 min im Unternehmen
                                Duration = TimeSpan.Zero,//320 Unterrichtseinheiten je 45 min pro Modul + 120 Unterrichtseinheiten je 60 min im Unternehmen - 6 Module
                                ExternalId = "https://www.biwe-bbq.de/weiterbildungen/anzeige/tq-lagerlogistik",
                                InstructorId = 0,
                                KeyPhrases = "",
                                Language = CultureInfo.CurrentCulture,
                                Occurrence = OccurrenceType.FullTime,
                                PreRequisitesDescription = "Mindestens Sprachniveau B1 (wünschenswert B2), hohe Lernmotivation und Konzentrationsfähigkeit, Kommunikationsfähigkeit, Führerschein. Die Eignung wird in einem persönlichen Beratungsgespräch geprüft",
                                ShortDescription = "Die TQ unterstützt Sie beim Einstieg in den Beruf Fachkraft für Lagerlogistik (m/w/d) und gibt Ihnen die Chance auf einen höher qualifizierten Arbeitsplatz. Überschaubare Lernphasen durch fachspezifische Ausrichtung der einzelnen Module, ermöglichen ein flexibles Lernen.",
                                TargetGroup = "Arbeitssuchende und Beschäftigte, die keinen oder einen fachfremden Berufsabschluss haben und sich weiter qualifizieren möchten und einen anerkannten Berufsabschluss anstreben.",
                                TrainingProviderId = EduProvider.provider["Biwe"],
                                Title = "Fachkraft für Lagerlogistik (m/w/d) - TQ/ETAPP"
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
                                Language = CultureInfo.CurrentCulture,
                                Occurrence = OccurrenceType.FullTime,
                                PreRequisitesDescription = "Mindestens Sprachniveau B1 (wünschenswert B2), hohe Lernmotivation und Konzentrationsfähigkeit, Kommunikationsfähigkeit, Führerschein. Die Eignung wird in einem persönlichen Beratungsgespräch geprüft",
                                ShortDescription = "Die TQdigital unterstützt Sie beim Einstieg in den Beruf Fachlagerist (m/d/w) und gibt Ihnen die Chance auf einen höher qualifizierten Arbeitsplatz. Überschaubare Lernphasen durch fachspezifische Ausrichtung der einzelnen Module, ermöglichen ein flexibles Lernen.\r\nOb einzelne Module oder Berufsabschluss – mit unseren Teilqualifizierungen (TQ) können Sie sich flexibel weiterbilden.\r\n\r\nJedes einzelne TQ-Modul qualifiziert Sie gezielt für einen spezifischen Einsatzbereich eines Ausbildungsberufs. Alle Module zusammen bilden in Theorie und Praxis das gesamte Berufsbild ab. Wenn Sie alle Einzelteile abgeschlossen haben, können Sie im Anschluss mit einer Externenprüfung der IHK den Berufsabschluss erreichen.\r\n",
                                TargetGroup = "Arbeitssuchende und Beschäftigte, die keinen oder einen fachfremden Berufsabschluss haben und sich weiter qualifizieren möchten und einen anerkannten Berufsabschluss anstreben.",
                                TrainingProviderId = EduProvider.provider["bbw"],
                                Title = "FACHLAGERIST*IN – TQdigital"
                            }},
            { 2, new Course() },
        };
        //Vielleicht so?
        /*
         * Usecase 1 - Adrian
         * VOR dem Skill-Assessment werden diese Kurse angezeigt: Abschlussorientierte TQ Biwe sowie bbw TQ Digital ebenfalls TQ 1
         *
         */

        public static Dictionary<int, List<Course>> usecaseCourses = new Dictionary<int, List<Course>>();

    }
}
