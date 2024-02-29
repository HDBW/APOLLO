// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Taxonomy;

namespace De.HDBW.Apollo.Data.Converter
{
    public class OccupationJsonConverter : JsonConverter<Occupation>
    {
        public override Occupation? Read(
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
            foreach (var occ in jo.RootElement.EnumerateObject())
            {
                if (!string.Equals(occ.Name, nameof(Occupation.TaxonomyInfo), StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                var enumValue = occ.Value.Deserialize<Taxonomy>();

                switch (enumValue)
                {
                    case Taxonomy.KldB2010:
                        return jo.Deserialize<KldbOccupation>();
                    default:
                        return jo.Deserialize<UnknownOccupation>();
                }
            }

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
