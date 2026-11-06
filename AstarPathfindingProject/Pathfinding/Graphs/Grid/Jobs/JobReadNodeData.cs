using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Pathfinding.Jobs;
using Unity.Collections;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x02000234 RID: 564
	public struct JobReadNodeData : IJobParallelForBatched
	{
		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000E44 RID: 3652 RVA: 0x0001AA07 File Offset: 0x00018C07
		public bool allowBoundsChecks
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x00058608 File Offset: 0x00056808
		public void Execute(int startIndex, int count)
		{
			JobReadNodeData.Reader reader = new JobReadNodeData.Reader
			{
				nodes = (GridNodeBase[])this.nodesHandle.Target,
				nodePositions = this.nodePositions,
				nodePenalties = this.nodePenalties,
				nodeTags = this.nodeTags,
				nodeConnections = this.nodeConnections,
				nodeWalkableWithErosion = this.nodeWalkableWithErosion,
				nodeWalkable = this.nodeWalkable
			};
			GridIterationUtilities.ForEachCellIn3DSlice<JobReadNodeData.Reader>(this.slice, ref reader);
		}

		// Token: 0x04000AA7 RID: 2727
		public GCHandle nodesHandle;

		// Token: 0x04000AA8 RID: 2728
		public uint graphIndex;

		// Token: 0x04000AA9 RID: 2729
		public Slice3D slice;

		// Token: 0x04000AAA RID: 2730
		[WriteOnly]
		public NativeArray<Vector3> nodePositions;

		// Token: 0x04000AAB RID: 2731
		[WriteOnly]
		public NativeArray<uint> nodePenalties;

		// Token: 0x04000AAC RID: 2732
		[WriteOnly]
		public NativeArray<int> nodeTags;

		// Token: 0x04000AAD RID: 2733
		[WriteOnly]
		public NativeArray<ulong> nodeConnections;

		// Token: 0x04000AAE RID: 2734
		[WriteOnly]
		public NativeArray<bool> nodeWalkableWithErosion;

		// Token: 0x04000AAF RID: 2735
		[WriteOnly]
		public NativeArray<bool> nodeWalkable;

		// Token: 0x02000235 RID: 565
		private struct Reader : GridIterationUtilities.ISliceAction
		{
			// Token: 0x06000E46 RID: 3654 RVA: 0x00058694 File Offset: 0x00056894
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Execute(uint outerIdx, uint innerIdx)
			{
				if ((ulong)outerIdx < (ulong)((long)this.nodes.Length))
				{
					GridNodeBase gridNodeBase = this.nodes[(int)outerIdx];
					if (gridNodeBase != null)
					{
						this.nodePositions[(int)innerIdx] = (Vector3)gridNodeBase.position;
						this.nodePenalties[(int)innerIdx] = gridNodeBase.Penalty;
						this.nodeTags[(int)innerIdx] = (int)gridNodeBase.Tag;
						GridNode gridNode = gridNodeBase as GridNode;
						this.nodeConnections[(int)innerIdx] = (ulong)((gridNode != null) ? ((long)gridNode.GetAllConnectionInternal()) : ((long)((ulong)(gridNodeBase as LevelGridNode).gridConnections)));
						this.nodeWalkableWithErosion[(int)innerIdx] = gridNodeBase.Walkable;
						this.nodeWalkable[(int)innerIdx] = gridNodeBase.WalkableErosion;
						return;
					}
				}
				this.nodePositions[(int)innerIdx] = Vector3.zero;
				this.nodePenalties[(int)innerIdx] = 0U;
				this.nodeTags[(int)innerIdx] = 0;
				this.nodeConnections[(int)innerIdx] = 0UL;
				this.nodeWalkableWithErosion[(int)innerIdx] = false;
				this.nodeWalkable[(int)innerIdx] = false;
			}

			// Token: 0x04000AB0 RID: 2736
			public GridNodeBase[] nodes;

			// Token: 0x04000AB1 RID: 2737
			public NativeArray<Vector3> nodePositions;

			// Token: 0x04000AB2 RID: 2738
			public NativeArray<uint> nodePenalties;

			// Token: 0x04000AB3 RID: 2739
			public NativeArray<int> nodeTags;

			// Token: 0x04000AB4 RID: 2740
			public NativeArray<ulong> nodeConnections;

			// Token: 0x04000AB5 RID: 2741
			public NativeArray<bool> nodeWalkableWithErosion;

			// Token: 0x04000AB6 RID: 2742
			public NativeArray<bool> nodeWalkable;
		}
	}
}
