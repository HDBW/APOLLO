// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Net;
using System.Text.Json;
using Apollo.Api;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Apollo.RestService.Midleware
{
    /// <summary>
    /// Invoked on every exception. Returns the error code and the message in the response.
    /// </summary>
    public class ApolloExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// Returns the 500 error and serializes the error code with user friendly error.
        /// If the exception is not of type ApoloAPiException, only 500 is returned to avoid possible security issues.
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            var controllerName = context.RouteData.Values["controller"];
            var actionName = context.RouteData.Values["action"];

            string? errorMessage;

            if (context.Exception.GetType() == typeof(ApolloExceptionFilter))
            {
                ApolloApiException e = (ApolloApiException)context.Exception;
                JsonSerializer.Serialize(new { ErrorCode = e.ErrorCode, Message=e.Message});
                errorMessage = JsonSerializer.Serialize(new { ErrorCode = e.ErrorCode, Message = e.Message }); ;
            }
            else
                errorMessage = JsonSerializer.Serialize(new { ErrorCode="-1",  Message = "An error has ocurred in the application :(" });

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = 500;
            context.HttpContext.Response.WriteAsync(errorMessage);
        }
    }
}
