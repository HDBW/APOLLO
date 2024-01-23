// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace De.HDBW.Apollo.Data.JsonConverter
{
    public class CultureInfoJsonConverter : JsonConverter<CultureInfo>
    {
        public override CultureInfo Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            var name = reader.GetString();
            return CultureInfo.GetCultures(CultureTypes.AllCultures).First(c => c.Name == name);
        }

        public override void Write(
            Utf8JsonWriter writer,
            CultureInfo value,
            JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Name);
        }
    }
}
