﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using OccupationGrpcService.Protos;

namespace OccupationGrpcService.Services
{
    [ServiceContract(Name = "Occupation.Service")]
    public interface IOccupationService
    {
        [OperationContract]
        Task<OccupationResponse> GetOccupation(OccupationRequest request, CancellationToken ctx);

        [OperationContract]
        Task<OccupationCreationResponse> CreateOrUpdate(OccupationCreationRequest request, CancellationToken ctx);

        [OperationContract]
        Task<OccupationListResponse> GetOccupationsList(OccupationListRequest request, CancellationToken ctx);
    }
}
