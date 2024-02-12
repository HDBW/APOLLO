// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Taxonomy;

namespace Invite.Apollo.App.Graph.Common.Models.Esco
{

    /// <summary>
    /// This class represents a Taxonomy object skill.
    /// It tries to map the Taxonomy of the BA Dataset to a Skill.
    /// It also allows the AI to store the Taxonomy information of a Skill.
    /// We tried to find a way to map different Taxonomies to these base Skill Class.
    /// It is only the entry point for a taxonomy. It has no information about the graph complexity behind it other then the concept it relates to. 
    /// </summary>
    [DataContract]
    public class Skill
    {
        /// <summary>
        /// The Apollo Id of the Skill.
        /// </summary>
        public string Id { get; set; }

        public string Identifier { get; set; }
        public string UniqueIdentifier { get; set; }
        public string SkillUri { get; set; }
        public string PreferedLabel { get; set; }

        public Concept SkillConcept { get; set; }

        public List<string> AlternativeLabels { get; set; } = new List<string>();
        public List<Concept> BroaderConcepts { get; set; } = new List<Concept>();
        public List<Concept> NarrowerConcepts { get; set; } = new List<Concept>();

        public string Description { get; set; }
        public string Definition { get; set; }
        public string Version { get; set; }

        public string ScopeNote { get; set; }

        public CultureInfo Language { get; set; }

        public Taxonomy.Taxonomy TaxonomyInfo { get; set; }
    }
}
