using System;
using System.Collections.Generic;
using Unity.Collections;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x0200042A RID: 1066
	internal class DetachedAllocator : IDisposable
	{
		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x060021EC RID: 8684 RVA: 0x0007FF5F File Offset: 0x0007E15F
		public List<MeshWriteData> meshes
		{
			get
			{
				return this.m_MeshWriteDataPool.GetRange(0, this.m_MeshWriteDataCount);
			}
		}

		// Token: 0x060021ED RID: 8685 RVA: 0x0007FF74 File Offset: 0x0007E174
		public DetachedAllocator()
		{
			this.m_MeshWriteDataPool = new List<MeshWriteData>(16);
			this.m_MeshWriteDataCount = 0;
			this.m_VertsPool = new TempAllocator<Vertex>(8192, 2048, 65536);
			this.m_IndexPool = new TempAllocator<ushort>(16384, 4096, 131072);
		}

		// Token: 0x060021EE RID: 8686 RVA: 0x0007FFD1 File Offset: 0x0007E1D1
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x0007FFE4 File Offset: 0x0007E1E4
		protected void Dispose(bool disposing)
		{
			bool disposed = this.m_Disposed;
			if (!disposed)
			{
				if (disposing)
				{
					this.m_VertsPool.Dispose();
					this.m_IndexPool.Dispose();
				}
				this.m_Disposed = true;
			}
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x00080028 File Offset: 0x0007E228
		public MeshWriteData Alloc(int vertexCount, int indexCount)
		{
			bool flag = this.m_MeshWriteDataCount < this.m_MeshWriteDataPool.Count;
			MeshWriteData meshWriteData;
			if (flag)
			{
				meshWriteData = this.m_MeshWriteDataPool[this.m_MeshWriteDataCount];
			}
			else
			{
				meshWriteData = new MeshWriteData();
				this.m_MeshWriteDataPool.Add(meshWriteData);
			}
			this.m_MeshWriteDataCount++;
			bool flag2 = vertexCount == 0 || indexCount == 0;
			MeshWriteData meshWriteData2;
			if (flag2)
			{
				meshWriteData.Reset(default(NativeSlice<Vertex>), default(NativeSlice<ushort>));
				meshWriteData2 = meshWriteData;
			}
			else
			{
				meshWriteData.Reset(this.m_VertsPool.Alloc(vertexCount), this.m_IndexPool.Alloc(indexCount));
				meshWriteData2 = meshWriteData;
			}
			return meshWriteData2;
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x000800DA File Offset: 0x0007E2DA
		public void Clear()
		{
			this.m_VertsPool.Reset();
			this.m_IndexPool.Reset();
			this.m_MeshWriteDataCount = 0;
		}

		// Token: 0x04000E78 RID: 3704
		private TempAllocator<Vertex> m_VertsPool;

		// Token: 0x04000E79 RID: 3705
		private TempAllocator<ushort> m_IndexPool;

		// Token: 0x04000E7A RID: 3706
		private List<MeshWriteData> m_MeshWriteDataPool;

		// Token: 0x04000E7B RID: 3707
		private int m_MeshWriteDataCount;

		// Token: 0x04000E7C RID: 3708
		private bool m_Disposed;
	}
}
