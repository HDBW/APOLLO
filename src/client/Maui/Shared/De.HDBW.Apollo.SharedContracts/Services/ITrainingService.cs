// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Backend.Api;
using Invite.Apollo.App.Graph.Common.Models.Course;

namespace De.HDBW.Apollo.SharedContracts.Services
{
    public interface ITrainingService
    {
        Task<IEnumerable<CourseItem>> SearchTrainingsAsync(Filter? filter, CancellationToken token);

        Task<CourseItem?> GetTrainingAsync(long id, CancellationToken token);
    }
}
