// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Helper;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Xunit;

namespace De.HDBW.Apollo.Client.Tests.Helper
{
    public class ProfileExtensionsTests : IDisposable
    {
        public void Dispose()
        {
        }

        [Fact]
        public void TestQualfificationSorting()
        {
            var items = new List<Qualification>
            {
                new Qualification() { Name = "A", Description = "A", IssueDate = DateTime.Today, ExpirationDate = DateTime.Today },
                new Qualification() { Name = "A", Description = "A", IssueDate = DateTime.Today, },
                new Qualification() { Name = "A", Description = "A", IssueDate = DateTime.Today.AddDays(-1), },
                new Qualification() { Name = "A", Description = "A", },
                new Qualification() { Name = "B", Description = "A", },
                new Qualification() { Name = "C", Description = "A", IssueDate = DateTime.Today, ExpirationDate = DateTime.Today  },
            };

            var sorted = ProfileExtensions.AsSortedList(items);
            Assert.Equal(items[3], sorted[0]);
            Assert.Equal(items[4], sorted[1]);
            Assert.Equal(items[1], sorted[2]);
            Assert.Equal(items[0], sorted[3]);
            Assert.Equal(items[5], sorted[4]);
            Assert.Equal(items[2], sorted[5]);
        }
    }
}
