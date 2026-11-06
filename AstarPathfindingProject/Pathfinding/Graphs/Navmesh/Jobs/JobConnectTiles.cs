using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Jobs
{
	// Token: 0x020001FB RID: 507
	public struct JobConnectTiles : IJob
	{
		// Token: 0x06000DA9 RID: 3497 RVA: 0x00052EEC File Offset: 0x000510EC
		public static JobHandle ScheduleBatch(GCHandle tilesHandle, JobHandle dependency, IntRect tileRect, Vector2 tileWorldSize, float maxTileConnectionEdgeDistance)
		{
			int num = Mathf.Max(1, JobsUtility.JobWorkerCount);
			NativeArray<JobHandle> nativeArray = new NativeArray<JobHandle>(num, Allocator.Temp, NativeArrayOptions.ClearMemory);
			for (int i = 0; i <= 1; i++)
			{
				for (int j = 0; j <= 1; j++)
				{
					for (int k = 0; k < num; k++)
					{
						nativeArray[k] = new JobConnectTiles
						{
							tiles = tilesHandle,
							tileRect = tileRect,
							tileWorldSize = tileWorldSize,
							coordinateSum = i,
							direction = j,
							maxTileConnectionEdgeDistance = maxTileConnectionEdgeDistance,
							zOffset = k,
							zStride = num
						}.Schedule(dependency);
					}
					dependency = JobHandle.CombineDependencies(nativeArray);
				}
			}
			return dependency;
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x00052FA4 File Offset: 0x000511A4
		public static JobHandle ScheduleRecalculateBorders(GCHandle tilesHandle, JobHandle dependency, IntRect tileRect, IntRect innerRect, Vector2 tileWorldSize, float maxTileConnectionEdgeDistance)
		{
			int width = innerRect.Width;
			int height = innerRect.Height;
			NativeArray<JobHandle> nativeArray = new NativeArray<JobHandle>(2 * width + 2 * math.max(0, height - 2), Allocator.Temp, NativeArrayOptions.ClearMemory);
			int num = 0;
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					if (j == 0 || i == 0 || j == width - 1 || i == height - 1)
					{
						int num2 = innerRect.xmin + j;
						int num3 = innerRect.ymin + i;
						JobHandle jobHandle = dependency;
						for (int k = 0; k < 4; k++)
						{
							int num4 = num2 + ((k == 0) ? 1 : ((k == 1) ? (-1) : 0));
							int num5 = num3 + ((k == 2) ? 1 : ((k == 3) ? (-1) : 0));
							if (!innerRect.Contains(num4, num5) && tileRect.Contains(num4, num5))
							{
								jobHandle = new JobConnectTilesSingle
								{
									tiles = tilesHandle,
									tileIndex1 = num2 + num3 * tileRect.Width,
									tileIndex2 = num4 + num5 * tileRect.Width,
									tileWorldSize = tileWorldSize,
									maxTileConnectionEdgeDistance = maxTileConnectionEdgeDistance
								}.Schedule(jobHandle);
							}
						}
						nativeArray[num++] = jobHandle;
					}
				}
			}
			return JobHandle.CombineDependencies(nativeArray);
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x00053100 File Offset: 0x00051300
		public void Execute()
		{
			NavmeshTile[] array = (NavmeshTile[])this.tiles.Target;
			int height = this.tileRect.Height;
			int width = this.tileRect.Width;
			for (int i = this.zOffset; i < height; i += this.zStride)
			{
				for (int j = 0; j < width; j++)
				{
					if ((j + i) % 2 == this.coordinateSum)
					{
						int num = j + i * width;
						int num2;
						if (this.direction == 0 && j < width - 1)
						{
							num2 = j + 1 + i * width;
						}
						else
						{
							if (this.direction != 1 || i >= height - 1)
							{
								goto IL_00AD;
							}
							num2 = j + (i + 1) * width;
						}
						NavmeshBase.ConnectTiles(array[num], array[num2], this.tileWorldSize.x, this.tileWorldSize.y, this.maxTileConnectionEdgeDistance);
					}
					IL_00AD:;
				}
			}
		}

		// Token: 0x040009A2 RID: 2466
		public GCHandle tiles;

		// Token: 0x040009A3 RID: 2467
		public int coordinateSum;

		// Token: 0x040009A4 RID: 2468
		public int direction;

		// Token: 0x040009A5 RID: 2469
		public int zOffset;

		// Token: 0x040009A6 RID: 2470
		public int zStride;

		// Token: 0x040009A7 RID: 2471
		private Vector2 tileWorldSize;

		// Token: 0x040009A8 RID: 2472
		private IntRect tileRect;

		// Token: 0x040009A9 RID: 2473
		public float maxTileConnectionEdgeDistance;

		// Token: 0x040009AA RID: 2474
		private static readonly ProfilerMarker ConnectTilesMarker = new ProfilerMarker("ConnectTiles");
	}
}
