using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000083 RID: 131
	public class MMGridGeneratorRandomWalkGround : MMGridGenerator
	{
		// Token: 0x060002EF RID: 751 RVA: 0x0000ED98 File Offset: 0x0000CF98
		public static int[,] Generate(int width, int height, int seed, int minHeightDifference, int maxHeightDifference, int minFlatDistance, int maxFlatDistance, int maxHeight)
		{
			Random random = new Random(seed.GetHashCode());
			Random.InitState(seed);
			int[,] array = MMGridGenerator.PrepareGrid(ref width, ref height);
			int num = Random.Range(0, maxHeight);
			int num2 = num;
			int num3 = -1;
			for (int i = 0; i < width; i++)
			{
				num = num2;
				int num4 = Random.Range(minHeightDifference, maxHeightDifference);
				int num5 = Random.Range(minFlatDistance, maxFlatDistance);
				if (num3 >= num5 - 1)
				{
					if (random.Next(2) > 0)
					{
						num -= num4;
					}
					else if (num2 + num4 < height)
					{
						num += num4;
					}
					num = Mathf.Clamp(num, 1, maxHeight);
					num3 = 0;
				}
				else
				{
					num3++;
				}
				for (int j = num; j >= 0; j--)
				{
					array[i, j] = 1;
				}
				num2 = num;
			}
			return array;
		}
	}
}
