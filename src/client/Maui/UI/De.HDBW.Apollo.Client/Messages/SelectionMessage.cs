// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Messages
{
    public class SelectionMessage
    {
        public SelectionMessage(string id, string text)
        {
            Id = id;
            Text = text;
        }

        public string Id { get; }

        public string Text { get; }
    }
}
