// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public abstract partial class InteractionShape : ObservableObject
    {
        [ObservableProperty]
        private bool _isSelected;

        protected InteractionShape(bool isSelected)
        {
            IsSelected = isSelected;
        }
    }
}
