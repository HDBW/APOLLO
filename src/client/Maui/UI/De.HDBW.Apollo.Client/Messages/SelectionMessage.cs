// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Messages
{
    public class SelectionMessage
    {
        public SelectionMessage()
        {
        }

        public SelectionMessage(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}
