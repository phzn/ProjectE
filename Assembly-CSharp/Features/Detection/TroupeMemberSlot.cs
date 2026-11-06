using System;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012C0 RID: 4800
	[Serializable]
	public class TroupeMemberSlot
	{
		// Token: 0x040099D2 RID: 39378
		[Tooltip("Etichetta del ruolo per debug (es. 'Interviewer', 'Cameraman', 'Tech')")]
		public string roleTag;

		// Token: 0x040099D3 RID: 39379
		[Tooltip("Asset NPC da spawnare per questo ruolo")]
		public CharacterObject npcAsset;

		// Token: 0x040099D4 RID: 39380
		[Tooltip("Prefab base override per questo slot.\nSe assegnato, sostituisce il characterBasePrefab dello spawner e bypassa il pool generico.\nUsare per NPC che richiedono componenti aggiuntivi pre-configurati (es. NPCReporterBase con NPCPropHolder).")]
		public OutfitSystem basePrefabOverride;

		// Token: 0x040099D5 RID: 39381
		[Tooltip("Prop da equipaggiare dopo lo spawn (null = nessun prop)")]
		public NPCPropDefinitionSO prop;

		// Token: 0x040099D6 RID: 39382
		[Tooltip("AnimatorOverrideController specifico del ruolo.\nSovrascrive il controller base dell'NPC con clip di prop-holding.\nNull = usa il controller base invariato.")]
		public AnimatorOverrideController animatorOverride;
	}
}
