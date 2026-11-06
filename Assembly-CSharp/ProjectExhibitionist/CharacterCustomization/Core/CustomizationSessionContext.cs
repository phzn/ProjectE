using System;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core
{
	// Token: 0x02000AD3 RID: 2771
	[Serializable]
	public class CustomizationSessionContext
	{
		// Token: 0x06004E9F RID: 20127 RVA: 0x001E810F File Offset: 0x001E630F
		public bool IsValid()
		{
			return this.openInHub || (this.targetType != CustomizationTargetType.None && !string.IsNullOrWhiteSpace(this.characterId));
		}

		// Token: 0x06004EA0 RID: 20128 RVA: 0x001E8133 File Offset: 0x001E6333
		public static CustomizationSessionContext Create(CustomizationTargetType targetType, string characterId, bool isDefaultCharacter, string sourceScene)
		{
			return new CustomizationSessionContext
			{
				openInHub = false,
				targetType = targetType,
				characterId = characterId,
				isDefaultCharacter = isDefaultCharacter,
				sourceScene = sourceScene
			};
		}

		// Token: 0x06004EA1 RID: 20129 RVA: 0x001E815D File Offset: 0x001E635D
		public static CustomizationSessionContext CreateHub(string sourceScene)
		{
			return new CustomizationSessionContext
			{
				openInHub = true,
				sourceScene = sourceScene
			};
		}

		// Token: 0x04004FF5 RID: 20469
		[Header("Entry")]
		public bool openInHub;

		// Token: 0x04004FF6 RID: 20470
		[Header("Target")]
		public CustomizationTargetType targetType;

		// Token: 0x04004FF7 RID: 20471
		public string characterId;

		// Token: 0x04004FF8 RID: 20472
		public bool isDefaultCharacter;

		// Token: 0x04004FF9 RID: 20473
		[Header("Preset")]
		public string presetId;

		// Token: 0x04004FFA RID: 20474
		public bool skipPresetAutoApply;

		// Token: 0x04004FFB RID: 20475
		[Header("Runtime")]
		public Transform spawnPoint;

		// Token: 0x04004FFC RID: 20476
		public GameObject runtimeInstance;

		// Token: 0x04004FFD RID: 20477
		[Header("Debug")]
		public string sourceScene;
	}
}
