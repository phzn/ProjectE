using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Pathfinding.Util
{
	// Token: 0x020002A7 RID: 679
	[BurstCompile]
	internal static class MeshUtility
	{
		// Token: 0x06001106 RID: 4358 RVA: 0x00066664 File Offset: 0x00064864
		public static void GetMeshData(Mesh.MeshDataArray meshData, int meshIndex, out NativeArray<Vector3> vertices, out NativeArray<int> indices)
		{
			Mesh.MeshData meshData2 = meshData[meshIndex];
			vertices = new NativeArray<Vector3>(meshData2.vertexCount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			meshData2.GetVertices(vertices);
			int num = 0;
			for (int i = 0; i < meshData2.subMeshCount; i++)
			{
				num += meshData2.GetSubMesh(i).indexCount;
			}
			indices = new NativeArray<int>(num, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			int num2 = 0;
			for (int j = 0; j < meshData2.subMeshCount; j++)
			{
				SubMeshDescriptor subMesh = meshData2.GetSubMesh(j);
				meshData2.GetIndices(indices.GetSubArray(num2, subMesh.indexCount), j, true);
				num2 += subMesh.indexCount;
			}
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x00066716 File Offset: 0x00064916
		[BurstCompile]
		public static void MakeTrianglesClockwise(ref UnsafeSpan<Int3> vertices, ref UnsafeSpan<int> triangles)
		{
			MeshUtility.MakeTrianglesClockwise_00000FC7$BurstDirectCall.Invoke(ref vertices, ref triangles);
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x00066720 File Offset: 0x00064920
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void MakeTrianglesClockwise$BurstManaged(ref UnsafeSpan<Int3> vertices, ref UnsafeSpan<int> triangles)
		{
			for (int i = 0; i < triangles.Length; i += 3)
			{
				if (!VectorMath.IsClockwiseXZ(*vertices[*triangles[i]], *vertices[*triangles[i + 1]], *vertices[*triangles[i + 2]]))
				{
					int num = *triangles[i];
					*triangles[i] = *triangles[i + 2];
					*triangles[i + 2] = num;
				}
			}
		}

		// Token: 0x020002A8 RID: 680
		[BurstCompile]
		public struct JobMergeNearbyVertices : IJob
		{
			// Token: 0x06001109 RID: 4361 RVA: 0x000667A8 File Offset: 0x000649A8
			public unsafe void Execute()
			{
				NativeArray<int> nativeArray = new NativeArray<int>(this.vertices.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				for (int i = 0; i < nativeArray.Length; i++)
				{
					nativeArray[i] = i;
				}
				nativeArray.Sort(new MeshUtility.JobMergeNearbyVertices.CoordinateSorter
				{
					vertices = this.vertices.AsUnsafeSpan<Int3>().Reinterpret<int3>()
				});
				UnsafeSpan<int> unsafeSpan = nativeArray.AsUnsafeSpan<int>();
				NativeArray<int> nativeArray2 = new NativeArray<int>(this.vertices.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				UnsafeSpan<int3> unsafeSpan2 = this.vertices.AsUnsafeSpan<Int3>().Reinterpret<int3>();
				UnsafeSpan<int> unsafeSpan3 = this.triangles.AsUnsafeSpan<int>();
				UnsafeSpan<int3> unsafeSpan4 = new NativeArray<int3>(this.vertices.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory).AsUnsafeSpan<int3>();
				int num = 0;
				int num2 = (int)math.ceil(math.sqrt((float)this.mergeRadiusSq));
				uint num3 = 1U;
				for (uint num4 = 0U; num4 < unsafeSpan.length; num4 += 1U)
				{
					if (*unsafeSpan[num4] != -1)
					{
						int3 @int = *unsafeSpan2[*unsafeSpan[num4]];
						nativeArray2[*unsafeSpan[num4]] = num;
						while (num3 < unsafeSpan.length && unsafeSpan2[*unsafeSpan[num3]].x <= @int.x + num2)
						{
							num3 += 1U;
						}
						int3 int2 = @int;
						int num5 = 1;
						for (uint num6 = num4 + 1U; num6 < num3; num6 += 1U)
						{
							if (*unsafeSpan[num6] != -1)
							{
								int3 int3 = *unsafeSpan2[*unsafeSpan[num6]];
								if (math.lengthsq(int3 - @int) <= (float)this.mergeRadiusSq)
								{
									int2 += int3;
									num5++;
									nativeArray2[*unsafeSpan[num6]] = num;
									*unsafeSpan[num6] = -1;
								}
							}
						}
						*unsafeSpan4[num] = int2 / num5;
						num++;
					}
				}
				this.vertices.Length = num;
				unsafeSpan4.Slice(0, num).CopyTo(this.vertices.AsUnsafeSpan<Int3>().Reinterpret<int3>());
				for (uint num7 = 0U; num7 < unsafeSpan3.length; num7 += 1U)
				{
					*unsafeSpan3[num7] = nativeArray2[*unsafeSpan3[num7]];
				}
			}

			// Token: 0x04000C3B RID: 3131
			public NativeList<Int3> vertices;

			// Token: 0x04000C3C RID: 3132
			public NativeList<int> triangles;

			// Token: 0x04000C3D RID: 3133
			public int mergeRadiusSq;

			// Token: 0x020002A9 RID: 681
			private struct CoordinateSorter : IComparer<int>
			{
				// Token: 0x0600110A RID: 4362 RVA: 0x00066A1C File Offset: 0x00064C1C
				public int Compare(int a, int b)
				{
					Hint.Assume(a < (int)this.vertices.length);
					Hint.Assume(b < (int)this.vertices.length);
					return this.vertices[a].x.CompareTo(this.vertices[b].x);
				}

				// Token: 0x04000C3E RID: 3134
				public UnsafeSpan<int3> vertices;
			}
		}

		// Token: 0x020002AA RID: 682
		[BurstCompile]
		public struct JobRemoveDegenerateTriangles : IJob
		{
			// Token: 0x0600110B RID: 4363 RVA: 0x00066A78 File Offset: 0x00064C78
			public static int3 cross(int3 lhs, int3 rhs)
			{
				return (lhs * rhs.yzx - lhs.yzx * rhs).yzx;
			}

			// Token: 0x0600110C RID: 4364 RVA: 0x00066AAC File Offset: 0x00064CAC
			public unsafe void Execute()
			{
				int num = 0;
				UnsafeSpan<int3> unsafeSpan = this.vertices.AsUnsafeSpan<Int3>().Reinterpret<int3>();
				UnsafeSpan<int3> unsafeSpan2 = this.triangles.AsUnsafeSpan<int>().Reinterpret<int3>(4);
				UnsafeSpan<int> unsafeSpan3 = this.tags.AsUnsafeSpan<int>();
				uint num2 = 0U;
				for (uint num3 = 0U; num3 < unsafeSpan2.length; num3 += 1U)
				{
					int3 @int = *unsafeSpan2[num3];
					if (math.all(MeshUtility.JobRemoveDegenerateTriangles.cross(*unsafeSpan[@int.y] - *unsafeSpan[@int.x], *unsafeSpan[@int.z] - *unsafeSpan[@int.x]) == 0))
					{
						num++;
					}
					else
					{
						*unsafeSpan2[num2] = @int;
						*unsafeSpan3[num2] = *unsafeSpan3[num3];
						num2 += 1U;
					}
				}
				this.triangles.Length = (int)(num2 * 3U);
				this.tags.Length = (int)num2;
				if (this.verbose && num > 0)
				{
					Debug.LogWarning(string.Format("Input mesh contained {0} degenerate triangles. These have been removed.\nA degenerate triangle is a triangle with zero area. It resembles a line or a point.", num));
				}
			}

			// Token: 0x04000C3F RID: 3135
			public NativeList<Int3> vertices;

			// Token: 0x04000C40 RID: 3136
			public NativeList<int> triangles;

			// Token: 0x04000C41 RID: 3137
			public NativeList<int> tags;

			// Token: 0x04000C42 RID: 3138
			public bool verbose;
		}

		// Token: 0x020002AB RID: 683
		// (Invoke) Token: 0x0600110E RID: 4366
		internal delegate void MakeTrianglesClockwise_00000FC7$PostfixBurstDelegate(ref UnsafeSpan<Int3> vertices, ref UnsafeSpan<int> triangles);

		// Token: 0x020002AC RID: 684
		internal static class MakeTrianglesClockwise_00000FC7$BurstDirectCall
		{
			// Token: 0x06001111 RID: 4369 RVA: 0x00066BF5 File Offset: 0x00064DF5
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (MeshUtility.MakeTrianglesClockwise_00000FC7$BurstDirectCall.Pointer == 0)
				{
					MeshUtility.MakeTrianglesClockwise_00000FC7$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(MeshUtility.MakeTrianglesClockwise_00000FC7$BurstDirectCall.DeferredCompilation, methodof(MeshUtility.MakeTrianglesClockwise$BurstManaged(ref UnsafeSpan<Int3>, ref UnsafeSpan<int>)).MethodHandle, typeof(MeshUtility.MakeTrianglesClockwise_00000FC7$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = MeshUtility.MakeTrianglesClockwise_00000FC7$BurstDirectCall.Pointer;
			}

			// Token: 0x06001112 RID: 4370 RVA: 0x00066C24 File Offset: 0x00064E24
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				MeshUtility.MakeTrianglesClockwise_00000FC7$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001113 RID: 4371 RVA: 0x00066C3C File Offset: 0x00064E3C
			public static void Constructor()
			{
				MeshUtility.MakeTrianglesClockwise_00000FC7$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(MeshUtility.MakeTrianglesClockwise(ref UnsafeSpan<Int3>, ref UnsafeSpan<int>)).MethodHandle);
			}

			// Token: 0x06001114 RID: 4372 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x06001115 RID: 4373 RVA: 0x00066C4D File Offset: 0x00064E4D
			// Note: this type is marked as 'beforefieldinit'.
			static MakeTrianglesClockwise_00000FC7$BurstDirectCall()
			{
				MeshUtility.MakeTrianglesClockwise_00000FC7$BurstDirectCall.Constructor();
			}

			// Token: 0x06001116 RID: 4374 RVA: 0x00066C54 File Offset: 0x00064E54
			public static void Invoke(ref UnsafeSpan<Int3> vertices, ref UnsafeSpan<int> triangles)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = MeshUtility.MakeTrianglesClockwise_00000FC7$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Collections.UnsafeSpan`1<Pathfinding.Int3>&,Pathfinding.Collections.UnsafeSpan`1<System.Int32>&), ref vertices, ref triangles, functionPointer);
						return;
					}
				}
				MeshUtility.MakeTrianglesClockwise$BurstManaged(ref vertices, ref triangles);
			}

			// Token: 0x04000C43 RID: 3139
			private static IntPtr Pointer;

			// Token: 0x04000C44 RID: 3140
			private static IntPtr DeferredCompilation;
		}
	}
}
