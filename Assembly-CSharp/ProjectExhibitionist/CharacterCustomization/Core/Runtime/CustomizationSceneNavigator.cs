using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000B01 RID: 2817
	public class CustomizationSceneNavigator : MonoBehaviour
	{
		// Token: 0x06004FBD RID: 20413 RVA: 0x001EE388 File Offset: 0x001EC588
		public void OpenCustomizationHub()
		{
			CustomizationSessionContext customizationSessionContext = CustomizationSessionContext.CreateHub(SceneManager.GetActiveScene().name);
			this.OpenCustomization(customizationSessionContext);
		}

		// Token: 0x06004FBE RID: 20414 RVA: 0x001EE3AF File Offset: 0x001EC5AF
		public void OpenMainCharacterCustomization(string characterId)
		{
			this.OpenMainCharacterCustomization(characterId, this.defaultMainCharacterIsLocked, string.Empty);
		}

		// Token: 0x06004FBF RID: 20415 RVA: 0x001EE3C4 File Offset: 0x001EC5C4
		public void OpenMainCharacterCustomization(string characterId, bool isDefaultCharacter, string presetId)
		{
			CustomizationSessionContext customizationSessionContext = CustomizationSessionContext.Create(CustomizationTargetType.MainCharacter, characterId, isDefaultCharacter, SceneManager.GetActiveScene().name);
			customizationSessionContext.presetId = presetId;
			this.OpenCustomization(customizationSessionContext);
		}

		// Token: 0x06004FC0 RID: 20416 RVA: 0x001EE3F5 File Offset: 0x001EC5F5
		public void OpenNpcCustomization(string characterId)
		{
			this.OpenNpcCustomization(characterId, false, string.Empty);
		}

		// Token: 0x06004FC1 RID: 20417 RVA: 0x001EE404 File Offset: 0x001EC604
		public void OpenNpcCustomization(string characterId, bool isDefaultCharacter, string presetId)
		{
			CustomizationSessionContext customizationSessionContext = CustomizationSessionContext.Create(CustomizationTargetType.Npc, characterId, isDefaultCharacter, SceneManager.GetActiveScene().name);
			customizationSessionContext.presetId = presetId;
			this.OpenCustomization(customizationSessionContext);
		}

		// Token: 0x06004FC2 RID: 20418 RVA: 0x001EE438 File Offset: 0x001EC638
		public void OpenCustomization(CustomizationSessionContext context)
		{
			if (context == null || !context.IsValid())
			{
				Debug.LogError("[CustomizationNavigator] Cannot open customization scene: invalid context.");
				return;
			}
			if (string.IsNullOrWhiteSpace(this.customizationSceneName))
			{
				Debug.LogError("[CustomizationNavigator] customizationSceneName is empty.");
				return;
			}
			CustomizationSessionStore.SetPendingContext(context);
			SceneManager.LoadScene(this.customizationSceneName);
		}

		// Token: 0x040050BA RID: 20666
		[Header("Scene")]
		[SerializeField]
		private string customizationSceneName = "CharacterCustomization";

		// Token: 0x040050BB RID: 20667
		[Header("Defaults")]
		[SerializeField]
		private bool defaultMainCharacterIsLocked = true;
	}
}
