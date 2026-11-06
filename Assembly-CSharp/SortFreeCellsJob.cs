using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

// Token: 0x0200007E RID: 126
[BurstCompile(CompileSynchronously = true)]
public struct SortFreeCellsJob : IJob
{
	// Token: 0x060005D0 RID: 1488 RVA: 0x0002A94C File Offset: 0x00028B4C
	public void Execute()
	{
		this.sortedCells.Clear();
		int num = 0;
		for (int i = 0; i < this.cells.Length; i++)
		{
			if (this.cells[i].isValid)
			{
				num++;
			}
		}
		if (num == 0)
		{
			return;
		}
		int num2 = math.min(this.maxResults, num);
		for (int j = 0; j < num2; j++)
		{
			int num3 = -1;
			float num4 = float.MaxValue;
			for (int k = 0; k < this.cells.Length; k++)
			{
				FreeCellResult freeCellResult = this.cells[k];
				if (freeCellResult.isValid && freeCellResult.distanceSqr < num4)
				{
					num4 = freeCellResult.distanceSqr;
					num3 = k;
				}
			}
			if (num3 >= 0)
			{
				this.sortedCells.Add(in this.cells[num3].position);
				FreeCellResult freeCellResult2 = this.cells[num3];
				freeCellResult2.isValid = false;
				this.cells[num3] = freeCellResult2;
			}
		}
	}

	// Token: 0x04000475 RID: 1141
	public NativeArray<FreeCellResult> cells;

	// Token: 0x04000476 RID: 1142
	[ReadOnly]
	public int maxResults;

	// Token: 0x04000477 RID: 1143
	public NativeList<int2> sortedCells;
}
