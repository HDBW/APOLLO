// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Invite.Apollo.App.Graph.Common.Models
{
    public partial class GetTrainingResponse
    {
        [Newtonsoft.Json.JsonProperty("training", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Training? Training { get; set; }

    }
}
