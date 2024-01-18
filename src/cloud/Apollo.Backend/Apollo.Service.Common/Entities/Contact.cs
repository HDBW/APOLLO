// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;

namespace Apollo.Common.Entities
{
    public class Contact
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        //[BsonElement("Firstname")]
        public string? Firstname { get; set; }

        //[BsonElement("Surname")]
        public string? Surname { get; set; }

        //[BsonElement("Mail")]
        public string? Mail { get; set; }

        //[BsonElement("Phone")]
        public string? Phone { get; set; }

        //[BsonElement("Organization")]
        public string? Organization { get; set; }

        //[BsonElement("Address")]
        public string? Address { get; set; }

        //[BsonElement("City")]
        public string? City { get; set; }

        //[BsonElement("ZipCode")]
        public string? ZipCode { get; set; }

        public string? Region { get; set; }

        public string? Country { get; set; }

        //[BsonElement("AppointmentUrl")]
        public Uri EAppointmentUrl { get; set; }

        public ContactType? ContactType { get; set; }
    }
}
