using System;
using Pathfinding.Collections;
using Pathfinding.Sync;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Jobs
{
	// Token: 0x020001F5 RID: 501
	[BurstCompile(FloatMode = FloatMode.Default)]
	public struct JobBuildTileMeshFromVertices : IJob
	{
		// Token: 0x06000D9F RID: 3487 RVA: 0x0005220C File Offset: 0x0005040C
		public static Promise<TileBuilder.TileBuilderOutput> Schedule(NativeArray<Vector3> vertices, NativeArray<int> indices, Matrix4x4 meshToGraph, bool recalculateNormals)
		{
			if (vertices.Length > 4095)
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Too many vertices in the navmesh graph. Provided ",
					vertices.Length.ToString(),
					", but the maximum number of vertices per tile is ",
					4095.ToString(),
					". You can raise this limit by enabling ASTAR_RECAST_LARGER_TILES in the A* Inspector Optimizations tab"
				}));
			}
			NativeArray<TileMesh.TileMeshUnsafe> nativeArray = new NativeArray<TileMesh.TileMeshUnsafe>(1, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			return new Promise<TileBuilder.TileBuilderOutput>(new JobBuildTileMeshFromVertices
			{
				vertices = vertices,
				indices = indices,
				meshToGraph = meshToGraph,
				outputBuffers = nativeArray,
				recalculateNormals = recalculateNormals
			}.Schedule(default(JobHandle)), new TileBuilder.TileBuilderOutput
			{
				tileMeshes = new TileMeshesUnsafe(nativeArray, new IntRect(0, 0, 0, 0), new Vector2(100000f, 100000f))
			});
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x000522EC File Offset: 0x000504EC
		public unsafe void Execute()
		{
			NativeList<Int3> nativeList = new NativeList<Int3>(this.vertices.Length, Allocator.Temp);
			nativeList.Length = this.vertices.Length;
			NativeList<int> nativeList2 = new NativeList<int>(this.indices.Length / 3, Allocator.Temp);
			nativeList2.Length = this.indices.Length / 3;
			NativeList<int> nativeList3 = new NativeList<int>(this.indices.Length, Allocator.Temp);
			nativeList3.AddRange(this.indices);
			JobBuildTileMeshFromVertices.JobTransformTileCoordinates jobTransformTileCoordinates = default(JobBuildTileMeshFromVertices.JobTransformTileCoordinates);
			jobTransformTileCoordinates.vertices = this.vertices;
			jobTransformTileCoordinates.outputVertices = nativeList.AsArray();
			jobTransformTileCoordinates.matrix = this.meshToGraph;
			jobTransformTileCoordinates.Execute();
			TileMesh.TileMeshUnsafe* unsafePtr = (TileMesh.TileMeshUnsafe*)this.outputBuffers.GetUnsafePtr<TileMesh.TileMeshUnsafe>();
			MeshUtility.JobMergeNearbyVertices jobMergeNearbyVertices = default(MeshUtility.JobMergeNearbyVertices);
			jobMergeNearbyVertices.vertices = nativeList;
			jobMergeNearbyVertices.triangles = nativeList3;
			jobMergeNearbyVertices.mergeRadiusSq = 0;
			jobMergeNearbyVertices.Execute();
			MeshUtility.JobRemoveDegenerateTriangles jobRemoveDegenerateTriangles = default(MeshUtility.JobRemoveDegenerateTriangles);
			jobRemoveDegenerateTriangles.vertices = nativeList;
			jobRemoveDegenerateTriangles.triangles = nativeList3;
			jobRemoveDegenerateTriangles.tags = nativeList2;
			jobRemoveDegenerateTriangles.Execute();
			unsafePtr->verticesInTileSpace = nativeList.AsUnsafeSpan<Int3>().Clone(Allocator.Persistent);
			unsafePtr->triangles = nativeList3.AsUnsafeSpan<int>().Clone(Allocator.Persistent);
			unsafePtr->tags = nativeList2.AsUnsafeSpan<int>().Reinterpret<uint>().Clone(Allocator.Persistent);
			if (this.recalculateNormals)
			{
				MeshUtility.MakeTrianglesClockwise(ref unsafePtr->verticesInTileSpace, ref unsafePtr->triangles);
			}
			nativeList.Dispose();
		}

		// Token: 0x0400096A RID: 2410
		public NativeArray<Vector3> vertices;

		// Token: 0x0400096B RID: 2411
		public NativeArray<int> indices;

		// Token: 0x0400096C RID: 2412
		public Matrix4x4 meshToGraph;

		// Token: 0x0400096D RID: 2413
		public NativeArray<TileMesh.TileMeshUnsafe> outputBuffers;

		// Token: 0x0400096E RID: 2414
		public bool recalculateNormals;

		// Token: 0x020001F6 RID: 502
		[BurstCompile(FloatMode = FloatMode.Fast)]
		public struct JobTransformTileCoordinates : IJob
		{
			// Token: 0x06000DA1 RID: 3489 RVA: 0x00052478 File Offset: 0x00050678
			public void Execute()
			{
				if (this.vertices.Length != this.outputVertices.Length)
				{
					throw new ArgumentException("Input and output arrays must have the same length");
				}
				for (int i = 0; i < this.vertices.Length; i++)
				{
					this.outputVertices[i] = (Int3)this.matrix.MultiplyPoint3x4(this.vertices[i]);
				}
			}

			// Token: 0x0400096F RID: 2415
			public NativeArray<Vector3> vertices;

			// Token: 0x04000970 RID: 2416
			public NativeArray<Int3> outputVertices;

			// Token: 0x04000971 RID: 2417
			public Matrix4x4 matrix;
		}
	}
}
