using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment.Enums
{
    [DataContract]
    public enum AssessmentType
    {
        SkillAssessment = 0,
        SoftSkillAssessment = 1,
        Survey = 2
    }
}
