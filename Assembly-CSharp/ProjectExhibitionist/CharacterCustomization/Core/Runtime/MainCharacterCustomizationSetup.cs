using System;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000B10 RID: 2832
	public class MainCharacterCustomizationSetup : MonoBehaviour
	{
		// Token: 0x06005017 RID: 20503 RVA: 0x001EFB4E File Offset: 0x001EDD4E
		public void OpenMainCharacterCustomization()
		{
			if (this.hubController == null)
			{
				Debug.LogError("[MainCharacterCustomizationSetup] Missing CustomizationHubController reference.");
				return;
			}
			this.hubController.OpenMainCharacterCustomization(this.mainCharacterId, this.isDefaultCharacter, string.Empty, false);
		}

		// Token: 0x06005018 RID: 20504 RVA: 0x001EFB86 File Offset: 0x001EDD86
		public void OpenMainCharacterPreset(string presetId)
		{
			if (this.hubController == null)
			{
				Debug.LogError("[MainCharacterCustomizationSetup] Missing CustomizationHubController reference.");
				return;
			}
			this.hubController.OpenMainCharacterCustomization(this.mainCharacterId, this.isDefaultCharacter, presetId, false);
		}

		// Token: 0x040050EA RID: 20714
		[Header("References")]
		[SerializeField]
		private CustomizationHubController hubController;

		// Token: 0x040050EB RID: 20715
		[Header("Main Character")]
		[SerializeField]
		private string mainCharacterId = "Ciel";

		// Token: 0x040050EC RID: 20716
		[SerializeField]
		private bool isDefaultCharacter = true;
	}
}
