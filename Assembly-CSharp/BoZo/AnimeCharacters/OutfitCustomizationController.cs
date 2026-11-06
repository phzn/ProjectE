using System;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x0200089D RID: 2205
	public class OutfitCustomizationController : CharacterCustomizationController
	{
		// Token: 0x06003B59 RID: 15193 RVA: 0x00161019 File Offset: 0x0015F219
		protected override void Start()
		{
			base.SetCategoryName("Outfit");
			base.SetSubCategories(this.subCategories);
			base.Start();
			Debug.Log(string.Format("OutfitCustomizationController inizializzato con {0} sottocategorie", this.subCategories.Count));
		}
	}
}
