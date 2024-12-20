﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;

using System;
using Invite.Apollo.App.Graph.Common.Models;

namespace Invite.Apollo.App.Graph.Common.Models.Trainings
{
    public record Occurence
    {
        /// <summary>
        /// The CorrelationId is used to identify the occurence of the appointment that might be grouped with other appointment.
        /// Used for recurring appointment.
        /// </summary>
        public string? CorrelationId { get; set; }
        // [BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        //[BsonElement("StartDate")]
        public DateTime StartDate { get; set; }

        //[BsonElement("EndDate")]
        public DateTime EndDate { get; set; }

        public TimeSpan Duration => EndDate - StartDate;

        //[BsonElement("Description")]
        public string? Description { get; set; }

        //[BsonElement("Location")]
        public Contact? Location { get; set; }
    }
}
