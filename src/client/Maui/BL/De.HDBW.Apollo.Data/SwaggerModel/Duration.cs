// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Invite.Apollo.App.Graph.Common.Models
{
    public class Duration
    {
        [Newtonsoft.Json.JsonProperty("ticks", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Ticks { get; set; }

        [Newtonsoft.Json.JsonProperty("days", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Days { get; set; }

        [Newtonsoft.Json.JsonProperty("hours", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Hours { get; set; }

        [Newtonsoft.Json.JsonProperty("milliseconds", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Milliseconds { get; set; }

        [Newtonsoft.Json.JsonProperty("microseconds", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Microseconds { get; set; }

        [Newtonsoft.Json.JsonProperty("nanoseconds", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Nanoseconds { get; set; }

        [Newtonsoft.Json.JsonProperty("minutes", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Minutes { get; set; }

        [Newtonsoft.Json.JsonProperty("seconds", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Seconds { get; set; }

        [Newtonsoft.Json.JsonProperty("totalDays", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int TotalDays { get; set; }

        [Newtonsoft.Json.JsonProperty("totalHours", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int TotalHours { get; set; }

        [Newtonsoft.Json.JsonProperty("totalMilliseconds", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int TotalMilliseconds { get; set; }

        [Newtonsoft.Json.JsonProperty("totalMicroseconds", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int TotalMicroseconds { get; set; }

        [Newtonsoft.Json.JsonProperty("totalNanoseconds", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int TotalNanoseconds { get; set; }

        [Newtonsoft.Json.JsonProperty("totalMinutes", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int TotalMinutes { get; set; }

        [Newtonsoft.Json.JsonProperty("totalSeconds", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int TotalSeconds { get; set; }
    }
}
