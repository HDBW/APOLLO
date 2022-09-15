using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.Serialization;
using ProtoBuf.Grpc;

namespace Graph.Apollo.Cloud.Common.Models;


[DataContract]
public class Course
{
    /// <summary>
    /// Segment Metadata
    /// </summary>
    [DataMember(Order = 1), Required]
    public string ApolloId { get; set; }

    /// <summary>
    /// Segment Content
    /// </summary>

    /// <summary>
    /// Title of the course. Typically offered by the course provider.
    /// </summary>
    [DataMember(Order = 2), Required]
    public string Title { get; set; }

    /// <summary>
    /// Subtitle of the course. Typically offered by the course provider.
    /// </summary>
    [DataMember(Order = 3)]
    public string? Subtitle { get; set; }

    /// <summary>
    /// A short description of the course.
    /// Provided by the course provider
    /// </summary>
    [DataMember(Order = 4)]
    public string ShortDescription { get; set; }

    /// <summary>
    /// The full Text Description of the course.
    /// Provided by the course provider.
    /// </summary>
    [DataMember(Order = 5), Required]
    public string Description { get; set; }

    /// <summary>
    /// Modules describing further details of the course.
    /// Provided by the course providers.
    /// </summary>
    [DataMember(Order = 6)]
    public Dictionary<string,string>? Modules { get; set; }

    [DataMember(Order = 7), Required]
    public string Benefits { get; set; }

    //TODO: Replace with enum maybe?
    // Propably Autocalculated field
    [DataMember(Order = 8)]
    public bool? LoanOptionsAvailable { get; set; }
    
    [DataMember(Order = 9)]
    private Dictionary<string,Uri>? LoanOptions { get; set; }

    [DataMember(Order = 10)]
    public List<Uri>? PublicDocuments { get; set; }

    /// <summary>
    /// Is a property that describes how long the the training or course takes.
    /// </summary>
    [DataMember(Order = 11)]
    public TimeSpan? Duration { get; set; }

    /// <summary>
    /// Segment Taxonomy
    /// </summary>
    //TODO: Change to ESCO Competencies Type
    [DataMember()]
    public List<string> EscoCompetenciesExtracted { get; set; }
    //TODO: Change to ESCO Competencies Type
    [DataMember()]
    public List<string> ListOfEscoCompetenciesAssigned { get; set; }

    /// <summary>
    /// Represents a Helper Property returning all ESCO Assigned Skills and Competencies
    /// </summary>
    //TODO: Change to ESCO Competencies Type
    [DataMember()]
    public List<string> EscoCompetencies => ListOfEscoCompetenciesAssigned.Concat(EscoCompetenciesExtracted).ToList();

    [DataMember()]
    public List<string> HiddenCompetencies { get; set; }

    //TODO: Change to Occupation
    [DataMember()]
    public List<string> ExtractedOccupations { get; set; }

    //TODO: Change to Occupation
    [DataMember()]
    public List<string> AssignedOccupations { get; set; }

    /// <summary>
    /// Helper Property returning Assigned and Extracted Occupations.
    /// </summary>
    //TODO: Change to Occupation
    [DataMember()]
    public List<string> Occupations => AssignedOccupations.Concat(ExtractedOccupations).ToList();

    [DataMember()]
    public Dictionary<string, Uri> LinkedEntities { get; set; }

    /// <summary>
    /// Tags are used for SEO optimization of a training or course 
    /// </summary>
    [DataMember()]
    public List<string> Tags { get; set; }

    /// <summary>
    /// Proposed Segment Classification
    /// </summary>
    public bool IsOnline { get; set; }

    //TODO: Autocalculated Property
    public bool IsAvailableParttime { get; set; }
    //TODO: Autocalculated Propety
    public bool IsAbailableFulltime { get; set; }
    
    /// <summary>
    /// Segment Social
    /// </summary>

    //TOOO: Add Customer Feedback
    [DataMember()]
    public List<CourseFeedback> CourseFeedbacks { get; }

    ///<summary>
    /// Segment Booking
    /// </summary>

    /// <summary>
    /// The url to the course providers booking page.
    /// typically trackback uri including apollo id.
    /// </summary>
    [DataMember(),Required]
    public Uri BookingUrl { get; set; }

    /// <summary>
    /// Is a list of historical prices of a course.
    /// </summary>
    [DataMember()]
    public List<CoursePrice> CoursePricesHistory { get; set; }



    /// <summary>
    /// Segment AI Stuff
    /// </summary>

    /// <summary>
    /// This is a AI generated field based on the course feedbacks given by the users.
    /// </summary>
    [DataMember()] 
    public string FeedbackSummarization { get; set; }

    /// <summary>
    /// Is a AI based Summerization of the course.
    /// </summary>
    [DataMember()]
    public string Summarization { get; set; }

    /// <summary>
    /// Is a AI based Extraction of the Key Phrases of the course.
    /// </summary>
    [DataMember()] 
    public string KeyPhrases { get; set; }

    [DataMember()]
    public string ExecutiveSummary { get; set; }

    [DataMember()]
    public string SentimentSummarization { get; set; }
    
    [DataMember()]
    public string OpinionMining { get; set; }

    /// <summary>
    /// A list of questions and answers regarding a specific course.
    /// </summary>
    [DataMember()]
    public Dictionary<string,string> CourseFaq { get; set; }

    //TODO: Course Telemetry such as Viewed, Listed, Relevance, ...


    /// <summary>
    /// Segment Course Content Management
    /// </summary>

    /// <summary>
    /// Indicates the date when a course or training was published to the app.
    /// </summary>
    [DataMember()]
    public DateTime PublishingDateTime { get; set; }

    /// <summary>
    /// Indicates the date when a course or training is about to retire or depricated.
    /// </summary>
    [DataMember()]
    public DateTime? DepricationDateTime { get; set; }

    /// <summary>
    /// Description why a course or training is about to retire or be depricated.
    /// </summary>
    [DataMember()]
    public string? DepricationReason { get; set; }

    /// <summary>
    /// Technical Datetime when a course or training is Unpublished!
    /// </summary>
    [DataMember()]
    public DateTime? UnpublishingDateTime { get; set; }

    /// <summary>
    /// Id of the course or training that is replacing the current course or training.
    /// </summary>
    [DataMember()]
    public string ReplacementId { get; set; }
    /// <summary>
    /// Id offered by the training provider of the course or training that is replacing the current course or training.
    /// </summary>
    [DataMember()]
    public string ReplacementIdExternal { get; set; }

    [DataMember()]
    public string ReplacementUrlExternal { get; set; }

    /// <summary>
    ///  Auto calculated property if a course or training is published
    /// </summary>
    [DataMember()]
    public bool IsPublished
    {
        get
        {
            return DateTime.Now >= PublishingDateTime && UnpublishingDateTime switch
            {
                null => true,
                _ => DateTime.Now < UnpublishingDateTime
            };
        }
    }
}