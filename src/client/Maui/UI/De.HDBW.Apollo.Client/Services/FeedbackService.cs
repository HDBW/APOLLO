// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly TelemetryClient _telemetryClient;

        public FeedbackService(ILogger<FeedbackService> logger)
        {
            Logger = logger;
            _telemetryClient = new TelemetryClient(TelemetryConstants.Configuration);
            _telemetryClient.Context.Session.Id = TelemetryConstants.SessionId;
        }

        private ILogger<FeedbackService> Logger { get; }

        public async Task<bool> SendFeedbackAsync(string feedback, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            try
            {
                // TODO: Serialize not empty Assessment into single message and trace it.
                _telemetryClient.TrackTrace(feedback);
                await _telemetryClient.FlushAsync(token).ConfigureAwait(false);
                return true;
            }
            catch (OperationCanceledException)
            {
                Logger?.LogDebug($"Canceled {nameof(SendFeedbackAsync)} in {GetType()}.");
            }
            catch (ObjectDisposedException)
            {
                Logger?.LogDebug($"Canceled {nameof(SendFeedbackAsync)} in {GetType()}.");
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknow error while {nameof(SendFeedbackAsync)} in {GetType()}.");
            }

            return false;
        }
    }
}
