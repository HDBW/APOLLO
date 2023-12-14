// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;

using System;

namespace Apollo.Common.Entities
{
    public record Occurence
    {
        /// <summary>
        /// The CorrelationId is used to identify the occurence of the appointment that might be grouped with other appointment.
        /// Used for recurring appointment.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("correlationId")]
        public string CorrelationId { get; set; }

        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        [Newtonsoft.Json.JsonProperty("id")]
        public string? Id { get; set; }

        //[BsonElement("StartDate")]
        [Newtonsoft.Json.JsonProperty("startDate")]
        public DateTime StartDate { get; set; }

        //[BsonElement("EndDate")]
        [Newtonsoft.Json.JsonProperty("endDate")]
        public DateTime EndDate { get; set; }

        [Newtonsoft.Json.JsonProperty("duration")]
        public TimeSpan Duration => EndDate - StartDate;

        //[BsonElement("Description")]
        [Newtonsoft.Json.JsonProperty("description")]
        public string Description { get; set; }

        //[BsonElement("Location")]
        [Newtonsoft.Json.JsonProperty("location")]
        public Contact Location { get; set; }
    }
}
