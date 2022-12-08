using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Assessment.Models.Course
{
    public class EduProvider
    {
        #region Implementation of IEntity
        
        public long Id { get; set; }

        public long Ticks { get; set; }

        #endregion

        #region Implementation of IBackendEntity
        
        public Uri Schema { get; set; } = null!;

        #endregion

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Uri Website { get; set; } = null!;

        public Uri Logo { get; set; } = null!;
    }


    
}
