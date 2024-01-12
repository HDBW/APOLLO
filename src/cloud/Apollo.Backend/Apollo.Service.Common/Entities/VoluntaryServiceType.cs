// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;

namespace Apollo.Common.Entities
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Clients uses the following Enum:
    ///      public enum VoluntaryServiceType
    /// {
    ///     Unknown = 0,
    ///     // Sonstiges
    ///     Other = 1,
    ///     // Freiwilliges Soziales Jahr (FSJ)
    ///     VoluntarySocialYear = 2,
    ///     // Bundesfreiwilligendienst
    ///     FederalVolunteerService = 3,
    ///     // Freiwilliges Ökologisches Jahr (FÖJ)
    ///     VoluntaryEcologicalYear = 4,
    ///     // Freiwilliges Soziales Trainingsjahr (FSTJ)
    ///     VoluntarySocialTrainingYear = 5,
    ///     // Freiwilliges Kulturelles Jahr (FKJ)
    ///     VoluntaryCulturalYear = 6,
    ///     // Freiwilliges Soziales Jahr im Sport
    ///     VoluntarySocialYearInSport = 7,
    ///     // Freiwilliges Jahr in der Denkmalpflege (FJD)
    ///     VoluntaryYearInMonumentConservation = 8,
    /// }
    /// </remarks>
    public class VoluntaryServiceType
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public CultureInfo CultureInfo { get; set; }
    }
}
