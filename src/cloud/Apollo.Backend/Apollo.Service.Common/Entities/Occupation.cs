// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Apollo.Common.Entities
{
    public class Occupation: EntityBase
    {
        ///// <summary>
        ///// This can be used as Unique Identifier for the Occupation within the apollo system.
        ///// </summary>
        //public string? Id { get; set; }

        /// <summary>
        /// This is the unique identifier for the occupation if there is one in the specific Taxonomie.
        /// PLEASE NOTE: This can not be used as a Primary Key since it is not unique across all Taxonomies.
        /// TYPICALLY THIS IS A URI IN MOST TAXONOMIES.
        /// USE Id FOR DAL
        /// </summary>
        public string? UniqueIdentifier { get; set; }

        public string? OccupationUri { get; set; }

        /// <summary>
        /// Can be a KLDB DKZ or a ESCO Code or a ISCO Code.
        /// </summary>
        public string? ClassificationCode { get; set; }

        /// <summary>
        /// This is mostly a number that might not be unique. 
        /// It is an identifier for the occupation in a specific Taxonomie.
        /// Can also be refered to as the Code for the occupation.
        /// PLEASE NOTE: This can not be used as a Primary Key since it is not unique across all Taxonomies.
        /// TYPICALLY THIS IS A URI IN MOST TAXONOMIES.
        /// USE Id FOR DAL
        /// </summary>
        public string? Identifier { get; set; }

        /// <summary>
        /// Some Taxonomies have a concept that is a human readable description of the occupation. And a common ground definition which is language independent. Mostly in English. It describes the basic concept of a occupation.
        /// For example the concept of a "baker" is: Bakers make a wide range of breads, pastries, and other baked goods. They follow all the processes from receipt and storage of raw materials, preparation of raw materials for bread-making, measurement and mixing of ingredients into dough and proof. They tend ovens to bake products to an adequate temperature and time.
        /// https://esco.ec.europa.eu/en/classification/occupation?uri=http%3A%2F%2Fdata.europa.eu%2Fesco%2Foccupation%2F1aadb308-432a-4d01-b54b-b4f7f76dd419
        /// For each Memberstate of the EU there is a translation of the concept in the official languages as well as the .
        /// This is in most cases a URL to the concept.
        /// </summary>
        public string? Concept { get; set; }

        /// <summary>
        /// This is a url to a regulatory aspect of the occupation. For example the "baker" has a regulatory aspect in Germany set by the HWK in the law: HwO.
        /// </summary>
        public string RegulatoryAspect { get; set; } = String.Empty;

        /// <summary>
        /// This indicates wheter a occupation has/is a apprenticeship or not.
        /// UI Query related
        /// </summary>
        public bool HasApprenticeShip { get; set; }

        /// <summary>
        /// Indicates if this is an Occupation which is regulated and has a university degree.
        /// UI Query related
        /// </summary>
        public bool IsUniversityOccupation { get; set; }

        /// <summary>
        /// Indicates if this is an Academic Career.
        /// UI Query related
        /// </summary>
        public bool IsUniversityDegree { get; set; }

        /// <summary>
        /// The prefered term is the term that is used in the Taxonomie. For example the prefered term for the occupation "baker" is "Bäcker/in".
        /// UI Query related
        /// </summary>
        public List<string> PreferedTerm { get; set; }

        /// <summary>
        /// This is a list of terms that are commonly used in a region or language but are not the prefered term.
        /// For example "Bäckergeselle" is a common term for a "baker" in Germany but not the prefered term.
        /// </summary>
        public List<string> NonePreferedTerm { get; set; } = new();

        /// <summary>
        /// Related Taxonomy
        /// </summary>
        public Taxonomy TaxonomyInfo { get; set; }

        /// <summary>
        /// This information is relevant since some Taxonomies have different versions.
        /// </summary>
        public string TaxonomieVersion { get; set; } = String.Empty;


        /// <summary>
        /// This is the language of the occupation.
        /// </summary>
        public string? CultureString { get; set; }

        /// <summary>
        /// Represents the culture information for the entity.
        /// The Backend Converter utilizes this property.
        /// </summary>
        [JsonIgnore]
        public CultureInfo? Culture;



        //[ProtoMember(15)]
        //public CultureInfo Culture { get; set; }

        /// <summary>
        /// Describes the Occupation
        /// </summary>
        public string? Description { get; set; }


        public List<string> BroaderConcepts { get; set; } = new();

        public List<string?> NarrowerConcepts { get; set; } = new();

        public List<string?> RelatedConcepts { get; set; } = new();

        public List<string> Skills { get; set; } = new();

        public List<string> EssentialSkills { get; set; } = new();

        public List<string> OptionalSkills { get; set; } = new();

        public List<string> EssentialKnowledge { get; set; } = new();

        public List<string> OptionalKnowledge { get; set; } = new();

        public List<string> Documents { get; set; } = new();

      
        public Dictionary<string, string> OccupationGroup { get; set; } = new Dictionary<string, string>();


        public bool DkzApprenticeship { get; set; } = false;

        public bool QualifiedProfessional { get; set; } = false;

        public bool NeedsUniversityDegree { get; set; } = false;

        public bool IsMilitaryApprenticeship { get; set; }

        public bool IsGovernmentApprenticeship { get; set; }

        public DateTime? ValidFrom { get; set; }

        public DateTime? ValidTill { get; set; }
    }
}
