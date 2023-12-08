// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Invite.Apollo.App.Graph.Common.Models
{
    public partial class FieldExpression
    {
        [Newtonsoft.Json.JsonProperty("fieldName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string? FieldName { get; set; }

        [Newtonsoft.Json.JsonProperty("operator", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public QueryOperator Operator { get; set; }

        [Newtonsoft.Json.JsonProperty("argument", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<object>? Argument { get; set; }

        [Newtonsoft.Json.JsonProperty("distinct", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool Distinct { get; set; }
    }
}
