using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000014 RID: 20
[CreateAssetMenu(fileName = "New Head SubCategory", menuName = "Character Creator/Head SubCategory")]
public class HeadSubCategorySO : ScriptableObject
{
	// Token: 0x04000051 RID: 81
	[Tooltip("Nome della sottocategoria")]
	public string subCategoryName;

	// Token: 0x04000052 RID: 82
	[Tooltip("Icona della sottocategoria (opzionale)")]
	public Sprite icon;

	// Token: 0x04000053 RID: 83
	[Tooltip("Descrizione della sottocategoria (opzionale)")]
	[TextArea(2, 5)]
	public string description;

	// Token: 0x04000054 RID: 84
	[Tooltip("La sottocategoria ha parti distinte? (es: Hair Front/Back)")]
	public bool hasParts;

	// Token: 0x04000055 RID: 85
	[Tooltip("Parti della sottocategoria (se hasParts = true)")]
	public List<HeadPartSO> parts = new List<HeadPartSO>();

	// Token: 0x04000056 RID: 86
	[Tooltip("Tab di configurazione (se hasParts = false)")]
	public List<HeadConfigTabSO> configTabs = new List<HeadConfigTabSO>();
}
