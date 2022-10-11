// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.Serialization;
using Graph.Apollo.Cloud.Common.Models.Taxonomy;

namespace Graph.Apollo.Cloud.Common.Models.Assessment
{
    [DataContract]
    public class Assessment
    {
        [DataMember(Order = 1)]
        public AssessmentItem Value { get; set; }

        [IgnoreDataMember]
        public long Ticks
        {
            get => Value.Ticks;
            set => Value.Ticks = value;
        }

        [DataMember(Order = 2)]
        public List<Question> Questions { get; set; }

        [DataMember(Order = 3)]
        public List<Skill> Skills { get; set; }

        [DataMember(Order = 4)]
        public Occupation Occupation { get; set; }
        
    }

    [DataContract]
    public class AssessmentRequest
    {
        [DataMember(Order = 1)]
        public string Title { get; set; }
    }

    [DataContract]
    public class AssessmentResult
    {
        [DataMember(Order = 1)]
        public List<Assessment> Assessments { get; set; }
    }
}
