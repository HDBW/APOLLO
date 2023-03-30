using System.ComponentModel.DataAnnotations;
using ProtoBuf;


namespace Invite.Apollo.App.Graph.Common.Models
{
    /// <summary>
    /// Data Contract for client local database store
    /// </summary>
    public interface IEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long Ticks { get; set; }
    }
}
