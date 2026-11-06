using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000896 RID: 2198
	public class HeadCustomizationController : CharacterCustomizationController
	{
		// Token: 0x06003B2F RID: 15151 RVA: 0x0015FB48 File Offset: 0x0015DD48
		protected override void Start()
		{
			base.Start();
			this.categoryName = "Head";
			if (this.subCategories != null && this.subCategories.Count > 0)
			{
				Debug.Log(string.Format("HeadCustomizationController: Found {0} subcategories", this.subCategories.Count));
				using (List<CharacterSubCategorySO>.Enumerator enumerator = this.subCategories.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CharacterSubCategorySO characterSubCategorySO = enumerator.Current;
						Debug.Log("HeadCustomizationController: Subcategory " + characterSubCategorySO.subCategoryName);
					}
					return;
				}
			}
			Debug.LogWarning("HeadCustomizationController: No subcategories found!");
		}
	}
}
