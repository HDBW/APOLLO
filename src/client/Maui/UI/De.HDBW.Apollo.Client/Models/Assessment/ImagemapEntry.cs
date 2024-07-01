// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.SharedContracts.Models;
using De.HDBW.Apollo.SharedContracts.Questions;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class ImagemapEntry : AbstractQuestionEntry
    {
        [ObservableProperty]
        private ObservableCollection<Shape> _shapes = new ObservableCollection<Shape>();

        [ObservableProperty]
        private ImageEntry? _image;

        private ImagemapEntry(Imagemap data, string basePath, int density, Dictionary<string, int> imageSizeConfig)
            : base(data)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(basePath);
            Shapes = new ObservableCollection<Shape>(data.Shapes);
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
    }
}
