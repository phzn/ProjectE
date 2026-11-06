using System;
using Pathfinding.Jobs;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Rules
{
	// Token: 0x02000218 RID: 536
	[Preserve]
	public class RuleElevationPenalty : GridGraphRule
	{
		// Token: 0x06000E13 RID: 3603 RVA: 0x00055F18 File Offset: 0x00054118
		public override void Register(GridGraphRules rules)
		{
			if (!this.elevationToPenalty.IsCreated)
			{
				this.elevationToPenalty = new NativeArray<float>(64, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			}
			for (int i = 0; i < this.elevationToPenalty.Length; i++)
			{
				this.elevationToPenalty[i] = Mathf.Max(0f, this.penaltyScale * this.curve.Evaluate((float)i * 1f / (float)(this.elevationToPenalty.Length - 1)));
			}
			Vector2 clampedElevationRange = new Vector2(math.max(0f, this.elevationRange.x), math.max(1f, this.elevationRange.y));
			rules.AddJobSystemPass(GridGraphRule.Pass.BeforeConnections, delegate(GridGraphRules.Context context)
			{
				Matrix4x4 matrix4x = Matrix4x4.Scale(new Vector3(1f, 1f / (clampedElevationRange.y - clampedElevationRange.x), 1f)) * Matrix4x4.Translate(new Vector3(0f, -clampedElevationRange.x, 0f));
				new RuleElevationPenalty.JobElevationPenalty
				{
					elevationToPenalty = this.elevationToPenalty,
					nodePositions = context.data.nodes.positions,
					worldToGraph = matrix4x * context.data.transform.matrix.inverse,
					penalty = context.data.nodes.penalties
				}.Schedule(context.tracker);
			});
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x00055FEB File Offset: 0x000541EB
		public override void DisposeUnmanagedData()
		{
			if (this.elevationToPenalty.IsCreated)
			{
				this.elevationToPenalty.Dispose();
			}
		}

		// Token: 0x04000A0E RID: 2574
		public float penaltyScale = 10000f;

		// Token: 0x04000A0F RID: 2575
		public Vector2 elevationRange = new Vector2(0f, 100f);

		// Token: 0x04000A10 RID: 2576
		public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04000A11 RID: 2577
		private NativeArray<float> elevationToPenalty;

		// Token: 0x02000219 RID: 537
		[BurstCompile(FloatMode = FloatMode.Fast)]
		public struct JobElevationPenalty : IJob
		{
			// Token: 0x06000E16 RID: 3606 RVA: 0x0005605C File Offset: 0x0005425C
			public void Execute()
			{
				for (int i = 0; i < this.penalty.Length; i++)
				{
					float num = math.clamp(this.worldToGraph.MultiplyPoint3x4(this.nodePositions[i]).y, 0f, 1f) * (float)(this.elevationToPenalty.Length - 1);
					int num2 = (int)num;
					float num3 = this.elevationToPenalty[num2];
					float num4 = this.elevationToPenalty[math.min(num2 + 1, this.elevationToPenalty.Length - 1)];
					ref NativeArray<uint> ptr = ref this.penalty;
					int num5 = i;
					ptr[num5] += (uint)math.lerp(num3, num4, num - (float)num2);
				}
			}

			// Token: 0x04000A12 RID: 2578
			[ReadOnly]
			public NativeArray<float> elevationToPenalty;

			// Token: 0x04000A13 RID: 2579
			[ReadOnly]
			public NativeArray<Vector3> nodePositions;

			// Token: 0x04000A14 RID: 2580
			public Matrix4x4 worldToGraph;

			// Token: 0x04000A15 RID: 2581
			public NativeArray<uint> penalty;
		}
	}
}
