// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    public class QuestionResultItem : IEntity
    {
        public long Id { get; set; }

        public long AssessmentId { get; set; }

        public long QuestionId { get; set; }

        public long Ticks { get; set; }
    }
}
