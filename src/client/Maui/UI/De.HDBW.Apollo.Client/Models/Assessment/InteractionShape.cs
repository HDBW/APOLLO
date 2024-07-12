// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public abstract partial class InteractionShape : ObservableObject
    {
        [ObservableProperty]
        private bool _isSelected;
        private Action _interactedHandler;

        protected InteractionShape(bool isSelected, Action interactedHandler)
        {
            ArgumentNullException.ThrowIfNull(interactedHandler);
            IsSelected = isSelected;
            _interactedHandler = interactedHandler;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(IsSelected))
            {
                _interactedHandler?.Invoke();
            }
        }
    }
}
