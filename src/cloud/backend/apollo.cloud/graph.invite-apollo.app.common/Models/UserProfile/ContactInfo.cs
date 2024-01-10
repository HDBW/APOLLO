using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    /// <summary>
    /// This is the Contact Information of the User
    /// This is PII and GDPR relevant
    /// This is an Instance of List because a User can have multiple Contact Information
    /// </summary>
    public class ContactInfo
    {
        /// <summary>
        /// Indicates the Type of the Contact Information
        /// </summary>
        public ContactType ContactType { get; set; }

        /// <summary>
        /// This is the Address of the Contact
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// This is the City of the Contact
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// This is the ZipCode of the Contact
        /// </summary>
        public string? ZipCode { get; set; }

        /// <summary>
        /// This is the Region of the Contact
        /// </summary>
        public string? Region { get; set; }

        /// <summary>
        /// This is the Country of the Contact
        /// </summary>
        public string? Country { get; set; }

        /// <summary>
        /// This is the Email of the Contact
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// This is the Phonenumber of the Contact
        /// </summary>
        public string? Phone { get; set; }
    }
}
