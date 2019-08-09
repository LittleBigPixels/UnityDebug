using System;

namespace LBF.Unity
{
    public static class LogExtensions
    {
        public static void Format(this ILog log, String message, object arg0)
        {
            log.Write(String.Format(message, arg0));
        }

        public static void Format(this ILog log, String message, object arg0, object arg1)
        {
            log.Write(String.Format(message, arg0, arg1));
        }

        public static void WarningFormat(this ILog log, String message, object arg0)
        {
            log.Warning(String.Format(message, arg0));
        }

        public static void WarningFormat(this ILog log, String message, object arg0, object arg1)
        {
            log.Warning(String.Format(message, arg0, arg1));
        }

        public static void ErrorFormat(this ILog log, String message, object arg0)
        {
            log.Error(String.Format(message, arg0));
        }

        public static void ErrorFormat(this ILog log, String message, object arg0, object arg1)
        {
            log.Error(String.Format(message, arg0, arg1));
        }
    }
}
