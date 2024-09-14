#nullable enable

namespace ApplicationFlow.Exit.Runtime
{
    using Abstract;

    public sealed class RuntimeExitRoutine : IApplicationExitRoutine
    {
        void IApplicationExitRoutine.Perform() => UnityEngine.Application.Quit();
    }
}
