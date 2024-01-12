// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using System.Runtime.Serialization;

namespace Apollo.Common.Entities
{
    [DataContract]
    public class Concept
    {
        /// <summary>
        /// The Apollo Id of the Concept.
        /// </summary>
        public string Id { get; set; }

        public string Identifier { get; set; }
        public string UniqueIdentifier { get; set; }
        public string ConceptUri { get; set; }
        public string PreferedLabel { get; set; }
        public List<string> AlternativeLabels { get; set; }
        public List<Concept> BroaderConcepts { get; set; }
        public List<Concept> NarrowerConcepts { get; set; }

        public string Description { get; set; }
        public string Definition { get; set; }
        public string Version { get; set; }

        public string ScopeNote { get; set; }

        public CultureInfo Language { get; set; }

        public Taxonomy TaxonomyInfo { get; set; }
    }
}
