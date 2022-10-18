using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models
{
    [DataContract]
    public class Document : IApolloGraphItem, IEntity
    {

        #region BackendStuff

        [DataMember(Order = 1,IsRequired = true)]
        public string BackendId { get; set; } = null!;

        [DataMember(Order = 2)]
        public Uri Schema { get; set; } = null!;

        #endregion


        #region ClientStuff

        [Key]
        [DataMember(Order = 3,IsRequired = true)]
        public long Id { get; set; }

        [DataMember(Order = 4,IsRequired = true)]
        public long Ticks { get; set; }

        #endregion

        /// <summary>
        /// Url to the document on the CDN
        /// </summary>
        [DataMember(Order = 5, IsRequired = true)]
        public Uri DocumentUrl { get; set; } = null!;

        /// <summary>
        /// Hash of the document
        /// </summary>
        [DataMember(Order = 6)]
        public string Hash { get; set; } = null!;

        /// <summary>
        /// Description of the document
        /// </summary>
        [DataMember(Order = 7)]
        public string TextSummarization { get; set; } = null!;

        /// <summary>
        /// Key Takeaways of the document
        /// </summary>
        [DataMember(Order = 8)]
        public string KeyPhrases { get; set; } = null!;
    }
}
