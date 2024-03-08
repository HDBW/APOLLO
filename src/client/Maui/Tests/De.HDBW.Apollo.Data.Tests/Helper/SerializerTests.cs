// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Text.Json;
using De.HDBW.Apollo.Data.Helper;
using Invite.Apollo.App.Graph.Common.Backend.Api;
using Xunit;

namespace De.HDBW.Apollo.Data.Tests.Helper
{
    public class SerializerTests
    {
        [Fact]
        public void TestExceptionParsing()
        {
            var response = "{\"ErrorCode\": 330,\"Message\": \"User with ID 'f070f18d-7ae9-46c8-acef-2115347d3a59' not found.\"\r\n}";
            var exeption = SerializationHelper.Deserialize<ApolloApiException>(response);
            Assert.NotNull(exeption);
        }
    }
}
