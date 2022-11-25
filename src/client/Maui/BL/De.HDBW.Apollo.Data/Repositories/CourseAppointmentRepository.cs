// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class CourseAppointmentRepository :
        AbstractInMemoryRepository<CourseAppointment>,
        ICourseAppointmentRepository
    {
        public CourseAppointmentRepository(ILogger<CourseAppointmentRepository> logger)
            : base(logger)
        {
        }

        public Task<IEnumerable<CourseAppointment>> GetItemsByForeignKeyAsync(long id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var result = new ReadOnlyCollection<CourseAppointment>(Items.Where(i => i.CourseId == id).ToList());
            return Task.FromResult(result as IEnumerable<CourseAppointment>);
        }
    }
}
