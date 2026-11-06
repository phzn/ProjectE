using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding.Drawing;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding.Collections
{
	// Token: 0x0200027B RID: 635
	[BurstCompile]
	public struct BBTree : IDisposable
	{
		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06001001 RID: 4097 RVA: 0x00060F68 File Offset: 0x0005F168
		public IntRect Size
		{
			get
			{
				if (this.tree.Length != 0)
				{
					return this.tree[0].rect;
				}
				return default(IntRect);
			}
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x00060F9D File Offset: 0x0005F19D
		public void Dispose()
		{
			this.nodePermutation.Dispose();
			this.tree.Dispose();
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x00060FB5 File Offset: 0x0005F1B5
		public BBTree(UnsafeSpan<int> triangles, UnsafeSpan<Int3> vertices)
		{
			if (triangles.Length % 3 != 0)
			{
				throw new ArgumentException("triangles must be a multiple of 3 in length");
			}
			BBTree.Build(ref triangles, ref vertices, out this);
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x00060FD7 File Offset: 0x0005F1D7
		[BurstCompile]
		private static void Build(ref UnsafeSpan<int> triangles, ref UnsafeSpan<Int3> vertices, out BBTree bbTree)
		{
			BBTree.Build_00000EE6$BurstDirectCall.Invoke(ref triangles, ref vertices, out bbTree);
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x00060FE4 File Offset: 0x0005F1E4
		private static int SplitByX(NativeArray<IntRect> nodesBounds, NativeArray<int> permutation, int from, int to, int divider)
		{
			int num = to;
			for (int i = from; i < num; i++)
			{
				IntRect intRect = nodesBounds[permutation[i]];
				if ((intRect.xmin + intRect.xmax) / 2 > divider)
				{
					num--;
					int num2 = permutation[num];
					permutation[num] = permutation[i];
					permutation[i] = num2;
					i--;
				}
			}
			return num;
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x00061050 File Offset: 0x0005F250
		private static int SplitByZ(NativeArray<IntRect> nodesBounds, NativeArray<int> permutation, int from, int to, int divider)
		{
			int num = to;
			for (int i = from; i < num; i++)
			{
				IntRect intRect = nodesBounds[permutation[i]];
				if ((intRect.ymin + intRect.ymax) / 2 > divider)
				{
					num--;
					int num2 = permutation[num];
					permutation[num] = permutation[i];
					permutation[i] = num2;
					i--;
				}
			}
			return num;
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x000610BC File Offset: 0x0005F2BC
		private static int BuildSubtree(NativeArray<int> permutation, NativeArray<IntRect> nodeBounds, ref UnsafeList<int> nodes, ref UnsafeList<BBTree.BBTreeBox> tree, int from, int to, bool odd, int depth)
		{
			IntRect intRect = BBTree.NodeBounds(permutation, nodeBounds, from, to);
			int length = tree.Length;
			BBTree.BBTreeBox bbtreeBox = new BBTree.BBTreeBox(intRect);
			tree.Add(in bbtreeBox);
			if (to - from <= 4)
			{
				if (depth > 26)
				{
					Debug.LogWarning(string.Format("Maximum tree height of {0} exceeded (got depth of {1}). Querying this tree may fail. Is the tree very unbalanced?", 26, depth));
				}
				BBTree.BBTreeBox bbtreeBox2 = tree[length];
				int num = (bbtreeBox2.nodeOffset = nodes.Length);
				tree[length] = bbtreeBox2;
				nodes.Length += 4;
				for (int i = 0; i < 4; i++)
				{
					nodes[num + i] = ((i < to - from) ? permutation[from + i] : (-1));
				}
				return length;
			}
			int num3;
			if (odd)
			{
				int num2 = (intRect.xmin + intRect.xmax) / 2;
				num3 = BBTree.SplitByX(nodeBounds, permutation, from, to, num2);
			}
			else
			{
				int num4 = (intRect.ymin + intRect.ymax) / 2;
				num3 = BBTree.SplitByZ(nodeBounds, permutation, from, to, num4);
			}
			int num5 = (to - from) / 8;
			if (num3 <= from + num5 || num3 >= to - num5)
			{
				if (!odd)
				{
					int num6 = (intRect.xmin + intRect.xmax) / 2;
					num3 = BBTree.SplitByX(nodeBounds, permutation, from, to, num6);
				}
				else
				{
					int num7 = (intRect.ymin + intRect.ymax) / 2;
					num3 = BBTree.SplitByZ(nodeBounds, permutation, from, to, num7);
				}
				if (num3 <= from + num5 || num3 >= to - num5)
				{
					num3 = (from + to) / 2;
				}
			}
			int num8 = BBTree.BuildSubtree(permutation, nodeBounds, ref nodes, ref tree, from, num3, !odd, depth + 1);
			int num9 = BBTree.BuildSubtree(permutation, nodeBounds, ref nodes, ref tree, num3, to, !odd, depth + 1);
			BBTree.BBTreeBox bbtreeBox3 = tree[length];
			bbtreeBox3.left = num8;
			bbtreeBox3.right = num9;
			tree[length] = bbtreeBox3;
			return length;
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x000612A8 File Offset: 0x0005F4A8
		private static IntRect NodeBounds(NativeArray<int> permutation, NativeArray<IntRect> nodeBounds, int from, int to)
		{
			int2 @int = nodeBounds[permutation[from]].Min.ToInt2();
			int2 int2 = nodeBounds[permutation[from]].Max.ToInt2();
			for (int i = from + 1; i < to; i++)
			{
				IntRect intRect = nodeBounds[permutation[i]];
				int2 int3 = new int2(intRect.xmin, intRect.ymin);
				int2 int4 = new int2(intRect.xmax, intRect.ymax);
				@int = math.min(@int, int3);
				int2 = math.max(int2, int4);
			}
			return new IntRect(@int.x, @int.y, int2.x, int2.y);
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x00061367 File Offset: 0x0005F567
		public float DistanceSqrLowerBound(float3 p, in BBTree.ProjectionParams projection)
		{
			if (this.tree.Length == 0)
			{
				return float.PositiveInfinity;
			}
			return projection.SquaredRectPointDistanceOnPlane(this.tree[0].rect, p);
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x00061394 File Offset: 0x0005F594
		public unsafe void QueryClosest(float3 p, ref NearestNodeConstraint constraint, in BBTree.ProjectionParams projection, ref float distanceSqr, ref NNInfo previous, GraphNode[] nodes, UnsafeSpan<int> triangles, UnsafeSpan<Int3> vertices)
		{
			if (this.tree.Length == 0)
			{
				return;
			}
			checked
			{
				BBTree.NearbyNodesIterator.BoxWithDist* ptr = stackalloc BBTree.NearbyNodesIterator.BoxWithDist[unchecked((UIntPtr)26) * (UIntPtr)sizeof(BBTree.NearbyNodesIterator.BoxWithDist)];
				UnsafeSpan<BBTree.NearbyNodesIterator.BoxWithDist> unsafeSpan = new UnsafeSpan<BBTree.NearbyNodesIterator.BoxWithDist>((void*)ptr, 26, Allocator.None);
				*unsafeSpan[0] = new BBTree.NearbyNodesIterator.BoxWithDist
				{
					index = 0,
					distSqr = 0f
				};
				BBTree.NearbyNodesIterator nearbyNodesIterator = new BBTree.NearbyNodesIterator
				{
					stack = unsafeSpan,
					stackSize = 1,
					indexInLeaf = 0,
					point = p,
					projection = projection,
					distanceThresholdSqr = distanceSqr,
					tieBreakingDistanceThreshold = float.PositiveInfinity,
					tree = this.tree.AsUnsafeSpan<BBTree.BBTreeBox>(),
					nodes = this.nodePermutation.AsUnsafeSpan<int>(),
					triangles = triangles,
					vertices = vertices
				};
				NNInfo nninfo = previous;
				bool allNodesAreSuitable = constraint.allNodesAreSuitable;
				while (nearbyNodesIterator.stackSize > 0 && nearbyNodesIterator.MoveNext())
				{
					BBTree.CloseNode current = nearbyNodesIterator.current;
					if (allNodesAreSuitable || constraint.Suitable(nodes[current.node]))
					{
						nearbyNodesIterator.distanceThresholdSqr = current.distanceSq;
						nearbyNodesIterator.tieBreakingDistanceThreshold = current.tieBreakingDistance;
						nninfo = new NNInfo(nodes[current.node], current.closestPointOnNode, current.distanceSq);
					}
				}
				distanceSqr = nearbyNodesIterator.distanceThresholdSqr;
				previous = nninfo;
			}
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x00061509 File Offset: 0x0005F709
		public void DrawGizmos(CommandBuilder draw)
		{
			Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
			if (this.tree.Length == 0)
			{
				return;
			}
			this.DrawGizmos(ref draw, 0, 0);
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x00061544 File Offset: 0x0005F744
		private void DrawGizmos(ref CommandBuilder draw, int boxi, int depth)
		{
			BBTree.BBTreeBox bbtreeBox = this.tree[boxi];
			Vector3 vector = (Vector3)new Int3(bbtreeBox.rect.xmin, 0, bbtreeBox.rect.ymin);
			Vector3 vector2 = (Vector3)new Int3(bbtreeBox.rect.xmax, 0, bbtreeBox.rect.ymax);
			Vector3 vector3 = (vector + vector2) * 0.5f;
			Vector3 vector4 = vector2 - vector;
			vector4 = new Vector3(vector4.x, 1f, vector4.z);
			vector3.y += (float)(depth * 2);
			draw.xz.WireRectangle(vector3, new float2(vector4.x, vector4.z), AstarMath.IntToColor(depth, 1f));
			if (!bbtreeBox.IsLeaf)
			{
				this.DrawGizmos(ref draw, bbtreeBox.left, depth + 1);
				this.DrawGizmos(ref draw, bbtreeBox.right, depth + 1);
			}
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x00061644 File Offset: 0x0005F844
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void Build$BurstManaged(ref UnsafeSpan<int> triangles, ref UnsafeSpan<Int3> vertices, out BBTree bbTree)
		{
			int num = triangles.Length / 3;
			UnsafeList<BBTree.BBTreeBox> unsafeList = new UnsafeList<BBTree.BBTreeBox>((int)((float)num * 0.75f), Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			UnsafeList<int> unsafeList2 = new UnsafeList<int>((int)((float)num * 1.5f), Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			NativeArray<int> nativeArray = new NativeArray<int>(num, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < num; i++)
			{
				nativeArray[i] = i;
			}
			NativeArray<IntRect> nativeArray2 = new NativeArray<IntRect>(num, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			for (int j = 0; j < num; j++)
			{
				int2 xz = ((int3)(*vertices[*triangles[j * 3]])).xz;
				int2 xz2 = ((int3)(*vertices[*triangles[j * 3 + 1]])).xz;
				int2 xz3 = ((int3)(*vertices[*triangles[j * 3 + 2]])).xz;
				int2 @int = math.min(xz, math.min(xz2, xz3));
				int2 int2 = math.max(xz, math.max(xz2, xz3));
				nativeArray2[j] = new IntRect(@int.x, @int.y, int2.x, int2.y);
			}
			if (num > 0)
			{
				BBTree.BuildSubtree(nativeArray, nativeArray2, ref unsafeList2, ref unsafeList, 0, num, false, 0);
			}
			nativeArray2.Dispose();
			nativeArray.Dispose();
			bbTree = new BBTree
			{
				tree = unsafeList,
				nodePermutation = unsafeList2
			};
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x000617CF File Offset: 0x0005F9CF
		public static void Initialize$NearbyNodesIterator_MoveNext_00000EF8$BurstDirectCall()
		{
			BBTree.NearbyNodesIterator.MoveNext_00000EF8$BurstDirectCall.Initialize();
		}

		// Token: 0x04000BB0 RID: 2992
		private UnsafeList<BBTree.BBTreeBox> tree;

		// Token: 0x04000BB1 RID: 2993
		private UnsafeList<int> nodePermutation;

		// Token: 0x04000BB2 RID: 2994
		private const int MaximumLeafSize = 4;

		// Token: 0x04000BB3 RID: 2995
		private const int MAX_TREE_HEIGHT = 26;

		// Token: 0x0200027C RID: 636
		[BurstCompile]
		public readonly struct ProjectionParams
		{
			// Token: 0x1700025C RID: 604
			// (get) Token: 0x0600100F RID: 4111 RVA: 0x000617D6 File Offset: 0x0005F9D6
			public bool alignedWithXZPlane
			{
				get
				{
					return this.alignedWithXZPlaneBacking > 0;
				}
			}

			// Token: 0x06001010 RID: 4112 RVA: 0x000617E1 File Offset: 0x0005F9E1
			public float SquaredRectPointDistanceOnPlane(IntRect rect, float3 p)
			{
				return BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane(in this, ref rect, ref p);
			}

			// Token: 0x06001011 RID: 4113 RVA: 0x000617ED File Offset: 0x0005F9ED
			[BurstCompile(FloatMode = FloatMode.Fast)]
			[IgnoredByDeepProfiler]
			private static float SquaredRectPointDistanceOnPlane(in BBTree.ProjectionParams projection, ref IntRect rect, ref float3 p)
			{
				return BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane_00000EF1$BurstDirectCall.Invoke(in projection, ref rect, ref p);
			}

			// Token: 0x06001012 RID: 4114 RVA: 0x000617F8 File Offset: 0x0005F9F8
			public ProjectionParams(ref Pathfinding.DistanceMetric distanceMetric, GraphTransform graphTransform)
			{
				if (!(distanceMetric.projectionAxis != Vector3.zero))
				{
					this.projectionAxis = float3.zero;
					this.planeProjection = default(float2x3);
					this.projectedUpNormalized = default(float2);
					this.distanceMetricType = BBTree.DistanceMetric.Euclidean;
					this.alignedWithXZPlaneBacking = 1;
					this.distanceScaleAlongProjectionAxis = 0f;
					return;
				}
				if (float.IsPositiveInfinity(distanceMetric.projectionAxis.x))
				{
					this.projectionAxis = new float3(0f, 1f, 0f);
				}
				else
				{
					this.projectionAxis = math.normalizesafe(graphTransform.InverseTransformVector(distanceMetric.projectionAxis), default(float3));
				}
				if (this.projectionAxis.x * this.projectionAxis.x + this.projectionAxis.z * this.projectionAxis.z < 0.0001f)
				{
					this.projectedUpNormalized = float2.zero;
					this.planeProjection = new float2x3(1f, 0f, 0f, 0f, 0f, 1f);
					this.distanceMetricType = BBTree.DistanceMetric.ScaledManhattan;
					this.alignedWithXZPlaneBacking = 1;
					this.distanceScaleAlongProjectionAxis = math.max(distanceMetric.distanceScaleAlongProjectionDirection, 0f);
					return;
				}
				float3 @float = math.normalizesafe(math.cross(new float3(1f, 0f, 1f), this.projectionAxis), default(float3));
				if (math.all(@float == 0f))
				{
					@float = math.normalizesafe(math.cross(new float3(-1f, 0f, 1f), this.projectionAxis), default(float3));
				}
				float3 float2 = math.normalizesafe(math.cross(this.projectionAxis, @float), default(float3));
				this.planeProjection = math.transpose(new float3x2(@float, float2));
				this.projectedUpNormalized = ((math.lengthsq(this.planeProjection.c1) <= 0.0001f) ? float2.zero : math.normalize(this.planeProjection.c1));
				this.distanceMetricType = BBTree.DistanceMetric.ScaledManhattan;
				this.alignedWithXZPlaneBacking = (math.all(this.projectedUpNormalized == 0f) ? 1 : 0);
				this.distanceScaleAlongProjectionAxis = math.max(distanceMetric.distanceScaleAlongProjectionDirection, 0f);
			}

			// Token: 0x06001013 RID: 4115 RVA: 0x00061A48 File Offset: 0x0005FC48
			[BurstCompile(FloatMode = FloatMode.Fast)]
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static float SquaredRectPointDistanceOnPlane$BurstManaged(in BBTree.ProjectionParams projection, ref IntRect rect, ref float3 p)
			{
				if (projection.alignedWithXZPlane)
				{
					float2 @float = new float2((float)rect.xmin, (float)rect.ymin) * 0.001f;
					float2 float2 = new float2((float)rect.xmax, (float)rect.ymax) * 0.001f;
					return math.lengthsq(math.clamp(p.xz, @float, float2) - p.xz);
				}
				float3 float3 = new float3((float)rect.xmin, 0f, (float)rect.ymin) * 0.001f - p;
				float3 float4 = new float3((float)rect.xmax, 0f, (float)rect.ymax) * 0.001f - p;
				float3 float5 = new float3((float)rect.xmin, 0f, (float)rect.ymax) * 0.001f - p;
				float3 float6 = new float3((float)rect.xmax, 0f, (float)rect.ymin) * 0.001f - p;
				float2 float7 = math.mul(projection.planeProjection, float3);
				float2 float8 = math.mul(projection.planeProjection, float5);
				float2 float9 = math.mul(projection.planeProjection, float6);
				float2 float10 = math.mul(projection.planeProjection, float4);
				float2 float11 = new float2(projection.projectedUpNormalized.y, -projection.projectedUpNormalized.x);
				float4 float12 = math.mul(math.transpose(new float2x4(float7, float8, float9, float10)), float11);
				float num = math.clamp(0f, math.cmin(float12), math.cmax(float12));
				return num * num;
			}

			// Token: 0x04000BB4 RID: 2996
			public readonly float2x3 planeProjection;

			// Token: 0x04000BB5 RID: 2997
			public readonly float2 projectedUpNormalized;

			// Token: 0x04000BB6 RID: 2998
			public readonly float3 projectionAxis;

			// Token: 0x04000BB7 RID: 2999
			public readonly float distanceScaleAlongProjectionAxis;

			// Token: 0x04000BB8 RID: 3000
			public readonly BBTree.DistanceMetric distanceMetricType;

			// Token: 0x04000BB9 RID: 3001
			private readonly byte alignedWithXZPlaneBacking;

			// Token: 0x0200027D RID: 637
			// (Invoke) Token: 0x06001015 RID: 4117
			internal delegate float SquaredRectPointDistanceOnPlane_00000EF1$PostfixBurstDelegate(in BBTree.ProjectionParams projection, ref IntRect rect, ref float3 p);

			// Token: 0x0200027E RID: 638
			internal static class SquaredRectPointDistanceOnPlane_00000EF1$BurstDirectCall
			{
				// Token: 0x06001018 RID: 4120 RVA: 0x00061BFA File Offset: 0x0005FDFA
				[BurstDiscard]
				private static void GetFunctionPointerDiscard(ref IntPtr A_0)
				{
					if (BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane_00000EF1$BurstDirectCall.Pointer == 0)
					{
						BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane_00000EF1$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane_00000EF1$BurstDirectCall.DeferredCompilation, methodof(BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane$BurstManaged(ref BBTree.ProjectionParams, ref IntRect, ref float3)).MethodHandle, typeof(BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane_00000EF1$PostfixBurstDelegate).TypeHandle);
					}
					A_0 = BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane_00000EF1$BurstDirectCall.Pointer;
				}

				// Token: 0x06001019 RID: 4121 RVA: 0x00061C28 File Offset: 0x0005FE28
				private static IntPtr GetFunctionPointer()
				{
					IntPtr intPtr = (IntPtr)0;
					BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane_00000EF1$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
					return intPtr;
				}

				// Token: 0x0600101A RID: 4122 RVA: 0x00061C40 File Offset: 0x0005FE40
				public static void Constructor()
				{
					BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane_00000EF1$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane(ref BBTree.ProjectionParams, ref IntRect, ref float3)).MethodHandle);
				}

				// Token: 0x0600101B RID: 4123 RVA: 0x00003788 File Offset: 0x00001988
				public static void Initialize()
				{
				}

				// Token: 0x0600101C RID: 4124 RVA: 0x00061C51 File Offset: 0x0005FE51
				// Note: this type is marked as 'beforefieldinit'.
				static SquaredRectPointDistanceOnPlane_00000EF1$BurstDirectCall()
				{
					BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane_00000EF1$BurstDirectCall.Constructor();
				}

				// Token: 0x0600101D RID: 4125 RVA: 0x00061C58 File Offset: 0x0005FE58
				public static float Invoke(in BBTree.ProjectionParams projection, ref IntRect rect, ref float3 p)
				{
					if (BurstCompiler.IsEnabled)
					{
						IntPtr functionPointer = BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane_00000EF1$BurstDirectCall.GetFunctionPointer();
						if (functionPointer != 0)
						{
							return calli(System.Single(Pathfinding.Collections.BBTree/ProjectionParams&,Pathfinding.IntRect&,Unity.Mathematics.float3&), ref projection, ref rect, ref p, functionPointer);
						}
					}
					return BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane$BurstManaged(in projection, ref rect, ref p);
				}

				// Token: 0x04000BBA RID: 3002
				private static IntPtr Pointer;

				// Token: 0x04000BBB RID: 3003
				private static IntPtr DeferredCompilation;
			}
		}

		// Token: 0x0200027F RID: 639
		private struct CloseNode
		{
			// Token: 0x04000BBC RID: 3004
			public int node;

			// Token: 0x04000BBD RID: 3005
			public float distanceSq;

			// Token: 0x04000BBE RID: 3006
			public float tieBreakingDistance;

			// Token: 0x04000BBF RID: 3007
			public float3 closestPointOnNode;
		}

		// Token: 0x02000280 RID: 640
		public enum DistanceMetric : byte
		{
			// Token: 0x04000BC1 RID: 3009
			Euclidean,
			// Token: 0x04000BC2 RID: 3010
			ScaledManhattan
		}

		// Token: 0x02000281 RID: 641
		[BurstCompile]
		private struct NearbyNodesIterator : IEnumerator<BBTree.CloseNode>, IEnumerator, IDisposable
		{
			// Token: 0x1700025D RID: 605
			// (get) Token: 0x0600101E RID: 4126 RVA: 0x00061C8D File Offset: 0x0005FE8D
			public BBTree.CloseNode Current
			{
				get
				{
					return this.current;
				}
			}

			// Token: 0x0600101F RID: 4127 RVA: 0x00061C95 File Offset: 0x0005FE95
			public bool MoveNext()
			{
				return BBTree.NearbyNodesIterator.MoveNext(ref this);
			}

			// Token: 0x06001020 RID: 4128 RVA: 0x00003788 File Offset: 0x00001988
			void IDisposable.Dispose()
			{
			}

			// Token: 0x06001021 RID: 4129 RVA: 0x00003F84 File Offset: 0x00002184
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x1700025E RID: 606
			// (get) Token: 0x06001022 RID: 4130 RVA: 0x00003F84 File Offset: 0x00002184
			object IEnumerator.Current
			{
				get
				{
					throw new NotSupportedException();
				}
			}

			// Token: 0x06001023 RID: 4131 RVA: 0x00061C9D File Offset: 0x0005FE9D
			[BurstCompile(FloatMode = FloatMode.Default)]
			private static bool MoveNext(ref BBTree.NearbyNodesIterator it)
			{
				return BBTree.NearbyNodesIterator.MoveNext_00000EF8$BurstDirectCall.Invoke(ref it);
			}

			// Token: 0x06001024 RID: 4132 RVA: 0x00061CA8 File Offset: 0x0005FEA8
			[BurstCompile(FloatMode = FloatMode.Default)]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal unsafe static bool MoveNext$BurstManaged(ref BBTree.NearbyNodesIterator it)
			{
				float num = it.distanceThresholdSqr;
				while (it.stackSize != 0)
				{
					BBTree.NearbyNodesIterator.BoxWithDist boxWithDist = *it.stack[it.stackSize - 1];
					if (boxWithDist.distSqr > num)
					{
						it.stackSize--;
						it.indexInLeaf = 0;
					}
					else
					{
						BBTree.BBTreeBox bbtreeBox = *it.tree[boxWithDist.index];
						if (bbtreeBox.IsLeaf)
						{
							for (int i = it.indexInLeaf; i < 4; i++)
							{
								int num2 = *it.nodes[bbtreeBox.nodeOffset + i];
								if (num2 == -1)
								{
									break;
								}
								uint num3 = (uint)(num2 * 3);
								uint num4 = (uint)(num2 * 3 + 1);
								uint num5 = (uint)(num2 * 3 + 2);
								if (num5 >= it.triangles.length)
								{
									throw new Exception("Invalid node index");
								}
								Hint.Assume(num3 < it.triangles.length && num4 < it.triangles.length && num5 < it.triangles.length);
								Int3 @int = *it.vertices[*it.triangles[num3]];
								Int3 int2 = *it.vertices[*it.triangles[num4]];
								Int3 int3 = *it.vertices[*it.triangles[num5]];
								if (it.projection.distanceMetricType == BBTree.DistanceMetric.Euclidean)
								{
									float3 @float = (float3)@int;
									float3 float2 = (float3)int2;
									float3 float3 = (float3)int3;
									float3 float4;
									Polygon.ClosestPointOnTriangleByRef(in @float, in float2, in float3, in it.point, out float4);
									float num6 = math.distancesq(float4, it.point);
									if (num6 < num)
									{
										it.indexInLeaf = i + 1;
										it.current = new BBTree.CloseNode
										{
											node = num2,
											distanceSq = num6,
											tieBreakingDistance = 0f,
											closestPointOnNode = float4
										};
										return true;
									}
								}
								else
								{
									float3 float5;
									float num7;
									float num8;
									Polygon.ClosestPointOnTriangleProjected(ref @int, ref int2, ref int3, ref it.projection, ref it.point, out float5, out num7, out num8);
									if (num7 < num || (num7 == num && num8 < it.tieBreakingDistanceThreshold))
									{
										it.indexInLeaf = i + 1;
										it.current = new BBTree.CloseNode
										{
											node = num2,
											distanceSq = num7,
											tieBreakingDistance = num8,
											closestPointOnNode = float5
										};
										return true;
									}
								}
							}
							it.indexInLeaf = 0;
							it.stackSize--;
						}
						else
						{
							it.stackSize--;
							int left = bbtreeBox.left;
							int right = bbtreeBox.right;
							float num9 = it.projection.SquaredRectPointDistanceOnPlane(it.tree[left].rect, it.point);
							float num10 = it.projection.SquaredRectPointDistanceOnPlane(it.tree[right].rect, it.point);
							if (num10 < num9)
							{
								Memory.Swap<int>(ref left, ref right);
								Memory.Swap<float>(ref num9, ref num10);
							}
							if (it.stackSize + 2 > it.stack.Length)
							{
								throw new InvalidOperationException("Tree is too deep. Overflowed the internal stack.");
							}
							if (num10 <= num)
							{
								int num11 = it.stackSize;
								it.stackSize = num11 + 1;
								*it.stack[num11] = new BBTree.NearbyNodesIterator.BoxWithDist
								{
									index = right,
									distSqr = num10
								};
							}
							if (num9 <= num)
							{
								int num11 = it.stackSize;
								it.stackSize = num11 + 1;
								*it.stack[num11] = new BBTree.NearbyNodesIterator.BoxWithDist
								{
									index = left,
									distSqr = num9
								};
							}
						}
					}
				}
				return false;
			}

			// Token: 0x04000BC3 RID: 3011
			public UnsafeSpan<BBTree.NearbyNodesIterator.BoxWithDist> stack;

			// Token: 0x04000BC4 RID: 3012
			public int stackSize;

			// Token: 0x04000BC5 RID: 3013
			public UnsafeSpan<BBTree.BBTreeBox> tree;

			// Token: 0x04000BC6 RID: 3014
			public UnsafeSpan<int> nodes;

			// Token: 0x04000BC7 RID: 3015
			public UnsafeSpan<int> triangles;

			// Token: 0x04000BC8 RID: 3016
			public UnsafeSpan<Int3> vertices;

			// Token: 0x04000BC9 RID: 3017
			public int indexInLeaf;

			// Token: 0x04000BCA RID: 3018
			public float3 point;

			// Token: 0x04000BCB RID: 3019
			public BBTree.ProjectionParams projection;

			// Token: 0x04000BCC RID: 3020
			public float distanceThresholdSqr;

			// Token: 0x04000BCD RID: 3021
			public float tieBreakingDistanceThreshold;

			// Token: 0x04000BCE RID: 3022
			internal BBTree.CloseNode current;

			// Token: 0x02000282 RID: 642
			public struct BoxWithDist
			{
				// Token: 0x04000BCF RID: 3023
				public int index;

				// Token: 0x04000BD0 RID: 3024
				public float distSqr;
			}

			// Token: 0x02000283 RID: 643
			// (Invoke) Token: 0x06001026 RID: 4134
			internal delegate bool MoveNext_00000EF8$PostfixBurstDelegate(ref BBTree.NearbyNodesIterator it);

			// Token: 0x02000284 RID: 644
			internal static class MoveNext_00000EF8$BurstDirectCall
			{
				// Token: 0x06001029 RID: 4137 RVA: 0x00062060 File Offset: 0x00060260
				[BurstDiscard]
				private static void GetFunctionPointerDiscard(ref IntPtr A_0)
				{
					if (BBTree.NearbyNodesIterator.MoveNext_00000EF8$BurstDirectCall.Pointer == 0)
					{
						BBTree.NearbyNodesIterator.MoveNext_00000EF8$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(BBTree.NearbyNodesIterator.MoveNext_00000EF8$BurstDirectCall.DeferredCompilation, methodof(BBTree.NearbyNodesIterator.MoveNext$BurstManaged(ref BBTree.NearbyNodesIterator)).MethodHandle, typeof(BBTree.NearbyNodesIterator.MoveNext_00000EF8$PostfixBurstDelegate).TypeHandle);
					}
					A_0 = BBTree.NearbyNodesIterator.MoveNext_00000EF8$BurstDirectCall.Pointer;
				}

				// Token: 0x0600102A RID: 4138 RVA: 0x0006208C File Offset: 0x0006028C
				private static IntPtr GetFunctionPointer()
				{
					IntPtr intPtr = (IntPtr)0;
					BBTree.NearbyNodesIterator.MoveNext_00000EF8$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
					return intPtr;
				}

				// Token: 0x0600102B RID: 4139 RVA: 0x000620A4 File Offset: 0x000602A4
				public static void Constructor()
				{
					BBTree.NearbyNodesIterator.MoveNext_00000EF8$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(BBTree.NearbyNodesIterator.MoveNext(ref BBTree.NearbyNodesIterator)).MethodHandle);
				}

				// Token: 0x0600102C RID: 4140 RVA: 0x00003788 File Offset: 0x00001988
				public static void Initialize()
				{
				}

				// Token: 0x0600102D RID: 4141 RVA: 0x000620B5 File Offset: 0x000602B5
				// Note: this type is marked as 'beforefieldinit'.
				static MoveNext_00000EF8$BurstDirectCall()
				{
					BBTree.NearbyNodesIterator.MoveNext_00000EF8$BurstDirectCall.Constructor();
				}

				// Token: 0x0600102E RID: 4142 RVA: 0x000620BC File Offset: 0x000602BC
				public static bool Invoke(ref BBTree.NearbyNodesIterator it)
				{
					if (BurstCompiler.IsEnabled)
					{
						IntPtr functionPointer = BBTree.NearbyNodesIterator.MoveNext_00000EF8$BurstDirectCall.GetFunctionPointer();
						if (functionPointer != 0)
						{
							return calli(System.Boolean(Pathfinding.Collections.BBTree/NearbyNodesIterator&), ref it, functionPointer);
						}
					}
					return BBTree.NearbyNodesIterator.MoveNext$BurstManaged(ref it);
				}

				// Token: 0x04000BD1 RID: 3025
				private static IntPtr Pointer;

				// Token: 0x04000BD2 RID: 3026
				private static IntPtr DeferredCompilation;
			}
		}

		// Token: 0x02000285 RID: 645
		private struct BBTreeBox
		{
			// Token: 0x1700025F RID: 607
			// (get) Token: 0x0600102F RID: 4143 RVA: 0x000620ED File Offset: 0x000602ED
			public bool IsLeaf
			{
				get
				{
					return this.nodeOffset >= 0;
				}
			}

			// Token: 0x06001030 RID: 4144 RVA: 0x000620FC File Offset: 0x000602FC
			public BBTreeBox(IntRect rect)
			{
				this.nodeOffset = -1;
				this.rect = rect;
				this.left = (this.right = -1);
			}

			// Token: 0x04000BD3 RID: 3027
			public IntRect rect;

			// Token: 0x04000BD4 RID: 3028
			public int nodeOffset;

			// Token: 0x04000BD5 RID: 3029
			public int left;

			// Token: 0x04000BD6 RID: 3030
			public int right;
		}

		// Token: 0x02000286 RID: 646
		// (Invoke) Token: 0x06001032 RID: 4146
		internal delegate void Build_00000EE6$PostfixBurstDelegate(ref UnsafeSpan<int> triangles, ref UnsafeSpan<Int3> vertices, out BBTree bbTree);

		// Token: 0x02000287 RID: 647
		internal static class Build_00000EE6$BurstDirectCall
		{
			// Token: 0x06001035 RID: 4149 RVA: 0x00062127 File Offset: 0x00060327
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (BBTree.Build_00000EE6$BurstDirectCall.Pointer == 0)
				{
					BBTree.Build_00000EE6$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(BBTree.Build_00000EE6$BurstDirectCall.DeferredCompilation, methodof(BBTree.Build$BurstManaged(ref UnsafeSpan<int>, ref UnsafeSpan<Int3>, ref BBTree)).MethodHandle, typeof(BBTree.Build_00000EE6$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = BBTree.Build_00000EE6$BurstDirectCall.Pointer;
			}

			// Token: 0x06001036 RID: 4150 RVA: 0x00062154 File Offset: 0x00060354
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				BBTree.Build_00000EE6$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001037 RID: 4151 RVA: 0x0006216C File Offset: 0x0006036C
			public static void Constructor()
			{
				BBTree.Build_00000EE6$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(BBTree.Build(ref UnsafeSpan<int>, ref UnsafeSpan<Int3>, ref BBTree)).MethodHandle);
			}

			// Token: 0x06001038 RID: 4152 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x06001039 RID: 4153 RVA: 0x0006217D File Offset: 0x0006037D
			// Note: this type is marked as 'beforefieldinit'.
			static Build_00000EE6$BurstDirectCall()
			{
				BBTree.Build_00000EE6$BurstDirectCall.Constructor();
			}

			// Token: 0x0600103A RID: 4154 RVA: 0x00062184 File Offset: 0x00060384
			public static void Invoke(ref UnsafeSpan<int> triangles, ref UnsafeSpan<Int3> vertices, out BBTree bbTree)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = BBTree.Build_00000EE6$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Collections.UnsafeSpan`1<System.Int32>&,Pathfinding.Collections.UnsafeSpan`1<Pathfinding.Int3>&,Pathfinding.Collections.BBTree&), ref triangles, ref vertices, ref bbTree, functionPointer);
						return;
					}
				}
				BBTree.Build$BurstManaged(ref triangles, ref vertices, out bbTree);
			}

			// Token: 0x04000BD7 RID: 3031
			private static IntPtr Pointer;

			// Token: 0x04000BD8 RID: 3032
			private static IntPtr DeferredCompilation;
		}
	}
}
