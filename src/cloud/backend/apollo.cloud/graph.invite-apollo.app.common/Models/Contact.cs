// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;

using System;
using Invite.Apollo.App.Graph.Common.Models.Lists;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;

namespace Invite.Apollo.App.Graph.Common.Models
{
    public class Contact
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        //[BsonElement("Name")] public string Name { get; set; }

        //[BsonElement("Surname")]
        public string Surname { get; set; }

        //[BsonElement("Mail")]
        public string Mail { get; set; }

        //[BsonElement("Phone")]
        public string Phone { get; set; }

        //[BsonElement("Organization")]
        public string Organization { get; set; }

        //[BsonElement("Address")]
        public string Address { get; set; }

        //[BsonElement("City")]
        public string City { get; set; }

        //[BsonElement("ZipCode")]
        public string ZipCode { get; set; }

        /// <summary>
        /// This is the Region of the Contact
        /// </summary>
        public string? Region { get; set; }

        /// <summary>
        /// This is the Country of the Contact
        /// </summary>
        public string? Country { get; set; }

        //[BsonElement("AppointmentUrl")]
        public Uri EAppointmentUrl { get; set; }

        public ApolloListItem ContactType { get; set; }
    }
}
