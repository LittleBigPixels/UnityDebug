using System;

namespace LBF.Unity
{
    public class UnityDebugLog : ILog
    {
        public bool Enabled { get; set; }

        String m_category;

        public UnityDebugLog(String category)
        {
            Enabled = true;
            m_category = category;
        }

        public void Write(string message)
        {
            if (Enabled == false) return;
            UnityEngine.Debug.LogFormat("[{0}] {1}", m_category, message);
        }

        public void Warning(string message)
        {
            if (Enabled == false) return;
            UnityEngine.Debug.LogWarningFormat("[{0}] {1}", m_category, message);
        }

        public void Error(string message)
        {
            if (Enabled == false) return;
            UnityEngine.Debug.LogErrorFormat("[{0}] {1}", m_category, message);
        }

        public void Assert(bool expression, string message)
        {
            if (Enabled == false) return;
            if (expression == false)
                Error(message);
        }

        public void Assert(bool expression)
        {
            if (Enabled == false) return;
            if (expression == false)
                Error("An assert has failed");
        }
    }
}
