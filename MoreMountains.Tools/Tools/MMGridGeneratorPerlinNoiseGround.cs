using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200007F RID: 127
	public class MMGridGeneratorPerlinNoiseGround : MMGridGenerator
	{
		// Token: 0x060002E4 RID: 740 RVA: 0x0000EA30 File Offset: 0x0000CC30
		public static int[,] Generate(int width, int height, float seed)
		{
			int[,] array = MMGridGenerator.PrepareGrid(ref width, ref height);
			for (int i = 0; i < width; i++)
			{
				for (int j = Mathf.FloorToInt((Mathf.PerlinNoise((float)i, seed) - 0.5f) * (float)height) + height / 2; j >= 0; j--)
				{
					MMGridGenerator.SetGridCoordinate(array, i, j, 1);
				}
			}
			return array;
		}
	}
}
