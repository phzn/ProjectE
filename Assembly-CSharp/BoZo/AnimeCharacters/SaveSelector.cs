using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000854 RID: 2132
	public class SaveSelector : MonoBehaviour
	{
		// Token: 0x0600387E RID: 14462 RVA: 0x00149E04 File Offset: 0x00148004
		public void Init(CharacterData characterData, Sprite icon, CharacterCreator characterCreator, string storageKey)
		{
			this.button = base.GetComponentInChildren<Button>();
			this.button.onClick.AddListener(new UnityAction(this.OnSelect));
			this.data = characterData;
			this.characterCreator = characterCreator;
			this.storageKey = storageKey;
			this.icon.overrideSprite = icon;
		}

		// Token: 0x0600387F RID: 14463 RVA: 0x00149E5B File Offset: 0x0014805B
		private void OnSelect()
		{
			this.characterCreator.LoadCharacter(this.data, this.storageKey);
		}

		// Token: 0x04003966 RID: 14694
		public Image icon;

		// Token: 0x04003967 RID: 14695
		private CharacterData data;

		// Token: 0x04003968 RID: 14696
		private string storageKey;

		// Token: 0x04003969 RID: 14697
		private CharacterCreator characterCreator;

		// Token: 0x0400396A RID: 14698
		private Button button;
	}
}
