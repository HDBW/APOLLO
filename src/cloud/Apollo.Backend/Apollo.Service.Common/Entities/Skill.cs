// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using System.Runtime.Serialization;

namespace Apollo.Common.Entities
{
    /// <summary>
    /// This class represents a Taxonomy object skill.
    /// It tries to map the Taxonomy of the BA Dataset to a Skill.
    /// It also allows the AI to store the Taxonomy information of a Skill.
    /// We tried to find a way to map different Taxonomies to these base Skill Class.
    /// It is only the entry point for a taxonomy. It has no information about the graph complexity behind it other then the concept it relates to. 
    /// </summary>
    [DataContract]
    public class Skill:ObjectBase
    {
        /// <summary>
        /// The Id of the user to whom the skill belongs.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The Id of the user to whom the skill belongs.
        /// </summary>
        public string ProfileId { get; set; }

        /// <summary>
        /// A List in different Cultures for the Lists Title
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// A List of descriptions for the different cultures
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// This is a list of synonyms for the title in different cultures
        /// </summary>
        public List<string>? AlternativeLabels { get; set; }

        /// <summary>
        /// A List of Occupations this Skill is essential for
        /// </summary>
        public List<string>? IsEssentialForOccupationsID { get; set; }

        /// <summary>
        /// A List of Occupations that Skill is used optionally
        /// </summary>
        public List<string> ? IsOptionalForOccupationsID { get; set; }

        /// <summary>
        /// That is a URI to the Origin of the Skill
        /// </summary>
        public Uri? SkillUri { get; set; }

        /// <summary>
        /// Indicates the Taxonomy Version
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// This could be very usefull for example:
        /// This Skill requires something or excludes something?
        /// </summary>
        public string? ScopeNote { get; set; }

        /// <summary>
        /// Could be one of the supported Taxonomies of Apollo which is ESCO, KLDB or ...
        /// </summary>
        public Taxonomy? TaxonomyInfo { get; set; }

        /// <summary>
        /// Indicates how a Skill is aquired in Apollo
        /// Currently we know: Assessment can result in a skill associated to a Profile
        /// as well as the users entry in EducationInfo and CarrerInfo will result in Profile as source   
        /// </summary>
        public string? SkillSource { get; set; }

        /// <summary>
        /// When was the Skill acquired
        /// </summary>
        public DateTime? FromWhen { get; set; }

        /// <summary>
        /// Last the time the skill was used based on Profile?
        /// As long as a skill is optional or essential for an occupation in carrerinfo
        /// we will continue to count the skill used? maybe? 
        /// </summary>
        public DateTime? LastTimeUsed { get; set; }

        /// <summary>
        /// This is a autocalculated Property?
        /// </summary>
        ///public double HowLongUsed => (LastTimeUsed - FromWhen).TotalDays;
        
        /// <summary>
        /// I don´t know how??? But would be cool
        /// </summary>
        public string? HowOftenUsed { get; set; }

        /// <summary>
        /// Discussion we used to talk about:
        /// HowSkilled or Niveau but leo said its Level :/
        /// Change if you like ^_^
        /// We decided not the exact value yet but:
        /// We need something indicating that something was
        /// acquired from apprenticeship or university degree vs years of experience.
        /// </summary>
        public dynamic? Level { get; set; }


        /// <summary>
        /// Discussion we used to talk about:
        /// HowSkilled or Niveau but leo said its Level :/
        /// </summary>
        public string? Culture { get; set; }
    }
}
