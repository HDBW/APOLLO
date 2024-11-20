// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Image = De.HDBW.Apollo.SharedContracts.Models.Image;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class SelectableImageEntry : ImageEntry
    {
        [ObservableProperty]
        private bool _isSelected;
        private Action _interactedHandler;

        private SelectableImageEntry(Image data, string basePath, int density, int size, Action interactedHandler)
            : base(data, basePath, density, size)
        {
            ArgumentNullException.ThrowIfNull(interactedHandler);
            _interactedHandler = interactedHandler;
        }

        public static SelectableImageEntry Import(Image data, string basePath, int density, int size, Action interactedHandler)
        {
            return new SelectableImageEntry(data, basePath, density, size, interactedHandler);
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        private Task ToggleSelection(CancellationToken cancellationToken)
        {
            IsSelected = !IsSelected;
            _interactedHandler.Invoke();
            return Task.CompletedTask;
        }
    }
}
