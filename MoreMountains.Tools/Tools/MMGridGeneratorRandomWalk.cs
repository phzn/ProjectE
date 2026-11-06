using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000081 RID: 129
	public class MMGridGeneratorRandomWalk : MMGridGenerator
	{
		// Token: 0x060002E8 RID: 744 RVA: 0x0000EAF4 File Offset: 0x0000CCF4
		public static int[,] Generate(int width, int height, int seed, int fillPercentage, Vector2Int startingPoint, int maxIterations)
		{
			int[,] array = MMGridGenerator.PrepareGrid(ref width, ref height);
			array = MMGridGeneratorFull.Generate(width, height, true);
			Random random = new Random(seed);
			int num = width * height * fillPercentage / 100;
			int num2 = 0;
			int num3 = startingPoint.x;
			int num4 = startingPoint.y;
			array[num3, num4] = 0;
			num2++;
			int num5 = 0;
			while (num2 < num && num5 < maxIterations)
			{
				switch (random.Next(4))
				{
				case 0:
					if (num4 + 1 < height)
					{
						num4++;
						array = MMGridGeneratorRandomWalk.Carve(array, num3, num4, ref num2);
					}
					break;
				case 1:
					if (num4 - 1 > 1)
					{
						num4--;
						array = MMGridGeneratorRandomWalk.Carve(array, num3, num4, ref num2);
					}
					break;
				case 2:
					if (num3 - 1 > 1)
					{
						num3--;
						array = MMGridGeneratorRandomWalk.Carve(array, num3, num4, ref num2);
					}
					break;
				case 3:
					if (num3 + 1 < width)
					{
						num3++;
						array = MMGridGeneratorRandomWalk.Carve(array, num3, num4, ref num2);
					}
					break;
				}
				num5++;
			}
			return array;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000EBF3 File Offset: 0x0000CDF3
		private static int[,] Carve(int[,] grid, int x, int y, ref int fillCounter)
		{
			if (grid[x, y] == 1)
			{
				grid[x, y] = 0;
				fillCounter++;
			}
			return grid;
		}
	}
}
