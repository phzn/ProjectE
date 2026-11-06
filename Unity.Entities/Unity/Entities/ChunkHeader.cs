using System;

namespace Unity.Entities
{
	// Token: 0x020001C5 RID: 453
	[ChunkSerializable]
	public struct ChunkHeader : ICleanupComponentData, IComponentData, IQueryTypeParameter
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000AC2 RID: 2754 RVA: 0x00033C84 File Offset: 0x00031E84
		public static ChunkHeader Null
		{
			get
			{
				return default(ChunkHeader);
			}
		}

		// Token: 0x040004CF RID: 1231
		public ArchetypeChunk ArchetypeChunk;
	}
}
