// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Graph.Apollo.Cloud.Common.Models.Assessment;
using Grpc.Core;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;

class Program
{
    static async Task Main(string[] args)
    {
        GrpcClientFactory.AllowUnencryptedHttp2 = true;
        using var http = GrpcChannel.ForAddress("http://localhost:5260");
        {
            var service = http.CreateGrpcService<IAssessmentService>();

            AssessmentRequest assessmentrequest = new() { Title = "1" };

            var response = await service.GetAssessmentsAsync(assessmentrequest);
            Console.WriteLine(response.Assessments.Count);

        }
        Console.WriteLine("Press [Enter] to exit");
        Console.ReadLine();
    }
}

