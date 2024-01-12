// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;

namespace Apollo.Common.Entities
{
    /// <summary>
    /// Client uses this enum to display the information and offers a selection for the german system.
    /// Ist this important information - It seems to be relevant for a lot of occupations.
    /// Unknown = 0,
    /// // Fahrerlaubnis B
    /// B = 1,
    /// // Fahrerlaubnis BE
    /// BE = 2,
    /// // Gabelstaplerschein
    /// Forklift = 3,
    /// // Fahrerlaubnis C1E
    /// C1E = 4,
    /// // Fahrerlaubnis C1
    /// C1 = 5,
    /// // Fahrerlaubnis L
    /// L = 6,
    /// // Fahrerlaubnis AM
    /// AM = 7,
    /// // Fahrerlaubnis A
    /// A = 8,
    /// // Fahrerlaubnis CE
    /// CE = 9,
    /// // Fahrerlaubnis C
    /// C = 10,
    /// // Fahrerlaubnis A1
    /// A1 = 11,
    /// // Fahrerlaubnis B96
    /// B96 = 12,
    /// // Fahrerlaubnis T
    /// T = 13,
    /// // Fahrerlaubnis A2
    /// A2 = 14,
    /// // Fahrerlaubnis Mofa und Krankenfahrstühle
    /// Moped = 15,
    /// // Fahrerkarte
    /// Drivercard = 16,
    /// // Fahrerlaubnis Fahrgastbeförderung
    /// PassengerTransport = 17,
    /// // Fahrerlaubnis D
    /// D = 18,
    /// // Fahrlehrerlaubnis Klasse BE
    /// InstructorBE = 19,
    /// // Führerschein Baumaschinen
    /// ConstructionMachines = 20,
    /// // Fahrerlaubnis DE
    /// DE = 21,
    /// // Fahrerlaubnis D1
    /// D1 = 22,
    /// // Fahrerlaubnis D1E
    /// D1E = 23,
    /// // Fahrlehrerlaubnis Klasse A
    /// InstructorA = 24,
    /// // Fahrlehrerlaubnis Klasse CE
    /// InstructorCE = 25,
    /// // Gespannführerschein
    /// TrailerDriving = 26,
    /// // Fahrlehrerlaubnis Klasse DE
    /// InstructorDE = 27,
    /// // Lokomotiv-/Triebfahrzeugführerschein Klasse 1
    /// Class1 = 28,
    /// // Lokomotiv-/Triebfahrzeugführerschein Klasse 3
    /// Class3 = 29,
    /// // Lokomotiv-/Triebfahrzeugführerschein Klasse 2
    /// Class2 = 30,
    /// // Seminarerlaubnis ASF
    /// InstructorASF = 31,
    /// // Seminarerlaubnis ASP
    /// InstructorASP = 32,
    /// </summary>
    public class DriversLicense
    {
        public string Id { get; set; }

        public string Value { get; set; }

        public string Code { get; set; }

        public CultureInfo CultureInfo { get; set; }
    }
}
