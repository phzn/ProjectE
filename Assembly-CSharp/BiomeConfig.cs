using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200002D RID: 45
[CreateAssetMenu(fileName = "BiomeConfig", menuName = "MapGeneration/BiomeConfig", order = 10)]
public class BiomeConfig : ScriptableObject
{
	// Token: 0x0600019B RID: 411 RVA: 0x0000BB90 File Offset: 0x00009D90
	private void InitializeZonePools()
	{
		if (this._isInitialized)
		{
			return;
		}
		this._zonePools = new Dictionary<ZonePoolKey, List<ZoneData>>();
		foreach (IGrouping<ZonePoolKey, ZoneData> grouping in from z in this.availableZones
			where z != null && !ProceduralZonePolicy.IsExcludedZone(z)
			group z by new ZonePoolKey(z.targetChunkSize, z.occupancy))
		{
			this._zonePools[grouping.Key] = grouping.ToList<ZoneData>();
		}
		this._isInitialized = true;
		if (Application.isEditor)
		{
			Debug.Log(string.Format("[BiomeConfig] '{0}' inizializzato con {1} pool di zone", this.biomeName, this._zonePools.Count));
		}
	}

	// Token: 0x0600019C RID: 412 RVA: 0x0000BC7C File Offset: 0x00009E7C
	public ZoneData GetRandomZone(ChunkSize chunkSize, ZoneOccupancy occupancy)
	{
		if (!this._isInitialized)
		{
			this.InitializeZonePools();
		}
		ZonePoolKey zonePoolKey = new ZonePoolKey(chunkSize, occupancy);
		List<ZoneData> list;
		if (!this._zonePools.TryGetValue(zonePoolKey, out list) || list.Count == 0)
		{
			Debug.LogWarning(string.Format("[BiomeConfig] '{0}' non ha zone disponibili per {1}", this.biomeName, zonePoolKey));
			return null;
		}
		float num = list.Sum<ZoneData>((ZoneData z) => z.spawnWeight);
		float num2 = Random.Range(0f, num);
		float num3 = 0f;
		foreach (ZoneData zoneData in list)
		{
			num3 += zoneData.spawnWeight;
			if (num2 <= num3)
			{
				return zoneData;
			}
		}
		return list[list.Count - 1];
	}

	// Token: 0x0600019D RID: 413 RVA: 0x0000BD74 File Offset: 0x00009F74
	public float GetSpawnWeightAtDistance(float distance)
	{
		float num = this.probabilityCurve.Evaluate(distance);
		return this.baseSpawnWeight * num;
	}

	// Token: 0x0600019E RID: 414 RVA: 0x0000BD96 File Offset: 0x00009F96
	public bool CanSpawnAtDistance(float distance)
	{
		return this.GetSpawnWeightAtDistance(distance) > 0f;
	}

	// Token: 0x0600019F RID: 415 RVA: 0x0000BDA8 File Offset: 0x00009FA8
	private void OnValidate()
	{
		this._isInitialized = false;
		if (this.availableZones == null || this.availableZones.Count == 0)
		{
			Debug.LogWarning("[BiomeConfig] '" + this.biomeName + "' non ha zone assegnate!");
			return;
		}
		int num = (from z in this.availableZones
			group z by new ZonePoolKey(z.targetChunkSize, z.occupancy)).Count<IGrouping<ZonePoolKey, ZoneData>>();
		Debug.Log(string.Format("[BiomeConfig] '{0}' ha {1} zone in {2} pool diversi", this.biomeName, this.availableZones.Count, num));
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x0000BE47 File Offset: 0x0000A047
	public void RefreshZonePools()
	{
		this._isInitialized = false;
		this.InitializeZonePools();
	}

	// Token: 0x04000153 RID: 339
	[Header("Biome Identity")]
	[Tooltip("Nome del bioma (es: Residential, Suburban, Metropolis)")]
	public string biomeName = "New Biome";

	// Token: 0x04000154 RID: 340
	[Tooltip("Colore per debug/gizmos")]
	public Color debugColor = Color.white;

	// Token: 0x04000155 RID: 341
	[Header("Distance-Based Probability")]
	[Tooltip("Curva di probabilità in base alla distanza dal punto di partenza (X=distanza in metri, Y=probabilità 0-1)")]
	public AnimationCurve probabilityCurve = AnimationCurve.Linear(0f, 1f, 1000f, 0.5f);

	// Token: 0x04000156 RID: 342
	[Tooltip("Peso base di spawn (maggiore = più probabile). Viene moltiplicato per il valore della curva. In modalità distretti (BiomeField) è la quota di territorio del bioma.")]
	[Range(0f, 100f)]
	public float baseSpawnWeight = 1f;

	// Token: 0x04000157 RID: 343
	[Tooltip("Bioma di ripiego quando questo bioma non ha zone compatibili con uno slot (es. City non ha zone Quarter → usa Suburban). Lascia vuoto per pescare dall'intera lista del chunk.")]
	public BiomeConfig fallbackBiome;

	// Token: 0x04000158 RID: 344
	[Header("Zone Pools")]
	[Tooltip("Zone disponibili per questo bioma, organizzate per dimensione e occupancy")]
	public List<ZoneData> availableZones = new List<ZoneData>();

	// Token: 0x04000159 RID: 345
	[Tooltip("Modificatori del peso dei chunk quando la cella è in questo bioma (es. City: MonoChunk x20 per avere isolati pieni). I chunk non elencati mantengono il peso base.")]
	public List<BiomeConfig.ChunkWeightOverride> chunkWeightOverrides = new List<BiomeConfig.ChunkWeightOverride>();

	// Token: 0x0400015A RID: 346
	private Dictionary<ZonePoolKey, List<ZoneData>> _zonePools;

	// Token: 0x0400015B RID: 347
	private bool _isInitialized;

	// Token: 0x0200002E RID: 46
	[Serializable]
	public class ChunkWeightOverride
	{
		// Token: 0x0400015C RID: 348
		[Tooltip("ChunkData il cui peso viene modificato dentro questo bioma")]
		public ChunkData chunk;

		// Token: 0x0400015D RID: 349
		[Tooltip("Moltiplicatore del peso di spawn (1 = invariato, >1 più frequente, <1 più raro)")]
		public float multiplier = 1f;
	}
}
