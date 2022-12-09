
using System.Globalization;
using Grpc.Core;
using Grpc.Net.Client;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Esco;
using ProtoBuf.Grpc.Client;

class Program
{
    static async Task Main(string[] args)
    {
        GrpcClientFactory.AllowUnencryptedHttp2 = true;
        using var http = GrpcChannel.ForAddress("http://localhost:5260");
        {
            var service = http.CreateGrpcService<IAssessmentGRPCService>();

            //TODO: Bring Occupation Back
            //Occupation occ = new();
            //occ.Schema = "http://data.europa.eu/esco/occupation/a10eb17a-3c78-4f7a-a1da-8f31146339d3";

            AssessmentRequest assessmentrequest = new()
            {
                CorrelationId = "1234",
                //Ticks = DateTime.Now.Ticks
            };

            var response = await service.GetAssessmentsAsync(assessmentrequest);
            //Console.WriteLine(response.Assessments.Count);
            Console.WriteLine(response.CorrelationId);

        }
        Console.WriteLine("Press [Enter] to exit");
        Console.ReadLine();
    }
}

