using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    public class CategoryRecomendationItem : BaseItem
    {
        [DataMember(Order = 5)]
        public long CategoryId { get; set; }

        [DataMember(Order = 6)]
        public long CourseId { get; set; }
    }
}
