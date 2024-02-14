﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;
using De.HDBW.Apollo.Data.JsonConverter;

namespace De.HDBW.Apollo.Data.Helper
{
    public static class SerializationHelper
    {
        public static JsonSerializerOptions Options { get; } = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            Converters = { new CultureInfoJsonConverter() },
        };

        public static string? Serialize<TU>(this TU? data)
        {
            if (data == null)
            {
                return null;
            }

            return JsonSerializer.Serialize(data, Options);
        }

        public static Task SerializeAsync<TU>(this Stream stream, TU? data, CancellationToken token)
        {
            if (data == null)
            {
                return Task.CompletedTask;
            }

            return JsonSerializer.SerializeAsync(stream, data, Options, token);
        }

        public static TU? Deserialize<TU>(this string? data)
        {
            if (data == null)
            {
                return default;
            }

            return JsonSerializer.Deserialize<TU>(data, Options);
        }

        public static ValueTask<TU?> DeserializeAsync<TU>(this Stream stream, CancellationToken token)
        {
            if (stream == null)
            {
                return ValueTask.FromResult<TU?>(default);
            }

            return JsonSerializer.DeserializeAsync<TU>(stream, Options, token);
        }
    }
}