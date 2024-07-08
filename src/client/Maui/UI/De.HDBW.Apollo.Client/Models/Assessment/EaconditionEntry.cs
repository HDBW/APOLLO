// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.SharedContracts;
using De.HDBW.Apollo.SharedContracts.Questions;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class EaconditionEntry : AbstractQuestionEntry<Eacondition>
    {
        [ObservableProperty]
        private ObservableCollection<SelectableImageEntry> _images = new ObservableCollection<SelectableImageEntry>();

        [ObservableProperty]
        private SelectableImageEntry? _image;

        [ObservableProperty]
        private string? _beispielberufe;

        [ObservableProperty]
        private string? _voraussetzungen;

        [ObservableProperty]
        private string? _infotext;

        [ObservableProperty]
        private string? _bezeichnung;

        [ObservableProperty]
        private string? _situation;

        protected EaconditionEntry(Eacondition data, string basePath, int density, Dictionary<string, int> imageSizeConfig)
            : base(data)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(basePath);
            Images = new ObservableCollection<SelectableImageEntry>(data.Images.Select(x => SelectableImageEntry.Import(x, basePath, density, imageSizeConfig[nameof(data.Images)])));
            Image = Images.FirstOrDefault();
            Beispielberufe = GenerateHtmlList(data.Beispielberufe);
            Voraussetzungen = GenerateHtmlList(data.Voraussetzungen);
            Infotext = data.Infotext;
            Bezeichnung = data.Bezeichnung;
            Situation = data.Situation;
            foreach (var link in data.Links ?? new List<Reliant>())
            {
                Links.Add(link);
            }
        }

        public List<Reliant> Links { get; } = new List<Reliant>();

        public static EaconditionEntry Import(Eacondition data, string basePath, int density, Dictionary<string, int> imageSizeConfig)
        {
            return new EaconditionEntry(data, basePath, density, imageSizeConfig);
        }

        public override double? GetScore()
        {
            // TODO
            return Data.CalculateScore(1);
        }

        private string? GenerateHtmlList(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            var parts = text.Split(";").Select(x => $"<li>{x}</li>");
            return $"<ul>{Environment.NewLine}{string.Join(Environment.NewLine, parts)}{Environment.NewLine}</ul>";
        }
    }
}
