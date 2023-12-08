// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Invite.Apollo.App.Graph.Common.Models
{
    public class PostTrainingsResponse
    {
        [Newtonsoft.Json.JsonProperty("trainings", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ICollection<Training>? Trainings { get; set; }
    }
}
