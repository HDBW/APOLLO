// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;

namespace Apollo.Common.Entities
{
    /// <summary>
    /// This is the Graduation of a School Type of the User
    /// </summary>
    /// <remarks>
    /// Client uses the following enum:
    ///      public enum SchoolGraduation
    /// {
    ///     Unknown = 0,
    ///     // Hauptschulabschluss
    ///     SecondarySchoolCertificate = 1,
    ///     // Fachhochschulreife - allows to study at a university of applied sciences
    ///     AdvancedTechnicalCollegeCertificate = 2,
    ///     // Allgemeine Hochschulreife - allows to study at a university
    ///     HigherEducationEntranceQualificationALevel = 3,
    ///     // Mittlere Reife / Mittlerer Bildungsabschluss
    ///     IntermediateSchoolCertificate = 4,
    ///     // Qualifizierender / erweiterter Hauptschulabschluss
    ///     ExtendedSecondarySchoolLeavingCertificate = 5,
    ///     // kein Schulabschluss
    ///     NoSchoolLeavingCertificate = 6,
    ///     // Schulabschluss der Förderschule
    ///     SpecialSchoolLeavingCertificate = 7,
    ///     // Fachgebundene Hochschulreife - allows to study at a university of applied sciences or in a specific subject area in a university
    ///     SubjectRelatedEntranceQualification = 8,
    ///     // Abgänger Klasse 11-13 ohne Abschluss
    ///     AdvancedTechnicalCollegeWithoutCertificate = 9,
    /// }
    /// </remarks>
    public class SchoolGraduation
    {

        public string Id { get; set; }

        public string Value { get; set; }
        public string Name { get; set; }

        public CultureInfo CultureInfo { get; set; }
    }
}
