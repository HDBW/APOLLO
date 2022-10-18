using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    /// <summary>
    /// Backend Specific Implementation of a Course
    /// DO NOT USE IN CLIENT 
    /// </summary>
    [DataContract]
    public class Course : IApolloGraphItem
    {
        public Course(string backendId,Uri schemaUrl)
        {
            BackendId = backendId;
            Schema = schemaUrl;
        }

        public string BackendId { get; set; }
        public Uri Schema { get; set; }
    }
}
