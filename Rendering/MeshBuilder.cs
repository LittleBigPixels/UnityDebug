using LBF.Unity.Geometry;
using System.Collections.Generic;
using UnityEngine;

namespace LBF.Unity
{
    public class MeshBuilderParameters
    {
        public bool DrawLines;
        public bool DrawSurfaces;

        public Color LineColor;
        public Color SurfaceColor;

        public MeshBuilderParameters()
        {
            Reset();
        }

        public void Reset()
        {
            DrawLines = true;
            DrawSurfaces = true;

            LineColor = Color.red;
            SurfaceColor = new Color(0.9f, 0.45f, 0, 0.5f);
        }
    }

    public class MeshBuilder
    {
        public bool Enabled { get; set; }
        public MeshBuilderParameters Parameters { get; set; }

        IMeshBuilderImplementation m_meshImpl;
        bool m_forceWireframe;

        List<int> m_surfaceIndexTempBuffer;
        List<int> m_lineIndexTempBuffer;

        public MeshBuilder(IMeshBuilderImplementation meshImpl)
        {
            m_meshImpl = meshImpl;
            m_forceWireframe = false;

            Parameters = new MeshBuilderParameters();
            Enabled = true;

            m_surfaceIndexTempBuffer = new List<int>();
            m_lineIndexTempBuffer = new List<int>();
        }

        public int VertexLine(Vector3 v)
        {
            if (Enabled == false) return 0;
            if (Parameters.DrawLines == false) return 0;
            return m_meshImpl.Vertex(v, Parameters.LineColor);
        }

        public int VertexSurface(Vector3 v)
        {
            if (Enabled == false) return 0;
            if (Parameters.DrawSurfaces == false) return 0;
            return m_meshImpl.Vertex(v, Parameters.SurfaceColor);
        }

        public void IndexLine(int i)
        {
            if (Enabled == false) return;
            if (Parameters.DrawLines == false) return;
            m_meshImpl.Line(i);
        }

        public void IndexLine(int i1, int i2)
        {
            if (Enabled == false) return;
            if (Parameters.DrawLines == false) return;
            m_meshImpl.Line(i1, i2);
        }

        public void IndexSurface(int i)
        {
            if (Enabled == false) return;
            if (Parameters.DrawSurfaces == false) return;
            if (m_forceWireframe) return;
            m_meshImpl.Triangle(i);
        }

        public void IndexSurface(int i1, int i2, int i3)
        {
            if (Enabled == false) return;
            if (Parameters.DrawSurfaces == false) return;
            if (m_forceWireframe) return;
            m_meshImpl.Triangle(i1, i2, i3);
        }

        public void Line(Vector2 v1, Vector2 v2)
        {
            if (Enabled == false) return;
            int i1 = VertexLine(v1.FromHorizontal());
            int i2 = VertexLine(v2.FromHorizontal());
            IndexLine(i1, i2);
        }

        public void Line(Vector3 v1, Vector3 v2)
        {
            if (Enabled == false) return;
            int i1 = VertexLine(v1);
            int i2 = VertexLine(v2);
            IndexLine(i1, i2);
        }

        public void Line(IEnumerable<Vector3> vectors)
        {
            if (Enabled == false) return;
            bool first = true;     
            foreach(var v in vectors)
            {
                int index = VertexLine(v);
                if (!first) IndexLine(index - 1, index);
                first = false;
            }
        }

        public void Tube(Vector3 v1, Vector3 v2, float radius)
        {
            if (Enabled == false) return;

            Vector3 dir = v2 - v1;
            dir.Normalize();            

            var localX = dir.Orthogonal().normalized;
            var localY = Vector3.Cross(dir, localX);

            var nPoint = 12;
            Vector3[] circlePoints = new Vector3[nPoint];
            for (int i = 0; i < nPoint; i++)
            {
                float angle = 2 * Mathf.PI / nPoint * i;
                circlePoints[i] = radius * localX * Mathf.Cos(angle) + radius * localY * Mathf.Sin(angle);
            }

            for (int i = 0; i < nPoint; i++)
                Line(v1 + circlePoints[i], v2 + circlePoints[i]);
        }

        public void Triangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            if (Enabled == false) return;
            int i1 = VertexSurface(v1);
            int i2 = VertexSurface(v2);
            int i3 = VertexSurface(v3);
            IndexSurface(i1, i2, i3);
        }

        public void Arrow(Vector3 from, Vector3 to, float headSize)
        {
            if (Enabled == false) return;
            if (from == to) return;

            Line(from, to);

            var dir = to - from;
            dir.Normalize();
            var ortho = Vector3.zero;
            if (dir != Vector3.up)
                ortho = Vector3.Cross(dir, Vector3.up);
            else
                ortho = Vector3.Cross(dir, Vector3.right);

            ortho.Normalize();
            var ortho2 = Vector3.Cross(dir, ortho);

            //Draw circle
            var nSide = 3;
            var points = new Vector3[nSide];
            var circleCenter = to - dir * headSize;
            for (int i = 0; i < nSide; i++)
            {
                var angle = i * 2 * Mathf.PI / nSide;
                points[i] = circleCenter + headSize * Mathf.Cos(angle) * ortho + headSize * Mathf.Sin(angle) * ortho2;
            }

            for (int i = 0; i < nSide; i++)
            {
                Line(points[i], points[(i + 1) % nSide]);
                Line(points[i], to);
            }
        }

