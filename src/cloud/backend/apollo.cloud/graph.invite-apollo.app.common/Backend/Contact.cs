// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;

namespace Apollo.Common.Entities
{
    public class Contact
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        [Newtonsoft.Json.JsonProperty("id")]
        public string? Id { get; set; }

        //[BsonElement("Name")]
        [Newtonsoft.Json.JsonProperty("firstname")]
        public string Firstname { get; set; }

        //[BsonElement("Surname")]
        [Newtonsoft.Json.JsonProperty("surname")]
        public string Surname { get; set; }

        //[BsonElement("Mail")]
        [Newtonsoft.Json.JsonProperty("mail")]
        public string Mail { get; set; }

        //[BsonElement("Phone")]
        [Newtonsoft.Json.JsonProperty("phone")]
        public string Phone { get; set; }

        //[BsonElement("Organization")]
        [Newtonsoft.Json.JsonProperty("organization")]
        public string Organization { get; set; }

        //[BsonElement("Address")]
        [Newtonsoft.Json.JsonProperty("address")]
        public string Address { get; set; }

        //[BsonElement("City")]
        [Newtonsoft.Json.JsonProperty("city")]
        public string City { get; set; }

        //[BsonElement("ZipCode")]
        [Newtonsoft.Json.JsonProperty("zipCode")]
        public string ZipCode { get; set; }

        //[BsonElement("AppointmentUrl")]
        [Newtonsoft.Json.JsonProperty("eAppointmentUrl")]
        public Uri EAppointmentUrl { get; set; }
    }
}
