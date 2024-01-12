// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;

namespace Apollo.Common.Entities
{
    /// <summary>
    /// This is used in Education Information of the User
    /// </summary>
    /// <remarks>
    /// Client uses following enum:
    ///          Unknown = 0,
    /// //Master / Diplom / Magister
    /// Master = 1,
    /// // Bachelor
    /// Bachelor = 2,
    /// // Anerkennung des Abschlusses wird geprüft
    /// Pending = 3,
    /// // Promotion
    /// Doctorate = 4,
    /// // Staatsexamen
    /// StateExam = 5,
    /// // Nicht reglementierter, nicht anerkannter Abschluss
    /// UnregulatedUnrecognized = 6,
    /// // Reglementierter und nicht anerkannter Abschluss
    /// RegulatedUnrecognized = 7,
    /// // Teilweise anerkannter Abschluss
    /// PartialRecognized = 8,
    /// // Kirchliches Examen
    /// EcclesiasticalExam = 9,
    /// // Nicht relevant
    /// Other = 10,
    /// </remarks>
    public class UniversityDegree
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public CultureInfo CultureInfo { get; set; }

    }
}
