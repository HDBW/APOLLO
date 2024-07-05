// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.SharedContracts.Models;
using De.HDBW.Apollo.SharedContracts.Questions;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class ImagemapEntry : AbstractQuestionEntry<Imagemap>
    {
        [ObservableProperty]
        private ObservableCollection<InteractionShape> _shapes = new ObservableCollection<InteractionShape>();

        [ObservableProperty]
        private ImageEntry? _image;

        private ImagemapEntry(Imagemap data, string basePath, int density, Dictionary<string, int> imageSizeConfig)
            : base(data)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(basePath);
            Shapes = new ObservableCollection<InteractionShape>(data.Shapes.Select(shape => shape switch
            {
                CircleShape circle => new InteractionCircle(new Point(circle.X, circle.Y), circle.Radius, false) as InteractionShape,
                RectangleShape rect => new InteractionRectangle(new Rect(rect.X, rect.Y, rect.Width, rect.Height), false),
                //Todo: PolygonShape poly =>
                _ => throw new ArgumentException(),
            }));
            if (data.Image == null)
            {
                return;
            }

            Image = ImageEntry.Import(data.Image, basePath, density, imageSizeConfig[nameof(data.Image)]);
        }

        public static ImagemapEntry Import(Imagemap data, string basePath, int density, Dictionary<string, int> imageSizeConfig)
        {
            return new ImagemapEntry(data, basePath, density, imageSizeConfig);
        }

        public override double GetScore()
        {
            //TODO:
            return Data.CalculateScore("");
        }
    }
}
