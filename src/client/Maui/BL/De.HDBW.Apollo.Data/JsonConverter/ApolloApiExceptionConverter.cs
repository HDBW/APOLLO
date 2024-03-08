// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Invite.Apollo.App.Graph.Common.Backend.Api;

namespace De.HDBW.Apollo.Data.JsonConverter
{
    public class ApolloApiExceptionConverter : JsonConverter<ApolloApiException>
    {
        public override ApolloApiException? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var info = new SerializationInfo(typeToConvert, new FormatterConverter());

            // Populate it with JSON properties
            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                string propertyName = reader.GetString()!;
                reader.Read();
                try
                {
                    object? value = JsonSerializer.Deserialize(ref reader, typeof(object), options);
                    info.AddValue(propertyName, value);
                }
                catch
                {
                }
            }

            int errorCode = -1;
            string message = string.Empty;
            foreach (SerializationEntry entry in info)
            {
                switch (entry.Name)
                {
                    case nameof(ApolloApiException.ErrorCode):
                        int.TryParse(entry.Value?.ToString() ?? "-1", out errorCode);
                        break;
                    case nameof(ApolloApiException.Message):
                        message = entry.Value?.ToString() ?? string.Empty;
                        break;
                }
            }

            return new ApolloApiException(errorCode, message)!;
        }

        public override void Write(Utf8JsonWriter writer, ApolloApiException value, JsonSerializerOptions options)
        {
        }
    }
}
