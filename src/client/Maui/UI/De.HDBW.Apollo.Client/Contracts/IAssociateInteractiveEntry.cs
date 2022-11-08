// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace De.HDBW.Apollo.Client.Contracts
{
    public interface IAssociateInteractiveEntry : IInteractiveEntry
    {
        IRelayCommand DragSartingCommand { get; }

        IRelayCommand DropCompletedCommand { get; }
    }
}
