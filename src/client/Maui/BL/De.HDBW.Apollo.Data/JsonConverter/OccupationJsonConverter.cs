// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Taxonomy;

namespace De.HDBW.Apollo.Data.Converter
{
    public class OccupationJsonConverter : JsonConverter<Occupation>
    {
        public override Occupation Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            var jo = JsonDocument.ParseValue(ref reader);
            //if (jo.ContainsKey("TaxonomyInfo"))
            //{
            //    if (jo.ContainsKey("$type"))
            //    {
            //        jo.Remove("$type");
            //    }

            //    var data = jo.ToString();
            //    switch (jo["TaxonomyInfo"]?.ToObject(typeof(Taxonomy)))
            //    {
            //        case Taxonomy.KldB2010:
            //            var z = new KldbOccupation();
            //            JsonConvert.PopulateObject(data, z);
            //            var x = System.Text.Json.JsonSerializer.Deserialize<KldbOccupation>(data);
            //            return x;
            //        default:
            //            return System.Text.Json.JsonSerializer.Deserialize<UnknownOccupation>(data);
            //    }
            //}

            return null;
        }

        public override void Write(
           Utf8JsonWriter writer,
           Occupation value,
           JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value);
        }
    }
}
