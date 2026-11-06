using System;
using UnityEngine;

// Token: 0x02000011 RID: 17
[CreateAssetMenu(fileName = "New Head Config Tab", menuName = "Character Creator/Head Config Tab")]
public class HeadConfigTabSO : ScriptableObject
{
	// Token: 0x04000043 RID: 67
	[Tooltip("Nome della tab di configurazione")]
	public string tabName;

	// Token: 0x04000044 RID: 68
	[Tooltip("Icona della tab (opzionale)")]
	public Sprite icon;

	// Token: 0x04000045 RID: 69
	[Tooltip("Tipo di configurazione")]
	public HeadConfigTabSO.ConfigType configType;

	// Token: 0x04000046 RID: 70
	[Tooltip("Prefab del pannello UI da mostrare per questa tab (opzionale)")]
	public GameObject uiPrefab;

	// Token: 0x02000012 RID: 18
	public enum ConfigType
	{
		// Token: 0x04000048 RID: 72
		Style,
		// Token: 0x04000049 RID: 73
		Shapes,
		// Token: 0x0400004A RID: 74
		Expressions,
		// Token: 0x0400004B RID: 75
		Colors,
		// Token: 0x0400004C RID: 76
		Custom
	}
}
