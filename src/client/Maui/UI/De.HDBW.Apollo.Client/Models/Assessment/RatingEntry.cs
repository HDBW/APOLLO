// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.SharedContracts.Questions;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class RatingEntry : AbstractQuestionEntry<Rating>
    {
        [ObservableProperty]
        private int _numberOfChoices;

        [ObservableProperty]
        private bool _is0Selected;

        [ObservableProperty]
        private bool _is1Selected;

        [ObservableProperty]
        private bool _is2Selected;

        [ObservableProperty]
        private bool _is3Selected;

        [ObservableProperty]
        private bool _is4Selected;

        [ObservableProperty]
        private bool _is5Selected;

        private RatingEntry(Rating data)
            : base(data)
        {
            NumberOfChoices = data.NumberOfChoices;
        }

        public override bool DidInteract { get; protected set; }

        public static RatingEntry Import(Rating data)
        {
            return new RatingEntry(data);
        }

        public override double? GetScore()
        {
            if (Is0Selected)
            {
                return Data.CalculateScore(1);
            }
            else if (Is1Selected)
            {
                return Data.CalculateScore(2);
            }
            else if (Is2Selected)
            {
                return Data.CalculateScore(3);
            }
            else if (Is3Selected)
            {
                return Data.CalculateScore(4);
            }
            else if (Is4Selected)
            {
                return Data.CalculateScore(5);
            }
            else if (Is5Selected)
            {
                return Data.CalculateScore(6);
            }

            return null;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(Is0Selected) ||
                e.PropertyName == nameof(Is1Selected) ||
                e.PropertyName == nameof(Is2Selected) ||
                e.PropertyName == nameof(Is3Selected) ||
                e.PropertyName == nameof(Is4Selected) ||
                e.PropertyName == nameof(Is5Selected))
            {
                DidInteract = true;
            }
        }
    }
}
