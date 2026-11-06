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
	// Token: 0x02000216 RID: 534
	[Preserve]
	public class RuleAnglePenalty : GridGraphRule
	{
		// Token: 0x06000E0E RID: 3598 RVA: 0x00055CCC File Offset: 0x00053ECC
		public override void Register(GridGraphRules rules)
		{
			if (!this.angleToPenalty.IsCreated)
			{
				this.angleToPenalty = new NativeArray<float>(32, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			}
			for (int i = 0; i < this.angleToPenalty.Length; i++)
			{
				this.angleToPenalty[i] = Mathf.Max(0f, this.curve.Evaluate(90f * (float)i / (float)(this.angleToPenalty.Length - 1)) * this.penaltyScale);
			}
			rules.AddJobSystemPass(GridGraphRule.Pass.BeforeConnections, delegate(GridGraphRules.Context context)
			{
				new RuleAnglePenalty.JobPenaltyAngle
				{
					angleToPenalty = this.angleToPenalty,
					up = context.data.up,
					nodeNormals = context.data.nodes.normals,
					penalty = context.data.nodes.penalties
				}.Schedule(context.tracker);
			});
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x00055D5D File Offset: 0x00053F5D
		public override void DisposeUnmanagedData()
		{
			if (this.angleToPenalty.IsCreated)
			{
				this.angleToPenalty.Dispose();
			}
		}

		// Token: 0x04000A07 RID: 2567
		public float penaltyScale = 10000f;

		// Token: 0x04000A08 RID: 2568
		public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 90f, 1f);

		// Token: 0x04000A09 RID: 2569
		private NativeArray<float> angleToPenalty;

		// Token: 0x02000217 RID: 535
		[BurstCompile(FloatMode = FloatMode.Fast)]
		public struct JobPenaltyAngle : IJob
		{
			// Token: 0x06000E12 RID: 3602 RVA: 0x00055E1C File Offset: 0x0005401C
			public void Execute()
			{
				float4 @float = new float4(this.up.x, this.up.y, this.up.z, 0f);
				for (int i = 0; i < this.penalty.Length; i++)
				{
					float4 float2 = this.nodeNormals[i];
					if (math.any(float2))
					{
						float num = math.acos(math.dot(float2, @float)) * (float)(this.angleToPenalty.Length - 1) / 3.1415927f;
						int num2 = (int)num;
						float num3 = this.angleToPenalty[math.max(num2, 0)];
						float num4 = this.angleToPenalty[math.min(num2 + 1, this.angleToPenalty.Length - 1)];
						ref NativeArray<uint> ptr = ref this.penalty;
						int num5 = i;
						ptr[num5] += (uint)math.lerp(num3, num4, num - (float)num2);
					}
				}
			}

			// Token: 0x04000A0A RID: 2570
			public Vector3 up;

			// Token: 0x04000A0B RID: 2571
			[ReadOnly]
			public NativeArray<float> angleToPenalty;

			// Token: 0x04000A0C RID: 2572
			[ReadOnly]
			public NativeArray<float4> nodeNormals;

			// Token: 0x04000A0D RID: 2573
			public NativeArray<uint> penalty;
		}
	}
}
