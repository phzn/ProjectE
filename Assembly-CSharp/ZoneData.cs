using System;
using UnityEngine;

// Token: 0x020000AE RID: 174
[CreateAssetMenu(fileName = "ZoneData", menuName = "MapGeneration/ZoneData", order = 1)]
public class ZoneData : ScriptableObject
{
	// Token: 0x060006F8 RID: 1784 RVA: 0x00031E44 File Offset: 0x00030044
	public bool IsCompatibleWith(ZoneSlot slot)
	{
		return this.targetChunkSize == slot.parentChunkSize && this.occupancy == slot.occupancy;
	}

	// Token: 0x040005C5 RID: 1477
	[Header("Zone Configuration")]
	[Tooltip("Prefab della zona da istanziare")]
	public GameObject zonePrefab;

	// Token: 0x040005C6 RID: 1478
	[Tooltip("Dimensione del chunk per cui è pensata questa zona")]
	public ChunkSize targetChunkSize;

	// Token: 0x040005C7 RID: 1479
	[Tooltip("Quanto spazio occupa rispetto al chunk parent")]
	public ZoneOccupancy occupancy = ZoneOccupancy.Full;

	// Token: 0x040005C8 RID: 1480
	[Header("Zone Properties")]
	[Tooltip("Nome descrittivo della zona (es: 'Residential Block', 'Park')")]
	public string zoneName;

	// Token: 0x040005C9 RID: 1481
	[Tooltip("Peso per la selezione random (maggiore = più probabile)")]
	[Range(0f, 100f)]
	public float spawnWeight = 1f;
}
