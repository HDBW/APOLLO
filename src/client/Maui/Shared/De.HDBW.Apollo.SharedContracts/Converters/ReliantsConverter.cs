// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace De.HDBW.Apollo.SharedContracts.Converters
{
    public class ReliantsConverter : JsonConverter<Reliants>
    {
        public override Reliants? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.String:
                        var content = reader.GetString();
                        if (string.IsNullOrWhiteSpace(content) || !content.Contains("{"))
                        {
                            return null;
                        }

                        return JsonSerializer.Deserialize<Reliants>(content);
                    default:
                        throw new JsonException();
                }
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Reliants value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
