using System;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008A8 RID: 2216
	[CreateAssetMenu(fileName = "New Config Tab", menuName = "Character Creator/Config Tab")]
	public class ConfigTabSO : ScriptableObject
	{
		// Token: 0x04003BB7 RID: 15287
		[Tooltip("Nome della tab di configurazione")]
		public string tabName;

		// Token: 0x04003BB8 RID: 15288
		[Tooltip("Icona della tab (opzionale)")]
		public Sprite icon;

		// Token: 0x04003BB9 RID: 15289
		[Tooltip("Tipo di configurazione")]
		public ConfigTabSO.ConfigType configType;

		// Token: 0x04003BBA RID: 15290
		[Tooltip("Prefab del pannello UI da mostrare per questa tab (opzionale)")]
		public GameObject uiPrefab;

		// Token: 0x020008A9 RID: 2217
		public enum ConfigType
		{
			// Token: 0x04003BBC RID: 15292
			Style,
			// Token: 0x04003BBD RID: 15293
			Shapes,
			// Token: 0x04003BBE RID: 15294
			Expressions,
			// Token: 0x04003BBF RID: 15295
			Colors,
			// Token: 0x04003BC0 RID: 15296
			Custom,
			// Token: 0x04003BC1 RID: 15297
			Decals,
			// Token: 0x04003BC2 RID: 15298
			Accessories
		}
	}
}
