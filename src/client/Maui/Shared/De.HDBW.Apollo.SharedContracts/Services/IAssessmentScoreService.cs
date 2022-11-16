// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;

namespace De.HDBW.Apollo.SharedContracts.Services
{
    public interface IAssessmentScoreService
    {
        Task<AssessmentScore> GetAssessmentScoreAsync(IEnumerable<AnswerItemResult> answerItemResults, CancellationToken token);
    }
}
