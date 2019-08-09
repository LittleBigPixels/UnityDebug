using System;

namespace LBF.Unity
{
    public interface ILogWithProvider : ILog
    {
        ILog this[String key] { get; }
    }
}
