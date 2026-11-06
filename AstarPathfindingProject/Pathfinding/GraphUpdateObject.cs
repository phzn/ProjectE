using System;
using Pathfinding.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200002F RID: 47
	public class GraphUpdateObject
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000260 RID: 608 RVA: 0x0000B2F8 File Offset: 0x000094F8
		[Obsolete("Use the graphMask field instead")]
		public NNConstraintGraphUpdateObjectProxy nnConstraint
		{
			get
			{
				return new NNConstraintGraphUpdateObjectProxy(this);
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000261 RID: 609 RVA: 0x0000B300 File Offset: 0x00009500
		public GraphUpdateStage stage
		{
			get
			{
				switch (this.internalStage)
				{
				case -3:
					return GraphUpdateStage.Aborted;
				case -1:
					return GraphUpdateStage.Created;
				case 0:
					return GraphUpdateStage.Applied;
				}
				return GraphUpdateStage.Pending;
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00003788 File Offset: 0x00001988
		public virtual void WillUpdateNode(GraphNode node)
		{
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00003788 File Offset: 0x00001988
		[Obsolete("Use AstarPath.Snapshot instead", true)]
		public virtual void RevertFromBackup()
		{
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000B338 File Offset: 0x00009538
		public virtual void Apply(GraphNode node)
		{
			if (this.shape == null || this.shape.Contains(node))
			{
				node.Penalty = (uint)((ulong)node.Penalty + (ulong)((long)this.addPenalty));
				if (this.modifyWalkability)
				{
					node.Walkable = this.setWalkability;
				}
				if (this.modifyTag)
				{
					node.Tag = this.setTag;
				}
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000B3A0 File Offset: 0x000095A0
		public virtual void ApplyJob(GraphUpdateObject.GraphUpdateData data, JobDependencyTracker dependencyTracker)
		{
			if (this.addPenalty == 0 && !this.modifyWalkability && !this.modifyTag)
			{
				return;
			}
			new GraphUpdateObject.JobGraphUpdate
			{
				shape = ((this.shape != null) ? new GraphUpdateShape.BurstShape(this.shape, Allocator.Persistent) : GraphUpdateShape.BurstShape.Everything),
				data = data,
				bounds = this.bounds,
				penaltyDelta = this.addPenalty,
				modifyWalkability = this.modifyWalkability,
				walkabilityValue = this.setWalkability,
				modifyTag = this.modifyTag,
				tagValue = (int)this.setTag.value
			}.Schedule(dependencyTracker);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000B453 File Offset: 0x00009653
		public GraphUpdateObject()
		{
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000B482 File Offset: 0x00009682
		public GraphUpdateObject(Bounds b)
		{
			this.bounds = b;
		}

		// Token: 0x04000166 RID: 358
		public Bounds bounds;

		// Token: 0x04000167 RID: 359
		public bool updatePhysics = true;

		// Token: 0x04000168 RID: 360
		public bool resetPenaltyOnPhysics = true;

		// Token: 0x04000169 RID: 361
		public bool updateErosion = true;

		// Token: 0x0400016A RID: 362
		public GraphMask graphMask = GraphMask.everything;

		// Token: 0x0400016B RID: 363
		public int addPenalty;

		// Token: 0x0400016C RID: 364
		public bool modifyWalkability;

		// Token: 0x0400016D RID: 365
		public bool setWalkability;

		// Token: 0x0400016E RID: 366
		public bool modifyTag;

		// Token: 0x0400016F RID: 367
		public PathfindingTag setTag;

		// Token: 0x04000170 RID: 368
		[Obsolete("This field does not do anything anymore. Use AstarPath.Snapshot instead.")]
		public bool trackChangedNodes;

		// Token: 0x04000171 RID: 369
		public GraphUpdateShape shape;

		// Token: 0x04000172 RID: 370
		internal int internalStage = -1;

		// Token: 0x04000173 RID: 371
		internal const int STAGE_CREATED = -1;

		// Token: 0x04000174 RID: 372
		internal const int STAGE_PENDING = -2;

		// Token: 0x04000175 RID: 373
		internal const int STAGE_ABORTED = -3;

		// Token: 0x04000176 RID: 374
		internal const int STAGE_APPLIED = 0;

		// Token: 0x02000030 RID: 48
		public struct GraphUpdateData
		{
			// Token: 0x04000177 RID: 375
			public NativeArray<Vector3> nodePositions;

			// Token: 0x04000178 RID: 376
			public NativeArray<uint> nodePenalties;

			// Token: 0x04000179 RID: 377
			public NativeArray<bool> nodeWalkable;

			// Token: 0x0400017A RID: 378
			public NativeArray<int> nodeTags;

			// Token: 0x0400017B RID: 379
			public NativeArray<float4> nodeNormals;

			// Token: 0x0400017C RID: 380
			public NativeArray<int> nodeIndices;
		}

		// Token: 0x02000031 RID: 49
		[BurstCompile]
		public struct JobGraphUpdate : IJob
		{
			// Token: 0x06000268 RID: 616 RVA: 0x0000B4B8 File Offset: 0x000096B8
			public void Execute()
			{
				for (int i = 0; i < this.data.nodeIndices.Length; i++)
				{
					int num = this.data.nodeIndices[i];
					if (math.any(this.data.nodeNormals[num]) && this.bounds.Contains(this.data.nodePositions[num]) && this.shape.Contains(this.data.nodePositions[num]))
					{
						ref NativeArray<uint> ptr = ref this.data.nodePenalties;
						int num2 = num;
						ptr[num2] += (uint)this.penaltyDelta;
						if (this.modifyWalkability)
						{
							this.data.nodeWalkable[num] = this.walkabilityValue;
						}
						if (this.modifyTag)
						{
							this.data.nodeTags[num] = this.tagValue;
						}
					}
				}
			}

			// Token: 0x0400017D RID: 381
			public GraphUpdateShape.BurstShape shape;

			// Token: 0x0400017E RID: 382
			public GraphUpdateObject.GraphUpdateData data;

			// Token: 0x0400017F RID: 383
			public Bounds bounds;

			// Token: 0x04000180 RID: 384
			public int penaltyDelta;

			// Token: 0x04000181 RID: 385
			public bool modifyWalkability;

			// Token: 0x04000182 RID: 386
			public bool walkabilityValue;

			// Token: 0x04000183 RID: 387
			public bool modifyTag;

			// Token: 0x04000184 RID: 388
			public int tagValue;
		}
	}
}
