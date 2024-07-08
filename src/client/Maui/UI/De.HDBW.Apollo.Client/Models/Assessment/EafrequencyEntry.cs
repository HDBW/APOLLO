// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.SharedContracts.Questions;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class EafrequencyEntry : AbstractQuestionEntry<Eafrequency>
    {
        private readonly Action<EafrequencyEntry> _completedHandler;

        [ObservableProperty]
        private ObservableCollection<ImageEntry> _images = new ObservableCollection<ImageEntry>();

        [ObservableProperty]
        private string? _situation;

        [ObservableProperty]
        private bool _is0Selected;

        [ObservableProperty]
        private bool _is1Selected;

        [ObservableProperty]
        private bool _is2Selected;

        [ObservableProperty]
        private bool _is3Selected;

        private EafrequencyEntry(Eafrequency data, string basePath, int density, Dictionary<string, int> imageSizeConfig, Action<EafrequencyEntry> completedHandler)
            : base(data)
        {
            ArgumentNullException.ThrowIfNull(basePath);
            ArgumentNullException.ThrowIfNull(completedHandler);
            _completedHandler = completedHandler;
            Images = new ObservableCollection<ImageEntry>(data.Images.Select(x => ImageEntry.Import(x, basePath, density, imageSizeConfig[nameof(data.Images)])));
            Situation = data.Situation;
        }

        public static EafrequencyEntry Import(Eafrequency data, string basePath, int density, Dictionary<string, int> imageSizeConfig, Action<EafrequencyEntry> completedHandler)
        {
            return new EafrequencyEntry(data, basePath, density, imageSizeConfig, completedHandler);
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

            return null;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(Is0Selected) ||
                e.PropertyName == nameof(Is1Selected) ||
                e.PropertyName == nameof(Is2Selected) ||
                e.PropertyName == nameof(Is3Selected))
            {
                _completedHandler.Invoke(this);
            }
        }
    }
}
