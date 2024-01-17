using System;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class CourseContact : BaseItem
    {
        [DataMember(Order = 5)]
        public string ContactName { get; set; } = string.Empty;

        [DataMember(Order = 6)]
        public string ContactMail { get; set; } = string.Empty;

        [DataMember(Order = 7)]
        public string ContactPhone { get; set; } = string.Empty;

        [DataMember(Order = 8)]
        public Uri Url { get; set; } = null!;

        //[DataMember(Order = 9)]
        //public Uri Image { get; set; } = null!;
    }
}
