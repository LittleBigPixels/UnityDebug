using System;

namespace LBF.Unity
{
    public interface ILog
    {
        void Write(String message);
        void Warning(String message);
        void Error(String message);
        void Assert(bool expression);
        void Assert(bool expression, String message);
    }
}
