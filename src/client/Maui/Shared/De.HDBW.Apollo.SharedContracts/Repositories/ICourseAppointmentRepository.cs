﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.Course;

namespace De.HDBW.Apollo.SharedContracts.Repositories
{
    public interface ICourseAppointmentRepository :
        IRepository<CourseAppointment>,
        IDatabaseRepository<CourseAppointment>
    {
        Task<IEnumerable<CourseAppointment>> GetItemsByForeignKeyAsync(long value, CancellationToken token);
    }
}
