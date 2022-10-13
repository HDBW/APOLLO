// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Contracts
{
    public interface IFeedbackService
    {
        // TODO: feedback is a list assessment of type input.
        Task<bool> SendFeedbackAsync(string feedback, CancellationToken token);
    }
}
