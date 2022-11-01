// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Course;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class EduProviderItemRepository :
        AbstractInMemoryRepository<EduProviderItem>,
        IEduProviderItemRepository
    {
    }
}
