#nullable enable

using UnityEditor;

namespace ApplicationFlow.Exit.EditorPlaymode
{
    using Abstract;

    public sealed class EditorPlaymodeExitRoutine : IApplicationExitRoutine
    {
        void IApplicationExitRoutine.Perform() => EditorApplication.ExitPlaymode();
    }
}
