using System;

namespace LBF.Unity
{
    public class DebugContext : IDebugContext
    {
        public MeshBuilder Renderer { get; protected set; }
        public ILog Log { get { return m_log; } }

        UnityDebugLog m_log;

        public DebugContext(String category, IMeshBuilderImplementation meshBuilderImpl)
        {
            Renderer = new MeshBuilder(meshBuilderImpl);
            m_log = new UnityDebugLog(category);
        }
    }
}