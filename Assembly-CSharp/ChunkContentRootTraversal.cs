using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000034 RID: 52
public static class ChunkContentRootTraversal
{
	// Token: 0x060001B9 RID: 441 RVA: 0x0000C5F8 File Offset: 0x0000A7F8
	public static void Collect(ChunkInstance chunk, bool includeInactive, List<GameObject> roots)
	{
		roots.Clear();
		if (chunk == null)
		{
			return;
		}
		ChunkContentRootTraversal.AddIfUncovered(chunk.chunkRoot, includeInactive, roots);
		if (chunk.spawnedZones == null)
		{
			return;
		}
		for (int i = 0; i < chunk.spawnedZones.Count; i++)
		{
			ChunkContentRootTraversal.AddIfUncovered(chunk.spawnedZones[i], includeInactive, roots);
		}
	}

	// Token: 0x060001BA RID: 442 RVA: 0x0000C650 File Offset: 0x0000A850
	private static void AddIfUncovered(GameObject candidate, bool includeInactive, List<GameObject> roots)
	{
		if (candidate == null)
		{
			return;
		}
		Transform transform = candidate.transform;
		for (int i = 0; i < roots.Count; i++)
		{
			GameObject gameObject = roots[i];
			if (candidate == gameObject)
			{
				return;
			}
			if ((includeInactive || candidate.activeInHierarchy) && transform.IsChildOf(gameObject.transform))
			{
				return;
			}
		}
		roots.Add(candidate);
	}
}
