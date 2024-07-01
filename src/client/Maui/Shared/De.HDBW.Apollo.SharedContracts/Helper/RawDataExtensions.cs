// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using De.HDBW.Apollo.SharedContracts.Converters;

namespace De.HDBW.Apollo.SharedContracts.Helper
{
    public static class RawDataExtensions
    {
        private static readonly TextEncoderSettings EncoderSettings = new TextEncoderSettings(UnicodeRanges.All)
        {
        };

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(EncoderSettings),
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull | JsonIgnoreCondition.WhenWritingDefault,
        };

        static RawDataExtensions()
        {
            Options.Converters.Add(new ImageConverter());
            Options.Converters.Add(new ReliantsConverter());
            Options.Converters.Add(new QuestionTypeConverter());
            Options.Converters.Add(new ShapeTypeConverter());
        }

        public static RawData? ToRawData(this Invite.Apollo.App.Graph.Common.Models.Assessments.RawData data)
        {
            return JsonSerializer.Deserialize<RawData>(data.Data, Options);
        }
    }
}
