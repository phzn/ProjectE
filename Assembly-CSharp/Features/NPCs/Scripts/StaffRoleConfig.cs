using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.NPCs.Scripts
{
	// Token: 0x02000EBD RID: 3773
	[Serializable]
	public class StaffRoleConfig
	{
		// Token: 0x0600710D RID: 28941 RVA: 0x002DE9A8 File Offset: 0x002DCBA8
		public GameObject GetRandomPrefab()
		{
			if (this.prefabs != null && this.prefabs.Count > 0)
			{
				List<GameObject> list = this.prefabs.FindAll((GameObject p) => p != null);
				if (list.Count > 0)
				{
					return list[Random.Range(0, list.Count)];
				}
			}
			return this.prefab;
		}

		// Token: 0x040073CE RID: 29646
		public string roleName;

		// Token: 0x040073CF RID: 29647
		public NPCRole role;

		// Token: 0x040073D0 RID: 29648
		[Tooltip("Prefab singolo (legacy). Se 'prefabs' ha elementi, questo viene ignorato.")]
		public GameObject prefab;

		// Token: 0x040073D1 RID: 29649
		[Tooltip("Lista di prefab tra cui scegliere casualmente per questo ruolo.")]
		public List<GameObject> prefabs = new List<GameObject>();

		// Token: 0x040073D2 RID: 29650
		[Range(0f, 10f)]
		public int minCount = 1;

		// Token: 0x040073D3 RID: 29651
		[Range(0f, 10f)]
		public int maxCount = 1;

		// Token: 0x040073D4 RID: 29652
		[Range(0f, 1f)]
		public float spawnProbability = 1f;

		// Token: 0x040073D5 RID: 29653
		[Tooltip("Punti di spawn specifici per questo ruolo. Se vuoto, usa lo spawn point globale.")]
		public List<Transform> spawnPoints = new List<Transform>();
	}
}
