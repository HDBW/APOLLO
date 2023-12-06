// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public record Training : IContent
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    //Note we need a longid for mobile clients should we convert to longid in mongo?
    // I guess there is a converation from objectid to type long?
    //[BsonElement("MobileId")]
    //public long? MobileId { get; set; }

    [Required]
    [BsonElement("Title")]
    public string TrainingName { get; set; }

    /// <summary>
    /// External Unique Identifier from the Training Providers
    /// </summary>
    [Required]
    [BsonElement("Identifier")]
    public string Identifier { get; set; }

    /// <summary>
    /// Description of the Training or Course
    /// </summary>
    [BsonElement("Description")]
    public string Description { get; set; }

    /// <summary>
    /// Short Description of the Training or Course
    /// </summary>
    [BsonElement("ShortDescription")]
    public string ShortDescription { get; set; }

    /// <summary>
    /// Content or Agenda, typically a bulletpoint list with the major topics of the Training or Course
    /// </summary>
    [BsonElement("Content")]
    public List<string> Content { get; set; }

    /// <summary>
    /// Value proposition of the Training or Course
    /// </summary>
    [BsonElement("Benefit")]
    public List<string> BenefitList { get; set; }

    /// <summary>
    /// Typically a List of Certificates that can be obtained by the Training or Course
    /// That ranges from Attendance Certification, Qualification, Exam Preparation, Certification, ...
    /// </summary>
    [BsonElement("Certificate")]
    public List<string> Certificate { get; set; }

    //TODO: Maybe Flagged Enum for Certification Type
    //[Flags]
    //public enum Certification;

    [BsonElement("Prerequisites")]
    public List<string> Prerequisites { get; set; }

    /// <summary>
    /// Financial Aids provided for a Training
    /// </summary>
    [BsonElement("Loans")]
    public List<Loans> Loans { get; set; }

    /// <summary>
    /// The Education Provider offering the Training or Course
    /// </summary>
    [BsonElement("TrainingsProvider")]
    public EduProvider TrainingProvider { get; set; }

    /// <summary>
    /// Sometimes the Training Provider is not the same as the Course Provider
    /// </summary>
    [BsonElement("CourseProvider")]
    public EduProvider CourseProvider { get; set; }

    /// <summary>
    /// The Appointments of the Training or Course
    /// </summary>
    [BsonElement("Appointments")]
    public TrainingsAppointments Appointments { get; set; }

    /// <summary>
    /// Training Provider Url or Target
    /// Allows Deeplinking or Referral Linking
    /// </summary>
    [BsonElement("ProductUrl")]
    public Uri ProductUrl { get; set; }

    /// <summary>
    /// Defined as City + Contact
    /// Basically Allowing a Overview of all Trainings in a City
    /// Note: This value if not set will be AutoGenerated by the Appointments
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
    //public enum LessonType

    // It should be bool but what do I know about education ofc it is not bool
    /// <summary>
    /// Indicates if a custom Start Date is available. Typically a explanation is provided by the Training Provider
    /// </summary>
    [BsonElement("IndividualStartDate")]
    public string IndividualStartDate { get; set; }

    //TODO: Review
    /// <summary>
    /// Maybe we should make this a class of its own?
    /// Since comparison is done by more information such as where does it happens, what does it include ...
    /// </summary>
    [BsonElement("Price")]
    public decimal? Price { get; set; }

    /// <summary>
    /// Allows the Training Provider to give more Details about the Offerings
    /// </summary>
    [BsonElement("PriceDescription")]
    public string PriceDescription { get; set; }

    /// <summary>
    /// Indicates if the Training or Course is available for people with disabilities
    /// </summary>
    [BsonElement("Accessibility")]
    public bool AccessibilityAvailable { get; set; }

    #region Metadata

    /// <summary>
    /// Some Training Providers have a different title for Search Engine Optimization
    /// </summary>
    [BsonElement("SeoTitle")]
    public string SeoTitle { get; set; }

    /// <summary>
    /// Some Training Providers have offering a list of Tags for Search Engine Optimization
    /// </summary>
    [BsonElement("SeoTags")]
    public List<string> Tags { get; set; }

    /// <summary>
    /// Some Training Providers have offering a list of Categories for Search Engine Optimization
    /// </summary>
    [BsonElement("Categories")]
    public List<string> Categories { get; set; }

    #endregion

    #region IContentPublising


    [BsonElement("PublishingDate")]
    public DateTime PublishingDate { get; set; }

    public bool IsPublished => UnpublishingDate >= DateTime.Now && DateTime.Now >= PublishingDate;

    [BsonElement("UnpublishingDate")]
    public DateTime UnpublishingDate { get; set; }
    [BsonElement("Successor")]
    public string? Successor { get; set; }
    [BsonElement("Predecessor")]
    public string? Predecessor { get; set; }

    #endregion
}