using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000031 RID: 49
[CreateAssetMenu(fileName = "BiomeManager", menuName = "MapGeneration/BiomeManager", order = 11)]
public class BiomeManager : ScriptableObject
{
	// Token: 0x060001AF RID: 431 RVA: 0x0000C204 File Offset: 0x0000A404
	public BiomeConfig SelectBiome(float distanceFromStart)
	{
		if (this.biomes == null || this.biomes.Count == 0)
		{
			Debug.LogError("[BiomeManager] Nessun bioma configurato! Usa startingBiome come fallback.");
			return this.startingBiome;
		}
		List<ValueTuple<BiomeConfig, float>> list = new List<ValueTuple<BiomeConfig, float>>();
		foreach (BiomeConfig biomeConfig in this.biomes)
		{
			if (!(biomeConfig == null))
			{
				float spawnWeightAtDistance = biomeConfig.GetSpawnWeightAtDistance(distanceFromStart);
				if (spawnWeightAtDistance > 0f)
				{
					list.Add(new ValueTuple<BiomeConfig, float>(biomeConfig, spawnWeightAtDistance));
					if (this.enableDebugLogs)
					{
						Debug.Log(string.Format("[BiomeManager] Bioma '{0}' @ {1}m → peso: {2:F2}", biomeConfig.biomeName, distanceFromStart, spawnWeightAtDistance));
					}
				}
			}
		}
		if (list.Count == 0)
		{
			if (this.enableDebugLogs)
			{
				Debug.LogWarning(string.Format("[BiomeManager] Nessun bioma disponibile @ {0}m, uso startingBiome '{1}'", distanceFromStart, this.startingBiome.biomeName));
			}
			return this.startingBiome;
		}
		float num = list.Sum<ValueTuple<BiomeConfig, float>>(([TupleElementNames(new string[] { "biome", "weight" })] ValueTuple<BiomeConfig, float> x) => x.Item2);
		float num2 = Random.Range(0f, num);
		float num3 = 0f;
		foreach (ValueTuple<BiomeConfig, float> valueTuple in list)
		{
			BiomeConfig item = valueTuple.Item1;
			float item2 = valueTuple.Item2;
			num3 += item2;
			if (num2 <= num3)
			{
				if (this.enableDebugLogs)
				{
					Debug.Log(string.Format("[BiomeManager] Selezionato bioma '{0}' @ {1}m", item.biomeName, distanceFromStart));
				}
				return item;
			}
		}
		BiomeConfig item3 = list[0].Item1;
		if (this.enableDebugLogs)
		{
			Debug.LogWarning("[BiomeManager] Fallback a '" + item3.biomeName + "'");
		}
		return item3;
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x0000C3F4 File Offset: 0x0000A5F4
	public List<BiomeConfig> GetAvailableBiomesAtDistance(float distance)
	{
		return this.biomes.Where<BiomeConfig>((BiomeConfig b) => b != null && b.CanSpawnAtDistance(distance)).ToList<BiomeConfig>();
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x0000C42C File Offset: 0x0000A62C
	public BiomeConfig GetDominantBiomeAtDistance(float distance)
	{
		BiomeConfig biomeConfig = null;
		float num = 0f;
		foreach (BiomeConfig biomeConfig2 in this.biomes)
		{
			if (!(biomeConfig2 == null))
			{
				float spawnWeightAtDistance = biomeConfig2.GetSpawnWeightAtDistance(distance);
				if (spawnWeightAtDistance > num)
				{
					num = spawnWeightAtDistance;
					biomeConfig = biomeConfig2;
				}
			}
		}
		return biomeConfig ?? this.startingBiome;
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x0000C4A8 File Offset: 0x0000A6A8
	private void OnValidate()
	{
		if (this.biomes == null || this.biomes.Count == 0)
		{
			Debug.LogWarning("[BiomeManager] Nessun bioma assegnato!");
			return;
		}
		if (this.startingBiome == null)
		{
			Debug.LogWarning("[BiomeManager] startingBiome non assegnato! Usa il primo bioma della lista come fallback.");
			this.startingBiome = this.biomes[0];
		}
		foreach (BiomeConfig biomeConfig in this.biomes)
		{
			if (biomeConfig == null)
			{
				Debug.LogWarning("[BiomeManager] Bioma null nella lista!");
			}
			else if (biomeConfig.probabilityCurve == null || biomeConfig.probabilityCurve.keys.Length == 0)
			{
				Debug.LogWarning("[BiomeManager] Bioma '" + biomeConfig.biomeName + "' non ha una curva di probabilità valida!");
			}
		}
		Debug.Log(string.Format("[BiomeManager] Configurato con {0} biomi. Starting biome: '{1}'", this.biomes.Count, this.startingBiome.biomeName));
	}

	// Token: 0x04000166 RID: 358
	[Header("Biome Progression")]
	[Tooltip("Lista di tutti i biomi disponibili nel gioco")]
	public List<BiomeConfig> biomes = new List<BiomeConfig>();

	// Token: 0x04000167 RID: 359
	[Header("Starting Biome")]
	[Tooltip("Bioma iniziale (es: Residential). Usato come fallback se nessun altro bioma è disponibile.")]
	public BiomeConfig startingBiome;

	// Token: 0x04000168 RID: 360
	[Header("Districts (BiomeField)")]
	[Tooltip("Lato di un distretto in celle chunk (cellSize 60 → 3 = ~180m). Più piccolo = i biomi si alternano più spesso.")]
	[Range(2f, 10f)]
	public int districtSizeCells = 3;

	// Token: 0x04000169 RID: 361
	[Header("Debug Settings")]
	[Tooltip("Abilita log dettagliati per il debug")]
	public bool enableDebugLogs;
}
