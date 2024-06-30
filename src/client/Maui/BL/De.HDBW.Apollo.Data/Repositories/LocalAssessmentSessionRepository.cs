// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class LocalAssessmentSessionRepository :
        AbstractDataBaseRepository<LocalAssessmentSession>,
        ILocalAssessmentSessionRepository
    {
        public LocalAssessmentSessionRepository(
            IDataBaseConnectionProvider dataBaseConnectionProvider,
            ILogger<LocalAssessmentSessionRepository> logger)
            : base(dataBaseConnectionProvider, logger)
        {
        }
    }
}
