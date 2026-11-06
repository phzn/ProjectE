using System;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.UIElements
{
	// Token: 0x0200027A RID: 634
	internal struct MeshWriteDataInterface
	{
		// Token: 0x060011F0 RID: 4592 RVA: 0x000410CC File Offset: 0x0003F2CC
		public static MeshWriteDataInterface FromMeshWriteData(MeshWriteData data)
		{
			return new MeshWriteDataInterface
			{
				vertices = new IntPtr(data.m_Vertices.GetUnsafePtr<Vertex>()),
				indices = new IntPtr(data.m_Indices.GetUnsafePtr<ushort>()),
				vertexCount = data.m_Vertices.Length,
				indexCount = data.m_Indices.Length
			};
		}

		// Token: 0x040007F1 RID: 2033
		public IntPtr vertices;

		// Token: 0x040007F2 RID: 2034
		public IntPtr indices;

		// Token: 0x040007F3 RID: 2035
		public int vertexCount;

		// Token: 0x040007F4 RID: 2036
		public int indexCount;
	}
}
