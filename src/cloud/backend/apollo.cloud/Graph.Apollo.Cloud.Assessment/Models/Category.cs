﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Google.Protobuf.WellKnownTypes;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Newtonsoft.Json;

namespace Invite.Apollo.App.Graph.Assessment.Models
{
    public class Category : BaseItem
    {

        public string Title { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Threshold
        /// TODO: Remove before DataBase Creation
        /// </summary>
        public int ResultLimit { get; set; }
        
        public string Description { get; set; } = string.Empty;

        //TODO: Remove after December
        public long CourseId { get; set; }

        public List<Question> Questions { get; set; }
    }
}
