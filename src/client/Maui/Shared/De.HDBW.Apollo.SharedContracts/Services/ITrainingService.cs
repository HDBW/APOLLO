﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Backend.Api;
using Invite.Apollo.App.Graph.Common.Models.Trainings;

namespace De.HDBW.Apollo.SharedContracts.Services
{
    public interface ITrainingService
    {
        Task<IEnumerable<Training>> SearchSuggesionsAsync(Filter? filter, int? skip, int? top, CancellationToken token);

        Task<IEnumerable<Training>> SearchTrainingsAsync(Filter? filter, List<string> visibleFields, int? skip, int? top, CancellationToken token);

        Task<Training?> GetAsync(string id, CancellationToken token);
    }
}
