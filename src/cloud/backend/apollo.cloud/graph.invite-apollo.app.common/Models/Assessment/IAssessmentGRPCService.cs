﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using ProtoBuf.Grpc;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    [ServiceContract(Name = "AssessmentService")]
    public interface IAssessmentGRPCService
    {
        [OperationContract]
        ValueTask<AssessmentResponse> GetAssessmentsAsync(AssessmentRequest request);

        [OperationContract]
        ValueTask<AnswerResponse> GetAnswersAsync(AnswersRequest request);

        [OperationContract]
        ValueTask<AnswerResponse> GetAnswersAsync(AnswersRequest request, CallContext context = default);

        [OperationContract]
        ValueTask<AnswerResponse> GetAnswersAsync(AnswersRequest request, CancellationToken cancellationToken = default);

        [OperationContract]
        ValueTask<QuestionResponse> GetQuestionsAsync(QuestionRequest request);

        /// <summary>
        /// 
        /// FIXME: CLS COMPLIANCE CA1014
        /// WE USE CALLCONTEXT INSTEAD OF CLIENT AND SERVER IMPLEMENTATION TO GENERATE BOTH AT THE SAME TIME!
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>
        /// QuestionResponse
        /// 
        /// </returns>
        [OperationContract]
        ValueTask<QuestionResponse> GetQuestionsAsync(QuestionRequest request, CallContext context = default);

        [OperationContract]
        ValueTask<QuestionResponse> GetQuestionsAsync(QuestionRequest request, CancellationToken cancellationToken = default);

    }
}
