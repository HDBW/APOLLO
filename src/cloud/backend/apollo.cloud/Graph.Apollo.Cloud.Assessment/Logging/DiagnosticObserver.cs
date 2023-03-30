using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Invite.Apollo.App.Graph.Assessment.Logs
{
    //https://learn.microsoft.com/de-de/ef/core/logging-events-diagnostics/diagnostic-listeners
    //https://github.com/dotnet/EntityFramework.Docs/tree/main/samples/core/Miscellaneous/Logging
    public class DiagnosticObserver : IObserver<DiagnosticListener>
    {
        public void OnCompleted()
            => throw new NotImplementedException();

        public void OnError(Exception error)
            => throw new NotImplementedException();

        public void OnNext(DiagnosticListener value)
        {
            if (value.Name == DbLoggerCategory.Name) // "Microsoft.EntityFrameworkCore"
            {
                value.Subscribe(new KeyValueObserver());
            }
        }
    }
}
