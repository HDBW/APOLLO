// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations.Schema;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    public class CourseApolloLabels : BaseItem
    {
        [ForeignKey(nameof(CourseItem))]
        public long CourseId { get; set; }

        [ForeignKey(nameof(ApolloLabel))]
        public long LabelId { get; set; }
    }
}
