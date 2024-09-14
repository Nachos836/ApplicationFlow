#nullable enable

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

using static System.Threading.CancellationTokenSource;

namespace ApplicationFlow.Exit.Runtime
{
    using Abstract;

    public sealed class RuntimeExitNotifier : IApplicationExitNotifier, IAsyncStartable, IDisposable
    {
        private CancellationTokenSource? _abortion;

        CancellationToken IApplicationExitNotifier.AboutToExitToken => _abortion?.Token ?? CancellationToken.None;

        UniTask IAsyncStartable.StartAsync(CancellationToken cancellation)
        {
            _abortion?.Dispose();
            _abortion ??= CreateLinkedTokenSource(cancellation, UnityEngine.Application.exitCancellationToken);

            return UniTask.CompletedTask;
        }

        void IDisposable.Dispose() => _abortion?.Dispose();
    }
}
