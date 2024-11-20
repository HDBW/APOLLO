// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using Invite.Apollo.App.Graph.Common.Models.Assessments;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class DecoEntry : ObservableObject
    {
        protected DecoEntry(AssessmentType type)
        {
            Type = type;
        }

        public AssessmentType Type { get; }

        public static DecoEntry Import(AssessmentType type)
        {
            return new DecoEntry(type);
        }
    }
}
