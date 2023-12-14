// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Apollo.Common.Entities
{
    /// <summary>
    /// Needed for Filter and Wolfram Alpha Implementation
    /// </summary>
    public class GeoLocation
    {
        [Newtonsoft.Json.JsonProperty("latitude")]
        public double Latitude { get; set; }

        [Newtonsoft.Json.JsonProperty("longitude")]
        public double Longitude { get; set; }
    }
}
