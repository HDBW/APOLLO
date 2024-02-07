﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Common.Entities;
using Invite.Apollo.App.Graph.Common.Models.Course;

namespace De.HDBW.Apollo.SharedContracts.Services
{
    public interface ITrainingService
    {
        Task<IEnumerable<Training>> SearchSuggesionsAsync(Filter? filter, CancellationToken token);

        Task<IEnumerable<Training>> SearchTrainingsAsync(Filter? filter, List<string> visibleFields, CancellationToken token);

        Task<Training?> GetTrainingAsync(long id, CancellationToken token);
    }
}
