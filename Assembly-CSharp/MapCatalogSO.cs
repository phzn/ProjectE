using System;
using UnityEngine;

// Token: 0x02000081 RID: 129
[CreateAssetMenu(fileName = "MapCatalog", menuName = "MapGeneration/Map Catalog", order = 12)]
public class MapCatalogSO : ScriptableObject
{
	// Token: 0x0400047C RID: 1148
	[Header("Chunk Data")]
	[Tooltip("Tutti i ChunkData spawnabili dalla mappa procedurale.")]
	public ChunkData[] availableChunks;

	// Token: 0x0400047D RID: 1149
	[Header("Biomes (Optional)")]
	[Tooltip("Se assegnato, le zone vengono scelte per quartieri coerenti (Residential/Suburban/City) tramite BiomeField.")]
	public BiomeManager biomeManager;
}
