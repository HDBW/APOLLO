// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;


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
