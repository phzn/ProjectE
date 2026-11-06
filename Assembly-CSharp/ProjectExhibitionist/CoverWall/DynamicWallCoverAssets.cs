using System;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace ProjectExhibitionist.CoverWall
{
	// Token: 0x020009FA RID: 2554
	public class DynamicWallCoverAssets : ScriptableObject
	{
		// Token: 0x060048C0 RID: 18624 RVA: 0x001BB3D8 File Offset: 0x001B95D8
		public static DynamicWallCoverAssets Load()
		{
			return Resources.Load<DynamicWallCoverAssets>("DynamicWallCoverAssets");
		}

		// Token: 0x040048A2 RID: 18594
		public const string ResourcePath = "DynamicWallCoverAssets";

		// Token: 0x040048A3 RID: 18595
		public InteractionConfigSO interactionConfig;

		// Token: 0x040048A4 RID: 18596
		public InteractablePromptUI promptPrefab;
	}
}
