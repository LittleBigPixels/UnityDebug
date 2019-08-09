using System.Collections.Generic;
using UnityEngine;

namespace LBF.Unity
{
    public class BufferMeshImplementation : IMeshBuilderImplementation
    {
        public List<Vector3> Vertices;
        public List<Color> Colors;
        public List<int> LineIndices;
        public List<int> TriangleIndices;
        public int VertexCount;

        public BufferMeshImplementation()
        {
            Vertices = new List<Vector3>();
            Colors = new List<Color>();
            LineIndices = new List<int>();
            TriangleIndices = new List<int>();
        }

        public void Reset()
        {
            Vertices.Clear();
            Colors.Clear();
            LineIndices.Clear();
            TriangleIndices.Clear();

            VertexCount = 0;
        }

        public int Vertex(Vector3 v)
        {
            Vertices.Add(v);
            return VertexCount++;
        }

        public int Vertex(Vector3 v, Color color)
        {
            Vertices.Add(v);
            Colors.Add(color);
            return VertexCount++;
        }

        public void Line(int i)
        {
            LineIndices.Add(i);
        }

        public void Line(int i1, int i2)
        {
            LineIndices.Add(i1);
            LineIndices.Add(i2);
        }

        public void Triangle(int i)
        {
            TriangleIndices.Add(i);
        }

        public void Triangle(int i1, int i2, int i3)
        {
            TriangleIndices.Add(i1);
            TriangleIndices.Add(i2);
            TriangleIndices.Add(i3);
        }

        public void Quad(int i1, int i2, int i3, int i4)
        {
            Triangle(i1, i2, i3);
            Triangle(i3, i4, i1);
        }
    }
}