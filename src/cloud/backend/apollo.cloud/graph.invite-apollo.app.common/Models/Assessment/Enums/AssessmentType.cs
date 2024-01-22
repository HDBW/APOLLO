// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment.Enums
{
    [DataContract]
    public enum AssessmentType
    {        
        Unknown = -1,
        SkillAssessment = 0,
        SoftSkillAssessment = 1,        
        Survey = 2,
        ExperienceAssessment = 3,
        Cloze = 4
    }
}
