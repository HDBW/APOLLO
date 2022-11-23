// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Input;

namespace De.HDBW.Apollo.Client.Contracts
{
    public interface IAssociateSourceInteractiveEntry : IInteractiveEntry
    {
        int IndexToAssociate { get; }

        bool IsNotAssociated { get; }

        bool IsAssociated { get; set; }

        IRelayCommand DragStartingCommand { get; }

        IRelayCommand DropCompletedCommand { get; }
    }
}
