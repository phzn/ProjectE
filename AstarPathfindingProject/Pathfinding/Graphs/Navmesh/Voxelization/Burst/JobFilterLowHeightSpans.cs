using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001EE RID: 494
	[BurstCompile(CompileSynchronously = true)]
	internal struct JobFilterLowHeightSpans : IJob
	{
		// Token: 0x06000D8F RID: 3471 RVA: 0x00050578 File Offset: 0x0004E778
		public void Execute()
		{
			int num = this.field.width * this.field.depth;
			NativeList<LinkedVoxelSpan> linkedSpans = this.field.linkedSpans;
			int i = 0;
			int num2 = 0;
			while (i < num)
			{
				for (int j = 0; j < this.field.width; j++)
				{
					int num3 = i + j;
					while (num3 != -1 && linkedSpans[num3].bottom != 4294967295U)
					{
						uint top = linkedSpans[num3].top;
						if (((linkedSpans[num3].next != -1) ? linkedSpans[linkedSpans[num3].next].bottom : 65536U) - top < this.voxelWalkableHeight)
						{
							LinkedVoxelSpan linkedVoxelSpan = linkedSpans[num3];
							linkedVoxelSpan.area = 0;
							linkedSpans[num3] = linkedVoxelSpan;
						}
						num3 = linkedSpans[num3].next;
					}
				}
				i += this.field.width;
				num2++;
			}
		}

		// Token: 0x04000941 RID: 2369
		public LinkedVoxelField field;

		// Token: 0x04000942 RID: 2370
		public uint voxelWalkableHeight;
	}
}
