// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class MetaDataMetaDataRelationRepository :
        AbstractInMemoryRepository<MetaDataMetaDataRelation>,
        IMetaDataMetaDataRelationRepository
    {
        public MetaDataMetaDataRelationRepository(ILogger<MetaDataMetaDataRelationRepository> logger)
            : base(logger)
        {
        }

        public Task<IEnumerable<MetaDataMetaDataRelation>> GetItemsBySourceIdsAsync(IEnumerable<long> ids, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var result = new ReadOnlyCollection<MetaDataMetaDataRelation>(new List<MetaDataMetaDataRelation>());
            if (ids != null)
            {
                result = new ReadOnlyCollection<MetaDataMetaDataRelation>(Items.Where(i => ids.Contains(i.SourceId)).ToList());
            }

            return Task.FromResult(result as IEnumerable<MetaDataMetaDataRelation>);
        }
    }
}
