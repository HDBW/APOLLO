// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Enums;

namespace De.HDBW.Apollo.Client.Messages
{
    public class LiveCycleChangedMessage
    {
        public LiveCycleChangedMessage(LifeCycleState state)
        {
            State = state;
        }

        public LifeCycleState State { get; }
    }
}
