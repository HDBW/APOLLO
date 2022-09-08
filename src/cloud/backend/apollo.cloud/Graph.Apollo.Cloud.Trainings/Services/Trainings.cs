using System.Xml.Linq;
using Graph.Apollo.Cloud.Common;


namespace Graph.Apollo.Cloud.Trainings.Services
{
    public class Trainings : ITrainings
    {
        private readonly ILogger<Trainings> _logger;

        public Trainings(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Trainings>();
        }

        public string GetTrainings()
        {
            _logger.LogInformation($"Retrieving trainings ...");
            return "Hello World";
        }
    }
}
