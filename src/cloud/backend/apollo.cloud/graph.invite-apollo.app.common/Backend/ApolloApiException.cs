﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using Newtonsoft.Json;
using ZstdSharp.Unsafe;

namespace Apollo.Api
{
    /// <summary>
    /// Custom Exception with the error code.
    /// </summary>
    public class ApolloApiException : Exception
    {
        /// <summary>
        /// The error code that describes the failure.
        /// </summary>
        public int ErrorCode { get; private set; }

        public ApolloApiException(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        [JsonConstructor()]
        public ApolloApiException(int errorCode, string message, Exception ex) : base(message, ex)
        {
            ErrorCode = errorCode;
        }
        
        public ApolloApiException() { }
    }
}