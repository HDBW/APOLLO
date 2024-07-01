// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Image = De.HDBW.Apollo.SharedContracts.Models.Image;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class SelectableImageEntry : ImageEntry
    {
        [ObservableProperty]
        private bool _isSelected;

        private SelectableImageEntry(Image data, string basePath, int density, int size)
            : base(data, basePath, density, size)
        {
        }

        public static new SelectableImageEntry Import(Image data, string basePath, int density, int size)
        {
            return new SelectableImageEntry(data, basePath, density, size);
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        private Task ToggleSelection(CancellationToken cancellationToken)
        {
            IsSelected = !IsSelected;
            return Task.CompletedTask;
        }
    }
}
