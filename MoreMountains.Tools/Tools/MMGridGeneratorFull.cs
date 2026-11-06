using System;

namespace MoreMountains.Tools
{
	// Token: 0x0200007C RID: 124
	public class MMGridGeneratorFull : MMGridGenerator
	{
		// Token: 0x060002DC RID: 732 RVA: 0x0000E6CC File Offset: 0x0000C8CC
		public static int[,] Generate(int width, int height, bool full)
		{
			int[,] array = MMGridGenerator.PrepareGrid(ref width, ref height);
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					MMGridGenerator.SetGridCoordinate(array, i, j, full ? 1 : 0);
				}
			}
			return array;
		}
	}
}
