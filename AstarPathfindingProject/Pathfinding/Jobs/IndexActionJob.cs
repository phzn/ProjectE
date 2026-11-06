using System;
using Pathfinding.Graphs.Grid;
using Unity.Burst;
using Unity.Jobs;

namespace Pathfinding.Jobs
{
	// Token: 0x0200017B RID: 379
	[BurstCompile]
	public struct IndexActionJob<T> : IJob where T : struct, GridIterationUtilities.ISliceAction
	{
		// Token: 0x06000BBF RID: 3007 RVA: 0x000411C0 File Offset: 0x0003F3C0
		public void Execute()
		{
			for (int i = 0; i < this.length; i++)
			{
				this.action.Execute((uint)i, (uint)i);
			}
		}

		// Token: 0x040007AA RID: 1962
		public T action;

		// Token: 0x040007AB RID: 1963
		public int length;
	}
}
