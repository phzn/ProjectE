using System;
using Pathfinding.Graphs.Grid;
using Unity.Collections;

namespace Pathfinding.Jobs
{
	// Token: 0x0200017D RID: 381
	public struct JobAND : GridIterationUtilities.ISliceAction
	{
		// Token: 0x06000BC1 RID: 3009 RVA: 0x00041204 File Offset: 0x0003F404
		public void Execute(uint outerIdx, uint innerIdx)
		{
			ref NativeArray<bool> ptr = ref this.result;
			ptr[(int)outerIdx] = ptr[(int)outerIdx] & this.data[(int)outerIdx];
		}

		// Token: 0x040007AE RID: 1966
		public NativeArray<bool> result;

		// Token: 0x040007AF RID: 1967
		[ReadOnly]
		public NativeArray<bool> data;
	}
}
