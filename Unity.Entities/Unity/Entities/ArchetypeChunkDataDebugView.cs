using System;

namespace Unity.Entities
{
	// Token: 0x02000046 RID: 70
	internal sealed class ArchetypeChunkDataDebugView
	{
		// Token: 0x06000206 RID: 518 RVA: 0x0000A55E File Offset: 0x0000875E
		public ArchetypeChunkDataDebugView(ArchetypeChunkData chunkData)
		{
			this.m_ChunkData = chunkData;
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000207 RID: 519 RVA: 0x0000A570 File Offset: 0x00008770
		public ArchetypeChunk[] Items
		{
			get
			{
				ArchetypeChunk[] array = new ArchetypeChunk[this.m_ChunkData.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new ArchetypeChunk(this.m_ChunkData[i], null);
				}
				return array;
			}
		}

		// Token: 0x04000087 RID: 135
		private ArchetypeChunkData m_ChunkData;
	}
}
