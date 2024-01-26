// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.Taxonomy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace De.HDBW.Apollo.Data.Converter
{
    public class OccupationJsonConverter : JsonConverter<Occupation>
    {
        public override Occupation? ReadJson(JsonReader reader, Type objectType, Occupation? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (hasExistingValue)
            {
                return existingValue;
            }

            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            JObject jo = JObject.Load(reader);
            if (jo.ContainsKey("TaxonomyInfo"))
            {
                if (jo.ContainsKey("$type"))
                {
                    jo.Remove("$type");
                }

                var data = jo.ToString();
                switch (jo["TaxonomyInfo"].ToObject(typeof(Taxonomy)))
                {
                    case Taxonomy.KldB2010:
                        var z = new KldbOccupation();
                        JsonConvert.PopulateObject(data, z);
                        var x = System.Text.Json.JsonSerializer.Deserialize<KldbOccupation>(data);
                        return x;
                    default:
                        return System.Text.Json.JsonSerializer.Deserialize<UnknownOccupation>(data);
                }
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, Occupation? value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
