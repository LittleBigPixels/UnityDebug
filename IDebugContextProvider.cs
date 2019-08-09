using System;

namespace LBF.Unity
{
    public interface IDebugContextProvider
    {
        IDebugContext this[String key] { get; }
        IDebugContext Get(String key);
    }
}