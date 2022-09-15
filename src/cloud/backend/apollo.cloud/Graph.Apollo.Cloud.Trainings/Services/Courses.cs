using System.Xml.Linq;
using Graph.Apollo.Cloud.Common;


namespace Graph.Apollo.Cloud.Courses.Services
{
    public class Courses : ICourses
    {
        private readonly ILogger<Courses> _logger;

        public Courses(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Courses>();
        }

        public string GetCourses()
        {
            _logger.LogInformation($"Retrieving courses ...");
            return "Hello World";
        }
    }
}
