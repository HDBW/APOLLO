using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models
{
    public class QnA : IEntity, IApolloGraphItem
    {
        #region client stuff
        [Key]
        [DataMember(Order = 1)]
        public long Id { get; set; }

        [DataMember(Order = 2)]
        public long Ticks { get; set; }

        #endregion

        #region cloud stuff

        [DataMember(Order = 3, IsRequired = true)]
        public string BackendId { get; set; } = null!;

        [DataMember(Order = 4)]
        public Uri Schema { get; set; } = null!;

        #endregion

        [DataMember(Order = 5, IsRequired = true)]
        public string Question { get; set; } = null!;

        [DataMember(Order = 6)]
        public string Answer { get; set; } = null!;

        [DataMember(Order = 7)]
        public Uri AnswerUrl { get; set; } = null!;

        public CultureInfo? Language { get; set; }
    }
}
