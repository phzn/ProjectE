using System;
using System.Runtime.InteropServices;
using Pathfinding.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x02000237 RID: 567
	public struct JobWriteNodeData : IJobParallelForBatched
	{
		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000E49 RID: 3657 RVA: 0x0001AA07 File Offset: 0x00018C07
		public bool allowBoundsChecks
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x000587FC File Offset: 0x000569FC
		public void Execute(int startIndex, int count)
		{
			GridNodeBase[] array = (GridNodeBase[])this.nodesHandle.Target;
			IntBounds intBounds = this.writeMask.Offset(-this.dataBounds.min);
			int3 size = this.writeMask.size;
			int num = startIndex / (size.x * size.y);
			int num2 = (startIndex + count) / (size.x * size.y);
			intBounds.min.z = this.writeMask.min.z + num - this.dataBounds.min.z;
			intBounds.max.z = this.writeMask.min.z + num2 - this.dataBounds.min.z;
			int3 size2 = this.dataBounds.size;
			for (int i = intBounds.min.y; i < intBounds.max.y; i++)
			{
				for (int j = intBounds.min.z; j < intBounds.max.z; j++)
				{
					int num3 = (i * size2.z + j) * size2.x;
					int num4 = (j + this.dataBounds.min.z) * this.nodeArrayBounds.x + this.dataBounds.min.x;
					int num5 = (i + this.dataBounds.min.y) * this.nodeArrayBounds.z * this.nodeArrayBounds.x + num4;
					for (int k = intBounds.min.x; k < intBounds.max.x; k++)
					{
						int num6 = num3 + k;
						int num7 = num5 + k;
						GridNodeBase gridNodeBase = array[num7];
						if (gridNodeBase != null)
						{
							gridNodeBase.GraphIndex = this.graphIndex;
							gridNodeBase.NodeInGridIndex = num4 + k;
							gridNodeBase.position = (Int3)this.nodePositions[num6];
							gridNodeBase.Penalty = this.nodePenalties[num6];
							gridNodeBase.Tag = (uint)this.nodeTags[num6];
							GridNode gridNode = gridNodeBase as GridNode;
							if (gridNode != null)
							{
								gridNode.SetAllConnectionInternal((int)this.nodeConnections[num6]);
							}
							else
							{
								LevelGridNode levelGridNode = gridNodeBase as LevelGridNode;
								if (levelGridNode != null)
								{
									levelGridNode.LayerCoordinateInGrid = i + this.dataBounds.min.y;
									levelGridNode.SetAllConnectionInternal(this.nodeConnections[num6]);
								}
							}
							gridNodeBase.Walkable = this.nodeWalkableWithErosion[num6];
							gridNodeBase.WalkableErosion = this.nodeWalkable[num6];
						}
					}
				}
			}
		}

		// Token: 0x04000ABB RID: 2747
		public GCHandle nodesHandle;

		// Token: 0x04000ABC RID: 2748
		public uint graphIndex;

		// Token: 0x04000ABD RID: 2749
		public int3 nodeArrayBounds;

		// Token: 0x04000ABE RID: 2750
		public IntBounds dataBounds;

		// Token: 0x04000ABF RID: 2751
		public IntBounds writeMask;

		// Token: 0x04000AC0 RID: 2752
		[ReadOnly]
		public NativeArray<Vector3> nodePositions;

		// Token: 0x04000AC1 RID: 2753
		[ReadOnly]
		public NativeArray<uint> nodePenalties;

		// Token: 0x04000AC2 RID: 2754
		[ReadOnly]
		public NativeArray<int> nodeTags;

		// Token: 0x04000AC3 RID: 2755
		[ReadOnly]
		public NativeArray<ulong> nodeConnections;

		// Token: 0x04000AC4 RID: 2756
		[ReadOnly]
		public NativeArray<bool> nodeWalkableWithErosion;

		// Token: 0x04000AC5 RID: 2757
		[ReadOnly]
		public NativeArray<bool> nodeWalkable;
	}
}
