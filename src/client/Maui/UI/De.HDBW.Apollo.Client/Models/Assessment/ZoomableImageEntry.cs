// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Input;
using Image = De.HDBW.Apollo.SharedContracts.Models.Image;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class ZoomableImageEntry : ImageEntry
    {
        private ZoomableImageEntry(Image data, string basePath, int density, int size)
            : base(data, basePath, density, size)
        {
        }

        public static new ZoomableImageEntry Import(Image data, string basePath, int density, int size)
        {
            return new ZoomableImageEntry(data, basePath, density, size);
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        private Task Zoom(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
