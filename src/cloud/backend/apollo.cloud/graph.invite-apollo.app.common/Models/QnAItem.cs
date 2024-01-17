using System;
using System.Runtime.Serialization;


namespace Invite.Apollo.App.Graph.Common.Models
{
    public class QnAItem : BaseItem
    {

        [DataMember(Order = 5, IsRequired = true)]
        public string Question { get; set; } = null!;

        [DataMember(Order = 6)]
        public string Answer { get; set; } = null!;

        [DataMember(Order = 7)]
        public Uri AnswerUrl { get; set; } = null!;
        /// TODO: CultureInfo
        public string? Language { get; set; }
    }
}
