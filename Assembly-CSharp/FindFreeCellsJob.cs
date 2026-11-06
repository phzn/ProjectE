using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

// Token: 0x0200007D RID: 125
[BurstCompile(CompileSynchronously = true)]
public struct FindFreeCellsJob : IJobParallelFor
{
	// Token: 0x060005CF RID: 1487 RVA: 0x0002A8C0 File Offset: 0x00028AC0
	public void Execute(int index)
	{
		int num = index % this.gridWidth - this.radius;
		int num2 = index / this.gridWidth - this.radius;
		int2 @int = this.playerCell + new int2(num, num2);
		bool flag = this.occupiedCells.Contains(@int);
		float num3 = (float)(num * num + num2 * num2);
		this.results[index] = new FreeCellResult
		{
			position = @int,
			distanceSqr = num3,
			isValid = !flag
		};
	}

	// Token: 0x04000470 RID: 1136
	[ReadOnly]
	public int2 playerCell;

	// Token: 0x04000471 RID: 1137
	[ReadOnly]
	public int radius;

	// Token: 0x04000472 RID: 1138
	[ReadOnly]
	public int gridWidth;

	// Token: 0x04000473 RID: 1139
	[ReadOnly]
	public NativeHashSet<int2> occupiedCells;

	// Token: 0x04000474 RID: 1140
	[WriteOnly]
	public NativeArray<FreeCellResult> results;
}
