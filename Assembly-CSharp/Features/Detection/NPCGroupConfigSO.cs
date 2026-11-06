using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012C1 RID: 4801
	[CreateAssetMenu(fileName = "NPCGroupConfig_", menuName = "Detection/NPC Group Config")]
	public class NPCGroupConfigSO : ScriptableObject
	{
		// Token: 0x040099D7 RID: 39383
		[Header("Composizione gruppo")]
		[Tooltip("members[0] = initiator/leader, members[1+] = additionalActors.\nL'ordine corrisponde agli slot di EncounterProfileSO.actorSlots.")]
		public TroupeMemberSlot[] members;

		// Token: 0x040099D8 RID: 39384
		[Header("Encounter")]
		[Tooltip("Profilo encounter da usare quando il gruppo ingaggia il player.\nConfigura camera, dialogo multi-speaker, overlay UI, post-process.")]
		public EncounterProfileSO encounterProfile;

		// Token: 0x040099D9 RID: 39385
		[Header("Spawn")]
		[Tooltip("Peso relativo per la selezione casuale del tipo di gruppo da spawnare.\n0 = mai selezionato. Valori più alti = più frequente.")]
		[Range(0f, 10f)]
		public float spawnWeight = 1f;
	}
}
