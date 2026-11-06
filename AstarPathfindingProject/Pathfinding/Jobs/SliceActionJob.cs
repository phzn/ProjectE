using System;
using Pathfinding.Graphs.Grid;
using Unity.Burst;
using Unity.Jobs;

namespace Pathfinding.Jobs
{
	// Token: 0x0200017C RID: 380
	[BurstCompile]
	public struct SliceActionJob<T> : IJob where T : struct, GridIterationUtilities.ISliceAction
	{
		// Token: 0x06000BC0 RID: 3008 RVA: 0x000411F1 File Offset: 0x0003F3F1
		public void Execute()
		{
			GridIterationUtilities.ForEachCellIn3DSlice<T>(this.slice, ref this.action);
		}

		// Token: 0x040007AC RID: 1964
		public T action;

		// Token: 0x040007AD RID: 1965
		public Slice3D slice;
	}
}
