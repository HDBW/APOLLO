using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    public class CourseApolloLabels : IEntity, IApolloGraphItem
    {
        #region Implementation of IEntity

        [Key]
        [DataMember(Order = 1)]
        public long Id { get; set; }

        [DataMember(Order = 2)]
        public long Ticks { get; set; }

        #endregion


        #region Implementation of IApolloGraphItem

        [DataMember(Order = 3, IsRequired = true)]
        public string BackendId { get; set; } = null!;

        [DataMember(Order = 4)]
        public Uri Schema { get; set; } = null!;

        #endregion

        [ForeignKey(nameof(CourseItem))]
        public long CourseId { get; set; }

        [ForeignKey(nameof(ApolloLabel))]
        public long LabelId { get; set; }
    }
}
