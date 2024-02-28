// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Globalization;
using ProtoBuf;

namespace Invite.Apollo.App.Graph.Common.Models.Taxonomy
{
    [ProtoContract]
    public class Occupation
    {
        /// <summary>
        /// This can be used as Unique Identifier for the Occupation within the apollo system.
        /// </summary>
        [ProtoMember(1)]
        public string? Id { get; set; }

        /// <summary>
        /// This is the unique identifier for the occupation if there is one in the specific Taxonomie.
        /// PLEASE NOTE: This can not be used as a Primary Key since it is not unique across all Taxonomies.
        /// TYPICALLY THIS IS A URI IN MOST TAXONOMIES.
        /// USE Id FOR DAL
        /// </summary>
        [ProtoMember(2)]
        public string? UniqueIdentifier { get; set; }

        [ProtoMember(3)]
        public string? OccupationUri { get; set; }

        /// <summary>
        /// Can be a KLDB DKZ or a ESCO Code or a ISCO Code.
        /// </summary>
        [ProtoMember(4)]
        public string? ClassificationCode { get; set; }

        /// <summary>
        /// This is mostly a number that might not be unique. 
        /// It is an identifier for the occupation in a specific Taxonomie.
        /// Can also be refered to as the Code for the occupation.
        /// PLEASE NOTE: This can not be used as a Primary Key since it is not unique across all Taxonomies.
        /// TYPICALLY THIS IS A URI IN MOST TAXONOMIES.
        /// USE Id FOR DAL
        /// </summary>
        [ProtoMember(5)]
        public string? Identifier { get; set; }

        /// <summary>
        /// Some Taxonomies have a concept that is a human readable description of the occupation. And a common ground definition which is language independent. Mostly in English. It describes the basic concept of a occupation.
        /// For example the concept of a "baker" is: Bakers make a wide range of breads, pastries, and other baked goods. They follow all the processes from receipt and storage of raw materials, preparation of raw materials for bread-making, measurement and mixing of ingredients into dough and proof. They tend ovens to bake products to an adequate temperature and time.
        /// https://esco.ec.europa.eu/en/classification/occupation?uri=http%3A%2F%2Fdata.europa.eu%2Fesco%2Foccupation%2F1aadb308-432a-4d01-b54b-b4f7f76dd419
        /// For each Memberstate of the EU there is a translation of the concept in the official languages as well as the .
        /// This is in most cases a URL to the concept.
        /// </summary>
        [ProtoMember(6)]
        public string? Concept { get; set; }

        /// <summary>
        /// This is a url to a regulatory aspect of the occupation. For example the "baker" has a regulatory aspect in Germany set by the HWK in the law: HwO.
        /// </summary>
        [ProtoMember(7)]
        public string RegulatoryAspect { get; set; } = String.Empty;

        /// <summary>
        /// This indicates wheter a occupation has/is a apprenticeship or not.
        /// UI Query related
        /// </summary>
        [ProtoMember(8)]
        public bool HasApprenticeShip { get; set; }

        /// <summary>
        /// Indicates if this is an Occupation which is regulated and has a university degree.
        /// UI Query related
        /// </summary>
        [ProtoMember(9)]
        public bool IsUniversityOccupation { get; set; }

        /// <summary>
        /// Indicates if this is an Academic Career.
        /// UI Query related
        /// </summary>
        [ProtoMember(10)]
        public bool IsUniversityDegree { get; set; }

        /// <summary>
        /// The prefered term is the term that is used in the Taxonomie. For example the prefered term for the occupation "baker" is "Bäcker/in".
        /// UI Query related
        /// </summary>
        [ProtoMember(11)]
        public List<string> PreferedTerm { get; set; }

        /// <summary>
        /// This is a list of terms that are commonly used in a region or language but are not the prefered term.
        /// For example "Bäckergeselle" is a common term for a "baker" in Germany but not the prefered term.
        /// </summary>
        [ProtoMember(12)]
        public List<string> NonePreferedTerm { get; set; } = new();

        /// <summary>
        /// Related Taxonomy
        /// </summary>
        [ProtoMember(13)]
        public Taxonomy TaxonomyInfo { get; set; }

        /// <summary>
        /// This information is relevant since some Taxonomies have different versions.
        /// </summary>
        [ProtoMember(14)]
        public string TaxonomieVersion { get; set; } = String.Empty;

        private CultureInfo? _culture;

        /// <summary>
        /// This is the language of the occupation.
        /// </summary>
        [ProtoMember(15)]
        public string? CultureString
        {
            get => _culture?.Name;
            set
            {
                var x = new CultureInfo(value);
                _culture = string.IsNullOrWhiteSpace(value)
                    ? null
                    : new System.Globalization.CultureInfo(value);
            }
        }

        [ProtoIgnore]
        public CultureInfo? Culture
        {
            get => _culture;
            set
            {
                _culture = value;
            }
        }

        //[ProtoMember(15)]
        //public CultureInfo Culture { get; set; }

        /// <summary>
        /// Describes the Occupation
        /// </summary>
        [ProtoMember(16)]
        public string? Description { get; set; }


        [ProtoMember(17)] public List<string> BroaderConcepts { get; set; } = new();

        [ProtoMember(18)]
        public List<string?> NarrowerConcepts { get; set; } = new();

        [ProtoMember(19)]
        public List<string?> RelatedConcepts { get; set; } = new();

        [ProtoMember(20)]
        public List<string> Skills { get; set; } = new();

        [ProtoMember(21)]
        public List<string> EssentialSkills { get; set; } = new();

        [ProtoMember(22)]
        public List<string> OptionalSkills { get; set; } = new();

        [ProtoMember(23)]
        public List<string> EssentialKnowledge { get; set; } = new();

        [ProtoMember(24)]
        public List<string> OptionalKnowledge { get; set; } = new();

        [ProtoMember(25)]
        public List<string> Documents { get; set; } = new List<string>();

        [ProtoMember(26)]
        public KeyValuePair<string, string> OccupationGroup { get; set; } = new KeyValuePair<string, string>();

        [ProtoMember(27)]
        public bool DkzApprenticeship { get; set; } = false;

        [ProtoMember(28)]
        public bool QualifiedProfessional { get; set; } = false;

        [ProtoMember(29)]
        public bool NeedsUniversityDegree { get; set; } = false;

        [ProtoMember(30)]
        public bool IsMilitaryApprenticeship { get; set; }

        [ProtoMember(31)]
        public bool IsGovernmentApprenticeship { get; set; }

        [ProtoMember(32)]
        public DateTime? ValidFrom { get; set; }

        [ProtoMember(33)]
        public DateTime? ValidTill { get; set; }
    }
}
