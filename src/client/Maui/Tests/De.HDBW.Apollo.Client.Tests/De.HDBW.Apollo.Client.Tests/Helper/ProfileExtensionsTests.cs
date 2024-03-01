// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Data.Tests.Extensions;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace De.HDBW.Apollo.Client.Tests.Helper
{
    public class ProfileExtensionsTests : IDisposable
    {
        public ProfileExtensionsTests(ITestOutputHelper outputHelper)
        {
            Logger = this.SetupLogger<ProfileExtensionsTests>(outputHelper);
        }

        private ILogger Logger { get; }

        public void Dispose()
        {
        }

        [Fact]
        public void TestQualfificationSorting()
        {
            var items = new List<Qualification>
            {
                new Qualification() { Name = "x", Description = "A", IssueDate = DateTime.Today, ExpirationDate = DateTime.Today },
                new Qualification() { Name = "A", Description = "A", IssueDate = DateTime.Today, ExpirationDate = DateTime.Today },
                new Qualification() { Name = "A", Description = "A", IssueDate = DateTime.Today, },
                new Qualification() { Name = "A", Description = "A", IssueDate = DateTime.Today.AddDays(-1), },
                new Qualification() { Name = "A", Description = "A", },
                new Qualification() { Name = "B", Description = "A", },
                new Qualification() { Name = "C", Description = "A", IssueDate = DateTime.Today.AddDays(-1), ExpirationDate = DateTime.Today },
                new Qualification() { Name = "C", Description = "A", ExpirationDate = DateTime.Today },
                new Qualification() { Name = "C", Description = "A", IssueDate = DateTime.Today.AddDays(-3), ExpirationDate = DateTime.Today.AddDays(-1) },
            };

            var sorted = ProfileExtensions.AsSortedList(items);
            var lines = new List<string>();
            foreach (var item in sorted)
            {
                lines.Add($"{item.Name} {GetDateRangeText(item.IssueDate, item.ExpirationDate)}");
            }

            Logger.LogDebug(string.Join(Environment.NewLine, lines));
            Assert.Equal(items[4], sorted[0]);
            Assert.Equal(items[5], sorted[1]);
            Assert.Equal(items[2], sorted[2]);
            Assert.Equal(items[3], sorted[3]);
            Assert.Equal(items[1], sorted[4]);
            Assert.Equal(items[7], sorted[5]);
            Assert.Equal(items[0], sorted[6]);
        }

        private string? GetDateRangeText(DateTime? start, DateTime? end)
        {
            if (start.HasValue && end.HasValue)
            {
                return $"{start?.ToShortDateString()}-{end?.ToShortDateString()}";
            }

            if (start.HasValue && !end.HasValue)
            {
                return $"seit {start?.ToShortDateString()}";
            }

            if (end.HasValue)
            {
                return $"bis {end?.ToShortDateString()}";
            }

            return null;
        }
    }
}
