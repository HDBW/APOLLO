using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Graph.Apollo.Cloud.Common.Models;

/// <summary>
/// CourseFeedback is the List of Feedback given by a user for a specific course
/// </summary>
[DataContract]
public class CourseFeedback
{
    [DataMember(Order = 1),Required]
    public string UserAlias { get; set; }
    /// <summary>
    /// Internal Field needed for Tracking and GDPR reasons
    /// </summary>
    private string UserId { get; set; }
    
    /// <summary>
    /// Indicates if a user has participated in the course.
    /// This is a future feature!
    /// </summary>
    private bool? TookCourse { get; set; }
    
    [DataMember(Order = 2)]
    public string Message { get; set; }

    [DataMember(Order = 3)]
    public bool? Enjoyed { get; set; }
    
    [DataMember(Order = 3),Required]
    public DateTime Created { get; set; }
}

