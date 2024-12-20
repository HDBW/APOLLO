﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Input;

namespace De.HDBW.Apollo.Client.Contracts
{
    public interface ISelectableEntry
    {
        bool IsSelected { get; set; }

        IRelayCommand ToggleSelectionCommand { get; }

        void UpdateSelectedState(bool isSelected);
    }
}
