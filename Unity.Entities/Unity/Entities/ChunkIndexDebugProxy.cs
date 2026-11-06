using System;
using System.Diagnostics;

namespace Unity.Entities
{
	// Token: 0x020002F1 RID: 753
	internal class ChunkIndexDebugProxy
	{
		// Token: 0x0600144E RID: 5198 RVA: 0x0004E2B1 File Offset: 0x0004C4B1
		private ChunkIndexDebugProxy(ChunkIndex chunkIndex)
		{
			this.m_ChunkIndex = chunkIndex;
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600144F RID: 5199 RVA: 0x0004E2C0 File Offset: 0x0004C4C0
		public unsafe Entity[] Entities
		{
			get
			{
				if (this.m_ChunkIndex == ChunkIndex.Null)
				{
					return null;
				}
				Entity* buffer = (Entity*)this.m_ChunkIndex.Buffer;
				int count = this.m_ChunkIndex.Count;
				Entity[] array = new Entity[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = buffer[i];
				}
				return array;
			}
		}

		// Token: 0x04000770 RID: 1904
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ChunkIndex m_ChunkIndex;
	}
}
