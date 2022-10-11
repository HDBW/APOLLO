// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Graph.Apollo.Cloud.Common.Models.Taxonomy;

namespace Graph.Apollo.Cloud.Common.Models.Assessment
{
    [DataContract]
    public class AssessmentItem : IEntity
    {
        /// <summary>
        /// Indicates unique Identifier for client database
        /// </summary>
        [DataMember(Order = 1)]
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Indicates latest update on Assessment
        /// </summary>
        [DataMember(Order = 2)]
        public long Ticks { get; set; }

        [DataMember(Order = 3)]
        public string Title { get; set; }

    }
}
