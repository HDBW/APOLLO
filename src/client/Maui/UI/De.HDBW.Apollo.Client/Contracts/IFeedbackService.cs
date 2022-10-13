namespace De.HDBW.Apollo.Client.Contracts
{
    public interface IFeedbackService
    {
        // TODO: feedback is a list assessment of type input.
        Task<bool> SendFeedbackAsync(string feedback, CancellationToken token);
    }
}
