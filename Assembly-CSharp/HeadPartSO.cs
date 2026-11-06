using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000013 RID: 19
[CreateAssetMenu(fileName = "New Head Part", menuName = "Character Creator/Head Part")]
public class HeadPartSO : ScriptableObject
{
	// Token: 0x0400004D RID: 77
	[Tooltip("Nome della parte")]
	public string partName;

	// Token: 0x0400004E RID: 78
	[Tooltip("Icona della parte (opzionale)")]
	public Sprite icon;

	// Token: 0x0400004F RID: 79
	[Tooltip("Descrizione della parte (opzionale)")]
	[TextArea(2, 5)]
	public string description;

	// Token: 0x04000050 RID: 80
	[Tooltip("Tab di configurazione disponibili per questa parte")]
	public List<HeadConfigTabSO> configTabs = new List<HeadConfigTabSO>();
}
