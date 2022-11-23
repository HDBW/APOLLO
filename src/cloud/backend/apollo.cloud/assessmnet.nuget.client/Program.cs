using System.Globalization;
using Grpc.Core;
using Grpc.Net.Client;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Esco;
using ProtoBuf;
using ProtoBuf.Grpc.Client;

class Program
{
    static async Task Main(string[] args)
    {
        GrpcClientFactory.AllowUnencryptedHttp2 = true;
        using var http = GrpcChannel.ForAddress("http://localhost:5260");
        {
            CheckFile("usecase1.bin");
            try
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
                Console.WriteLine(response.CorrelationId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            //Console.WriteLine(response.Assessments.Count);
        }
        Console.WriteLine("Press [Enter] to exit");
        Console.ReadLine();
    }

    private static void CheckFile(string filename)
    {
        UseCaseCollections expected;

        string filename1 = "usecase1.bin";

        using (var file = File.OpenRead(filename1))
        {
            expected = Serializer.Deserialize<UseCaseCollections>(file);
            file.Close();
        }
    }
}
