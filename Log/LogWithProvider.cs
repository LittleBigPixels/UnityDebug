namespace LBF.Unity
{
    public class LogMixed : ILogWithProvider
    {
        public ILog this[string key] {
            get { return m_contextProvider[key].Log; }
        }

        IDebugContextProvider m_contextProvider;
        ILog m_log;

        public LogMixed(ILog baseLog, IDebugContextProvider debugContextProvider)
        {
            m_log = baseLog;
            m_contextProvider = debugContextProvider;
        }

        public void Write(string message) { m_log.Write(message); }
        public void Warning(string message) { m_log.Warning(message); }
        public void Error(string message) { m_log.Error(message); }
        public void Assert(bool expression, string message) { m_log.Assert(expression, message); }
        public void Assert(bool expression) { m_log.Assert(expression); }
    }
}