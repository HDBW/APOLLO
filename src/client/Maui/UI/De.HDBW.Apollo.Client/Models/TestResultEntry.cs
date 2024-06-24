// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models
{
    public partial class TestResultEntry : ObservableObject
    {
        [ObservableProperty]
        private string _text;

        [ObservableProperty]
        private double _score;

        public TestResultEntry(string text, double score)
        {
            Text = text;
            Score = score;
        }
    }
}
