// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public record Training
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    //Note we need a longid for mobile clients should we convert to longid in mongo?
    // I guess there is a converation from objectid to type long?
    //[BsonElement("MobileId")]
    //public long? MobileId { get; set; }

    [BsonElement("Title")]
    public string TrainingName { get; set; }

    /// <summary>
    /// External Identifier for the Training Providers
    /// </summary>
    [BsonElement("Identifier")]
    public string Identifier { get; set; }

    [BsonElement("Description")]
    public string Description { get; set; }

    [BsonElement("ShortDescription")]
    public string ShortDescription { get; set; }

    [BsonElement("Content")]
    public List<string> Content { get; set; }

    [BsonElement("Benefit")]
    public List<string> BenefitList { get; set; }

    [BsonElement("Certificate")]
    public List<string> Certificate { get; set; }

    //TODO: Maybe Flagged Enum for Certification Type
    //[Flags]
    //public enum Certification;

    [BsonElement("Prerequisites")]
    public List<string> Prerequisites { get; set; }

    /// <summary>
    /// Financial Aid provided for a Training
    /// </summary>
    [BsonElement("Loans")]
    public List<Loans> Loans { get; set; }

    [BsonElement("TrainingsProvider")]
    public EduProvider TrainingProvider { get; set; }

    [BsonElement("CourseProvider")]
    public EduProvider CourseProvider { get; set; }

    [BsonElement("Appointments")]
    public TrainingsAppointments Appointments { get; set; }

    /// <summary>
    /// Training Provider Url or Target
    /// </summary>
    [BsonElement("ProductUrl")]
    public Uri ProductUrl { get; set; }

    /// <summary>
    /// Defined as City + Contact
    /// </summary>
    [BsonElement("Contacts")]
    Dictionary<string, Contact> Contacts { get; set; }

    //TODO: Maybe Flagged Enum for TrainingTypeAvailable
    /*
     * Online/Offline/Hybrid/Blended Learning/...
     */
    //[Flags]
    //public enum TrainingType

    //TODO: Maybe Flagged Enum for LessonTypeAvailable
    /*
     * Online/Offline/Hybrid/Blended Learning/...
     */
    //[Flags]
    //public enum TrainingType

    // It should be bool but what do I know about education ofc it is not bool
    [BsonElement("IndividualStartDate")]
    public string IndividualStartDate { get; set; }

    //TODO: Review
    /// <summary>
    /// Maybe we should make this a class of its own?
    /// Since comparison is done by more information such as where does it happens, what does it include ...
    /// </summary>
    [BsonElement("Price")]
    public decimal? Price { get; set; }

    [BsonElement("PriceDescription")]
    public string PriceDescription { get; set; }

    
}
