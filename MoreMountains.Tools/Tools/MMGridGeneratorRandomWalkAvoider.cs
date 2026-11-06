using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000082 RID: 130
	public class MMGridGeneratorRandomWalkAvoider : MMGridGenerator
	{
		// Token: 0x060002EB RID: 747 RVA: 0x0000EC18 File Offset: 0x0000CE18
		public static int[,] Generate(int width, int height, int seed, int fillPercentage, Vector2Int startingPoint, int[,] obstacles, int obstacleDistance, int maxIterations)
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
					if (num4 + 1 <= height && !MMGridGeneratorRandomWalkAvoider.ObstacleAt(obstacles, num3, num4 + obstacleDistance))
					{
						num4++;
						array = MMGridGeneratorRandomWalkAvoider.Carve(array, num3, num4, ref num2);
					}
					break;
				case 1:
					if (num4 - 1 > 1 && !MMGridGeneratorRandomWalkAvoider.ObstacleAt(obstacles, num3, num4 - obstacleDistance))
					{
						num4--;
						array = MMGridGeneratorRandomWalkAvoider.Carve(array, num3, num4, ref num2);
					}
					break;
				case 2:
					if (num3 - 1 > 1 && !MMGridGeneratorRandomWalkAvoider.ObstacleAt(obstacles, num3 - obstacleDistance, num4))
					{
						num3--;
						array = MMGridGeneratorRandomWalkAvoider.Carve(array, num3, num4, ref num2);
					}
					break;
				case 3:
					if (num3 + 1 <= width && !MMGridGeneratorRandomWalkAvoider.ObstacleAt(obstacles, num3 + obstacleDistance, num4))
					{
						num3++;
						array = MMGridGeneratorRandomWalkAvoider.Carve(array, num3, num4, ref num2);
					}
					break;
				}
				num5++;
			}
			return array;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000ED63 File Offset: 0x0000CF63
		private static bool ObstacleAt(int[,] obstacles, int x, int y)
		{
			return MMGridGenerator.GetValueAtGridCoordinate(obstacles, x, y, 1) == 1;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000ED71 File Offset: 0x0000CF71
		private static int[,] Carve(int[,] grid, int x, int y, ref int fillCounter)
		{
			if (MMGridGenerator.GetValueAtGridCoordinate(grid, x, y, 0) == 1)
			{
				MMGridGenerator.SetGridCoordinate(grid, x, y, 0);
				fillCounter++;
			}
			return grid;
		}
	}
}
