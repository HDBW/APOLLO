namespace Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums
{
    public enum UniversityDegree
    {
        Unknown = 0,
        //Master / Diplom / Magister
        Master = 1,
        // Bachelor
        Bachelor = 2,
        // Anerkennung des Abschlusses wird geprüft
        Pending = 3,
        // Promotion
        Doctorate = 4,
        // Staatsexamen
        StateExam = 5,
        // Nicht reglementierter, nicht anerkannter Abschluss
        UnregulatedUnrecognized = 6,
        // Reglementierter und nicht anerkannter Abschluss
        RegulatedUnrecognized = 7,
        // Teilweise anerkannter Abschluss
        PartialRecognized = 8,
        // Kirchliches Examen
        EcclesiasticalExam = 9,
        // Nicht relevant
        Other = 10,
    }
}
