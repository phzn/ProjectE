using System;
using UnityEngine;

// Token: 0x02000059 RID: 89
[Serializable]
public class ZoneSlot
{
	// Token: 0x06000469 RID: 1129 RVA: 0x00022B50 File Offset: 0x00020D50
	public ZoneSlot(Vector2Int pos, ChunkSize chunkSize, ZoneOccupancy occ, int rot = 0)
	{
		this.gridPosition = pos;
		this.parentChunkSize = chunkSize;
		this.occupancy = occ;
		this.rotation = rot;
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x00022B7C File Offset: 0x00020D7C
	public bool IsCompatibleWith(ZoneData zoneData)
	{
		return zoneData.targetChunkSize == this.parentChunkSize && zoneData.occupancy == this.occupancy;
	}

	// Token: 0x04000375 RID: 885
	[Tooltip("Posizione relativa della zona nel chunk (in unità di zona)")]
	public Vector2Int gridPosition;

	// Token: 0x04000376 RID: 886
	[Tooltip("Dimensione del chunk parent a cui appartiene questa slot")]
	public ChunkSize parentChunkSize;

	// Token: 0x04000377 RID: 887
	[Tooltip("Quanto spazio occupa questa slot rispetto al chunk")]
	public ZoneOccupancy occupancy = ZoneOccupancy.Full;

	// Token: 0x04000378 RID: 888
	[Tooltip("Rotazione iniziale della zona (0, 90, 180, 270)")]
	[Range(0f, 3f)]
	public int rotation;

	// Token: 0x04000379 RID: 889
	[Tooltip("Transform di riferimento dal prefab chunk (opzionale, usato per posizionamento preciso)")]
	public Transform spawnTransform;
}
