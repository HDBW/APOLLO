// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using De.HDBW.Apollo.Client.Contracts;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.Helper
{
    public static class DispatcherServiceExtensions
    {
        public static Task SafeExecuteOnMainThreadAsync(this IDispatcherService dispatcherService, Action action, ILogger log, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            Action method = new Action(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    log?.LogError(ex, "Exception thrown when invoking action via dispatcherService");
                    if (ex.InnerException != null)
                    {
                        log?.LogDebug("InnerException masked " + ex.InnerException.ToString());
                    }
                }
            });
            return dispatcherService.BeginInvokeOnMainThreadAsync(method, token);
        }

        public static Task SafeExecuteOnMainThreadAsync(this IDispatcherService dispatcherService, Func<Task> action, ILogger log, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            // Because ExecuteOnMainThreadAsync crashes and does not mask the exception, we need to mask it ourself.
            var method = new Func<Task>(async () =>
            {
                try
                {
                    await action();
                }
                catch (Exception ex)
                {
                    log?.LogTrace(ex, "Exception thrown when invoking action via dispatcherService");
                    if (ex.InnerException != null)
                    {
                        log?.LogDebug("InnerException masked " + ex.InnerException.ToString());
                    }
                }
            });
            return dispatcherService.ExecuteOnMainThreadAsync(method, token);
        }
    }
}
