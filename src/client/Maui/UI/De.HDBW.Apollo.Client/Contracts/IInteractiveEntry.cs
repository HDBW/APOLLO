// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace De.HDBW.Apollo.Client.Contracts
{
    public interface IInteractiveEntry
    {
        InteractionType Interaction { get; internal set; }

        object Data { get; }
    }
}
