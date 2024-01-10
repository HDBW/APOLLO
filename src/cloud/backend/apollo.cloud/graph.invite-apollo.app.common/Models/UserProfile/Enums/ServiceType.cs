namespace Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums
{
    /// <summary>
    /// This Enum represents the Type of the Service
    /// This is a reverse engineered Enum based on the Data we have in the BA Dataset.
    /// </summary>
    public enum ServiceType
    {
        Unknown,
        // Zivildienst
        CivilianService,
        // Grundwehrdienst
        MilitaryService,
        // Freiwilliger Wehrdienst
        VoluntaryMilitaryService,
        // Wehrübung
        MilitaryExercise,
    }
}
