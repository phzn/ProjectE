using System;
using Unity.Collections;

namespace Unity.Entities
{
	// Token: 0x020001C7 RID: 455
	internal struct ArchetypeChunkArray
	{
		// Token: 0x06000AC8 RID: 2760 RVA: 0x00033D74 File Offset: 0x00031F74
		internal static int TotalEntityCountInChunksIgnoreFiltering(NativeArray<ArchetypeChunk> chunks)
		{
			int num = 0;
			for (int i = 0; i < chunks.Length; i++)
			{
				num += chunks[i].Count;
			}
			return num;
		}
	}
}
