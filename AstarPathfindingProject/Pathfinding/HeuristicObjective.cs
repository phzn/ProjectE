using System;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Pathfinding.Graphs.Util;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Mathematics;

namespace Pathfinding
{
	// Token: 0x020000A7 RID: 167
	[BurstCompile]
	public readonly struct HeuristicObjective
	{
		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000592 RID: 1426 RVA: 0x0001AFB8 File Offset: 0x000191B8
		public bool hasHeuristic
		{
			get
			{
				return this.heuristic != Heuristic.None;
			}
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001AFC8 File Offset: 0x000191C8
		public HeuristicObjective(int3 point, Heuristic heuristic, float heuristicScale)
		{
			this.mx = point;
			this.mn = point;
			this.heuristic = heuristic;
			this.heuristicScale = heuristicScale;
			this.euclideanEmbeddingCosts = default(UnsafeSpan<uint>);
			this.euclideanEmbeddingPivots = 0U;
			this.targetNodeIndex = 0U;
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0001B010 File Offset: 0x00019210
		public HeuristicObjective(int3 point, Heuristic heuristic, float heuristicScale, uint targetNodeIndex, EuclideanEmbedding euclideanEmbedding)
		{
			this.mx = point;
			this.mn = point;
			this.heuristic = heuristic;
			this.heuristicScale = heuristicScale;
			this.euclideanEmbeddingCosts = ((euclideanEmbedding != null) ? euclideanEmbedding.costs.AsUnsafeSpanNoChecks<uint>() : default(UnsafeSpan<uint>));
			this.euclideanEmbeddingPivots = (uint)((euclideanEmbedding != null) ? euclideanEmbedding.pivotCount : 0);
			this.targetNodeIndex = targetNodeIndex;
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0001B078 File Offset: 0x00019278
		public HeuristicObjective(int3 mn, int3 mx, Heuristic heuristic, float heuristicScale, uint targetNodeIndex, EuclideanEmbedding euclideanEmbedding)
		{
			this.mn = mn;
			this.mx = mx;
			this.heuristic = heuristic;
			this.heuristicScale = heuristicScale;
			this.euclideanEmbeddingCosts = ((euclideanEmbedding != null) ? euclideanEmbedding.costs.AsUnsafeSpanNoChecks<uint>() : default(UnsafeSpan<uint>));
			this.euclideanEmbeddingPivots = (uint)((euclideanEmbedding != null) ? euclideanEmbedding.pivotCount : 0);
			this.targetNodeIndex = targetNodeIndex;
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0001B0DF File Offset: 0x000192DF
		public int Calculate(int3 point, uint nodeIndex)
		{
			return HeuristicObjective.Calculate(in this, ref point, nodeIndex);
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0001B0EA File Offset: 0x000192EA
		[BurstCompile]
		public static int Calculate(in HeuristicObjective objective, ref int3 point, uint nodeIndex)
		{
			return HeuristicObjective.Calculate_00000560$BurstDirectCall.Invoke(in objective, ref point, nodeIndex);
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0001B0F4 File Offset: 0x000192F4
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int Calculate$BurstManaged(in HeuristicObjective objective, ref int3 point, uint nodeIndex)
		{
			int3 @int = math.clamp(point, objective.mn, objective.mx);
			int3 int2 = point - @int;
			int num;
			switch (objective.heuristic)
			{
			case Heuristic.Manhattan:
				num = (int)((float)math.csum(math.abs(int2)) * objective.heuristicScale);
				goto IL_00EC;
			case Heuristic.DiagonalManhattan:
			{
				int2 = math.abs(int2);
				int x = int2.x;
				int y = int2.y;
				int z = int2.z;
				if (x > y)
				{
					Memory.Swap<int>(ref x, ref y);
				}
				if (y > z)
				{
					Memory.Swap<int>(ref y, ref z);
				}
				if (x > y)
				{
					Memory.Swap<int>(ref x, ref y);
				}
				num = (int)(objective.heuristicScale * (1.7321f * (float)x + 1.4142f * (float)(y - x) + (float)(z - y - x)));
				goto IL_00EC;
			}
			case Heuristic.Euclidean:
				num = (int)(math.length(int2) * objective.heuristicScale);
				goto IL_00EC;
			}
			num = 0;
			IL_00EC:
			if (objective.euclideanEmbeddingPivots > 0U)
			{
				num = math.max(num, (int)EuclideanEmbedding.GetHeuristic(objective.euclideanEmbeddingCosts, objective.euclideanEmbeddingPivots, nodeIndex, objective.targetNodeIndex));
			}
			return num;
		}

		// Token: 0x04000385 RID: 901
		private readonly int3 mn;

		// Token: 0x04000386 RID: 902
		private readonly int3 mx;

		// Token: 0x04000387 RID: 903
		private readonly Heuristic heuristic;

		// Token: 0x04000388 RID: 904
		private readonly float heuristicScale;

		// Token: 0x04000389 RID: 905
		private readonly UnsafeSpan<uint> euclideanEmbeddingCosts;

		// Token: 0x0400038A RID: 906
		private readonly uint euclideanEmbeddingPivots;

		// Token: 0x0400038B RID: 907
		private readonly uint targetNodeIndex;

		// Token: 0x020000A8 RID: 168
		// (Invoke) Token: 0x0600059A RID: 1434
		internal delegate int Calculate_00000560$PostfixBurstDelegate(in HeuristicObjective objective, ref int3 point, uint nodeIndex);

		// Token: 0x020000A9 RID: 169
		internal static class Calculate_00000560$BurstDirectCall
		{
			// Token: 0x0600059D RID: 1437 RVA: 0x0001B216 File Offset: 0x00019416
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (HeuristicObjective.Calculate_00000560$BurstDirectCall.Pointer == 0)
				{
					HeuristicObjective.Calculate_00000560$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(HeuristicObjective.Calculate_00000560$BurstDirectCall.DeferredCompilation, methodof(HeuristicObjective.Calculate$BurstManaged(ref HeuristicObjective, ref int3, uint)).MethodHandle, typeof(HeuristicObjective.Calculate_00000560$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = HeuristicObjective.Calculate_00000560$BurstDirectCall.Pointer;
			}

			// Token: 0x0600059E RID: 1438 RVA: 0x0001B244 File Offset: 0x00019444
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				HeuristicObjective.Calculate_00000560$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600059F RID: 1439 RVA: 0x0001B25C File Offset: 0x0001945C
			public static void Constructor()
			{
				HeuristicObjective.Calculate_00000560$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(HeuristicObjective.Calculate(ref HeuristicObjective, ref int3, uint)).MethodHandle);
			}

			// Token: 0x060005A0 RID: 1440 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x060005A1 RID: 1441 RVA: 0x0001B26D File Offset: 0x0001946D
			// Note: this type is marked as 'beforefieldinit'.
			static Calculate_00000560$BurstDirectCall()
			{
				HeuristicObjective.Calculate_00000560$BurstDirectCall.Constructor();
			}

			// Token: 0x060005A2 RID: 1442 RVA: 0x0001B274 File Offset: 0x00019474
			public static int Invoke(in HeuristicObjective objective, ref int3 point, uint nodeIndex)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = HeuristicObjective.Calculate_00000560$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(Pathfinding.HeuristicObjective&,Unity.Mathematics.int3&,System.UInt32), ref objective, ref point, nodeIndex, functionPointer);
					}
				}
				return HeuristicObjective.Calculate$BurstManaged(in objective, ref point, nodeIndex);
			}

			// Token: 0x0400038C RID: 908
			private static IntPtr Pointer;

			// Token: 0x0400038D RID: 909
			private static IntPtr DeferredCompilation;
		}
	}
}
