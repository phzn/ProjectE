using System;
using System.Collections.Generic;
using Features.Rank;
using UnityEngine;

// Token: 0x0200005A RID: 90
public sealed class ChunkSpawnResolver
{
	// Token: 0x0600046B RID: 1131 RVA: 0x00022B9C File Offset: 0x00020D9C
	public ChunkSpawnResolver(int sessionSeed, IReadOnlyList<ChunkData> availableChunks)
		: this(sessionSeed, availableChunks, null, false, 0.8f)
	{
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x00022BB0 File Offset: 0x00020DB0
	public ChunkSpawnResolver(int sessionSeed, IReadOnlyList<ChunkData> availableChunks, BiomeManager biomeManager, bool enableSeededPoiRelief = false, float kombiniAdmissionMultiplier = 0.8f)
	{
		this._sessionSeed = sessionSeed;
		this._enableSeededPoiRelief = enableSeededPoiRelief;
		this._kombiniAdmissionMultiplier = Mathf.Clamp01(kombiniAdmissionMultiplier);
		this._seededPoiReliefPattern = ChunkSpawnResolver.Hash((uint)(sessionSeed ^ -689373511), 0U, 0U);
		if (enableSeededPoiRelief)
		{
			this._naturalPoiAcceptance = new Dictionary<Vector2Int, bool>(4096);
		}
		this._availableChunks = availableChunks;
		this._lastPoiRestrictions = RankPoiPolicy.RestrictionsActive;
		this._biomeManager = biomeManager;
		this.BuildChunkLists(availableChunks);
		if (this._biomeManager != null && this._biomeManager.biomes != null)
		{
			this._biomeZoneSets = new Dictionary<BiomeConfig, HashSet<ZoneData>>();
			this._biomeChunkMultipliers = new Dictionary<BiomeConfig, Dictionary<ChunkData, float>>();
			foreach (BiomeConfig biomeConfig in this._biomeManager.biomes)
			{
				if (!(biomeConfig == null) && !this._biomeZoneSets.ContainsKey(biomeConfig))
				{
					HashSet<ZoneData> hashSet = new HashSet<ZoneData>();
					if (biomeConfig.availableZones != null)
					{
						foreach (ZoneData zoneData in biomeConfig.availableZones)
						{
							if (zoneData != null)
							{
								hashSet.Add(zoneData);
							}
						}
					}
					this._biomeZoneSets[biomeConfig] = hashSet;
					if (biomeConfig.chunkWeightOverrides != null && biomeConfig.chunkWeightOverrides.Count > 0)
					{
						Dictionary<ChunkData, float> dictionary = new Dictionary<ChunkData, float>();
						foreach (BiomeConfig.ChunkWeightOverride chunkWeightOverride in biomeConfig.chunkWeightOverrides)
						{
							if (chunkWeightOverride != null && chunkWeightOverride.chunk != null)
							{
								dictionary[chunkWeightOverride.chunk] = Mathf.Max(0f, chunkWeightOverride.multiplier);
							}
						}
						this._biomeChunkMultipliers[biomeConfig] = dictionary;
					}
				}
			}
		}
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x00022E34 File Offset: 0x00021034
	private void BuildChunkLists(IReadOnlyList<ChunkData> chunks)
	{
		this._smallChunks.Clear();
		this._largeChunks.Clear();
		this._smallChunksWeight = 0f;
		this._largeChunksWeight = 0f;
		this._maxLargeChunkWidth = 1;
		this._maxLargeChunkHeight = 1;
		if (chunks == null)
		{
			return;
		}
		for (int i = 0; i < chunks.Count; i++)
		{
			ChunkData chunkData = chunks[i];
			if (!(chunkData == null) && chunkData.spawnWeight > 0f && RankPoiPolicy.IsChunkAllowed(chunkData))
			{
				if (chunkData.gridSize.x <= 1 && chunkData.gridSize.y <= 1)
				{
					this._smallChunks.Add(chunkData);
					this._smallChunksWeight += chunkData.spawnWeight;
				}
				else
				{
					this._largeChunks.Add(chunkData);
					this._largeChunksWeight += chunkData.spawnWeight;
					this._maxLargeChunkWidth = Mathf.Max(this._maxLargeChunkWidth, Mathf.Max(1, chunkData.gridSize.x));
					this._maxLargeChunkHeight = Mathf.Max(this._maxLargeChunkHeight, Mathf.Max(1, chunkData.gridSize.y));
				}
			}
		}
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x00022F68 File Offset: 0x00021168
	private void EnsurePoiPolicy()
	{
		bool restrictionsActive = RankPoiPolicy.RestrictionsActive;
		if (restrictionsActive == this._lastPoiRestrictions)
		{
			return;
		}
		this._lastPoiRestrictions = restrictionsActive;
		this._cache.Clear();
		Dictionary<Vector2Int, bool> naturalPoiAcceptance = this._naturalPoiAcceptance;
		if (naturalPoiAcceptance != null)
		{
			naturalPoiAcceptance.Clear();
		}
		this.BuildChunkLists(this._availableChunks);
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x00022FB4 File Offset: 0x000211B4
	public ChunkSpawnResolver.SpawnEntry Resolve(Vector2Int cell)
	{
		this.EnsurePoiPolicy();
		ChunkSpawnResolver.SpawnEntry spawnEntry;
		if (this._cache.TryGetValue(cell, out spawnEntry))
		{
			return spawnEntry;
		}
		ChunkSpawnResolver.SpawnEntry spawnEntry2 = this.ResolvePreview(cell);
		this._cache[cell] = spawnEntry2;
		return spawnEntry2;
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x00022FF0 File Offset: 0x000211F0
	public ChunkSpawnResolver.SpawnEntry ResolvePreview(Vector2Int cell)
	{
		this.EnsurePoiPolicy();
		Vector2Int vector2Int = cell + this._travelOffset;
		ChunkSpawnResolver.SpawnEntry spawnEntry = this.Compute(vector2Int, false);
		ChunkSpawnResolver.SpawnEntry spawnEntry2 = this.EnforceSpecialSpacing(vector2Int, spawnEntry);
		return this.ApplySeededPoiRelief(vector2Int, spawnEntry2, ChunkSpawnResolver.SelectionContainsSpecial(spawnEntry2), true, ProceduralPoiKind.None);
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x00023034 File Offset: 0x00021234
	public ChunkSpawnResolver.SpawnEntry ResolveMetadataPreview(Vector2Int cell)
	{
		this.EnsurePoiPolicy();
		Vector2Int vector2Int = cell + this._travelOffset;
		bool flag;
		ProceduralPoiKind proceduralPoiKind;
		ChunkSpawnResolver.SpawnEntry spawnEntry = this.Compute(vector2Int, false, false, out flag, out proceduralPoiKind, true);
		if (!flag || this.SpecialCandidateWins(vector2Int, spawnEntry))
		{
			return this.ApplySeededPoiRelief(vector2Int, spawnEntry, flag, false, proceduralPoiKind);
		}
		ChunkSpawnResolver.SpawnEntry spawnEntry2 = (ProceduralZonePolicy.IsAlwaysSpecialChunk(spawnEntry.chunkData) ? this.BuildNonSpecialReplacement(vector2Int, spawnEntry, false) : spawnEntry);
		return this.ApplySeededPoiRelief(vector2Int, spawnEntry2, false, false, ProceduralPoiKind.None);
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x000230A4 File Offset: 0x000212A4
	public bool TryResolveContainingSpecialChunkOrigin(Vector2Int cell, out Vector2Int origin)
	{
		this.EnsurePoiPolicy();
		origin = cell;
		int num = cell.x - Mathf.Max(0, this._maxLargeChunkWidth - 1);
		int num2 = cell.y - Mathf.Max(0, this._maxLargeChunkHeight - 1);
		for (int i = num; i <= cell.x; i++)
		{
			for (int j = num2; j <= cell.y; j++)
			{
				Vector2Int vector2Int = new Vector2Int(i, j);
				ChunkData chunkData = this.SelectBaseChunkForSpacing(vector2Int + this._travelOffset);
				Vector2Int vector2Int2 = ChunkSpawnResolver.NormalizedGridSize(chunkData);
				if (!(chunkData == null) && ChunkSpawnResolver.IsMultiCell(vector2Int2) && (ChunkSpawnResolver.IsSubwayChunk(chunkData) || (ProceduralZonePolicy.IsAlwaysSpecialChunk(chunkData) && ChunkSpawnResolver.FootprintContainsCell(vector2Int, vector2Int2, cell))))
				{
					ChunkSpawnResolver.SpawnEntry spawnEntry = this.ResolveMetadataPreview(vector2Int);
					if (spawnEntry.IsValid && ProceduralZonePolicy.IsAlwaysSpecialChunk(spawnEntry.chunkData))
					{
						Vector2Int vector2Int3 = ChunkSpawnResolver.NormalizedGridSize(spawnEntry.chunkData);
						if (ChunkSpawnResolver.IsMultiCell(vector2Int3) && ChunkSpawnResolver.FootprintContainsCell(vector2Int, vector2Int3, cell))
						{
							origin = vector2Int;
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x000231BC File Offset: 0x000213BC
	public ChunkSpawnResolver.SpawnEntry ResolveWithoutSubway(Vector2Int cell)
	{
		ChunkSpawnResolver.SpawnEntry spawnEntry = this.ResolvePreviewWithoutSubway(cell);
		this._cache[cell] = spawnEntry;
		return spawnEntry;
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x000231E0 File Offset: 0x000213E0
	public ChunkSpawnResolver.SpawnEntry ResolvePreviewWithoutSubway(Vector2Int cell)
	{
		this.EnsurePoiPolicy();
		Vector2Int vector2Int = cell + this._travelOffset;
		ChunkSpawnResolver.SpawnEntry spawnEntry = this.Compute(vector2Int, true);
		ChunkSpawnResolver.SpawnEntry spawnEntry2 = this.EnforceSpecialSpacing(vector2Int, spawnEntry);
		return this.ApplySeededPoiRelief(vector2Int, spawnEntry2, ChunkSpawnResolver.SelectionContainsSpecial(spawnEntry2), true, ProceduralPoiKind.None);
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x00023224 File Offset: 0x00021424
	public ChunkSpawnResolver.SpawnEntry ResolveSmallFallback(Vector2Int cell, ChunkData excludedChunk, bool excludeSpecialZones)
	{
		this.EnsurePoiPolicy();
		Vector2Int vector2Int = cell + this._travelOffset;
		uint num = ChunkSpawnResolver.Hash((uint)(this._sessionSeed ^ -776648139), (uint)vector2Int.x, (uint)vector2Int.y);
		HashSet<ZoneData> hashSet = null;
		HashSet<ZoneData> hashSet2 = null;
		Dictionary<ChunkData, float> dictionary = null;
		BiomeConfig biomeForCell = this.GetBiomeForCell(vector2Int);
		if (biomeForCell != null && this._biomeZoneSets != null)
		{
			this._biomeZoneSets.TryGetValue(biomeForCell, out hashSet);
			if (biomeForCell.fallbackBiome != null)
			{
				this._biomeZoneSets.TryGetValue(biomeForCell.fallbackBiome, out hashSet2);
			}
			Dictionary<BiomeConfig, Dictionary<ChunkData, float>> biomeChunkMultipliers = this._biomeChunkMultipliers;
			if (biomeChunkMultipliers != null)
			{
				biomeChunkMultipliers.TryGetValue(biomeForCell, out dictionary);
			}
		}
		ChunkData chunkData = this.SelectSmallFallbackChunk(num, dictionary, excludedChunk, excludeSpecialZones);
		ChunkSpawnResolver.SpawnEntry spawnEntry = ((chunkData != null) ? this.BuildEntry(vector2Int, chunkData, num, hashSet, hashSet2, excludeSpecialZones) : default(ChunkSpawnResolver.SpawnEntry));
		return this.ApplyKombiniAdmission(vector2Int, spawnEntry, ChunkSpawnResolver.SelectedPoiKind(spawnEntry), PoiTypeWeightMultipliers.Uniform, true);
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x00023318 File Offset: 0x00021518
	public ChunkSpawnResolver.SpawnEntry ResolveWithoutSpecialZones(Vector2Int cell, ChunkData chunkData)
	{
		this.EnsurePoiPolicy();
		if (!RankPoiPolicy.IsChunkAllowed(chunkData))
		{
			return this.ResolveSmallFallback(cell, chunkData, true);
		}
		if (chunkData == null)
		{
			return default(ChunkSpawnResolver.SpawnEntry);
		}
		Vector2Int vector2Int = cell + this._travelOffset;
		uint num = ChunkSpawnResolver.Hash((uint)this._sessionSeed, (uint)vector2Int.x, (uint)vector2Int.y);
		HashSet<ZoneData> hashSet = null;
		HashSet<ZoneData> hashSet2 = null;
		BiomeConfig biomeForCell = this.GetBiomeForCell(vector2Int);
		if (biomeForCell != null && this._biomeZoneSets != null)
		{
			this._biomeZoneSets.TryGetValue(biomeForCell, out hashSet);
			if (biomeForCell.fallbackBiome != null)
			{
				this._biomeZoneSets.TryGetValue(biomeForCell.fallbackBiome, out hashSet2);
			}
		}
		return this.BuildEntry(vector2Int, chunkData, num, hashSet, hashSet2, true);
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x000233D7 File Offset: 0x000215D7
	public ChunkSpawnResolver.SpawnEntry ResolveDroughtRelief(Vector2Int cell)
	{
		return this.ResolveDroughtRelief(cell, PoiTypeWeightMultipliers.Uniform);
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x000233E8 File Offset: 0x000215E8
	public ChunkSpawnResolver.SpawnEntry ResolveDroughtRelief(Vector2Int cell, PoiTypeWeightMultipliers typeWeights)
	{
		this.EnsurePoiPolicy();
		Vector2Int vector2Int = cell + this._travelOffset;
		return this.ResolveDroughtReliefSample(vector2Int, typeWeights, true);
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x00023414 File Offset: 0x00021614
	private ChunkSpawnResolver.SpawnEntry ResolveDroughtReliefSample(Vector2Int sampleCell, PoiTypeWeightMultipliers typeWeights, bool captureZoneIndices)
	{
		uint num = ChunkSpawnResolver.Hash((uint)(this._sessionSeed ^ 2135587861), (uint)sampleCell.x, (uint)sampleCell.y);
		HashSet<ZoneData> hashSet = null;
		HashSet<ZoneData> hashSet2 = null;
		Dictionary<ChunkData, float> dictionary = null;
		BiomeConfig biomeForCell = this.GetBiomeForCell(sampleCell);
		if (biomeForCell != null && this._biomeZoneSets != null)
		{
			this._biomeZoneSets.TryGetValue(biomeForCell, out hashSet);
			if (biomeForCell.fallbackBiome != null)
			{
				this._biomeZoneSets.TryGetValue(biomeForCell.fallbackBiome, out hashSet2);
			}
			Dictionary<BiomeConfig, Dictionary<ChunkData, float>> biomeChunkMultipliers = this._biomeChunkMultipliers;
			if (biomeChunkMultipliers != null)
			{
				biomeChunkMultipliers.TryGetValue(biomeForCell, out dictionary);
			}
		}
		ChunkData chunkData;
		int num2;
		int num3;
		if (hashSet != null && this.TrySelectDroughtReliefCandidate(num, hashSet, dictionary, false, typeWeights, out chunkData, out num2, out num3, false, null) && ChunkSpawnResolver.PassesRecencyLocalityGate(chunkData, num3, typeWeights, ChunkSpawnResolver.Advance(num ^ 1640531527U)))
		{
			return this.BuildDroughtReliefEntry(sampleCell, chunkData, num2, num3, num, hashSet, hashSet2, typeWeights, captureZoneIndices);
		}
		if (hashSet2 != null && this.TrySelectDroughtReliefCandidate(ChunkSpawnResolver.Advance(num ^ 2496678331U), hashSet2, dictionary, false, typeWeights, out chunkData, out num2, out num3, false, null))
		{
			uint num4 = ChunkSpawnResolver.Advance(num ^ 2496678331U);
			if (ChunkSpawnResolver.PassesRecencyLocalityGate(chunkData, num3, typeWeights, ChunkSpawnResolver.Advance(num4 ^ 1640531527U)))
			{
				return this.BuildDroughtReliefEntry(sampleCell, chunkData, num2, num3, num, hashSet, hashSet2, typeWeights, captureZoneIndices);
			}
		}
		if (this.TrySelectDroughtReliefCandidate(ChunkSpawnResolver.Advance(num ^ 3518319157U), null, null, true, typeWeights, out chunkData, out num2, out num3, false, null))
		{
			return this.BuildDroughtReliefEntry(sampleCell, chunkData, num2, num3, num, hashSet, hashSet2, typeWeights, captureZoneIndices);
		}
		return default(ChunkSpawnResolver.SpawnEntry);
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x000235A4 File Offset: 0x000217A4
	public ChunkSpawnResolver.SpawnEntry ResolveSelectedSpecialWithRecency(Vector2Int cell, ChunkSpawnResolver.SpawnEntry seededEntry, PoiTypeWeightMultipliers typeWeights)
	{
		this.EnsurePoiPolicy();
		if (!RankPoiPolicy.IsSpawnAllowed(seededEntry))
		{
			return this.ResolveWithoutSpecialZones(cell, seededEntry.chunkData);
		}
		ChunkData chunkData = seededEntry.chunkData;
		if (seededEntry.IsValid && seededEntry.zoneIndices != null)
		{
			bool flag;
			if (chunkData == null)
			{
				flag = null != null;
			}
			else
			{
				ChunkTemplate template = chunkData.template;
				flag = ((template != null) ? template.zoneSlots : null) != null;
			}
			if (flag && chunkData.availableZones != null)
			{
				Vector2Int vector2Int = cell + this._travelOffset;
				HashSet<ZoneData> hashSet = null;
				HashSet<ZoneData> hashSet2 = null;
				BiomeConfig biomeForCell = this.GetBiomeForCell(vector2Int);
				if (biomeForCell != null && this._biomeZoneSets != null)
				{
					this._biomeZoneSets.TryGetValue(biomeForCell, out hashSet);
					if (biomeForCell.fallbackBiome != null)
					{
						this._biomeZoneSets.TryGetValue(biomeForCell.fallbackBiome, out hashSet2);
					}
				}
				int[] array = null;
				int num = Mathf.Min(seededEntry.zoneIndices.Length, chunkData.template.zoneSlots.Count);
				for (int i = 0; i < num; i++)
				{
					int num2 = seededEntry.zoneIndices[i];
					if (num2 >= 0 && num2 < chunkData.availableZones.Count && ProceduralZonePolicy.IsSpecialZone(chunkData.availableZones[num2]))
					{
						uint num3 = ChunkSpawnResolver.Hash((uint)(this._sessionSeed ^ -509935195), (uint)vector2Int.x, (uint)vector2Int.y);
						num3 = ChunkSpawnResolver.Advance(num3 ^ (uint)((i + 1) * -1640531527));
						ZoneSlot zoneSlot = chunkData.template.zoneSlots[i];
						int num4 = ChunkSpawnResolver.SelectSpecialZoneIndexWithRecency(chunkData, zoneSlot, num3, hashSet, typeWeights);
						if (num4 >= 0 && !ChunkSpawnResolver.PassesRecencyLocalityGate(chunkData, num4, typeWeights, ChunkSpawnResolver.Advance(num3 ^ 1640531527U)))
						{
							num4 = -1;
						}
						if (num4 < 0 && hashSet2 != null)
						{
							uint num5 = ChunkSpawnResolver.Advance(num3 ^ 2496678331U);
							num4 = ChunkSpawnResolver.SelectSpecialZoneIndexWithRecency(chunkData, zoneSlot, num5, hashSet2, typeWeights);
							if (num4 >= 0 && !ChunkSpawnResolver.PassesRecencyLocalityGate(chunkData, num4, typeWeights, ChunkSpawnResolver.Advance(num5 ^ 1640531527U)))
							{
								num4 = -1;
							}
						}
						if (num4 < 0)
						{
							num4 = ChunkSpawnResolver.SelectSpecialZoneIndexWithRecency(chunkData, zoneSlot, ChunkSpawnResolver.Advance(num3 ^ 3518319157U), null, typeWeights);
						}
						if (num4 >= 0 && num4 != num2)
						{
							if (array == null)
							{
								array = (int[])seededEntry.zoneIndices.Clone();
							}
							array[i] = num4;
						}
					}
				}
				ChunkSpawnResolver.SpawnEntry spawnEntry = ((array == null) ? seededEntry : new ChunkSpawnResolver.SpawnEntry(chunkData, seededEntry.rotation, array, seededEntry.wasPresented));
				return this.ApplyKombiniAdmission(vector2Int, spawnEntry, ChunkSpawnResolver.SelectedPoiKind(spawnEntry), typeWeights, true);
			}
		}
		return seededEntry;
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x00023810 File Offset: 0x00021A10
	public bool PassesUniqueSpecialRecencyAdmission(Vector2Int cell, ChunkSpawnResolver.SpawnEntry entry, PoiTypeWeightMultipliers typeWeights)
	{
		this.EnsurePoiPolicy();
		if (!RankPoiPolicy.IsSpawnAllowed(entry))
		{
			return false;
		}
		ProceduralPoiKind proceduralPoiKind = ChunkSpawnResolver.SelectedPoiKind(entry);
		if (proceduralPoiKind == ProceduralPoiKind.None)
		{
			return true;
		}
		ChunkData chunkData = entry.chunkData;
		if (entry.zoneIndices != null)
		{
			bool flag;
			if (chunkData == null)
			{
				flag = null != null;
			}
			else
			{
				ChunkTemplate template = chunkData.template;
				flag = ((template != null) ? template.zoneSlots : null) != null;
			}
			if (flag && chunkData.availableZones != null)
			{
				int num = Mathf.Min(entry.zoneIndices.Length, chunkData.template.zoneSlots.Count);
				int i = 0;
				while (i < num)
				{
					int num2 = entry.zoneIndices[i];
					if (num2 >= 0 && num2 < chunkData.availableZones.Count && ProceduralZonePolicy.GetPoiKind(chunkData.availableZones[num2]) == proceduralPoiKind)
					{
						int num3 = 0;
						ZoneSlot zoneSlot = chunkData.template.zoneSlots[i];
						for (int j = 0; j < chunkData.availableZones.Count; j++)
						{
							ZoneData zoneData = chunkData.availableZones[j];
							ProceduralPoiKind poiKind = ProceduralZonePolicy.GetPoiKind(zoneData);
							if (poiKind != ProceduralPoiKind.None && zoneData != null && !ProceduralZonePolicy.IsExcludedZone(zoneData) && zoneData.IsCompatibleWith(zoneSlot))
							{
								num3 |= 1 << (int)poiKind;
							}
						}
						if ((num3 & (num3 - 1)) != 0)
						{
							return true;
						}
						break;
					}
					else
					{
						i++;
					}
				}
			}
		}
		float num4 = typeWeights.For(proceduralPoiKind);
		if (num4 >= 0.9999f)
		{
			return true;
		}
		Vector2Int vector2Int = cell + this._travelOffset;
		return ChunkSpawnResolver.HashToFloat(ChunkSpawnResolver.Advance(ChunkSpawnResolver.Hash((uint)(this._sessionSeed ^ -1052792407), (uint)vector2Int.x, (uint)vector2Int.y) ^ ((uint)proceduralPoiKind * 2447445413U))) < num4;
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x000239BC File Offset: 0x00021BBC
	private static ProceduralPoiKind SelectedPoiKind(ChunkSpawnResolver.SpawnEntry entry)
	{
		ChunkData chunkData = entry.chunkData;
		if (!entry.IsValid)
		{
			return ProceduralPoiKind.None;
		}
		if (entry.zoneIndices != null && chunkData.availableZones != null)
		{
			for (int i = 0; i < entry.zoneIndices.Length; i++)
			{
				int num = entry.zoneIndices[i];
				if (num >= 0 && num < chunkData.availableZones.Count)
				{
					ProceduralPoiKind poiKind = ProceduralZonePolicy.GetPoiKind(chunkData.availableZones[num]);
					if (poiKind != ProceduralPoiKind.None)
					{
						return poiKind;
					}
				}
			}
		}
		if (!chunkData.containsSubway)
		{
			return ProceduralPoiKind.None;
		}
		return ProceduralPoiKind.Subway;
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x00023A3C File Offset: 0x00021C3C
	private static bool PassesRecencyLocalityGate(ChunkData chunkData, int zoneIndex, PoiTypeWeightMultipliers typeWeights, uint h)
	{
		if (((chunkData != null) ? chunkData.availableZones : null) == null || zoneIndex < 0 || zoneIndex >= chunkData.availableZones.Count)
		{
			return true;
		}
		ProceduralPoiKind poiKind = ProceduralZonePolicy.GetPoiKind(chunkData.availableZones[zoneIndex]);
		float num = typeWeights.For(poiKind);
		return num >= 0.9999f || ChunkSpawnResolver.HashToFloat(h) < num;
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x00023A99 File Offset: 0x00021C99
	public void SetTravelOffset(Vector2Int offset)
	{
		if (this._travelOffset == offset)
		{
			return;
		}
		this._travelOffset = offset;
		this._cache.Clear();
		Dictionary<Vector2Int, bool> naturalPoiAcceptance = this._naturalPoiAcceptance;
		if (naturalPoiAcceptance == null)
		{
			return;
		}
		naturalPoiAcceptance.Clear();
	}

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x0600047F RID: 1151 RVA: 0x00023ACC File Offset: 0x00021CCC
	public Vector2Int TravelOffset
	{
		get
		{
			return this._travelOffset;
		}
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x00023AD4 File Offset: 0x00021CD4
	public void Invalidate(Vector2Int cell)
	{
		this._cache.Remove(cell);
		Dictionary<Vector2Int, bool> naturalPoiAcceptance = this._naturalPoiAcceptance;
		if (naturalPoiAcceptance == null)
		{
			return;
		}
		naturalPoiAcceptance.Clear();
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x00023AF3 File Offset: 0x00021CF3
	public void Clear()
	{
		this._cache.Clear();
		Dictionary<Vector2Int, bool> naturalPoiAcceptance = this._naturalPoiAcceptance;
		if (naturalPoiAcceptance == null)
		{
			return;
		}
		naturalPoiAcceptance.Clear();
	}

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x06000482 RID: 1154 RVA: 0x00023B10 File Offset: 0x00021D10
	public int SessionSeed
	{
		get
		{
			return this._sessionSeed;
		}
	}

	// Token: 0x1700010B RID: 267
	// (get) Token: 0x06000483 RID: 1155 RVA: 0x00023B18 File Offset: 0x00021D18
	public int CachedCellCount
	{
		get
		{
			return this._cache.Count;
		}
	}

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x06000484 RID: 1156 RVA: 0x00023B25 File Offset: 0x00021D25
	public int CachedNaturalPoiCellCount
	{
		get
		{
			Dictionary<Vector2Int, bool> naturalPoiAcceptance = this._naturalPoiAcceptance;
			if (naturalPoiAcceptance == null)
			{
				return 0;
			}
			return naturalPoiAcceptance.Count;
		}
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x00023B38 File Offset: 0x00021D38
	private ChunkSpawnResolver.SpawnEntry Compute(Vector2Int cell, bool forceNonSubway = false)
	{
		bool flag;
		return this.Compute(cell, forceNonSubway, true, out flag);
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x00023B50 File Offset: 0x00021D50
	private ChunkSpawnResolver.SpawnEntry Compute(Vector2Int cell, bool forceNonSubway, bool captureZoneIndices, out bool containsSpecial)
	{
		ProceduralPoiKind proceduralPoiKind;
		return this.Compute(cell, forceNonSubway, captureZoneIndices, out containsSpecial, out proceduralPoiKind, false);
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x00023B6C File Offset: 0x00021D6C
	private ChunkSpawnResolver.SpawnEntry Compute(Vector2Int cell, bool forceNonSubway, bool captureZoneIndices, out bool containsSpecial, out ProceduralPoiKind selectedKind, bool captureSelectedKind = true)
	{
		containsSpecial = false;
		selectedKind = ProceduralPoiKind.None;
		uint num = ChunkSpawnResolver.Hash((uint)this._sessionSeed, (uint)cell.x, (uint)cell.y);
		HashSet<ZoneData> hashSet = null;
		HashSet<ZoneData> hashSet2 = null;
		Dictionary<ChunkData, float> dictionary = null;
		BiomeConfig biomeForCell = this.GetBiomeForCell(cell);
		if (biomeForCell != null && this._biomeZoneSets != null)
		{
			this._biomeZoneSets.TryGetValue(biomeForCell, out hashSet);
			if (biomeForCell.fallbackBiome != null)
			{
				this._biomeZoneSets.TryGetValue(biomeForCell.fallbackBiome, out hashSet2);
			}
			Dictionary<BiomeConfig, Dictionary<ChunkData, float>> biomeChunkMultipliers = this._biomeChunkMultipliers;
			if (biomeChunkMultipliers != null)
			{
				biomeChunkMultipliers.TryGetValue(biomeForCell, out dictionary);
			}
		}
		ChunkData chunkData = this.SelectChunk(num, dictionary);
		chunkData = this.EnforceSubwaySpacing(cell, num, chunkData, dictionary);
		if (forceNonSubway && ChunkSpawnResolver.IsSubwayChunk(chunkData))
		{
			chunkData = this.SelectNonSubwayReplacement(chunkData, ChunkSpawnResolver.Advance(num ^ 970237893U), dictionary);
		}
		if (chunkData == null)
		{
			return default(ChunkSpawnResolver.SpawnEntry);
		}
		return this.BuildEntry(cell, chunkData, num, hashSet, hashSet2, false, captureZoneIndices, out containsSpecial, out selectedKind, captureSelectedKind);
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x00023C6C File Offset: 0x00021E6C
	private ChunkSpawnResolver.SpawnEntry ApplySeededPoiRelief(Vector2Int sampleCell, ChunkSpawnResolver.SpawnEntry naturalEntry, bool naturalContainsSpecial, bool captureZoneIndices, ProceduralPoiKind naturalKind = ProceduralPoiKind.None)
	{
		if (naturalContainsSpecial)
		{
			return this.ApplyKombiniAdmission(sampleCell, naturalEntry, captureZoneIndices ? ChunkSpawnResolver.SelectedPoiKind(naturalEntry) : naturalKind, PoiTypeWeightMultipliers.Uniform, captureZoneIndices);
		}
		if (!this._enableSeededPoiRelief || !naturalEntry.IsValid || !this.IsSeededPoiReliefAnchor(sampleCell) || this.HasAcceptedNaturalSpecialNearRelief(sampleCell))
		{
			return naturalEntry;
		}
		ChunkSpawnResolver.SpawnEntry spawnEntry = this.ResolveDroughtReliefSample(sampleCell, PoiTypeWeightMultipliers.Uniform, captureZoneIndices);
		if (!spawnEntry.IsValid || ChunkSpawnResolver.IsSubwayChunk(spawnEntry.chunkData) || spawnEntry.chunkData.gridSize.x > 1 || spawnEntry.chunkData.gridSize.y > 1)
		{
			return naturalEntry;
		}
		return spawnEntry;
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x00023D10 File Offset: 0x00021F10
	private bool PassesKombiniAdmission(Vector2Int sampleCell)
	{
		return this._kombiniAdmissionMultiplier >= 1f || (this._kombiniAdmissionMultiplier > 0f && ChunkSpawnResolver.HashToFloat(ChunkSpawnResolver.Hash((uint)(this._sessionSeed ^ -1514973435), (uint)sampleCell.x, (uint)sampleCell.y)) < this._kombiniAdmissionMultiplier);
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x00023D68 File Offset: 0x00021F68
	private bool TrySelectKombiniAlternative(Vector2Int sampleCell, ChunkTemplate originalTemplate, PoiTypeWeightMultipliers typeWeights, out ChunkData chunk, out int slot, out int zone)
	{
		uint num = ChunkSpawnResolver.Hash((uint)(this._sessionSeed ^ -1255572915), (uint)sampleCell.x, (uint)sampleCell.y);
		return this.TrySelectDroughtReliefCandidate(num, null, null, true, typeWeights, out chunk, out slot, out zone, true, (originalTemplate != null) ? new ChunkShape?(originalTemplate.shape) : null);
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x00023DC8 File Offset: 0x00021FC8
	private ChunkSpawnResolver.SpawnEntry ApplyKombiniAdmission(Vector2Int sampleCell, ChunkSpawnResolver.SpawnEntry entry, ProceduralPoiKind selectedKind, PoiTypeWeightMultipliers typeWeights, bool captureZoneIndices)
	{
		ChunkData chunkData;
		int num;
		int num2;
		if (selectedKind != ProceduralPoiKind.Kombini || !entry.IsValid || entry.chunkData.gridSize.x > 1 || entry.chunkData.gridSize.y > 1 || ChunkSpawnResolver.IsSubwayChunk(entry.chunkData) || this.PassesKombiniAdmission(sampleCell) || !this.TrySelectKombiniAlternative(sampleCell, entry.chunkData.template, typeWeights, out chunkData, out num, out num2))
		{
			return entry;
		}
		HashSet<ZoneData> hashSet = null;
		HashSet<ZoneData> hashSet2 = null;
		BiomeConfig biomeForCell = this.GetBiomeForCell(sampleCell);
		if (biomeForCell != null && this._biomeZoneSets != null)
		{
			this._biomeZoneSets.TryGetValue(biomeForCell, out hashSet);
			if (biomeForCell.fallbackBiome != null)
			{
				this._biomeZoneSets.TryGetValue(biomeForCell.fallbackBiome, out hashSet2);
			}
		}
		uint num3 = ChunkSpawnResolver.Hash((uint)(this._sessionSeed ^ 2135587861), (uint)sampleCell.x, (uint)sampleCell.y);
		ChunkSpawnResolver.SpawnEntry spawnEntry = this.BuildDroughtReliefEntry(sampleCell, chunkData, num, num2, num3, hashSet, hashSet2, typeWeights, captureZoneIndices);
		return new ChunkSpawnResolver.SpawnEntry(spawnEntry.chunkData, entry.rotation, spawnEntry.zoneIndices, entry.wasPresented);
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x00023EE8 File Offset: 0x000220E8
	private bool IsSeededPoiReliefAnchor(Vector2Int sampleCell)
	{
		long num = (long)sampleCell.x;
		long num2 = (long)sampleCell.y;
		if ((this._seededPoiReliefPattern & 1U) != 0U)
		{
			long num3 = num;
			num = num2;
			num2 = num3;
		}
		if ((this._seededPoiReliefPattern & 2U) != 0U)
		{
			num = -num;
		}
		if ((this._seededPoiReliefPattern & 4U) != 0U)
		{
			num2 = -num2;
		}
		num -= (long)((ulong)((this._seededPoiReliefPattern >> 3) % 3U));
		num2 -= (long)((ulong)((this._seededPoiReliefPattern >> 5) % 5U));
		return num % 3L == 0L && (num2 - num / 3L) % 5L == 0L;
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x00023F60 File Offset: 0x00022160
	private bool HasAcceptedNaturalSpecialNearRelief(Vector2Int sampleCell)
	{
		int num = -(this._maxLargeChunkWidth - 1 + 2);
		int num2 = -(this._maxLargeChunkHeight - 1 + 2);
		for (int i = num; i <= 2; i++)
		{
			for (int j = num2; j <= 2; j++)
			{
				Vector2Int vector2Int = sampleCell + new Vector2Int(i, j);
				if (this.HasAcceptedNaturalSpecialConflict(vector2Int, sampleCell))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x00023FB8 File Offset: 0x000221B8
	private bool HasAcceptedNaturalSpecialConflict(Vector2Int origin, Vector2Int reliefCell)
	{
		bool flag2;
		bool flag = this._naturalPoiAcceptance.TryGetValue(origin, out flag2);
		if (flag && !flag2)
		{
			return false;
		}
		bool flag3;
		ChunkSpawnResolver.SpawnEntry spawnEntry = this.Compute(origin, false, false, out flag3);
		if (flag3 && !ChunkSpawnResolver.FootprintsWithinSpecialSpacing(reliefCell, Vector2Int.one, origin, ChunkSpawnResolver.NormalizedGridSize(spawnEntry.chunkData)))
		{
			return false;
		}
		if (flag)
		{
			return flag2;
		}
		flag2 = flag3 && this.SpecialCandidateWins(origin, spawnEntry);
		if (this._naturalPoiAcceptance.Count >= 4096)
		{
			this._naturalPoiAcceptance.Clear();
		}
		this._naturalPoiAcceptance[origin] = flag2;
		return flag2;
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x00024045 File Offset: 0x00022245
	private ChunkSpawnResolver.SpawnEntry EnforceSpecialSpacing(Vector2Int cell, ChunkSpawnResolver.SpawnEntry rawEntry)
	{
		if (!ChunkSpawnResolver.SelectionContainsSpecial(rawEntry))
		{
			return rawEntry;
		}
		if (this.SpecialCandidateWins(cell, rawEntry))
		{
			return rawEntry;
		}
		return this.BuildNonSpecialReplacement(cell, rawEntry, true);
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x00024068 File Offset: 0x00022268
	private bool SpecialCandidateWins(Vector2Int cell, ChunkSpawnResolver.SpawnEntry rawEntry)
	{
		ChunkData chunkData = rawEntry.chunkData;
		if (chunkData == null)
		{
			return false;
		}
		Vector2Int vector2Int = ChunkSpawnResolver.NormalizedGridSize(chunkData);
		bool flag = ChunkSpawnResolver.IsMultiCell(vector2Int);
		uint num = this.SpecialPriority(cell);
		int num2 = -(this._maxLargeChunkWidth - 1 + 2);
		int num3 = vector2Int.x - 1 + 2;
		int num4 = -(this._maxLargeChunkHeight - 1 + 2);
		int num5 = vector2Int.y - 1 + 2;
		for (int i = num2; i <= num3; i++)
		{
			for (int j = num4; j <= num5; j++)
			{
				if (i != 0 || j != 0)
				{
					Vector2Int vector2Int2 = cell + new Vector2Int(i, j);
					uint num6 = this.SpecialPriority(vector2Int2);
					bool flag2 = num6 > num || (num6 == num && ChunkSpawnResolver.ComesBefore(vector2Int2, cell));
					if (flag2 || (!flag && i <= 0 && j <= 0 && i >= -(this._maxLargeChunkWidth - 1) && j >= -(this._maxLargeChunkHeight - 1)))
					{
						bool flag3;
						ChunkSpawnResolver.SpawnEntry spawnEntry = this.Compute(vector2Int2, false, false, out flag3);
						if (flag3)
						{
							Vector2Int vector2Int3 = ChunkSpawnResolver.NormalizedGridSize(spawnEntry.chunkData);
							bool flag4 = ChunkSpawnResolver.IsMultiCell(vector2Int3);
							if (ChunkSpawnResolver.FootprintsWithinSpecialSpacing(cell, vector2Int, vector2Int2, vector2Int3))
							{
								if (ChunkSpawnResolver.FootprintsOverlap(cell, vector2Int, vector2Int2, vector2Int3) && flag != flag4)
								{
									if (flag4)
									{
										return false;
									}
								}
								else if (flag2)
								{
									return false;
								}
							}
						}
					}
				}
			}
		}
		return true;
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x000241BD File Offset: 0x000223BD
	private static Vector2Int NormalizedGridSize(ChunkData chunk)
	{
		if (!(chunk == null))
		{
			return new Vector2Int(Mathf.Max(1, chunk.gridSize.x), Mathf.Max(1, chunk.gridSize.y));
		}
		return Vector2Int.one;
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x000241F5 File Offset: 0x000223F5
	private static bool IsMultiCell(Vector2Int size)
	{
		return size.x > 1 || size.y > 1;
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x00024210 File Offset: 0x00022410
	private static bool FootprintsOverlap(Vector2Int firstOrigin, Vector2Int firstSize, Vector2Int secondOrigin, Vector2Int secondSize)
	{
		long num = (long)firstOrigin.x + (long)firstSize.x - 1L;
		long num2 = (long)firstOrigin.y + (long)firstSize.y - 1L;
		long num3 = (long)secondOrigin.x + (long)secondSize.x - 1L;
		long num4 = (long)secondOrigin.y + (long)secondSize.y - 1L;
		return (long)firstOrigin.x <= num3 && (long)secondOrigin.x <= num && (long)firstOrigin.y <= num4 && (long)secondOrigin.y <= num2;
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x000242A4 File Offset: 0x000224A4
	private static bool FootprintsWithinSpecialSpacing(Vector2Int firstOrigin, Vector2Int firstSize, Vector2Int secondOrigin, Vector2Int secondSize)
	{
		long num = (long)firstOrigin.x + (long)firstSize.x - 1L;
		long num2 = (long)firstOrigin.y + (long)firstSize.y - 1L;
		long num3 = (long)secondOrigin.x + (long)secondSize.x - 1L;
		long num4 = (long)secondOrigin.y + (long)secondSize.y - 1L;
		long num5 = ((num < (long)secondOrigin.x) ? ((long)secondOrigin.x - num) : ((num3 < (long)firstOrigin.x) ? ((long)firstOrigin.x - num3) : 0L));
		long num6 = ((num2 < (long)secondOrigin.y) ? ((long)secondOrigin.y - num2) : ((num4 < (long)firstOrigin.y) ? ((long)firstOrigin.y - num4) : 0L));
		return num5 <= 2L && num6 <= 2L;
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x00024378 File Offset: 0x00022578
	private static bool FootprintContainsCell(Vector2Int origin, Vector2Int size, Vector2Int cell)
	{
		long num = (long)origin.x + (long)size.x - 1L;
		long num2 = (long)origin.y + (long)size.y - 1L;
		return cell.x >= origin.x && (long)cell.x <= num && cell.y >= origin.y && (long)cell.y <= num2;
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x000243EC File Offset: 0x000225EC
	private ChunkSpawnResolver.SpawnEntry BuildNonSpecialReplacement(Vector2Int cell, ChunkSpawnResolver.SpawnEntry rawEntry, bool captureZoneIndices = true)
	{
		ChunkData chunkData = rawEntry.chunkData;
		if (chunkData == null)
		{
			return default(ChunkSpawnResolver.SpawnEntry);
		}
		HashSet<ZoneData> hashSet = null;
		HashSet<ZoneData> hashSet2 = null;
		Dictionary<ChunkData, float> dictionary = null;
		BiomeConfig biomeForCell = this.GetBiomeForCell(cell);
		if (biomeForCell != null && this._biomeZoneSets != null)
		{
			this._biomeZoneSets.TryGetValue(biomeForCell, out hashSet);
			if (biomeForCell.fallbackBiome != null)
			{
				this._biomeZoneSets.TryGetValue(biomeForCell.fallbackBiome, out hashSet2);
			}
			Dictionary<BiomeConfig, Dictionary<ChunkData, float>> biomeChunkMultipliers = this._biomeChunkMultipliers;
			if (biomeChunkMultipliers != null)
			{
				biomeChunkMultipliers.TryGetValue(biomeForCell, out dictionary);
			}
		}
		bool flag;
		ProceduralPoiKind proceduralPoiKind;
		if (!ProceduralZonePolicy.IsAlwaysSpecialChunk(chunkData))
		{
			uint num = ChunkSpawnResolver.Hash((uint)this._sessionSeed, (uint)cell.x, (uint)cell.y);
			return this.BuildEntry(cell, chunkData, num, hashSet, hashSet2, true, captureZoneIndices, out flag, out proceduralPoiKind, false);
		}
		uint num2 = ChunkSpawnResolver.Hash((uint)(this._sessionSeed ^ -776648139), (uint)cell.x, (uint)cell.y);
		ChunkData chunkData2 = this.SelectSmallFallbackChunk(num2, dictionary, chunkData, true);
		if (!(chunkData2 != null))
		{
			return default(ChunkSpawnResolver.SpawnEntry);
		}
		return this.BuildEntry(cell, chunkData2, num2, hashSet, hashSet2, true, captureZoneIndices, out flag, out proceduralPoiKind, false);
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x00024510 File Offset: 0x00022710
	private static bool SelectionContainsSpecial(ChunkSpawnResolver.SpawnEntry entry)
	{
		ChunkData chunkData = entry.chunkData;
		if (chunkData == null)
		{
			return false;
		}
		if (ProceduralZonePolicy.IsAlwaysSpecialChunk(chunkData))
		{
			return true;
		}
		int[] zoneIndices = entry.zoneIndices;
		if (zoneIndices == null || chunkData.availableZones == null)
		{
			return false;
		}
		foreach (int num in zoneIndices)
		{
			if (num >= 0 && num < chunkData.availableZones.Count && ProceduralZonePolicy.IsSpecialZone(chunkData.availableZones[num]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x00024588 File Offset: 0x00022788
	private bool TrySelectDroughtReliefCandidate(uint h, HashSet<ZoneData> zoneFilter, Dictionary<ChunkData, float> chunkMults, bool ignoreChunkMultipliers, PoiTypeWeightMultipliers typeWeights, out ChunkData selectedChunk, out int selectedSlot, out int selectedZone, bool excludeKombini = false, ChunkShape? requiredShape = null)
	{
		selectedChunk = null;
		selectedSlot = -1;
		selectedZone = -1;
		float num = 0f;
		for (int i = 0; i < this._smallChunks.Count; i++)
		{
			ChunkData chunkData = this._smallChunks[i];
			if (!(chunkData == null) && chunkData.gridSize.x <= 1 && chunkData.gridSize.y <= 1)
			{
				ChunkTemplate template = chunkData.template;
				if (((template != null) ? template.zoneSlots : null) != null && chunkData.availableZones != null && (requiredShape == null || chunkData.template.shape == requiredShape.Value))
				{
					float num2 = (ignoreChunkMultipliers ? Mathf.Max(0f, chunkData.spawnWeight) : ChunkSpawnResolver.AdjustedWeight(chunkData, chunkMults));
					if (num2 > 0f)
					{
						for (int j = 0; j < chunkData.template.zoneSlots.Count; j++)
						{
							ZoneSlot zoneSlot = chunkData.template.zoneSlots[j];
							for (int k = 0; k < chunkData.availableZones.Count; k++)
							{
								ZoneData zoneData = chunkData.availableZones[k];
								if (!(zoneData == null) && !ProceduralZonePolicy.IsExcludedZone(zoneData) && ProceduralZonePolicy.IsDroughtReliefZone(zoneData) && (!excludeKombini || !ProceduralZonePolicy.IsKombiniZone(zoneData)) && (zoneFilter == null || zoneFilter.Contains(zoneData)) && zoneData.IsCompatibleWith(zoneSlot))
								{
									float num3 = num2 * Mathf.Max(0f, zoneData.spawnWeight) * typeWeights.For(ProceduralZonePolicy.GetPoiKind(zoneData));
									if (num3 > 0f)
									{
										if (selectedChunk == null)
										{
											selectedChunk = chunkData;
											selectedSlot = j;
											selectedZone = k;
										}
										num += num3;
									}
								}
							}
						}
					}
				}
			}
		}
		if (selectedChunk == null)
		{
			return false;
		}
		if (num <= 0f)
		{
			return true;
		}
		float num4 = ChunkSpawnResolver.HashToFloat(ChunkSpawnResolver.Advance(h ^ 3679390609U)) * num;
		float num5 = 0f;
		for (int l = 0; l < this._smallChunks.Count; l++)
		{
			ChunkData chunkData2 = this._smallChunks[l];
			if (!(chunkData2 == null) && chunkData2.gridSize.x <= 1 && chunkData2.gridSize.y <= 1)
			{
				ChunkTemplate template2 = chunkData2.template;
				if (((template2 != null) ? template2.zoneSlots : null) != null && chunkData2.availableZones != null && (requiredShape == null || chunkData2.template.shape == requiredShape.Value))
				{
					float num6 = (ignoreChunkMultipliers ? Mathf.Max(0f, chunkData2.spawnWeight) : ChunkSpawnResolver.AdjustedWeight(chunkData2, chunkMults));
					if (num6 > 0f)
					{
						for (int m = 0; m < chunkData2.template.zoneSlots.Count; m++)
						{
							ZoneSlot zoneSlot2 = chunkData2.template.zoneSlots[m];
							for (int n = 0; n < chunkData2.availableZones.Count; n++)
							{
								ZoneData zoneData2 = chunkData2.availableZones[n];
								if (!(zoneData2 == null) && !ProceduralZonePolicy.IsExcludedZone(zoneData2) && ProceduralZonePolicy.IsDroughtReliefZone(zoneData2) && (!excludeKombini || !ProceduralZonePolicy.IsKombiniZone(zoneData2)) && (zoneFilter == null || zoneFilter.Contains(zoneData2)) && zoneData2.IsCompatibleWith(zoneSlot2))
								{
									float num7 = num6 * Mathf.Max(0f, zoneData2.spawnWeight) * typeWeights.For(ProceduralZonePolicy.GetPoiKind(zoneData2));
									if (num7 > 0f)
									{
										num5 += num7;
										selectedChunk = chunkData2;
										selectedSlot = m;
										selectedZone = n;
										if (num4 <= num5)
										{
											return true;
										}
									}
								}
							}
						}
					}
				}
			}
		}
		return true;
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x0002496C File Offset: 0x00022B6C
	private static int SelectSpecialZoneIndexWithRecency(ChunkData chunkData, ZoneSlot slot, uint h, HashSet<ZoneData> zoneFilter, PoiTypeWeightMultipliers typeWeights)
	{
		if (((chunkData != null) ? chunkData.availableZones : null) == null || slot == null)
		{
			return -1;
		}
		float num = 0f;
		int num2 = -1;
		for (int i = 0; i < chunkData.availableZones.Count; i++)
		{
			ZoneData zoneData = chunkData.availableZones[i];
			if (!(zoneData == null) && !ProceduralZonePolicy.IsExcludedZone(zoneData) && ProceduralZonePolicy.IsSpecialZone(zoneData) && (zoneFilter == null || zoneFilter.Contains(zoneData)) && zoneData.IsCompatibleWith(slot))
			{
				float num3 = Mathf.Max(0f, zoneData.spawnWeight) * typeWeights.For(ProceduralZonePolicy.GetPoiKind(zoneData));
				if (num3 > 0f)
				{
					if (num2 < 0)
					{
						num2 = i;
					}
					num += num3;
				}
			}
		}
		if (num2 < 0 || num <= 0f)
		{
			return -1;
		}
		float num4 = ChunkSpawnResolver.HashToFloat(ChunkSpawnResolver.Advance(h ^ 2722868949U)) * num;
		float num5 = 0f;
		int num6 = num2;
		for (int j = 0; j < chunkData.availableZones.Count; j++)
		{
			ZoneData zoneData2 = chunkData.availableZones[j];
			if (!(zoneData2 == null) && !ProceduralZonePolicy.IsExcludedZone(zoneData2) && ProceduralZonePolicy.IsSpecialZone(zoneData2) && (zoneFilter == null || zoneFilter.Contains(zoneData2)) && zoneData2.IsCompatibleWith(slot))
			{
				float num7 = Mathf.Max(0f, zoneData2.spawnWeight) * typeWeights.For(ProceduralZonePolicy.GetPoiKind(zoneData2));
				if (num7 > 0f)
				{
					num6 = j;
					num5 += num7;
					if (num4 <= num5)
					{
						return j;
					}
				}
			}
		}
		return num6;
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x00024AF4 File Offset: 0x00022CF4
	private ChunkSpawnResolver.SpawnEntry BuildDroughtReliefEntry(Vector2Int cell, ChunkData chunkData, int forcedSlotIndex, int forcedZoneIndex, uint h, HashSet<ZoneData> biomeSet, HashSet<ZoneData> fallbackSet, PoiTypeWeightMultipliers typeWeights, bool captureZoneIndices)
	{
		if (!(chunkData == null))
		{
			ChunkTemplate template = chunkData.template;
			if (((template != null) ? template.zoneSlots : null) != null && chunkData.availableZones != null && forcedSlotIndex >= 0 && forcedSlotIndex < chunkData.template.zoneSlots.Count && forcedZoneIndex >= 0 && forcedZoneIndex < chunkData.availableZones.Count)
			{
				ChunkData chunkData2;
				int num;
				int num2;
				if (ProceduralZonePolicy.IsKombiniZone(chunkData.availableZones[forcedZoneIndex]) && !this.PassesKombiniAdmission(cell) && this.TrySelectKombiniAlternative(cell, chunkData.template, typeWeights, out chunkData2, out num, out num2))
				{
					chunkData = chunkData2;
					forcedSlotIndex = num;
					forcedZoneIndex = num2;
				}
				h = ChunkSpawnResolver.Advance(h ^ 2722868949U);
				int num3 = (int)(h % 4U);
				int count = chunkData.template.zoneSlots.Count;
				int[] array = (captureZoneIndices ? new int[count] : null);
				HashSet<ZoneData> zoneSelectionScratch = this._zoneSelectionScratch;
				zoneSelectionScratch.Clear();
				for (int i = 0; i < count; i++)
				{
					h = ChunkSpawnResolver.Advance(h);
					int num4;
					if (i == forcedSlotIndex)
					{
						num4 = forcedZoneIndex;
					}
					else
					{
						num4 = ChunkSpawnResolver.SelectZoneIndexWithFallback(chunkData, chunkData.template.zoneSlots[i], h, biomeSet, fallbackSet, zoneSelectionScratch, true);
					}
					if (captureZoneIndices)
					{
						array[i] = num4;
					}
					if (num4 >= 0 && num4 < chunkData.availableZones.Count && chunkData.availableZones[num4] != null)
					{
						zoneSelectionScratch.Add(chunkData.availableZones[num4]);
					}
				}
				return new ChunkSpawnResolver.SpawnEntry(chunkData, num3, array, false);
			}
		}
		return default(ChunkSpawnResolver.SpawnEntry);
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x00024C80 File Offset: 0x00022E80
	public ChunkSpawnResolver.SpawnEntry ResolveSubwayConnection(Vector2Int cell, ChunkData station, int rotation)
	{
		if (!ChunkSpawnResolver.IsSubwayChunk(station))
		{
			return default(ChunkSpawnResolver.SpawnEntry);
		}
		uint num = ChunkSpawnResolver.Hash((uint)this._sessionSeed, (uint)cell.x, (uint)cell.y);
		ChunkSpawnResolver.SpawnEntry spawnEntry = this.BuildEntry(cell, station, num, null, null, false);
		return new ChunkSpawnResolver.SpawnEntry(station, rotation, spawnEntry.zoneIndices, false);
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x00024CD4 File Offset: 0x00022ED4
	private ChunkSpawnResolver.SpawnEntry BuildEntry(Vector2Int cell, ChunkData chunkData, uint h, HashSet<ZoneData> biomeSet, HashSet<ZoneData> fallbackSet, bool excludeSpecialZones)
	{
		bool flag;
		ProceduralPoiKind proceduralPoiKind;
		return this.BuildEntry(cell, chunkData, h, biomeSet, fallbackSet, excludeSpecialZones, true, out flag, out proceduralPoiKind, false);
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x00024CF8 File Offset: 0x00022EF8
	private ChunkSpawnResolver.SpawnEntry BuildEntry(Vector2Int cell, ChunkData chunkData, uint h, HashSet<ZoneData> biomeSet, HashSet<ZoneData> fallbackSet, bool excludeSpecialZones, bool captureZoneIndices, out bool containsSpecial, out ProceduralPoiKind selectedKind, bool captureSelectedKind = false)
	{
		containsSpecial = ProceduralZonePolicy.IsAlwaysSpecialChunk(chunkData);
		selectedKind = (chunkData.containsSubway ? ProceduralPoiKind.Subway : ProceduralPoiKind.None);
		h = ChunkSpawnResolver.Advance(h);
		int num = (int)(h % 4U);
		int[] array = null;
		if (chunkData.availableZones != null && chunkData.template != null && chunkData.template.zoneSlots != null && chunkData.template.zoneSlots.Count > 0)
		{
			HashSet<ZoneData> zoneSelectionScratch = this._zoneSelectionScratch;
			zoneSelectionScratch.Clear();
			bool flag = false;
			int count = chunkData.template.zoneSlots.Count;
			if (captureZoneIndices)
			{
				array = new int[count];
			}
			for (int i = 0; i < count; i++)
			{
				h = ChunkSpawnResolver.Advance(h);
				ZoneSlot zoneSlot = chunkData.template.zoneSlots[i];
				bool flag2 = excludeSpecialZones || flag;
				int num2 = ChunkSpawnResolver.SelectZoneIndexWithFallback(chunkData, zoneSlot, h, biomeSet, fallbackSet, zoneSelectionScratch, flag2);
				if (!flag2 && num2 >= 0 && ProceduralZonePolicy.IsKombiniZone(chunkData.availableZones[num2]) && !ChunkSpawnResolver.PassesKombiniSpawnGate(h))
				{
					num2 = ChunkSpawnResolver.SelectZoneIndexWithFallback(chunkData, zoneSlot, h, biomeSet, fallbackSet, zoneSelectionScratch, true);
				}
				if (captureZoneIndices)
				{
					array[i] = num2;
				}
				if (num2 >= 0 && chunkData.availableZones[num2] != null)
				{
					zoneSelectionScratch.Add(chunkData.availableZones[num2]);
					if (ProceduralZonePolicy.IsSpecialZone(chunkData.availableZones[num2]))
					{
						containsSpecial = true;
						flag = true;
						if (captureSelectedKind)
						{
							selectedKind = ProceduralZonePolicy.GetPoiKind(chunkData.availableZones[num2]);
						}
					}
				}
			}
		}
		return new ChunkSpawnResolver.SpawnEntry(chunkData, num, array, false);
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x00024E90 File Offset: 0x00023090
	private static int SelectZoneIndexWithFallback(ChunkData chunkData, ZoneSlot slot, uint h, HashSet<ZoneData> biomeSet, HashSet<ZoneData> fallbackSet, HashSet<ZoneData> usedInChunk, bool excludeSpecialZones)
	{
		int num = ChunkSpawnResolver.SelectZoneIndex(chunkData, slot, h, biomeSet, usedInChunk, excludeSpecialZones);
		if (num < 0 && fallbackSet != null)
		{
			num = ChunkSpawnResolver.SelectZoneIndex(chunkData, slot, h, fallbackSet, usedInChunk, excludeSpecialZones);
		}
		if (num < 0)
		{
			num = ChunkSpawnResolver.SelectZoneIndex(chunkData, slot, h, biomeSet, null, excludeSpecialZones);
		}
		if (num < 0 && fallbackSet != null)
		{
			num = ChunkSpawnResolver.SelectZoneIndex(chunkData, slot, h, fallbackSet, null, excludeSpecialZones);
		}
		if (num < 0)
		{
			num = ChunkSpawnResolver.SelectZoneIndex(chunkData, slot, h, null, usedInChunk, excludeSpecialZones);
		}
		if (num < 0)
		{
			num = ChunkSpawnResolver.SelectZoneIndex(chunkData, slot, h, null, null, excludeSpecialZones);
		}
		return num;
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x00024F0D File Offset: 0x0002310D
	private static bool PassesKombiniSpawnGate(uint h)
	{
		return ChunkSpawnResolver.Advance(h ^ 3266489909U) % 3U == 0U;
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x00024F20 File Offset: 0x00023120
	private ChunkData SelectSmallFallbackChunk(uint h, Dictionary<ChunkData, float> chunkMults, ChunkData excludedChunk, bool excludeSpecialZones)
	{
		float num = 0f;
		ChunkData chunkData = null;
		for (int i = 0; i < this._smallChunks.Count; i++)
		{
			ChunkData chunkData2 = this._smallChunks[i];
			if (!(chunkData2 == null) && !(chunkData2 == excludedChunk) && (!excludeSpecialZones || !ProceduralZonePolicy.IsAlwaysSpecialChunk(chunkData2)))
			{
				if (chunkData == null)
				{
					chunkData = chunkData2;
				}
				num += ChunkSpawnResolver.AdjustedWeight(chunkData2, chunkMults);
			}
		}
		if (chunkData == null)
		{
			return null;
		}
		if (num <= 0f)
		{
			return chunkData;
		}
		float num2 = ChunkSpawnResolver.HashToFloat(ChunkSpawnResolver.Advance(h ^ 2496678331U)) * num;
		float num3 = 0f;
		ChunkData chunkData3 = chunkData;
		for (int j = 0; j < this._smallChunks.Count; j++)
		{
			ChunkData chunkData4 = this._smallChunks[j];
			if (!(chunkData4 == null) && !(chunkData4 == excludedChunk) && (!excludeSpecialZones || !ProceduralZonePolicy.IsAlwaysSpecialChunk(chunkData4)))
			{
				chunkData3 = chunkData4;
				num3 += ChunkSpawnResolver.AdjustedWeight(chunkData4, chunkMults);
				if (num2 <= num3)
				{
					return chunkData4;
				}
			}
		}
		return chunkData3;
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x0002502C File Offset: 0x0002322C
	private ChunkData EnforceSubwaySpacing(Vector2Int cell, uint selectionHash, ChunkData selected, Dictionary<ChunkData, float> currentChunkMultipliers)
	{
		if (!ChunkSpawnResolver.IsSubwayChunk(selected))
		{
			return selected;
		}
		uint num = this.SubwayPriority(cell);
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (i != 0 || j != 0)
				{
					Vector2Int vector2Int = cell + new Vector2Int(i, j);
					if (ChunkSpawnResolver.IsSubwayChunk(this.SelectBaseChunkForSpacing(vector2Int)))
					{
						uint num2 = this.SubwayPriority(vector2Int);
						if (num2 > num || (num2 == num && ChunkSpawnResolver.ComesBefore(vector2Int, cell)))
						{
							return this.SelectNonSubwayReplacement(selected, ChunkSpawnResolver.Advance(selectionHash ^ 2769414579U), currentChunkMultipliers);
						}
					}
				}
			}
		}
		return selected;
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x000250C0 File Offset: 0x000232C0
	private ChunkData SelectBaseChunkForSpacing(Vector2Int cell)
	{
		uint num = ChunkSpawnResolver.Hash((uint)this._sessionSeed, (uint)cell.x, (uint)cell.y);
		Dictionary<ChunkData, float> dictionary = null;
		BiomeConfig biomeForCell = this.GetBiomeForCell(cell);
		if (biomeForCell != null && this._biomeChunkMultipliers != null)
		{
			this._biomeChunkMultipliers.TryGetValue(biomeForCell, out dictionary);
		}
		return this.SelectChunk(num, dictionary);
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x0002511C File Offset: 0x0002331C
	private ChunkData SelectNonSubwayReplacement(ChunkData rejected, uint h, Dictionary<ChunkData, float> chunkMults)
	{
		List<ChunkData> list = ((rejected != null && rejected.gridSize.x <= 1 && rejected.gridSize.y <= 1) ? this._smallChunks : this._largeChunks);
		float num = 0f;
		ChunkData chunkData = null;
		for (int i = 0; i < list.Count; i++)
		{
			ChunkData chunkData2 = list[i];
			if (!(chunkData2 == null) && !ChunkSpawnResolver.IsSubwayChunk(chunkData2))
			{
				if (chunkData == null)
				{
					chunkData = chunkData2;
				}
				num += ChunkSpawnResolver.AdjustedWeight(chunkData2, chunkMults);
			}
		}
		if (chunkData == null)
		{
			Debug.LogError("[ChunkSpawnResolver] Impossibile separare due Metro: non esiste un chunk non-Metro della stessa dimensione.");
			return rejected;
		}
		if (num <= 0f)
		{
			return chunkData;
		}
		float num2 = ChunkSpawnResolver.HashToFloat(h) * num;
		float num3 = 0f;
		ChunkData chunkData3 = chunkData;
		for (int j = 0; j < list.Count; j++)
		{
			ChunkData chunkData4 = list[j];
			if (!(chunkData4 == null) && !ChunkSpawnResolver.IsSubwayChunk(chunkData4))
			{
				chunkData3 = chunkData4;
				num3 += ChunkSpawnResolver.AdjustedWeight(chunkData4, chunkMults);
				if (num2 <= num3)
				{
					return chunkData4;
				}
			}
		}
		return chunkData3;
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x0002522D File Offset: 0x0002342D
	private uint SubwayPriority(Vector2Int cell)
	{
		return ChunkSpawnResolver.Hash((uint)(this._sessionSeed ^ 1831565813), (uint)cell.x, (uint)cell.y);
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x0002524E File Offset: 0x0002344E
	private uint SpecialPriority(Vector2Int cell)
	{
		return ChunkSpawnResolver.Hash((uint)(this._sessionSeed ^ -1255572915), (uint)cell.x, (uint)cell.y);
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x0002526F File Offset: 0x0002346F
	private static bool ComesBefore(Vector2Int a, Vector2Int b)
	{
		return a.x < b.x || (a.x == b.x && a.y < b.y);
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x000252A5 File Offset: 0x000234A5
	public static bool IsSubwayChunk(ChunkData chunkData)
	{
		return chunkData != null && chunkData.containsSubway;
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x000252B8 File Offset: 0x000234B8
	public BiomeConfig GetBiomeForCell(Vector2Int cell)
	{
		if (!(this._biomeManager != null))
		{
			return null;
		}
		return BiomeField.GetBiomeForCell(this._biomeManager, this._sessionSeed, cell);
	}

	// Token: 0x060004A9 RID: 1193 RVA: 0x000252DC File Offset: 0x000234DC
	private ChunkData SelectChunk(uint h, Dictionary<ChunkData, float> chunkMults)
	{
		if (this._smallChunks.Count == 0 && this._largeChunks.Count == 0)
		{
			return null;
		}
		float num = ((chunkMults == null) ? this._smallChunksWeight : ChunkSpawnResolver.SumAdjustedWeight(this._smallChunks, chunkMults));
		float num2 = ((chunkMults == null) ? this._largeChunksWeight : ChunkSpawnResolver.SumAdjustedWeight(this._largeChunks, chunkMults));
		float num3 = num + num2;
		if (num3 <= 0f)
		{
			if (this._smallChunks.Count <= 0)
			{
				return this._largeChunks[0];
			}
			return this._smallChunks[0];
		}
		else
		{
			if (ChunkSpawnResolver.HashToFloat(h) * num3 < num)
			{
				return ChunkSpawnResolver.SelectWeighted(this._smallChunks, num, ChunkSpawnResolver.Advance(h), chunkMults);
			}
			return ChunkSpawnResolver.SelectWeighted(this._largeChunks, num2, ChunkSpawnResolver.Advance(h), chunkMults);
		}
	}

	// Token: 0x060004AA RID: 1194 RVA: 0x0002539C File Offset: 0x0002359C
	private static float AdjustedWeight(ChunkData c, Dictionary<ChunkData, float> chunkMults)
	{
		float num = Mathf.Max(0f, c.spawnWeight);
		float num2;
		if (chunkMults != null && chunkMults.TryGetValue(c, out num2))
		{
			num *= num2;
		}
		return num;
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x000253D0 File Offset: 0x000235D0
	private static float SumAdjustedWeight(List<ChunkData> list, Dictionary<ChunkData, float> chunkMults)
	{
		float num = 0f;
		for (int i = 0; i < list.Count; i++)
		{
			num += ChunkSpawnResolver.AdjustedWeight(list[i], chunkMults);
		}
		return num;
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x00025408 File Offset: 0x00023608
	private static ChunkData SelectWeighted(List<ChunkData> list, float totalWeight, uint h, Dictionary<ChunkData, float> chunkMults)
	{
		if (list.Count == 0)
		{
			return null;
		}
		if (list.Count == 1)
		{
			return list[0];
		}
		float num = ChunkSpawnResolver.HashToFloat(h) * totalWeight;
		float num2 = 0f;
		for (int i = 0; i < list.Count; i++)
		{
			num2 += ChunkSpawnResolver.AdjustedWeight(list[i], chunkMults);
			if (num <= num2)
			{
				return list[i];
			}
		}
		return list[list.Count - 1];
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x0002547C File Offset: 0x0002367C
	private static int SelectZoneIndex(ChunkData chunkData, ZoneSlot slot, uint h, HashSet<ZoneData> biomeFilter, HashSet<ZoneData> excluded, bool excludeSpecialZones)
	{
		List<ZoneData> availableZones = chunkData.availableZones;
		if (availableZones == null || availableZones.Count == 0)
		{
			return -1;
		}
		float num = 0f;
		int num2 = -1;
		int num3 = -1;
		for (int i = 0; i < availableZones.Count; i++)
		{
			ZoneData zoneData = availableZones[i];
			if (!(zoneData == null) && !ProceduralZonePolicy.IsExcludedZone(zoneData) && (!excludeSpecialZones || !ProceduralZonePolicy.IsSpecialZone(zoneData)) && zoneData.IsCompatibleWith(slot) && (biomeFilter == null || biomeFilter.Contains(zoneData)) && (excluded == null || !excluded.Contains(zoneData)))
			{
				if (num2 < 0)
				{
					num2 = i;
				}
				num3 = i;
				num += Mathf.Max(0f, zoneData.spawnWeight);
			}
		}
		if (num2 < 0)
		{
			return -1;
		}
		if (num <= 0f)
		{
			return num2;
		}
		float num4 = ChunkSpawnResolver.HashToFloat(h) * num;
		float num5 = 0f;
		for (int j = 0; j < availableZones.Count; j++)
		{
			ZoneData zoneData2 = availableZones[j];
			if (!(zoneData2 == null) && !ProceduralZonePolicy.IsExcludedZone(zoneData2) && (!excludeSpecialZones || !ProceduralZonePolicy.IsSpecialZone(zoneData2)) && zoneData2.IsCompatibleWith(slot) && (biomeFilter == null || biomeFilter.Contains(zoneData2)) && (excluded == null || !excluded.Contains(zoneData2)))
			{
				num5 += Mathf.Max(0f, zoneData2.spawnWeight);
				if (num4 <= num5)
				{
					return j;
				}
			}
		}
		return num3;
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x000255D0 File Offset: 0x000237D0
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

	// Token: 0x060004AF RID: 1199 RVA: 0x0000C1D6 File Offset: 0x0000A3D6
	private static uint Advance(uint h)
	{
		h ^= h >> 16;
		h *= 73244475U;
		h ^= h >> 16;
		return h;
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x0000C1F2 File Offset: 0x0000A3F2
	private static float HashToFloat(uint h)
	{
		return (h & 16777215U) / 16777216f;
	}

	// Token: 0x0400037A RID: 890
	private const int SpecialSpacingRadius = 2;

	// Token: 0x0400037B RID: 891
	public const int SeededNaturalPoiCacheCapacity = 4096;

	// Token: 0x0400037C RID: 892
	private readonly int _sessionSeed;

	// Token: 0x0400037D RID: 893
	private readonly bool _enableSeededPoiRelief;

	// Token: 0x0400037E RID: 894
	private readonly float _kombiniAdmissionMultiplier;

	// Token: 0x0400037F RID: 895
	private readonly uint _seededPoiReliefPattern;

	// Token: 0x04000380 RID: 896
	private readonly Dictionary<Vector2Int, bool> _naturalPoiAcceptance;

	// Token: 0x04000381 RID: 897
	private readonly IReadOnlyList<ChunkData> _availableChunks;

	// Token: 0x04000382 RID: 898
	private bool _lastPoiRestrictions;

	// Token: 0x04000383 RID: 899
	private readonly Dictionary<Vector2Int, ChunkSpawnResolver.SpawnEntry> _cache = new Dictionary<Vector2Int, ChunkSpawnResolver.SpawnEntry>(256);

	// Token: 0x04000384 RID: 900
	private readonly HashSet<ZoneData> _zoneSelectionScratch = new HashSet<ZoneData>();

	// Token: 0x04000385 RID: 901
	private Vector2Int _travelOffset;

	// Token: 0x04000386 RID: 902
	private readonly List<ChunkData> _smallChunks = new List<ChunkData>(16);

	// Token: 0x04000387 RID: 903
	private readonly List<ChunkData> _largeChunks = new List<ChunkData>(16);

	// Token: 0x04000388 RID: 904
	private float _smallChunksWeight;

	// Token: 0x04000389 RID: 905
	private float _largeChunksWeight;

	// Token: 0x0400038A RID: 906
	private int _maxLargeChunkWidth = 1;

	// Token: 0x0400038B RID: 907
	private int _maxLargeChunkHeight = 1;

	// Token: 0x0400038C RID: 908
	private readonly BiomeManager _biomeManager;

	// Token: 0x0400038D RID: 909
	private readonly Dictionary<BiomeConfig, HashSet<ZoneData>> _biomeZoneSets;

	// Token: 0x0400038E RID: 910
	private readonly Dictionary<BiomeConfig, Dictionary<ChunkData, float>> _biomeChunkMultipliers;

	// Token: 0x0200005B RID: 91
	public readonly struct SpawnEntry
	{
		// Token: 0x060004B1 RID: 1201 RVA: 0x0002562E File Offset: 0x0002382E
		public SpawnEntry(ChunkData chunk, int rot, int[] zones, bool presented = false)
		{
			this.chunkData = chunk;
			this.rotation = rot;
			this.wasPresented = presented;
			this.zoneIndices = zones;
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x0002564D File Offset: 0x0002384D
		public bool IsValid
		{
			get
			{
				return this.chunkData != null;
			}
		}

		// Token: 0x0400038F RID: 911
		public readonly ChunkData chunkData;

		// Token: 0x04000390 RID: 912
		public readonly int rotation;

		// Token: 0x04000391 RID: 913
		public readonly bool wasPresented;

		// Token: 0x04000392 RID: 914
		public readonly int[] zoneIndices;
	}
}
