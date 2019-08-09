namespace LBF.Unity
{
    public class DebugContextWithProvider : IDebugContextWithProvider
    {
        public MeshBuilder Renderer { get { return m_context.Renderer; } }
        public ILog Log { get { return m_context.Log; } }

        public IDebugContext this[string key] {
            get { return m_contextProvider[key]; }
        }

        IDebugContextProvider m_contextProvider;
        IDebugContext m_context;

        public DebugContextWithProvider(IDebugContext debugContext, IDebugContextProvider debugContextProvider)
        {
            m_context = debugContext;
            m_contextProvider = debugContextProvider;
        }
    }
}