// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using Image = De.HDBW.Apollo.SharedContracts.Models.Image;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public class ImageEntry : ObservableObject
    {
        private readonly Image _data;

        private readonly string _basePath;

        private readonly int _density;

        private readonly int _size;

        protected ImageEntry(Image data, string basePath, int density, int size)
        {
            ArgumentNullException.ThrowIfNull(data);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(basePath);
            if (density < 1 || density > 4)
            {
                throw new ArgumentOutOfRangeException(nameof(density));
            }

            if (size < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            _data = data;
            _basePath = basePath;
            _density = density;
            _size = size;
        }

        public string AbsolutePath
        {
            get
            {
                var fileName = $"{_data.id}_{_size}@{_density}.jpg";
                return Path.Combine(_basePath, fileName);
            }
        }

        public static ImageEntry Import(Image data, string basePath, int density, int size)
        {
            return new ImageEntry(data, basePath, density, size);
        }

        public Image Export()
        {
            return _data;
        }
    }
}
