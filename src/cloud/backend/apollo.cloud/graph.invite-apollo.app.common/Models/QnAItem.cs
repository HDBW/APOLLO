﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models
{
    public class QnAItem : IEntity, IBackendEntity
    {
        #region Implementation of IEntity
        [Key]
        [DataMember(Order = 1)]
        public long Id { get; set; }

        [DataMember(Order = 2, IsRequired = true)]
        public long Ticks { get; set; }

        #endregion

        #region Implementation of IBackendEntity
        [DataMember(Order = 3, IsRequired = true)]
        public long BackendId { get; set; }

        [DataMember(Order = 4, IsRequired = true)]
        public Uri Schema { get; set; } = null!;

        #endregion

        [DataMember(Order = 5, IsRequired = true)]
        public string Question { get; set; } = null!;

        [DataMember(Order = 6)]
        public string Answer { get; set; } = null!;

        [DataMember(Order = 7)]
        public Uri AnswerUrl { get; set; } = null!;

        public CultureInfo? Language { get; set; }
    }
}