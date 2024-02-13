// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
namespace De.HDBW.Apollo.Client.Contracts
{
    public interface ITouchService
    {
        void TouchDownReceived(float x, float y);

        void TouchUpReceived(float x, float y);

        void UnregisterView(ITouchAwareView view);

        void RegisterView(ITouchAwareView view);
    }
}
