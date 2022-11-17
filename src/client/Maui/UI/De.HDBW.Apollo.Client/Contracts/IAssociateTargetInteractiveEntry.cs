// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Input;

namespace De.HDBW.Apollo.Client.Contracts
{
    public interface IAssociateTargetInteractiveEntry : IInteractiveEntry
    {
        int? AssociatedIndex { get; set; }

        bool HasAssociation { get; }

        IRelayCommand DropCommand { get; }

        IRelayCommand ClearAssociationCommand { get; }
    }
}
