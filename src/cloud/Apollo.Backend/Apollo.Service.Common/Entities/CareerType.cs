// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;

namespace Apollo.Common.Entities
{
    /// <summary>
    /// Client uses this to display the Career Type.
    /// 
    /// </summary>
    /// <remarks>
    /// Unknown = 0,
    /// // Sonstiges
    /// Other = 1,
    /// // Berufspraxis
    /// WorkExperience = 2,
    /// // Berufspraxis (Nebenbeschäftigung)
    /// PartTimeWorkExperience = 3,
    /// // Praktikum
    /// Internship = 4,
    /// // Selbständigkeit
    /// SelfEmployment = 5,
    /// // Wehrdienst/-übung/Zivildienst
    /// Service = 6,
    /// // Gemeinnützige Arbeit
    /// CommunityService = 7,
    /// // Freiwilligendienst
    /// VoluntaryService = 8,
    /// // Mutterschutz / Elternzeit
    /// ParentalLeave = 9,
    /// // Hausfrau/mann
    /// Homemaker = 10,
    /// // Außerberufliche Erfahrungen
    /// ExtraOccupationalExperience = 11,
    /// // Betr. pflegebed. Person
    /// PersonCare = 12,
    /// </remarks>
    public class CareerType : ApolloListItem
    {
        //public string Id { get; set; }
        //public string Name { get; set; }
        //public string Value { get; set; }
        //public CultureInfo CultureInfo { get; set; }
    }
}
