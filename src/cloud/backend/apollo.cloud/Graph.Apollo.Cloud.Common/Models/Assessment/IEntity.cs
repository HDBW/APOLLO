using System.ComponentModel.DataAnnotations;

namespace Graph.Apollo.Cloud.Common.Models.Assessment
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
