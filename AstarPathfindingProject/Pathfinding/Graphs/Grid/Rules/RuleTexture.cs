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
	// Token: 0x0200021F RID: 543
	[Preserve]
	public class RuleTexture : GridGraphRule
	{
		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000E1D RID: 3613 RVA: 0x0005642E File Offset: 0x0005462E
		public override int Hash
		{
			get
			{
				return base.Hash ^ ((this.texture != null) ? ((31 * this.texture.GetInstanceID()) ^ (int)this.texture.updateCount) : 0);
			}
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x00056464 File Offset: 0x00054664
		public override void Register(GridGraphRules rules)
		{
			if (this.texture == null)
			{
				return;
			}
			if (!this.texture.isReadable)
			{
				Debug.LogError("Texture for the texture rule on a grid graph is not marked as readable.", this.texture);
				return;
			}
			if (this.colors.IsCreated)
			{
				this.colors.Dispose();
			}
			this.colors = new NativeArray<Color32>(this.texture.GetPixels32(), Allocator.Persistent).Reinterpret<int>();
			int2 textureSize = new int2(this.texture.width, this.texture.height);
			float4 channelPenaltiesCombined = float4.zero;
			bool4 channelDeterminesWalkability = false;
			float4 channelPositionScalesCombined = float4.zero;
			for (int i = 0; i < 4; i++)
			{
				channelPenaltiesCombined[i] = ((this.channels[i] == RuleTexture.ChannelUse.Penalty || this.channels[i] == RuleTexture.ChannelUse.WalkablePenalty) ? this.channelScales[i] : 0f);
				channelDeterminesWalkability[i] = this.channels[i] == RuleTexture.ChannelUse.Walkable || this.channels[i] == RuleTexture.ChannelUse.WalkablePenalty;
				channelPositionScalesCombined[i] = ((this.channels[i] == RuleTexture.ChannelUse.Position) ? this.channelScales[i] : 0f);
			}
			channelPositionScalesCombined /= 255f;
			channelPenaltiesCombined /= 255f;
			if (math.any(channelPositionScalesCombined))
			{
				rules.AddJobSystemPass(GridGraphRule.Pass.BeforeCollision, delegate(GridGraphRules.Context context)
				{
					new RuleTexture.JobTexturePosition
					{
						colorData = this.colors,
						nodePositions = context.data.nodes.positions,
						nodeNormals = context.data.nodes.normals,
						bounds = context.data.nodes.bounds,
						colorDataSize = textureSize,
						scale = ((this.scalingMode == RuleTexture.ScalingMode.FixedScale) ? (1f / math.max(0.01f, this.nodesPerPixel)) : (textureSize / new float2((float)context.graph.width, (float)context.graph.depth))),
						channelPositionScale = channelPositionScalesCombined,
						graphToWorld = context.data.transform.matrix
					}.Schedule(context.tracker);
				});
			}
			rules.AddJobSystemPass(GridGraphRule.Pass.BeforeConnections, delegate(GridGraphRules.Context context)
			{
				new RuleTexture.JobTexturePenalty
				{
					colorData = this.colors,
					penalty = context.data.nodes.penalties,
					walkable = context.data.nodes.walkable,
					nodeNormals = context.data.nodes.normals,
					bounds = context.data.nodes.bounds,
					colorDataSize = textureSize,
					scale = ((this.scalingMode == RuleTexture.ScalingMode.FixedScale) ? (1f / math.max(0.01f, this.nodesPerPixel)) : (textureSize / new float2((float)context.graph.width, (float)context.graph.depth))),
					channelPenalties = channelPenaltiesCombined,
					channelDeterminesWalkability = channelDeterminesWalkability
				}.Schedule(context.tracker);
			});
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x00056612 File Offset: 0x00054812
		public override void DisposeUnmanagedData()
		{
			if (this.colors.IsCreated)
			{
				this.colors.Dispose();
			}
		}

		// Token: 0x04000A22 RID: 2594
		public Texture2D texture;

		// Token: 0x04000A23 RID: 2595
		public RuleTexture.ChannelUse[] channels = new RuleTexture.ChannelUse[4];

		// Token: 0x04000A24 RID: 2596
		public float[] channelScales = new float[] { 1000f, 1000f, 1000f, 1000f };

		// Token: 0x04000A25 RID: 2597
		public RuleTexture.ScalingMode scalingMode = RuleTexture.ScalingMode.StretchToFitGraph;

		// Token: 0x04000A26 RID: 2598
		public float nodesPerPixel = 1f;

		// Token: 0x04000A27 RID: 2599
		private NativeArray<int> colors;

		// Token: 0x02000220 RID: 544
		public enum ScalingMode
		{
			// Token: 0x04000A29 RID: 2601
			FixedScale,
			// Token: 0x04000A2A RID: 2602
			StretchToFitGraph
		}

		// Token: 0x02000221 RID: 545
		public enum ChannelUse
		{
			// Token: 0x04000A2C RID: 2604
			None,
			// Token: 0x04000A2D RID: 2605
			Penalty,
			// Token: 0x04000A2E RID: 2606
			Position,
			// Token: 0x04000A2F RID: 2607
			WalkablePenalty,
			// Token: 0x04000A30 RID: 2608
			Walkable
		}

		// Token: 0x02000222 RID: 546
		[BurstCompile]
		public struct JobTexturePosition : IJob, GridIterationUtilities.INodeModifier
		{
			// Token: 0x06000E21 RID: 3617 RVA: 0x0005666C File Offset: 0x0005486C
			public void ModifyNode(int dataIndex, int dataX, int dataLayer, int dataZ)
			{
				int2 xz = this.bounds.min.xz;
				int2 @int = math.clamp((int2)math.round((new float2((float)dataX, (float)dataZ) + xz) * this.scale), int2.zero, this.colorDataSize - new int2(1, 1));
				int num = @int.y * this.colorDataSize.x + @int.x;
				int4 int2 = new int4(this.colorData[num] & 255, (this.colorData[num] >> 8) & 255, (this.colorData[num] >> 16) & 255, (this.colorData[num] >> 24) & 255);
				float num2 = math.dot(this.channelPositionScale, int2);
				this.nodePositions[dataIndex] = this.graphToWorld.MultiplyPoint3x4(new Vector3((float)(this.bounds.min.x + dataX) + 0.5f, num2, (float)(this.bounds.min.z + dataZ) + 0.5f));
			}

			// Token: 0x06000E22 RID: 3618 RVA: 0x000567A6 File Offset: 0x000549A6
			public void Execute()
			{
				GridIterationUtilities.ForEachNode<RuleTexture.JobTexturePosition>(this.bounds.size, this.nodeNormals, ref this);
			}

			// Token: 0x04000A31 RID: 2609
			[ReadOnly]
			public NativeArray<int> colorData;

			// Token: 0x04000A32 RID: 2610
			[WriteOnly]
			public NativeArray<Vector3> nodePositions;

			// Token: 0x04000A33 RID: 2611
			[ReadOnly]
			public NativeArray<float4> nodeNormals;

			// Token: 0x04000A34 RID: 2612
			public Matrix4x4 graphToWorld;

			// Token: 0x04000A35 RID: 2613
			public IntBounds bounds;

			// Token: 0x04000A36 RID: 2614
			public int2 colorDataSize;

			// Token: 0x04000A37 RID: 2615
			public float2 scale;

			// Token: 0x04000A38 RID: 2616
			public float4 channelPositionScale;
		}

		// Token: 0x02000223 RID: 547
		[BurstCompile]
		public struct JobTexturePenalty : IJob, GridIterationUtilities.INodeModifier
		{
			// Token: 0x06000E23 RID: 3619 RVA: 0x000567C0 File Offset: 0x000549C0
			public void ModifyNode(int dataIndex, int dataX, int dataLayer, int dataZ)
			{
				int2 xz = this.bounds.min.xz;
				int2 @int = math.clamp((int2)math.round((new float2((float)dataX, (float)dataZ) + xz) * this.scale), int2.zero, this.colorDataSize - new int2(1, 1));
				int num = @int.y * this.colorDataSize.x + @int.x;
				int4 int2 = new int4(this.colorData[num] & 255, (this.colorData[num] >> 8) & 255, (this.colorData[num] >> 16) & 255, (this.colorData[num] >> 24) & 255);
				ref NativeArray<uint> ptr = ref this.penalty;
				ptr[dataIndex] += (uint)math.dot(this.channelPenalties, int2);
				this.walkable[dataIndex] = this.walkable[dataIndex] & !math.any(this.channelDeterminesWalkability & (int2 == 0));
			}

			// Token: 0x06000E24 RID: 3620 RVA: 0x000568F9 File Offset: 0x00054AF9
			public void Execute()
			{
				GridIterationUtilities.ForEachNode<RuleTexture.JobTexturePenalty>(this.bounds.size, this.nodeNormals, ref this);
			}

			// Token: 0x04000A39 RID: 2617
			[ReadOnly]
			public NativeArray<int> colorData;

			// Token: 0x04000A3A RID: 2618
			public NativeArray<uint> penalty;

			// Token: 0x04000A3B RID: 2619
			public NativeArray<bool> walkable;

			// Token: 0x04000A3C RID: 2620
			[ReadOnly]
			public NativeArray<float4> nodeNormals;

			// Token: 0x04000A3D RID: 2621
			public IntBounds bounds;

			// Token: 0x04000A3E RID: 2622
			public int2 colorDataSize;

			// Token: 0x04000A3F RID: 2623
			public float2 scale;

			// Token: 0x04000A40 RID: 2624
			public float4 channelPenalties;

			// Token: 0x04000A41 RID: 2625
			public bool4 channelDeterminesWalkability;
		}
	}
}
