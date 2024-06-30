// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;
using De.HDBW.Apollo.SharedContracts.Models;

namespace De.HDBW.Apollo.SharedContracts.Converters
{
    public class ImageConverter : JsonConverter<Image>
    {
        public override Image? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.String:
                        var content = reader.GetString();
                        if (string.IsNullOrWhiteSpace(content))
                        {
                            return null;
                        }

                        var image = JsonSerializer.Deserialize<Image>(content);
                        return image;
                    default:
                        throw new JsonException();
                }
            }

            var dto = new Image();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return dto;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propName = (reader.GetString() ?? string.Empty).ToLower();
                    reader.Read();

                    switch (propName)
                    {
                        case var _ when propName.Equals(nameof(Image.id).ToLower()):
                            dto.id = reader.GetString() ?? string.Empty;
                            break;
                        case var _ when propName.Equals(nameof(Image.name).ToLower()):
                            dto.name = reader.GetString() ?? string.Empty;
                            break;
                        case var _ when propName.Equals(nameof(Image.translatable).ToLower()):
                            dto.translatable = reader.GetBoolean();
                            break;
                        default:

                            if (JsonDocument.TryParseValue(ref reader, out JsonDocument? jsonDoc))
                            {
                                // dto.Property_3 = jsonDoc.RootElement.GetRawText();
                            }

                            break;
                    }
                }
                else
                {
                }
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Image value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
