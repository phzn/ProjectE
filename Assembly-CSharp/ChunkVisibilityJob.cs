using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

// Token: 0x0200007C RID: 124
[BurstCompile(CompileSynchronously = true)]
public struct ChunkVisibilityJob : IJobParallelFor
{
	// Token: 0x060005CC RID: 1484 RVA: 0x0002A5A8 File Offset: 0x000287A8
	public void Execute(int index)
	{
		ChunkJobData chunkJobData = this.chunks[index];
		float2 cellPosition = chunkJobData.cellPosition;
		float2 @float = chunkJobData.cellPosition + math.max(chunkJobData.cellSize, new float2(1f, 1f)) - 1f;
		float2 float2 = math.floor(this.playerCell);
		float num = math.max(math.max(cellPosition.x - float2.x, float2.x - @float.x), 0f);
		float num2 = math.max(math.max(cellPosition.y - float2.y, float2.y - @float.y), 0f);
		float num3 = math.max(num, num2);
		bool flag = false;
		if (num3 <= this.minVisibleRadius)
		{
			flag = true;
		}
		else if (num3 <= this.maxVisibleRadius)
		{
			float2 cellPosition2 = chunkJobData.cellPosition;
			float2 float3 = chunkJobData.cellPosition + chunkJobData.cellSize;
			float2 float4 = this.playerCell + 0.5f;
			flag = this.IsChunkInFOV(cellPosition2, float3, float4, this.cameraDirection, this.halfViewAngleCos);
		}
		this.results[index] = new ChunkVisibilityResult
		{
			id = chunkJobData.id,
			shouldBeActive = flag,
			distanceSqr = num3 * num3
		};
	}

	// Token: 0x060005CD RID: 1485 RVA: 0x0002A700 File Offset: 0x00028900
	private bool IsChunkInFOV(float2 chunkMin, float2 chunkMax, float2 playerPos, float2 camDir, float halfAngleCos)
	{
		float2 @float = new float2(chunkMin.x, chunkMin.y);
		float2 float2 = new float2(chunkMax.x, chunkMin.y);
		float2 float3 = new float2(chunkMin.x, chunkMax.y);
		float2 float4 = new float2(chunkMax.x, chunkMax.y);
		if (this.IsPointInFOV(@float, playerPos, camDir, halfAngleCos))
		{
			return true;
		}
		if (this.IsPointInFOV(float2, playerPos, camDir, halfAngleCos))
		{
			return true;
		}
		if (this.IsPointInFOV(float3, playerPos, camDir, halfAngleCos))
		{
			return true;
		}
		if (this.IsPointInFOV(float4, playerPos, camDir, halfAngleCos))
		{
			return true;
		}
		float2 float5 = (chunkMin + chunkMax) * 0.5f;
		if (this.IsPointInFOV(float5, playerPos, camDir, halfAngleCos))
		{
			return true;
		}
		float2 float6 = new float2((chunkMin.x + chunkMax.x) * 0.5f, chunkMax.y);
		float2 float7 = new float2((chunkMin.x + chunkMax.x) * 0.5f, chunkMin.y);
		float2 float8 = new float2(chunkMin.x, (chunkMin.y + chunkMax.y) * 0.5f);
		float2 float9 = new float2(chunkMax.x, (chunkMin.y + chunkMax.y) * 0.5f);
		return this.IsPointInFOV(float6, playerPos, camDir, halfAngleCos) || this.IsPointInFOV(float7, playerPos, camDir, halfAngleCos) || this.IsPointInFOV(float8, playerPos, camDir, halfAngleCos) || this.IsPointInFOV(float9, playerPos, camDir, halfAngleCos);
	}

	// Token: 0x060005CE RID: 1486 RVA: 0x0002A884 File Offset: 0x00028A84
	private bool IsPointInFOV(float2 point, float2 playerPos, float2 camDir, float halfAngleCos)
	{
		float2 @float = point - playerPos;
		float num = math.length(@float);
		if (num < 0.001f)
		{
			return true;
		}
		float2 float2 = @float / num;
		return math.dot(camDir, float2) >= halfAngleCos;
	}

	// Token: 0x04000469 RID: 1129
	[ReadOnly]
	public NativeArray<ChunkJobData> chunks;

	// Token: 0x0400046A RID: 1130
	[ReadOnly]
	public float2 playerCell;

	// Token: 0x0400046B RID: 1131
	[ReadOnly]
	public float minVisibleRadius;

	// Token: 0x0400046C RID: 1132
	[ReadOnly]
	public float maxVisibleRadius;

	// Token: 0x0400046D RID: 1133
	[ReadOnly]
	public float2 cameraDirection;

	// Token: 0x0400046E RID: 1134
	[ReadOnly]
	public float halfViewAngleCos;

	// Token: 0x0400046F RID: 1135
	[WriteOnly]
	public NativeArray<ChunkVisibilityResult> results;
}
