using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Pathfinding.Graphs.Navmesh.Jobs
{
	// Token: 0x020001FD RID: 509
	[BurstCompile]
	public struct JobConvertAreasToTags : IJob
	{
		// Token: 0x06000DAE RID: 3502 RVA: 0x00053238 File Offset: 0x00051438
		public void Execute()
		{
			for (int i = 0; i < this.areas.Length; i++)
			{
				int num = this.areas[i];
				this.areas[i] = (((num & 16384) != 0) ? ((num & 16383) - 1) : 0);
			}
		}

		// Token: 0x040009B0 RID: 2480
		public NativeList<int> areas;
	}
}
