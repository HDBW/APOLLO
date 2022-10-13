// Licensed to hdbw invite-apollo project under one or more agreements.
// The invite-apollo project presents this file to you under the MIT license.

using System.Runtime.Serialization;
using Graph.Apollo.Cloud.Common.Models.Taxonomy;

namespace Graph.Apollo.Cloud.Common.Models.Assessment
{
    [DataContract]
    public class Assessment
    {
        /// <summary>
        /// Creating default values
        /// </summary>
        public Assessment()
        {
            Value = new();
            Occupation = new();
            Questions = new();
            Skills = new();
            Occupation = new();
            }

        [DataMember(Order = 1)]
        public AssessmentItem Value { get; set; }

        [IgnoreDataMember]
        public long Ticks
        {
            get => Value.Ticks;
            set { Value.Ticks = value; }
        }

        [DataMember(Order = 2)]
        public List<Question> Questions { get; set; }

        [DataMember(Order = 3)]
        public List<Skill> Skills { get; set; }

        [DataMember(Order = 4)]
        public Occupation Occupation { get; set; }
        
    }

    /// <summary>
    /// Client to server request.
    /// TODO: Documentation grpc request
    /// </summary>
    [DataContract]
    public class AssessmentRequest
    {
        public AssessmentRequest(long id, Occupation occupation, List<Skill> skills)
        {
            Id = id;
            Occupation = occupation;
            Skills = skills;
        }

        public AssessmentRequest()
        {
            Id = new();
            Occupation = new();
            Skills = new();
        }

        /// <summary>
        /// Returns a single assessment by the id.
        /// </summary>
        [DataMember(Order = 1)]
        public long Id { get; set; }

        /// <summary>
        /// Returns Assessments that check a specific occupation
        /// </summary>
        [DataMember(Order = 2)]
        public Occupation Occupation { get; set; }

        /// <summary>
        /// Returns Assessments that contain a specific skill
        /// </summary>
        [DataMember(Order = 3)]
        public List<Skill> Skills { get; set; }

        //TODO: Consider query string.
    }

    /// <summary>
    /// Server to Client response
    /// TODO: Documentation grpc response
    /// </summary>
    [DataContract]
    public class AssessmentResult
    {
        /// <summary>
        /// Return 0 - n assessments matching the criteria of the request.
        /// </summary>
        [DataMember(Order = 1)]
        public List<Assessment> Assessments { get; set; }
    }

    
}
