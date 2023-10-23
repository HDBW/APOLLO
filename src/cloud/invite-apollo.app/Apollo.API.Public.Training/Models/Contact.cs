// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public record Contact
{
    /// <summary>
    /// Primary Identifier of the Contact set by the Service
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    /// <summary>
    /// First Name of the Contact
    /// </summary>
    [BsonElement("Name")]
    public string Name { get; set; }

    /// <summary>
    /// Last Name of the Contact
    /// </summary>
    [BsonElement("Surname")]
    public string Surname { get; set; }

    /// <summary>
    /// E-Mail Address of the Contact
    /// </summary>
    [BsonElement("Mail")]
    public string Mail { get; set; }

    /// <summary>
    /// Phone Number of the Contact
    /// </summary>
    [BsonElement("Phone")]
    public string Phone { get; set; }

    /// <summary>
    /// Organization of the Contact
    /// </summary>
    [BsonElement("Organization")]
    public string Organization { get; set; }

    /// <summary>
    /// Street or Address of the Contact
    /// </summary>
    [BsonElement("Address")]
    public string Address { get; set; }

    /// <summary>
    /// City of the Contact
    /// </summary>
    [BsonElement("City")]
    public string City { get; set; }

    /// <summary>
    /// Zip Code of the Contact
    /// </summary>
    [BsonElement("ZipCode")]
    public string ZipCode { get; set; }

    /// <summary>
    /// Allows to set a Url for E-Appointment Creation of a Contact
    /// </summary>
    [BsonElement("Appointment")]
    public Uri EAppointmentUrl { get; set; }
}
