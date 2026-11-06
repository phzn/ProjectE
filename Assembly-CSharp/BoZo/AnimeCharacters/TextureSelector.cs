using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Bozo.AnimeCharacters
{
	// Token: 0x0200085A RID: 2138
	public class TextureSelector : MonoBehaviour
	{
		// Token: 0x0600388C RID: 14476 RVA: 0x0014A1BC File Offset: 0x001483BC
		public void Init(TexturePackage texture, CharacterCreator characterCreator)
		{
			this.button = base.GetComponentInChildren<Button>();
			this.button.onClick.AddListener(new UnityAction(this.OnSelect));
			this.texture = texture;
			this.characterCreator = characterCreator;
			this.icon.overrideSprite = texture.icon;
		}

		// Token: 0x0600388D RID: 14477 RVA: 0x0014A210 File Offset: 0x00148410
		private void OnSelect()
		{
			if (this.texture.type == TextureType.Decal)
			{
				this.characterCreator.SetOutfitDecal(this.texture.texture);
			}
			if (this.texture.type == TextureType.Pattern)
			{
				this.characterCreator.SetOutfitPattern(this.texture.texture);
			}
		}

		// Token: 0x0600388E RID: 14478 RVA: 0x0014A265 File Offset: 0x00148465
		public void SetVisable(string type)
		{
			if (this.texture.catagory == type)
			{
				base.gameObject.SetActive(true);
				return;
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x0400397E RID: 14718
		public Image icon;

		// Token: 0x0400397F RID: 14719
		private TexturePackage texture;

		// Token: 0x04003980 RID: 14720
		private CharacterCreator characterCreator;

		// Token: 0x04003981 RID: 14721
		private Button button;
	}
}
