// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Common.Entities;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.Course.Enums;

namespace De.HDBW.Apollo.Data.Extensions
{
    internal static class TrainingExtensions
    {
        internal static CourseItem? ConvertToCourseItem(this Training item)
        {
            var result = new CourseItem();
            if (item == null)
            {
                return null;
            }

            result.Benefits = string.Join(',', (IEnumerable<string>)item.BenefitList ?? Array.Empty<string>());
            result.CourseProviderId = item.ProviderId.TryToLong();
            result.Description = item.Description ?? string.Empty;
            result.Id = item.Id.TryToLong();
            result.ShortDescription = item.ShortDescription ?? string.Empty;
            result.LoanOptions = string.Join(',', item.Loans?.Select(f => f?.Name ?? string.Empty) ?? Array.Empty<string>());
            result.Availability = item.AccessibilityAvailable ? CourseAvailability.Available : CourseAvailability.Unknown;
            result.CourseTagType = ((IEnumerable<string>)item.Tags ?? Array.Empty<string>()).Select(f => f.ToCourseTagType())?.FirstOrDefault() ?? CourseTagType.Unknown;
            result.CourseUrl = item.ProductUrl ?? new Uri("about:blank");
            result.PublishingDate = item.PublishingDate;
            result.UnPublishingDate = item.UnpublishingDate;
            result.Title = item.TrainingName ?? string.Empty;
            result.TrainingProviderId = item.TrainingProvider?.Id?.TryToLong() ?? 0;

            // TODO: CourseItem Duration, right ??
            result.Duration = TimeSpanExtensions.ToTotalHoursAndMinutes(TimeSpan.FromTicks(item.Appointment?.Sum(x => x.Duration.Ticks) ?? 0)) ?? string.Empty;

            // TODO: CourseItem PredecessorId, right ??
            result.PredecessorId = item.Predecessor.TryToLong();

            // TODO: CourseItem SuccessorId, right ??
            result.SuccessorId = item.Successor.TryToLong();

            // TODO: CourseItem PreRequisitesDescription, right ??
            result.PreRequisitesDescription = string.Join(',', (IEnumerable<string>)item.Prerequisites ?? Array.Empty<string>());

            // TODO: CourseItem Skills ??
            // TODO: CourseItem LatestUpdate ??
            // TODO: CourseItem Currency ??
            // TODO: CourseItem Deleted ??
            // TODO: CourseItem Deprecation ??
            // TODO: CourseItem DeprecationReason ??
            // TODO: CourseItem ExternalId ??
            // TODO: CourseItem InstructorId ??
            // TODO: CourseItem KeyPhrases ??
            // TODO: CourseItem Language ??
            // TODO: CourseItem LatestUpdateFromProvider ??
            // TODO: CourseItem LearningOutcomes ??
            // TODO: CourseItem Occurrence ??
            // TODO: CourseItem Schema ??
            // TODO: CourseItem TargetGroup ??
            // TODO: CourseItem Ticks ??
            // TODO: CourseItem Price, Obsolete ??
            return result;
        }

        internal static CourseAppointment? ConvertToCourseAppointment(this Training item)
        {
            var result = new CourseAppointment();
            if (item?.Appointment == null)
            {
                return null;
            }

            var apiAppointments = item.Appointment;

            // appointments.AppointmentType ??
            result.AppointmentType = apiAppointments.Any(x => x.IsGuaranteed) ? AppointmentType.IsGuaranteed : AppointmentType.Unknow;

            // TODO: CourseAppointment AvailableSeats ??
            // TODO: CourseAppointment BookingCode ??
            // TODO: CourseAppointment IsBookable ??
            // TODO: CourseAppointment Language ??
            // TODO: CourseAppointment LatestUpdate ??
            result.CourseId = item.Id.TryToLong();
            return result;
        }

        internal static EduProviderItem? ConvertToEduProviderItems(this Training item)
        {
            var result = new EduProviderItem();
            if (item?.CourseProvider == null)
            {
                return null;
            }

            var courseProvider = item.CourseProvider;
            result.Id = courseProvider.Id.TryToLong();
            result.Logo = courseProvider.Image ?? new Uri("about:blank");
            result.Name = courseProvider.Name ?? string.Empty;
            result.Description = courseProvider.Description ?? string.Empty;
            result.Website = courseProvider.Url ?? new Uri(string.Empty);

            // result.Ticks??
            return result;
        }

        internal static List<CourseContact>? ConvertToCourseContacts(this Training item)
        {
            if (item?.Contacts?.Any() != true)
            {
                return null;
            }

            var result = new List<CourseContact>();

            foreach (var contact in item.Contacts)
            {
                if (contact.Value == null)
                {
                    continue;
                }

                var resultContact = new CourseContact();
                resultContact.ContactMail = contact.Value.Mail ?? string.Empty;
                resultContact.ContactName = contact.Value.Surname ?? string.Empty;
                resultContact.ContactPhone = contact.Value.Phone ?? string.Empty;
                resultContact.Id = (contact.Value.Id ?? string.Empty).TryToLong();
                resultContact.Url = contact.Value.EAppointmentUrl ?? new Uri("about:blank");
                result.Add(resultContact);
            }

            return result;
        }

        internal static List<CourseContactRelation>? ConvertToCourseContactRelation(this Training item)
        {
            if (item?.Contacts?.Any() != true)
            {
                return null;
            }

            var result = new List<CourseContactRelation>();

            foreach (var contact in item.Contacts)
            {
                if (contact.Value == null)
                {
                    continue;
                }

                var resultCourseContactRelation = new CourseContactRelation();
                resultCourseContactRelation.CourseContactId = contact.Key.TryToLong();
                resultCourseContactRelation.CourseContactId = contact.Value.Id.TryToLong();
                resultCourseContactRelation.CourseId = item.Id.TryToLong();
                result.Add(resultCourseContactRelation);
            }

            return result;
        }
    }
}
