using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    [DataContract]
    public class UserProfileItem : BaseItem
    {


        [DataMember(Order = 5, IsRequired = true)]
        public string? Goal { get; set; } = string.Empty;

        [DataMember(Order = 6, IsRequired = false)]
        public string? FirstName { get; set; } = string.Empty;

        [DataMember(Order = 7, IsRequired = false)]
        public string? LastName { get; set; } = string.Empty;

        /// <summary>
        /// For Testung the value will be:
        /// - user1.png
        /// - user2.png
        /// - user3.png
        /// </summary>
        [DataMember(Order = 8, IsRequired = false)]
        public string? Image { get; set; } = string.Empty;
    }
}
