﻿using Invite.Apollo.App.Graph.Common.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Assessment.Models
{
    public class BaseItem: IEntity, IBackendEntity
    {
        #region Implementation of IEntity
        public long Id { get; set; }

        public long Ticks { get; set; }

        #endregion

        #region Implementation of IBackendEntity

        //public long BackendId { get; set; }

        public Uri Schema { get; set; } = null!;

        #endregion
    }
}