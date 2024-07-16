// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using Invite.Apollo.App.Graph.Common.Models.Assessments;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class ModuleScoreDecoEntry : DecoEntry
    {
        [ObservableProperty]
        private List<ModuleScoreEntry> _segments = new List<ModuleScoreEntry>();

        private ModuleScoreDecoEntry(List<ModuleScoreEntry> segments, AssessmentType type)
            : base(type)
        {
            Segments = new List<ModuleScoreEntry>(segments);
        }

        public static ModuleScoreDecoEntry Import(List<ModuleScoreEntry> segments, AssessmentType type)
        {
            return new ModuleScoreDecoEntry(segments, type);
        }
    }
}
