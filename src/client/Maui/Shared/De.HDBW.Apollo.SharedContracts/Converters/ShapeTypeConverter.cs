// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;
using De.HDBW.Apollo.SharedContracts.Enums;

namespace De.HDBW.Apollo.SharedContracts.Converters
{
    public class ShapeTypeConverter : JsonConverter<ShapeType>
    {
        public override ShapeType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.String:
                        var content = reader.GetString();
                        if (string.IsNullOrWhiteSpace(content))
                        {
                            throw new JsonException();
                        }

                        return Enum.Parse<ShapeType>(content);
                    default:
                        throw new JsonException();
                }
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, ShapeType value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
