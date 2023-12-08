// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using Invite.Apollo.App.Graph.Common.Models;

namespace De.HDBW.Apollo.Data.Extensions
{
    internal static class FilterExtensions
    {
        internal static HttpContent? ToHttpContent(this Filter item)
        {
            if (item == null)
            {
                return null;
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(item);
            var content = new StringContent(json);
            content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");

            return content;
        }
    }
}
