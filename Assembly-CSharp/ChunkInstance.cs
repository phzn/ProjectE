using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000039 RID: 57
public class ChunkInstance
{
	// Token: 0x060001CA RID: 458 RVA: 0x0000CAD1 File Offset: 0x0000ACD1
	public ChunkInstance(ChunkData data, Vector3 position, Quaternion rot)
	{
		this.chunkData = data;
		this.worldPosition = position;
		this.rotation = rot;
	}

	// Token: 0x060001CB RID: 459 RVA: 0x0000CB08 File Offset: 0x0000AD08
	public List<Vector2Int> GetOccupiedCells()
	{
		List<Vector2Int> list = new List<Vector2Int>();
		if (this.chunkData == null)
		{
			return list;
		}
		for (int i = 0; i < this.chunkData.gridSize.x; i++)
		{
			for (int j = 0; j < this.chunkData.gridSize.y; j++)
			{
				list.Add(this.originCell + new Vector2Int(i, j));
			}
		}
		return list;
	}

	// Token: 0x04000178 RID: 376
	public ChunkData chunkData;

	// Token: 0x04000179 RID: 377
	public GameObject chunkRoot;

	// Token: 0x0400017A RID: 378
	public Vector3 worldPosition;

	// Token: 0x0400017B RID: 379
	public Quaternion rotation;

	// Token: 0x0400017C RID: 380
	public Vector2Int originCell;

	// Token: 0x0400017D RID: 381
	public bool preservesCommittedZoneIdentity;

	// Token: 0x0400017E RID: 382
	public List<GameObject> spawnedZones = new List<GameObject>();

	// Token: 0x0400017F RID: 383
	public Collider[] cachedColliders;

	// Token: 0x04000180 RID: 384
	public bool collidersCurrentlyEnabled = true;

	// Token: 0x04000181 RID: 385
	public bool collidersCacheDirty = true;
}
