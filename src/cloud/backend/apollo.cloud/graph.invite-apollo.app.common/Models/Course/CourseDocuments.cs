using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class CourseDocuments : IApolloGraphItem, IEntity
    {
        #region BackendStuff

        [DataMember(Order = 1, IsRequired = true)]
        public string BackendId { get; set; } = null!;

        [DataMember(Order = 2)]
        public Uri Schema { get; set; } = null!;

        #endregion


        #region ClientStuff


        [Key]
        [DataMember(Order = 3, IsRequired = true)]
        public long Id { get; set; }

        [DataMember(Order = 4, IsRequired = true)]
        public long Ticks { get; set; }

        #endregion

        [ForeignKey(nameof(CourseItem))]
        [DataMember(Order = 5,IsRequired = true)]
        public long CourseId { get; set; }

        [ForeignKey(nameof(Document))]
        [DataMember(Order = 7, IsRequired = true)]
        public long DocumentId { get; set; }
    }
}
