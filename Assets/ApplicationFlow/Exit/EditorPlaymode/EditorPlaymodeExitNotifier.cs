#nullable enable

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor;
using VContainer.Unity;

using static System.Threading.CancellationTokenSource;

namespace ApplicationFlow.Exit.EditorPlaymode
{
    using Abstract;

    public sealed class EditorPlaymodeExitNotifier : IApplicationExitNotifier, IAsyncStartable, IDisposable
    {
        private CancellationTokenSource? _abortion;

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

            EditorApplication.playModeStateChanged += PlayModeStateChanged;

            return UniTask.CompletedTask;
        }

        void IDisposable.Dispose()
        {
            if (_abortion is null) return;

            _abortion.Dispose();
            _abortion = null;

            EditorApplication.playModeStateChanged -= PlayModeStateChanged;
        }

        private void PlayModeStateChanged(PlayModeStateChange change)
        {
            if (change != PlayModeStateChange.ExitingPlayMode) return;

            _abortion?.Cancel();
        }
    }
}
