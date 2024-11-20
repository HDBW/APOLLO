// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Messages
{
    public class SetValueMessage
    {
        public SetValueMessage(string id, string? value)
        {
            Id = id;
            Value = value;
        }

        public string Id { get; }

        public string? Value { get; }
    }
}
