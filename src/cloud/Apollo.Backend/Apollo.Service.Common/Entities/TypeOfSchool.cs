// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;

namespace Apollo.Common.Entities
{
    /// <summary>
    /// This defines the type of school
    /// This is should be string value i guess?
    /// </summary>
    /// <remarks>
    /// Client uses the following enum:
    /// Unknown = 0,
    /// // Sonstige Schule
    /// Other = 1,
    /// // Gymnasium
    /// HighSchool= 2,
    /// // Realschule
    /// SecondarySchool = 3,
    /// // Berufsfachschule
    /// VocationalCollege = 4,
    /// // Hauptschule
    /// MainSchool = 5,
    /// // Berufsoberschule/ Technische Oberschule
    /// VocationalHighSchool = 6,
    /// // Berufsschule
    /// VocationalSchool = 7,
    /// // Förderschule
    /// SpecialSchool = 8,
    /// // Integrierte Gesamtschule
    /// IntegratedComprehensiveSchool = 9,
    /// // Schulart mit mehreren Bildungsgängen
    /// SchoolWithMultipleCourses = 10,
    /// // Fachoberschule
    /// TechnicalCollege = 11,
    /// // Fachgymnasium
    /// TechnicalHighSchool = 12,
    /// // Fachschule
    /// TechnicalSchool = 13,
    /// // Kolleg
    /// College = 14,
    /// // Abendgymnasium
    /// EveningHighSchool = 15,
    /// // Berufsaufbauschule
    /// VocationalTrainingSchool = 16,
    /// // Abendrealschule
    /// NightSchool = 17,
    /// // Abendhauptschule
    /// EveningSchool = 18,
    /// // Freie Waldorfschule
    /// WaldorfSchool = 19,
    /// // Fachakademie
    /// TechnicalAcademy = 20,  
    /// </remarks>
    public class TypeOfSchool
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public CultureInfo CultureInfo { get; set; }

    }


}
