using System;
using UnityEngine;

namespace Features.TrashcanLoot
{
	// Token: 0x02000BF9 RID: 3065
	public class TrashcanTierSpawnConfigSO : ScriptableObject
	{
		// Token: 0x040058CC RID: 22732
		[Header("Pesi tier (chance = peso / somma)")]
		[Min(0f)]
		public float commonWeight = 70f;

		// Token: 0x040058CD RID: 22733
		[Min(0f)]
		public float rareWeight = 22f;

		// Token: 0x040058CE RID: 22734
		[Min(0f)]
		public float legendaryWeight = 8f;

		// Token: 0x040058CF RID: 22735
		[Header("Prefab tier")]
		public GameObject commonPrefab;

		// Token: 0x040058D0 RID: 22736
		public GameObject rarePrefab;

		// Token: 0x040058D1 RID: 22737
		public GameObject legendaryPrefab;

		// Token: 0x040058D2 RID: 22738
		[Tooltip("Sul roll Common mantiene l'istanza baked se possiede tutti i componenti root e gli anchor richiesti dal Common corrente. Disattiva per forzare il vecchio swap.")]
		public bool keepCompatibleBakedCommon = true;

		// Token: 0x040058D3 RID: 22739
		[Header("Budget event-driven")]
		[Tooltip("Numero massimo di bidoni sostituiti/validati nello stesso frame.")]
		[Min(1f)]
		public int maxCansProcessedPerFrame = 1;

		// Token: 0x040058D4 RID: 22740
		[Tooltip("Budget CPU indicativo per frame. Viene sempre processato almeno un bidone.")]
		[Min(0.05f)]
		public float processingBudgetMs = 0.5f;

		// Token: 0x040058D5 RID: 22741
		[Header("Debug")]
		public bool logProcessing;
	}
}
