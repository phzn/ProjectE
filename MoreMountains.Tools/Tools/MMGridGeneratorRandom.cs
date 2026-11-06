using System;

namespace MoreMountains.Tools
{
	// Token: 0x02000080 RID: 128
	public class MMGridGeneratorRandom : MMGridGenerator
	{
		// Token: 0x060002E6 RID: 742 RVA: 0x0000EA8C File Offset: 0x0000CC8C
		public static int[,] Generate(int width, int height, int seed, int fillPercentage)
		{
			int[,] array = MMGridGenerator.PrepareGrid(ref width, ref height);
			array = MMGridGeneratorFull.Generate(width, height, true);
			Random random = new Random(seed);
			for (int i = 0; i <= width; i++)
			{
				for (int j = 0; j <= height; j++)
				{
					int num = ((random.Next(0, 100) < fillPercentage) ? 1 : 0);
					MMGridGenerator.SetGridCoordinate(array, i, j, num);
				}
			}
			return array;
		}
	}
}
