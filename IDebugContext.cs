namespace LBF.Unity
{
    public interface IDebugContext
    {
        MeshBuilder Renderer { get; }
        ILog Log{ get; }
    }
}
