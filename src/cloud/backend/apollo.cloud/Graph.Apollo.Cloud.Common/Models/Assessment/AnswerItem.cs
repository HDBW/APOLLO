// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Graph.Apollo.Cloud.Common.Models.Assessment.Enums;

namespace Graph.Apollo.Cloud.Common.Models.Assessment
{
    [DataContract]
    public class AnswerItem : IEntity
    {
        [DataMember(Order = 1, IsRequired = true)]
        [Key]
        public long Id { get; set; }

        [DataMember(Order = 2, IsRequired = true)]
        public long QuestionId { get; set; }

        [DataMember(Order = 3, IsRequired = true)]
        public long Ticks { get; set; }

        [DataMember(Order = 4)]
        public AnswerType AnswerType { get; set; }

        [DataMember(Order = 5, IsRequired = true)]
        public bool CorrectAnswer { get; set; }

        [DataMember(Order = 6, IsRequired = true)]
        public string Value { get; set; }
    }
}
