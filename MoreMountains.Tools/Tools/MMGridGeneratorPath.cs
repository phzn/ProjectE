using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200007D RID: 125
	public class MMGridGeneratorPath : MMGridGenerator
	{
		// Token: 0x060002DE RID: 734 RVA: 0x0000E714 File Offset: 0x0000C914
		public static int[,] Generate(int width, int height, int seed, MMGridGeneratorPath.Directions direction, Vector2Int startPosition, int pathMinWidth, int pathMaxWidth, int directionChangeDistance, int widthChangePercentage, int directionChangePercentage)
		{
			int[,] array = MMGridGenerator.PrepareGrid(ref width, ref height);
			array = MMGridGeneratorFull.Generate(width, height, true);
			Random random = new Random(seed);
			Random.InitState(seed);
			int num = 1;
			int x = startPosition.x;
			int y = startPosition.y;
			MMGridGenerator.SetGridCoordinate(array, x, y, 0);
			switch (direction)
			{
			case MMGridGeneratorPath.Directions.TopToBottom:
			{
				int num2 = x;
				for (int i = -num; i <= num; i++)
				{
					MMGridGenerator.SetGridCoordinate(array, num2 + i, y, 0);
				}
				for (int j = y; j > 0; j--)
				{
					num = MMGridGeneratorPath.ComputeWidth(random, widthChangePercentage, pathMinWidth, pathMaxWidth, num);
					num2 = MMGridGeneratorPath.DetermineNextStep(random, num2, directionChangeDistance, directionChangePercentage, pathMaxWidth, width);
					for (int k = -num; k <= num; k++)
					{
						MMGridGenerator.SetGridCoordinate(array, num2 + k, j, 0);
					}
				}
				break;
			}
			case MMGridGeneratorPath.Directions.BottomToTop:
			{
				int num3 = x;
				for (int l = -num; l <= num; l++)
				{
					MMGridGenerator.SetGridCoordinate(array, num3 + l, y, 0);
				}
				for (int m = y; m < height; m++)
				{
					num = MMGridGeneratorPath.ComputeWidth(random, widthChangePercentage, pathMinWidth, pathMaxWidth, num);
					num3 = MMGridGeneratorPath.DetermineNextStep(random, num3, directionChangeDistance, directionChangePercentage, pathMaxWidth, width);
					for (int n = -num; n <= num; n++)
					{
						MMGridGenerator.SetGridCoordinate(array, num3 + n, m, 0);
					}
				}
				break;
			}
			case MMGridGeneratorPath.Directions.LeftToRight:
			{
				int num4 = y;
				for (int num5 = -num; num5 <= num; num5++)
				{
					MMGridGenerator.SetGridCoordinate(array, x, num4 + num5, 0);
				}
				for (int num6 = x; num6 < width; num6++)
				{
					num = MMGridGeneratorPath.ComputeWidth(random, widthChangePercentage, pathMinWidth, pathMaxWidth, num);
					num4 = MMGridGeneratorPath.DetermineNextStep(random, num4, directionChangeDistance, directionChangePercentage, pathMaxWidth, width);
					for (int num7 = -num; num7 <= num; num7++)
					{
						MMGridGenerator.SetGridCoordinate(array, num6, num4 + num7, 0);
					}
				}
				break;
			}
			case MMGridGeneratorPath.Directions.RightToLeft:
			{
				int num8 = y;
				for (int num9 = -num; num9 <= num; num9++)
				{
					MMGridGenerator.SetGridCoordinate(array, x, num8 + num9, 0);
				}
				for (int num10 = x; num10 > 0; num10--)
				{
					num = MMGridGeneratorPath.ComputeWidth(random, widthChangePercentage, pathMinWidth, pathMaxWidth, num);
					num8 = MMGridGeneratorPath.DetermineNextStep(random, num8, directionChangeDistance, directionChangePercentage, pathMaxWidth, width);
					for (int num11 = -num; num11 <= num; num11++)
					{
						MMGridGenerator.SetGridCoordinate(array, num10, num8 + num11, 0);
					}
				}
				break;
			}
			}
			return array;
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000E954 File Offset: 0x0000CB54
		private static int ComputeWidth(Random random, int widthChangePercentage, int pathMinWidth, int pathMaxWidth, int pathWidth)
		{
			if (random.Next(0, 100) > widthChangePercentage)
			{
				int num = Random.Range(-pathMaxWidth, pathMaxWidth);
				pathWidth += num;
				if (pathWidth < pathMinWidth)
				{
					pathWidth = pathMinWidth;
				}
				if (pathWidth > pathMaxWidth)
				{
					pathWidth = pathMaxWidth;
				}
			}
			return pathWidth;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000E990 File Offset: 0x0000CB90
		private static int DetermineNextStep(Random random, int x, int directionChangeDistance, int directionChangePercentage, int pathMaxWidth, int width)
		{
			if (random.Next(0, 100) > directionChangePercentage)
			{
				int num = Random.Range(-directionChangeDistance, directionChangeDistance);
				x += num;
				if (x < pathMaxWidth)
				{
					x = pathMaxWidth;
				}
				if (x > width - pathMaxWidth)
				{
					x = width - pathMaxWidth;
				}
			}
			return x;
		}

		// Token: 0x020003E9 RID: 1001
		public enum Directions
		{
			// Token: 0x04001E63 RID: 7779
			TopToBottom,
			// Token: 0x04001E64 RID: 7780
			BottomToTop,
			// Token: 0x04001E65 RID: 7781
			LeftToRight,
			// Token: 0x04001E66 RID: 7782
			RightToLeft
		}
	}
}
