using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Jobs
{
	// Token: 0x020001F9 RID: 505
	[BurstCompile]
	public struct JobCalculateTriangleConnections : IJob
	{
		// Token: 0x06000DA8 RID: 3496 RVA: 0x00052CE4 File Offset: 0x00050EE4
		public unsafe void Execute()
		{
			NativeParallelHashMap<int2, uint> nativeParallelHashMap = new NativeParallelHashMap<int2, uint>(128, Allocator.Temp);
			bool flag = false;
			for (int i = 0; i < this.tileMeshes.Length; i++)
			{
				nativeParallelHashMap.Clear();
				TileMesh.TileMeshUnsafe tileMeshUnsafe = this.tileMeshes[i];
				int length = tileMeshUnsafe.triangles.Length;
				UnsafeAppendBuffer unsafeAppendBuffer = new UnsafeAppendBuffer(length * 2 * 4, 4, Allocator.Persistent);
				UnsafeAppendBuffer unsafeAppendBuffer2 = new UnsafeAppendBuffer(length * 4, 4, Allocator.Persistent);
				int* ptr = tileMeshUnsafe.triangles.ptr;
				int j = 0;
				int num = 0;
				while (j < length)
				{
					flag |= !nativeParallelHashMap.TryAdd(new int2(ptr[j], ptr[j + 1]), (uint)(num | 0));
					flag |= !nativeParallelHashMap.TryAdd(new int2(ptr[j + 1], ptr[j + 2]), (uint)(num | 268435456));
					flag |= !nativeParallelHashMap.TryAdd(new int2(ptr[j + 2], ptr[j]), (uint)(num | 536870912));
					j += 3;
					num++;
				}
				for (int k = 0; k < length; k += 3)
				{
					int num2 = 0;
					for (int l = 0; l < 3; l++)
					{
						uint num3;
						if (nativeParallelHashMap.TryGetValue(new int2(ptr[k + (l + 1) % 3], ptr[k + l]), out num3))
						{
							uint num4 = num3 & 268435455U;
							int num5 = (int)(num3 >> 28);
							unsafeAppendBuffer.Add<uint>(num4);
							byte b = Connection.PackShapeEdgeInfo((byte)l, (byte)num5, true, true, true);
							unsafeAppendBuffer.Add<int>((int)b);
							num2++;
						}
					}
					unsafeAppendBuffer2.Add<int>(num2);
				}
				this.nodeConnections[i] = new JobCalculateTriangleConnections.TileNodeConnectionsUnsafe
				{
					neighbours = unsafeAppendBuffer,
					neighbourCounts = unsafeAppendBuffer2
				};
			}
			if (flag)
			{
				Debug.LogWarning("Duplicate triangle edges were found in the input mesh. These have been removed. Are you sure your mesh is suitable for being used as a navmesh directly?\nThis could be caused by the mesh's normals not being consistent.");
			}
		}

		// Token: 0x0400099E RID: 2462
		[ReadOnly]
		public NativeArray<TileMesh.TileMeshUnsafe> tileMeshes;

		// Token: 0x0400099F RID: 2463
		[WriteOnly]
		public NativeArray<JobCalculateTriangleConnections.TileNodeConnectionsUnsafe> nodeConnections;

		// Token: 0x020001FA RID: 506
		public struct TileNodeConnectionsUnsafe
		{
			// Token: 0x040009A0 RID: 2464
			public UnsafeAppendBuffer neighbours;

			// Token: 0x040009A1 RID: 2465
			public UnsafeAppendBuffer neighbourCounts;
		}
	}
}
