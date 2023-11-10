﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Net.Client;
using static Apollo.Services.Grpc.UserService;

namespace Apollo.GrpcService.UnitTests
{
    internal static class Helpers
    {

        internal static UserServiceClient GetClient()
        {
            var channel = Helpers.GetChannel();
            return new UserServiceClient(channel);
        }

        /// <summary>
        /// Creates the channel with the given URL for all UnitTests.
        /// </summary>
        /// <returns></returns>
        internal static GrpcChannel GetChannel()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7064");
            return channel;
        }
    }
}