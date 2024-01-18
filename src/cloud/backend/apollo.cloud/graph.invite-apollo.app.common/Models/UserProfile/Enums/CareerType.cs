// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums
{
    /// <summary>
    /// This Enum represents the Type of the Career
    /// Note this is a reverse engineered Enum based on the Data we have in the BA Dataset.
    /// </summary>
    public enum CareerType
    {
        Unknown = 0,
        // Sonstiges
        Other = 1,
        // Berufspraxis
        WorkExperience = 2,
        // Berufspraxis (Nebenbeschäftigung)
        PartTimeWorkExperience = 3,
        // Praktikum
        Internship = 4,
        // Selbständigkeit
        SelfEmployment = 5,
        // Wehrdienst/-übung/Zivildienst
        Service = 6,
        // Gemeinnützige Arbeit
        CommunityService = 7,
        // Freiwilligendienst
        VoluntaryService = 8,
        // Mutterschutz / Elternzeit
        ParentalLeave = 9,
        // Hausfrau/mann
        Homemaker = 10,
        // Außerberufliche Erfahrungen
        ExtraOccupationalExperience = 11,
        // Betr. pflegebed. Person
        PersonCare = 12,
    }
}
