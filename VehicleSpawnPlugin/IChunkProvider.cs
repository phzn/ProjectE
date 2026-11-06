using System;
using System.Collections.Generic;

namespace VehicleSpawnPlugin
{
	// Token: 0x0200000F RID: 15
	public interface IChunkProvider
	{
		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060000A3 RID: 163
		// (remove) Token: 0x060000A4 RID: 164
		event Action<ChunkInfo> OnChunkLoaded;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060000A5 RID: 165
		// (remove) Token: 0x060000A6 RID: 166
		event Action<ChunkInfo> OnChunkUnloaded;

		// Token: 0x060000A7 RID: 167
		IEnumerable<ChunkInfo> GetActiveChunks();
	}
}
