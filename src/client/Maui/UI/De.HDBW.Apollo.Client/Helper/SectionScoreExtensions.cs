// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.Assessments;

namespace De.HDBW.Apollo.Client.Helper
{
    public static class SectionScoreExtensions
    {
        public static ModuleScore ToModuleScore(this SegmentScore score)
        {
            return new ModuleScore()
            {
                Segment = score.Segment,
                AssessmentId = score.AssessmentId,
                ModuleId = score.ModuleId,
                Quantity = score.Quantity,
                ProfileId = score.ProfileId,
                Result = score.Result,
                ResultDescription = score.ResultDescription,
            };
        }
    }
}
