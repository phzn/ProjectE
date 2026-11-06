using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008AC RID: 2220
	[CreateAssetMenu(fileName = "NewStyleItem", menuName = "Character Creator/Style Item")]
	public class StyleItemSO : ScriptableObject
	{
		// Token: 0x06003B7C RID: 15228 RVA: 0x00161868 File Offset: 0x0015FA68
		public Dictionary<string, float> GetBlendShapeValues()
		{
			Dictionary<string, float> dictionary = new Dictionary<string, float>();
			foreach (StyleItemSO.BlendShapeValue blendShapeValue in this.blendShapeValues)
			{
				dictionary[blendShapeValue.blendShapeName] = blendShapeValue.value;
			}
			return dictionary;
		}

		// Token: 0x04003BC6 RID: 15302
		[Header("Informazioni di base")]
		public string itemName;

		// Token: 0x04003BC7 RID: 15303
		public Sprite icon;

		// Token: 0x04003BC8 RID: 15304
		[TextArea(2, 5)]
		public string description;

		// Token: 0x04003BC9 RID: 15305
		[Header("Riferimenti")]
		public OutfitType itemType;

		// Token: 0x04003BCA RID: 15306
		public GameObject prefab;

		// Token: 0x04003BCB RID: 15307
		[Header("Configurazione")]
		public bool replaceExisting = true;

		// Token: 0x04003BCC RID: 15308
		[Tooltip("Whether this item is offered in the NPC customization catalog.")]
		public bool showInNpcCatalog = true;

		// Token: 0x04003BCD RID: 15309
		[Tooltip("Optional extra slots cleared by a removal style, e.g. Bald clears front and back hair.")]
		public OutfitType[] additionalTypesToRemove = new OutfitType[0];

		// Token: 0x04003BCE RID: 15310
		[Header("Color Variants (opzionale)")]
		[Tooltip("Colorway predefinite selezionabili con le frecce sulla card (editor NPC). Catturabili col tool Tools/ProjectExhibitionist/Customization/Outfit Color Variant Capture.")]
		public OutfitColorVariantSetSO colorVariants;

		// Token: 0x04003BCF RID: 15311
		[Header("Blend Shape (opzionale)")]
		public bool applyBlendShapes;

		// Token: 0x04003BD0 RID: 15312
		[SerializeField]
		private List<StyleItemSO.BlendShapeValue> blendShapeValues = new List<StyleItemSO.BlendShapeValue>();

		// Token: 0x020008AD RID: 2221
		[Serializable]
		public class BlendShapeValue
		{
			// Token: 0x04003BD1 RID: 15313
			public string blendShapeName;

			// Token: 0x04003BD2 RID: 15314
			[Range(0f, 100f)]
			public float value;
		}
	}
}
