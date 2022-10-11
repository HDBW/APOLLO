// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Graph.Apollo.Cloud.Common.Models.Assessment;

namespace Graph.Apollo.Cloud.Assessment.Services;

public class AssessmentService : IAssessmentService
{
    private readonly ILogger<AssessmentService> _logger;

    public AssessmentService(ILogger<AssessmentService> logger)
    {
        _logger = logger;
    }

    ValueTask<AssessmentResult> IAssessmentService.GetAssessmentsAsync(AssessmentRequest request)
    {
        var result = new AssessmentResult {Assessments = new List<Common.Models.Assessment.Assessment>
            {
                new Common.Models.Assessment.Assessment(){Value = new AssessmentItem(){Id = 0,Ticks = DateTime.Now.Ticks, Title = "Hello"}},
                new Common.Models.Assessment.Assessment(){Value = new AssessmentItem(){Id = 0,Ticks = DateTime.Now.Ticks, Title = "World"}},
                new Common.Models.Assessment.Assessment(){Value = new AssessmentItem(){Id = 0,Ticks = DateTime.Now.Ticks, Title = "Test"}}
            }
        };
        return new ValueTask<AssessmentResult>(result);
    }
}
