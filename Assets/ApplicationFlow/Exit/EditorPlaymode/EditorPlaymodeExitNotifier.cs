#nullable enable

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor;

#if FLOW_WITH_VCONTAINER
using VContainer.Unity;
#endif

using static System.Threading.CancellationTokenSource;

namespace ApplicationFlow.Exit.EditorPlaymode
{
    using Abstract;

    public sealed class EditorPlaymodeExitNotifier : IApplicationExitNotifier, IAsyncStartable, IDisposable
    {
        private readonly Action<PlayModeStateChange> _playModeStateChanged;

        private CancellationTokenSource? _abortion;

        public EditorPlaymodeExitNotifier()
        {
            _playModeStateChanged = change =>
            {
                if (change != PlayModeStateChange.ExitingPlayMode) return;

                _abortion?.Cancel();
            };
        }

        CancellationToken IApplicationExitNotifier.AboutToExitToken
        {
            get
            {
                _abortion ??= CreateLinkedTokenSource(CancellationToken.None, UnityEngine.Application.exitCancellationToken);

                return _abortion.Token;
            }
        }

        UniTask IAsyncStartable.StartAsync(CancellationToken cancellation)
        {
            _abortion?.Dispose();
            _abortion = CreateLinkedTokenSource(cancellation, UnityEngine.Application.exitCancellationToken);

            EditorApplication.playModeStateChanged += _playModeStateChanged;

            return UniTask.CompletedTask;
        }

        void IDisposable.Dispose()
        {
            if (_abortion is null) return;

            _abortion.Dispose();
            _abortion = null;

            EditorApplication.playModeStateChanged -= _playModeStateChanged;
        }
    }
}
