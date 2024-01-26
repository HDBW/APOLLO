// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.Taxonomy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace De.HDBW.Apollo.Data.Converter
{
    internal class OccupationJsonConverter : Newtonsoft.Json.JsonConverter<Occupation>
    {

        public override Occupation? ReadJson(JsonReader reader, Type objectType, Occupation? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (!hasExistingValue)
            {
                return null;
            }

            JObject jo = JObject.Load(reader);
            if (jo.ContainsKey("TaxonomyInfo"))
            {
                switch (jo["TaxonomyInfo"].ToObject(typeof(Taxonomy)))
                {
                    case Taxonomy.KldB2010:
                        return JsonConvert.DeserializeObject<KldbOccupation>(jo.ToString());
                    default:
                        return JsonConvert.DeserializeObject<UnknownOccupation>(jo.ToString());
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
