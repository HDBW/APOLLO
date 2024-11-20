// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Contracts
{
    public interface IAudioPlayerService
    {
        bool IsPaused { get; }

        bool IsPlaying { get; }

        void Stop(EventHandler completedHandler);

        void Pause();

        void Resume();

        Task<bool> StartAsync(string absolutePath, EventHandler completedHandler, CancellationToken token);
    }
}
