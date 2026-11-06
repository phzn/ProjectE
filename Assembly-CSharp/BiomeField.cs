using System;
using UnityEngine;

// Token: 0x02000030 RID: 48
public static class BiomeField
{
	// Token: 0x060001A9 RID: 425 RVA: 0x0000BF14 File Offset: 0x0000A114
	public static BiomeConfig GetBiomeForCell(BiomeManager manager, int sessionSeed, Vector2Int cell)
	{
		if (manager == null || manager.biomes == null || manager.biomes.Count == 0)
		{
			return null;
		}
		if (cell.x * cell.x + cell.y * cell.y <= 4)
		{
			if (!(manager.startingBiome != null))
			{
				return manager.biomes[0];
			}
			return manager.startingBiome;
		}
		else
		{
			int num = Mathf.Max(2, (manager.districtSizeCells > 0) ? manager.districtSizeCells : 3);
			int num2 = BiomeField.FloorDiv(cell.x, num);
			int num3 = BiomeField.FloorDiv(cell.y, num);
			BiomeConfig biomeConfig = null;
			float num4 = float.MaxValue;
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					int num5 = num2 + i;
					int num6 = num3 + j;
					uint num7 = BiomeField.Hash((uint)(sessionSeed ^ -1640531527), (uint)num5, (uint)num6);
					float num8 = ((float)num5 + BiomeField.HashToFloat(num7)) * (float)num;
					num7 = BiomeField.Advance(num7);
					float num9 = ((float)num6 + BiomeField.HashToFloat(num7)) * (float)num;
					num7 = BiomeField.Advance(num7);
					float num10 = num8 - (float)cell.x;
					float num11 = num9 - (float)cell.y;
					float num12 = num10 * num10 + num11 * num11;
					if (num12 < num4)
					{
						num4 = num12;
						biomeConfig = BiomeField.PickWeightedBiome(manager, BiomeField.HashToFloat(num7));
					}
				}
			}
			if (!(biomeConfig != null))
			{
				return manager.startingBiome;
			}
			return biomeConfig;
		}
	}

	// Token: 0x060001AA RID: 426 RVA: 0x0000C088 File Offset: 0x0000A288
	private static BiomeConfig PickWeightedBiome(BiomeManager manager, float roll01)
	{
		float num = 0f;
		for (int i = 0; i < manager.biomes.Count; i++)
		{
			BiomeConfig biomeConfig = manager.biomes[i];
			if (biomeConfig != null)
			{
				num += Mathf.Max(0f, biomeConfig.baseSpawnWeight);
			}
		}
		if (num <= 0f)
		{
			return manager.startingBiome;
		}
		float num2 = roll01 * num;
		float num3 = 0f;
		BiomeConfig biomeConfig2 = null;
		for (int j = 0; j < manager.biomes.Count; j++)
		{
			BiomeConfig biomeConfig3 = manager.biomes[j];
			if (!(biomeConfig3 == null))
			{
				biomeConfig2 = biomeConfig3;
				num3 += Mathf.Max(0f, biomeConfig3.baseSpawnWeight);
				if (num2 <= num3)
				{
					return biomeConfig3;
				}
			}
		}
		return biomeConfig2;
	}

	// Token: 0x060001AB RID: 427 RVA: 0x0000C150 File Offset: 0x0000A350
	private static int FloorDiv(int a, int b)
	{
		int num = a / b;
		if (a % b != 0 && a < 0 != b < 0)
		{
			num--;
		}
		return num;
	}

	// Token: 0x060001AC RID: 428 RVA: 0x0000C178 File Offset: 0x0000A378
	private static uint Hash(uint seed, uint x, uint y)
	{
		uint num = seed ^ (x * 2654435761U);
		num = (num << 13) | (num >> 19);
		num *= 1540483477U;
		num ^= y * 2246822519U;
		num = (num << 17) | (num >> 15);
		num *= 2654435761U;
		num ^= num >> 16;
		num *= 73244475U;
		return num ^ (num >> 16);
	}

	// Token: 0x060001AD RID: 429 RVA: 0x0000C1D6 File Offset: 0x0000A3D6
	private static uint Advance(uint h)
	{
		h ^= h >> 16;
		h *= 73244475U;
		h ^= h >> 16;
		return h;
	}

	// Token: 0x060001AE RID: 430 RVA: 0x0000C1F2 File Offset: 0x0000A3F2
	private static float HashToFloat(uint h)
	{
		return (h & 16777215U) / 16777216f;
	}

	// Token: 0x04000163 RID: 355
	public const int DistrictSizeCells = 3;

	// Token: 0x04000164 RID: 356
	public const int StartRadiusCells = 2;

	// Token: 0x04000165 RID: 357
	private const uint FieldSalt = 2654435769U;
}
