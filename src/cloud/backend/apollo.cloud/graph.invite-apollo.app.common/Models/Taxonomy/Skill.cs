using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Text;
using Invite.Apollo.App.Graph.Common.Models.Assessment;

namespace Invite.Apollo.App.Graph.Common.Models.Esco
{

    [DataContract]
    public class Skill : IEntity, IBackendEntity
    {


        #region Implementation of IEntity
        [Key]
        public long Id { get; set; }
        public long Ticks { get; set; }

        #endregion

        #region Implementation of IBackendEntity

        public long BackendId { get; set; }
        public Uri Schema { get; set; }

        #endregion

        public string Value { get; set; }
    }
}
