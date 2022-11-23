using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Assessment.Models.Course
{
    public class Contact : BaseItem
    {
        public string ContactName { get; set; } = string.Empty;

        public string ContactMail { get; set; } = string.Empty;

        public string ContactPhone { get; set; } = string.Empty;
        public Uri Url { get; set; } = null!;

        //public Uri Image { get; set; } = null!;

        public List<CourseHasContacts> CourseContacts { get; set; }
    }
}
