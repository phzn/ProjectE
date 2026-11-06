using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000035 RID: 53
[CreateAssetMenu(fileName = "ChunkData", menuName = "MapGeneration/ChunkData", order = 0)]
public class ChunkData : ScriptableObject
{
	// Token: 0x1700004B RID: 75
	// (get) Token: 0x060001BB RID: 443 RVA: 0x0000C6B2 File Offset: 0x0000A8B2
	public bool HasCustomZones
	{
		get
		{
			return this.availableZones != null && this.availableZones.Count > 0;
		}
	}

	// Token: 0x060001BC RID: 444 RVA: 0x0000C6CC File Offset: 0x0000A8CC
	public bool CanSpawnInBiome(BiomeConfig currentBiome)
	{
		return this.allowedBiomes == null || this.allowedBiomes.Count == 0 || this.allowedBiomes.Contains(currentBiome);
	}

	// Token: 0x060001BD RID: 445 RVA: 0x0000C6F1 File Offset: 0x0000A8F1
	public Vector2 GetWorldSize(float cellSize)
	{
		return new Vector2((float)this.gridSize.x * cellSize, (float)this.gridSize.y * cellSize);
	}

	// Token: 0x060001BE RID: 446 RVA: 0x0000C714 File Offset: 0x0000A914
	public List<ZoneData> GetCompatibleZones(ZoneSlot slot)
	{
		return this.availableZones.Where<ZoneData>((ZoneData z) => z.IsCompatibleWith(slot)).ToList<ZoneData>();
	}

	// Token: 0x060001BF RID: 447 RVA: 0x0000C74C File Offset: 0x0000A94C
	public ZoneData GetRandomZoneForSlot(ZoneSlot slot, bool excludeSpecialZones = false)
	{
		if (this.availableZones == null || this.availableZones.Count == 0)
		{
			Debug.LogWarning("[ChunkData] Nessuna zona disponibile nel chunk '" + base.name + "'");
			return null;
		}
		float num = 0f;
		ZoneData zoneData = null;
		ZoneData zoneData2 = null;
		for (int i = 0; i < this.availableZones.Count; i++)
		{
			ZoneData zoneData3 = this.availableZones[i];
			if (!(zoneData3 == null) && !ProceduralZonePolicy.IsExcludedZone(zoneData3) && (!excludeSpecialZones || !ProceduralZonePolicy.IsSpecialZone(zoneData3)) && zoneData3.IsCompatibleWith(slot))
			{
				if (zoneData == null)
				{
					zoneData = zoneData3;
				}
				zoneData2 = zoneData3;
				num += Mathf.Max(0f, zoneData3.spawnWeight);
			}
		}
		if (zoneData == null)
		{
			Debug.LogWarning(string.Format("[ChunkData] Nessuna zona compatibile trovata per slot {0} (chunk: {1}, occupancy: {2})", slot.gridPosition, slot.parentChunkSize, slot.occupancy));
			return null;
		}
		if (num <= 0f)
		{
			return zoneData;
		}
		float num2 = Random.Range(0f, num);
		float num3 = 0f;
		for (int j = 0; j < this.availableZones.Count; j++)
		{
			ZoneData zoneData4 = this.availableZones[j];
			if (!(zoneData4 == null) && !ProceduralZonePolicy.IsExcludedZone(zoneData4) && (!excludeSpecialZones || !ProceduralZonePolicy.IsSpecialZone(zoneData4)) && zoneData4.IsCompatibleWith(slot))
			{
				num3 += Mathf.Max(0f, zoneData4.spawnWeight);
				if (num2 <= num3)
				{
					return zoneData4;
				}
			}
		}
		return zoneData2;
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x0000C8D4 File Offset: 0x0000AAD4
	private void OnValidate()
	{
		if (this.template == null)
		{
			Debug.LogWarning("[ChunkData] '" + base.name + "' non ha un template assegnato!");
			return;
		}
		if (this.HasCustomZones)
		{
			using (var enumerator = this.template.zoneSlots.Select((ZoneSlot s) => new { s.parentChunkSize, s.occupancy }).Distinct().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					<>f__AnonymousType0<ChunkSize, ZoneOccupancy> config = enumerator.Current;
					if (this.availableZones.Count<ZoneData>((ZoneData z) => z.targetChunkSize == config.parentChunkSize && z.occupancy == config.occupancy) == 0)
					{
						Debug.LogWarning(string.Format("[ChunkData] '{0}' ha zone custom ma nessuna compatibile per chunk {1} con occupancy {2}!", base.name, config.parentChunkSize, config.occupancy));
					}
				}
				return;
			}
		}
		if (this.allowedBiomes != null && this.allowedBiomes.Count > 0)
		{
			Debug.Log("[ChunkData] '" + base.name + "' userà zone dai BiomeConfig: " + string.Join(", ", this.allowedBiomes.Select<BiomeConfig, string>((BiomeConfig b) => b.biomeName)));
		}
	}

	// Token: 0x0400016D RID: 365
	[Header("Chunk Template")]
	[Tooltip("Template che definisce la forma e le slot del chunk")]
	public ChunkTemplate template;

	// Token: 0x0400016E RID: 366
	[Header("Grid Size")]
	[Tooltip("Dimensione del chunk in celle griglia (1x1 = 60x60, 2x2 = 120x120, etc.)")]
	public Vector2Int gridSize = new Vector2Int(1, 1);

	// Token: 0x0400016F RID: 367
	[Header("Available Zones (Override)")]
	[Tooltip("Pool di zone disponibili per questo chunk. Lascia vuoto per usare le zone dal BiomeConfig. Popola solo per chunk speciali.")]
	public List<ZoneData> availableZones = new List<ZoneData>();

	// Token: 0x04000170 RID: 368
	[Header("Biome Constraints")]
	[Tooltip("Biomi in cui questo chunk può spawnare. Lascia vuoto per spawnare in qualsiasi bioma. Popola per chunk specifici (es: chunk Metropolis con strade larghe).")]
	public List<BiomeConfig> allowedBiomes = new List<BiomeConfig>();

	// Token: 0x04000171 RID: 369
	[Header("Spawn Settings")]
	[Tooltip("Peso per la selezione random di questo chunk")]
	[Range(0f, 100f)]
	public float spawnWeight = 1f;

	// Token: 0x04000172 RID: 370
	[Header("Special Features")]
	[Tooltip("Questo chunk contiene una stazione Metro funzionante. Il resolver usa il flag per impedire che due chunk Metro vengano generati a contatto, anche in diagonale.")]
	public bool containsSubway;
}
