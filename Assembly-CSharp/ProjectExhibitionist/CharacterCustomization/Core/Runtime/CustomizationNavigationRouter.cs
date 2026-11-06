using System;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000AF8 RID: 2808
	public class CustomizationNavigationRouter : MonoBehaviour
	{
		// Token: 0x06004F92 RID: 20370 RVA: 0x001ED8E6 File Offset: 0x001EBAE6
		private void Awake()
		{
			if (this.hubController == null)
			{
				this.hubController = Object.FindObjectOfType<CustomizationHubController>(true);
			}
		}

		// Token: 0x06004F93 RID: 20371 RVA: 0x001ED902 File Offset: 0x001EBB02
		public void OpenCharacterCreator(bool createNewCharacter = true)
		{
			if (this.hubController == null)
			{
				Debug.LogError("[NavigationRouter] Missing CustomizationHubController reference.");
				return;
			}
			if (createNewCharacter)
			{
				this.hubController.OpenMainCharacterCustomization();
				return;
			}
			this.hubController.OpenSelectedTabCustomization();
		}

		// Token: 0x06004F94 RID: 20372 RVA: 0x001ED937 File Offset: 0x001EBB37
		public void OpenSelectedTabCustomization()
		{
			if (this.hubController == null)
			{
				Debug.LogError("[NavigationRouter] Missing CustomizationHubController reference.");
				return;
			}
			this.hubController.OpenSelectedTabCustomization();
		}

		// Token: 0x06004F95 RID: 20373 RVA: 0x001ED95D File Offset: 0x001EBB5D
		public void OpenForMainCharacter()
		{
			if (this.hubController == null)
			{
				Debug.LogError("[NavigationRouter] Missing CustomizationHubController reference.");
				return;
			}
			this.hubController.OpenMainCharacterCustomization();
		}

		// Token: 0x06004F96 RID: 20374 RVA: 0x001ED983 File Offset: 0x001EBB83
		public void OpenForNpc(string npcCharacterId)
		{
			if (this.hubController == null)
			{
				Debug.LogError("[NavigationRouter] Missing CustomizationHubController reference.");
				return;
			}
			if (string.IsNullOrWhiteSpace(npcCharacterId))
			{
				this.hubController.OpenNpcCustomization();
				return;
			}
			this.hubController.OpenNpcCustomization(npcCharacterId);
		}

		// Token: 0x06004F97 RID: 20375 RVA: 0x001ED9BE File Offset: 0x001EBBBE
		public void OpenMainCharacterCustomization(string characterId, bool isDefaultCharacter, string presetId, bool skipPresetAutoApply = false)
		{
			if (this.hubController == null)
			{
				Debug.LogError("[NavigationRouter] Missing CustomizationHubController reference.");
				return;
			}
			this.hubController.OpenMainCharacterCustomization(characterId, isDefaultCharacter, presetId, skipPresetAutoApply);
		}

		// Token: 0x06004F98 RID: 20376 RVA: 0x001ED9E9 File Offset: 0x001EBBE9
		public void OpenNpcCustomization(string npcCharacterId, bool isDefaultCharacter, string presetId, bool skipPresetAutoApply = false)
		{
			if (this.hubController == null)
			{
				Debug.LogError("[NavigationRouter] Missing CustomizationHubController reference.");
				return;
			}
			this.hubController.OpenNpcCustomization(npcCharacterId, isDefaultCharacter, presetId, skipPresetAutoApply);
		}

		// Token: 0x06004F99 RID: 20377 RVA: 0x001EDA14 File Offset: 0x001EBC14
		public void ReturnToHub()
		{
			if (this.hubController == null)
			{
				Debug.LogError("[NavigationRouter] Missing CustomizationHubController reference.");
				return;
			}
			this.hubController.BackToHub();
		}

		// Token: 0x04005095 RID: 20629
		[SerializeField]
		private CustomizationHubController hubController;
	}
}
