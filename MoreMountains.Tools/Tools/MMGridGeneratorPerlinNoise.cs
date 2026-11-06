using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200007E RID: 126
	public class MMGridGeneratorPerlinNoise : MMGridGenerator
	{
		// Token: 0x060002E2 RID: 738 RVA: 0x0000E9D8 File Offset: 0x0000CBD8
		public static int[,] Generate(int width, int height, float seed)
		{
			int[,] array = MMGridGenerator.PrepareGrid(ref width, ref height);
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					int num = Mathf.RoundToInt(Mathf.PerlinNoise((float)i * seed, (float)j * seed));
					MMGridGenerator.SetGridCoordinate(array, i, j, num);
				}
			}
			return array;
		}
	}
}
