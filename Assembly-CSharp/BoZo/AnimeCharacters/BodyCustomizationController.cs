using System;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x0200085B RID: 2139
	public class BodyCustomizationController : CharacterCustomizationController
	{
		// Token: 0x06003890 RID: 14480 RVA: 0x0014A293 File Offset: 0x00148493
		protected override void Start()
		{
			base.SetCategoryName("Body");
			base.SetSubCategories(this.subCategories);
			base.Start();
			Debug.Log(string.Format("BodyCustomizationController inizializzato con {0} sottocategorie", this.subCategories.Count));
		}
	}
}
