
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph.Apollo.Cloud.Common.Models.Assessment
{
    public interface IAssessmentService
    {
        ValueTask<AssessmentResult> GetAssessmentsAsync(AssessmentRequest request);
    }


}
