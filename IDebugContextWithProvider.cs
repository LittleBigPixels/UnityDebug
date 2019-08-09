using System;

namespace LBF.Unity
{
    public interface IDebugContextWithProvider : IDebugContext
    {
        IDebugContext this[String key] { get; }
    }
}