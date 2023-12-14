// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;

using System;

namespace Apollo.Common.Entities
{
    public record Loans
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        [Newtonsoft.Json.JsonProperty("id")]
        public string? Id { get; set; }

        //[BsonElement("Name")]
        [Newtonsoft.Json.JsonProperty("name")]
        public string Name { get; set; }

        //[BsonElement("Description")]
        [Newtonsoft.Json.JsonProperty("description")]
        public string Description { get; set; }

        //[BsonElement("Url")]
        [Newtonsoft.Json.JsonProperty("url")]
        public Uri Url { get; set; }

        //[BsonElement("Contact")]
        [Newtonsoft.Json.JsonProperty("loanContact")]
        public Contact LoanContact { get; set; }
    }
}
