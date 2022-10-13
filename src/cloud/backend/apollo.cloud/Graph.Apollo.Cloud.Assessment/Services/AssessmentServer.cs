// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Graph.Apollo.Cloud.Common.Models.Assessment;
using Graph.Apollo.Cloud.Common.Models.Taxonomy;

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
                new Common.Models.Assessment.Assessment(){Value = new AssessmentItem(){Id = 0,Ticks = DateTime.Now.Ticks, Title = "Hello 1"},Occupation = new Occupation(){Id = 1,Name = "Landschaftsshit",Schema = "http://data.europa.eu/esco/occupation/a10eb17a-3c78-4f7a-a1da-8f31146339d3"}},
                new Common.Models.Assessment.Assessment(){Value = new AssessmentItem(){Id = 0,Ticks = DateTime.Now.Ticks, Title = "Hello 3"},Occupation = new Occupation(){Id = 1,Name = "Landschaftsshit",Schema = "http://data.europa.eu/esco/occupation/a10eb17a-3c78-4f7a-a1da-8f31146339d3"}},
                new Common.Models.Assessment.Assessment(){Value = new AssessmentItem(){Id = 0,Ticks = DateTime.Now.Ticks, Title = "Hello 2"},Occupation = new Occupation(){Id = 1,Name = "Landschaftsshit",Schema = "http://data.europa.eu/esco/occupation/a10eb17a-3c78-4f7a-a1da-8f31146339d3"}},
            }
        };
        return new ValueTask<AssessmentResult>(result);
    }
}
