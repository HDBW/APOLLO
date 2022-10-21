using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Taxonomy
{
    [DataContract]
    public class Occupation : IEntity, IBackendEntity
    {
        #region Implementation of IEntity

        [DataMember(Order = 1,IsRequired = false)]
        public long Id { get; set; }

        [DataMember(Order = 2, IsRequired = true)]
        public long Ticks { get; set; }

        #endregion

        #region Implementation of IBackendEntity

        [DataMember(Order = 3, IsRequired = true)]
        public long BackendId { get; set; }

        [DataMember(Order = 4, IsRequired = true)]
        public Uri Schema { get; set; } = null!;

        #endregion

        [DataMember(Order = 4, IsRequired = false)]
        public CultureInfo CultureInfo { get; set; } = null!;

        [DataMember(Order = 5, IsRequired = true)]
        public string Name { get; set; } = null!;
    }
}
