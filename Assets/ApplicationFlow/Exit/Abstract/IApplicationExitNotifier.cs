#nullable enable

using System.Threading;

namespace ApplicationFlow.Exit.Abstract
{
    public interface IApplicationExitNotifier
    {
        CancellationToken AboutToExitToken { get; }
    }
}
