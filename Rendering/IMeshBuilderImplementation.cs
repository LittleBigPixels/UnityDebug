using UnityEngine;

namespace LBF.Unity
{
    public interface IMeshBuilderImplementation
    {
        int Vertex(Vector3 v, Color color);

        void Line(int i);
        void Line(int i1, int i2);
        void Triangle(int i);
        void Triangle(int i1, int i2, int i3);
    }
}
