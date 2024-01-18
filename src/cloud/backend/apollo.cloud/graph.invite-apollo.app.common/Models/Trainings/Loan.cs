// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;

using System;
using Invite.Apollo.App.Graph.Common.Models;

namespace Invite.Apollo.App.Graph.Common.Models.Trainings
{
    public record Loans
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        //[BsonElement("Name")]
        public string Name { get; set; }

        //[BsonElement("Description")]
        public string Description { get; set; }

        //[BsonElement("Url")]
        public Uri Url { get; set; }

        //[BsonElement("Contact")]
        public Contact LoanContact { get; set; }
    }
}
