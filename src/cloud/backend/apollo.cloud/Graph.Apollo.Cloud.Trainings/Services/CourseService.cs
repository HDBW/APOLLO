using Graph.Apollo.Cloud.Common;
using Grpc.Core;

namespace Graph.Apollo.Cloud.Courses.Services
{
    public class CourseService : Graph.Apollo.Cloud.Common.Courses.CoursesBase
    {
        private readonly ICourses _courses;
        private readonly ILogger<CourseService> _logger;

        public CourseService(ILogger<CourseService> logger, ICourses courses)
        {
            _logger = logger;
            _courses = courses;
        }

        public override Task<CoursesReply> GetCourses(CoursesRequest request, ServerCallContext context)
        {
            var message = _courses.GetCourses();
            return Task.FromResult(new CoursesReply
            {
                Message = message
            });
        }
    }
}