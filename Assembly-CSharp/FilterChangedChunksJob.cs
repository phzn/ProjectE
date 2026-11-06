using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

// Token: 0x0200007F RID: 127
[BurstCompile(CompileSynchronously = true)]
public struct FilterChangedChunksJob : IJob
{
	// Token: 0x060005D1 RID: 1489 RVA: 0x0002AA58 File Offset: 0x00028C58
	public void Execute()
	{
		this.chunksToActivate.Clear();
		this.chunksToDeactivate.Clear();
		for (int i = 0; i < this.chunks.Length; i++)
		{
			ChunkJobData chunkJobData = this.chunks[i];
			ChunkVisibilityResult chunkVisibilityResult = this.visibilityResults[i];
			if (chunkVisibilityResult.shouldBeActive && !chunkJobData.isCurrentlyActive)
			{
				this.chunksToActivate.Add(in i);
			}
			else if (!chunkVisibilityResult.shouldBeActive && chunkJobData.isCurrentlyActive)
			{
				this.chunksToDeactivate.Add(in i);
			}
		}
	}

	// Token: 0x04000478 RID: 1144
	[ReadOnly]
	public NativeArray<ChunkJobData> chunks;

	// Token: 0x04000479 RID: 1145
	[ReadOnly]
	public NativeArray<ChunkVisibilityResult> visibilityResults;

	// Token: 0x0400047A RID: 1146
	public NativeList<int> chunksToActivate;

	// Token: 0x0400047B RID: 1147
	public NativeList<int> chunksToDeactivate;
}
