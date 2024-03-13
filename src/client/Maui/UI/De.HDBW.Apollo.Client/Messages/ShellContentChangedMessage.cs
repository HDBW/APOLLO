// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Messages
{
    public class ShellContentChangedMessage
    {
        public ShellContentChangedMessage(Type? newViewModelType)
        {
            NewViewModelType = newViewModelType;
        }

        public Type? NewViewModelType { get; }
    }
}
