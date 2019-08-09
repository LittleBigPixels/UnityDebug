using System;
using UnityEngine;

namespace LBF.Unity
{
    public class DebugMeshDrawer
    {
        public bool DrawDebug = true;

        [NonSerialized]
        public BufferMeshImplementation DebugBuffer = null;
        [NonSerialized]
        public Material Material;

        Mesh m_mesh;

        public DebugMeshDrawer()
        {
            m_mesh = new Mesh();
            m_mesh.MarkDynamic();
        }

        public void Draw()
        {
            if (DebugBuffer == null) return;
            if (DrawDebug == false) return;
            if (m_mesh == null) return;

            m_mesh.Clear();
            m_mesh.vertices = DebugBuffer.Vertices.ToArray();
            m_mesh.colors = DebugBuffer.Colors.ToArray();
            m_mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            m_mesh.subMeshCount = 2;
            m_mesh.SetIndices(DebugBuffer.LineIndices.ToArray(), MeshTopology.Lines, 0);
            m_mesh.SetIndices(DebugBuffer.TriangleIndices.ToArray(), MeshTopology.Triangles, 1);
            m_mesh.UploadMeshData(false);

            Graphics.DrawMesh(m_mesh, Matrix4x4.identity, Material, 31, null, 0);
            Graphics.DrawMesh(m_mesh, Matrix4x4.identity, Material, 31, null, 1);

            DebugBuffer.Reset();
        }

        public void Clear()
        {
            if (m_mesh != null)
                m_mesh.Clear();
        }
    }
}