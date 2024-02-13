// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Models.Interactions;

namespace De.HDBW.Apollo.Client.Models.Generic
{
    public partial class RecommendationValue : ObservableObject
    {
        [ObservableProperty]
        private string? _headline;

        [ObservableProperty]
        private string? _subline;

        [ObservableProperty]
        private double? _progress;

        [ObservableProperty]
        private string? _progressDisplayText;

        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _recommendations;

        protected RecommendationValue(string? headline, string? subline, double? progress, List<InteractionEntry> recommendations)
        {
            Headline = headline;
            Subline = subline;
            Progress = progress;
            ProgressDisplayText = Progress != null ? string.Format("{0:P0}", Progress) : null;
            Recommendations = new ObservableCollection<InteractionEntry>(recommendations);
        }

        public static RecommendationValue Import(string? headline, string? subline, double? progress, List<InteractionEntry> recommendations)
        {
            return new RecommendationValue(headline, subline, progress, recommendations);
        }
    }
}
