﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using ProtoBuf;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    /// <summary>
    /// Represents the Answers to a Question in a AssessmentType
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class AnswerItem : BaseItem
    {

        [DataMember(Order = 5, IsRequired = true)]
        [ForeignKey(nameof(QuestionItem))]
        public long QuestionId { get; set; }
        
        [DataMember(Order = 6)]
        public AnswerType AnswerType { get; set; }

        [DataMember(Order = 7, IsRequired = true)]
        public string Value { get; set; } = string.Empty;

        [DataMember(Order = 8, IsRequired = false)]
        public int? Scalar { get; set; }
        
    }

    [DataContract]
    public class AnswersRequest : ICorrelationId
    {
        public AnswersRequest()
        {
            CorrelationId = string.Empty;
        }

        [DataMember(Order = 1,IsRequired = true)]
        public string CorrelationId { get; set; }



        [DataMember(Order = 3)]
        public long? Ticks { get; set; }


        [DataMember(Order = 5)]
        [ForeignKey(nameof(AssessmentItem))]
        public long? AssessmentId { get; set; }
    }

    [DataContract]
    public class AnswerResponse : ICorrelationId
    {
        public AnswerResponse()
        {
            Answers = new Collection<AnswerItem>();
            MetaData = new Collection<MetaDataItem>();
            AnswerMetaDataRelations = new Collection<AnswerMetaDataRelation>();
            MetaDataMetaDataRelations = new Collection<MetaDataMetaDataRelation>();
        }

        [DataMember(Order = 1, IsRequired = true)]
        public string CorrelationId { get; set; } = null!;

        [DataMember(Order = 2)]
        public Collection<AnswerItem> Answers { get; set; }

        [DataMember(Order = 3)]
        public Collection<MetaDataItem> MetaData { get; set; }

        [DataMember(Order = 4)]
        public Collection<AnswerMetaDataRelation> AnswerMetaDataRelations { get; set; }

        [DataMember(Order = 5)]
        public Collection<MetaDataMetaDataRelation> MetaDataMetaDataRelations { get; set; }
    } 


}
