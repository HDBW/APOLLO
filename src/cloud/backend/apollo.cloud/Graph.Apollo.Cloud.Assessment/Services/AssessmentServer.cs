// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Graph.Apollo.Cloud.Common.Models;

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
        var result = new AssessmentResult {Assessments = new List<Common.Models.Assessment>
            {
                new Common.Models.Assessment() {Id = 1, Title = "Assessment 1"},
                new Common.Models.Assessment() {Id = 2, Title = "Assessment 2"},
                new Common.Models.Assessment() { Id = 3, Title = "Assessment 3" }
            }
        };
        return new ValueTask<AssessmentResult>(result);
    }
}