        public void RectangleXZ(Vector3 center, float width, float length)
        {
            if (Enabled == false) return;
            if (Parameters.DrawLines)
            {
                var v1 = VertexLine(center + new Vector3(-width * 0.5f, 0, -length * 0.5f));
                var v2 = VertexLine(center + new Vector3(-width * 0.5f, 0, length * 0.5f));
                var v3 = VertexLine(center + new Vector3(width * 0.5f, 0, -length * 0.5f));
                var v4 = VertexLine(center + new Vector3(width * 0.5f, 0, length * 0.5f));

                IndexLine(v1, v2);
                IndexLine(v3, v4);
                IndexLine(v1, v3);
                IndexLine(v2, v4);
            }

            if (Parameters.DrawSurfaces)
            {
                var v1 = VertexSurface(center + new Vector3(-width * 0.5f, 0, -length * 0.5f));
                var v2 = VertexSurface(center + new Vector3(-width * 0.5f, 0, length * 0.5f));
                var v3 = VertexSurface(center + new Vector3(width * 0.5f, 0, -length * 0.5f));
                var v4 = VertexSurface(center + new Vector3(width * 0.5f, 0, length * 0.5f));

                IndexSurface(v1, v2, v3);
                IndexSurface(v3, v2, v4);
            }
        }

        public void Circle(Vector3 center, Vector3 normal, float radius)
        {
            if (Enabled == false) return;
            var localX = normal.Orthogonal().normalized;
            var localY = Vector3.Cross(normal, localX);

            int centerVertex = VertexSurface(center);

            m_lineIndexTempBuffer.Clear();
            m_surfaceIndexTempBuffer.Clear();

            var nPoint = 16;
            for (int i = 0; i < nPoint; i++)
            {
                float angle = 2 * Mathf.PI / nPoint * i;
                var pos = center + radius * localX * Mathf.Cos(angle) + radius * localY * Mathf.Sin(angle);
                m_lineIndexTempBuffer.Add(VertexLine(pos));
                m_surfaceIndexTempBuffer.Add(VertexSurface(pos));
            }

            for (int i = 1; i < nPoint + 1; i++)
            {
                IndexLine(m_lineIndexTempBuffer[i - 1], m_lineIndexTempBuffer[i % nPoint]);
                IndexSurface(centerVertex, m_surfaceIndexTempBuffer[i - 1], m_surfaceIndexTempBuffer[i % nPoint]);
            }
        }

        public void WireSphere(Vector3 center, float radius)
        {
            if (Enabled == false) return;
            m_forceWireframe = true;

            Circle(center, Vector3.up, radius);
            Circle(center, Vector3.left, radius);
            Circle(center, Vector3.forward, radius);

            m_forceWireframe = false;
        }

        public void Cylinder(Vector3 center, float height, float radius)
        {
            if (Enabled == false) return;
            var localX = Vector3.right;
            var localY = Vector3.forward;

            int centerTopVertex = VertexSurface(center + height * 0.5f * Vector3.up);
            int centerBottomVertex = VertexSurface(center - height * 0.5f * Vector3.up);

            var nPoint = 12;
            int[] lineTopVertices = new int[nPoint];
            int[] lineBottomVertices = new int[nPoint];
            int[] triUpVertices = new int[nPoint];
            int[] triBottomVertices = new int[nPoint];
            for (int i = 0; i < nPoint; i++)
            {
                float angle = 2 * Mathf.PI / nPoint * i;
                var pos = center + radius * localX * Mathf.Cos(angle) + radius * localY * Mathf.Sin(angle);

                lineTopVertices[i] = VertexLine(pos + height * 0.5f * Vector3.up);
                lineBottomVertices[i] = VertexLine(pos - height * 0.5f * Vector3.up);
                triUpVertices[i] = VertexSurface(pos + height * 0.5f * Vector3.up);
                triBottomVertices[i] = VertexSurface(pos - height * 0.5f * Vector3.up);
            }

            for (int i = 1; i < nPoint + 1; i++)
            {
                IndexLine(lineTopVertices[i - 1], lineTopVertices[i % nPoint]);
                IndexLine(lineBottomVertices[i - 1], lineBottomVertices[i % nPoint]);
                IndexLine(lineBottomVertices[i - 1], lineTopVertices[i - 1]);

                IndexSurface(centerTopVertex, triUpVertices[i % nPoint], triUpVertices[i - 1]);
                IndexSurface(centerBottomVertex, triBottomVertices[i - 1], triBottomVertices[i % nPoint]);
                IndexSurface(triBottomVertices[i - 1], triUpVertices[i - 1], triBottomVertices[i % nPoint]);
                IndexSurface(triBottomVertices[i % nPoint], triUpVertices[i - 1], triUpVertices[i % nPoint]);
            }
        }

        public void Contour(List<Vector3> contour)
        {
            if (Enabled == false) return;
            for (int i = 0; i < contour.Count; i++)
            {
                var p1 = contour[i];
                var p2 = contour[(i + 1) % contour.Count];
                Line(p1, p2);
            }
        }

        public void Path(List<Vector3> path)
        {
            if (Enabled == false) return;
            for (int i = 1; i < path.Count; i++)
            {
                var p1 = path[i - 1];
                var p2 = path[i];
                Line(p1, p2);
            }
        }

        public void SegmentXZ(Vector3 from, Vector3 to, float height)
        {
            if (Enabled == false) return;
            Line(from, to);

            if (height != 0)
            {
                var p1Offset = from + Vector3.up * height;
                var p2Offset = to + Vector3.up * height;
                Line(p1Offset, p2Offset);

                Triangle(from, to, p1Offset);
                Triangle(p1Offset, p2Offset, to);
            }
        }

        public void Contour3D(List<Vector3> contour, float height)
        {
            if (Enabled == false) return;
            for (int i = 0; i < contour.Count; i++)
            {
                var p1 = contour[i];
                var p2 = contour[(i + 1) % contour.Count];
                SegmentXZ(p1, p2, height);
            }
        }
    }
}