// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
namespace De.HDBW.Apollo.Client.Contracts
{
    public interface ITouchAwareView : IView
    {
        void OnTouchDown(float x, float y);

        void OnTouchUp(float x, float y);
    }
}
