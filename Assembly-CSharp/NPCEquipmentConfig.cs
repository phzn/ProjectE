using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B9 RID: 185
[CreateAssetMenu(fileName = "NPCEquipmentConfig", menuName = "NPC/Equipment Config")]
public class NPCEquipmentConfig : ScriptableObject
{
	// Token: 0x04000658 RID: 1624
	[Header("Equipment Entries")]
	[Tooltip("Lista di tutti i prop configurabili")]
	public List<NPCEquipmentConfig.EquipmentEntry> equipmentEntries = new List<NPCEquipmentConfig.EquipmentEntry>();

	// Token: 0x04000659 RID: 1625
	[Header("Action Reference (Read-Only Info)")]
	[TextArea(3, 10)]
	[Tooltip("Riferimento per gli indici delle azioni. Modifica questo testo per documentare i tuoi indici.")]
	public string actionIndexReference = "0 = None (Normal)\n1 = WritingPhone\n2 = TalkingPhone\n// Aggiungi qui i tuoi indici personalizzati";

	// Token: 0x020000BA RID: 186
	[Serializable]
	public class EquipmentEntry
	{
		// Token: 0x0400065A RID: 1626
		[Tooltip("Nome del prop da cercare nei children dell'NPC")]
		public string propName;

		// Token: 0x0400065B RID: 1627
		[Tooltip("Indici delle azioni che richiedono questo prop (UpperBodyAction values)")]
		public List<int> activeForActionIndices = new List<int>();

		// Token: 0x0400065C RID: 1628
		[Tooltip("Se true, attivo anche durante locomozione con queste azioni")]
		public bool activeForLocomotion = true;
	}
}
