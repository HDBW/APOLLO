// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.SharedContracts.Questions;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class RatingEntry : AbstractQuestionEntry<Rating>
    {
        [ObservableProperty]
        private int _numberOfChoices;

        private RatingEntry(Rating data)
            : base(data)
        {
            NumberOfChoices = data.NumberOfChoices;
        }

        public static RatingEntry Import(Rating data)
        {
            return new RatingEntry(data);
        }

        public override double GetScore()
        {
            // TODO
            return Data.CalculateScore(0);
        }
    }
}
