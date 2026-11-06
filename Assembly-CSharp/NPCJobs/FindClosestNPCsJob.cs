using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace NPCJobs
{
	// Token: 0x02000733 RID: 1843
	[BurstCompile(CompileSynchronously = true)]
	public struct FindClosestNPCsJob : IJob
	{
		// Token: 0x06002FA0 RID: 12192 RVA: 0x0010DC24 File Offset: 0x0010BE24
		public void Execute()
		{
			this.closestIndices.Clear();
			NativeArray<float> nativeArray = new NativeArray<float>(this.zoneResults.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<int> nativeArray2 = new NativeArray<int>(this.zoneResults.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
			for (int i = 0; i < this.zoneResults.Length; i++)
			{
				nativeArray[i] = this.zoneResults[i].distanceSqr;
				nativeArray2[i] = i;
			}
			int num = math.min(this.maxResults, this.zoneResults.Length);
			for (int j = 0; j < num; j++)
			{
				int num2 = j;
				float num3 = nativeArray[j];
				for (int k = j + 1; k < this.zoneResults.Length; k++)
				{
					if (nativeArray[k] < num3)
					{
						num3 = nativeArray[k];
						num2 = k;
					}
				}
				if (num2 != j)
				{
					float num4 = nativeArray[j];
					nativeArray[j] = nativeArray[num2];
					nativeArray[num2] = num4;
					int num5 = nativeArray2[j];
					nativeArray2[j] = nativeArray2[num2];
					nativeArray2[num2] = num5;
				}
				int num6 = nativeArray2[j];
				this.closestIndices.Add(in num6);
			}
			nativeArray.Dispose();
			nativeArray2.Dispose();
		}

		// Token: 0x04003027 RID: 12327
		[ReadOnly]
		public NativeArray<NPCZoneResult> zoneResults;

		// Token: 0x04003028 RID: 12328
		[ReadOnly]
		public int maxResults;

		// Token: 0x04003029 RID: 12329
		public NativeList<int> closestIndices;
	}
}
