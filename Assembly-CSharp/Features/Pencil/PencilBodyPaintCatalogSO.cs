using System;
using System.Collections.Generic;
using Features.Inventory;
using ProjectExhibitionist;
using ProjectExhibitionist.Interactions;
using TMPro;
using UnityEngine;

namespace Features.Pencil
{
	// Token: 0x02000E81 RID: 3713
	[CreateAssetMenu(fileName = "PencilBodyPaintCatalog", menuName = "Features/Pencil/Body Paint Catalog")]
	public sealed class PencilBodyPaintCatalogSO : ScriptableObject
	{
		// Token: 0x04007239 RID: 29241
		[Header("Pencil Item")]
		public InventoryItemSO pencilItem;

		// Token: 0x0400723A RID: 29242
		public InteractionConfigSO heldHudConfig;

		// Token: 0x0400723B RID: 29243
		[Header("Menu Style")]
		public TMP_FontAsset uiFont;

		// Token: 0x0400723C RID: 29244
		[Header("Temporary Effect")]
		[Min(1f)]
		[Tooltip("Total visible lifetime, including the final fade.")]
		public float totalDurationSeconds = 180f;

		// Token: 0x0400723D RID: 29245
		[Min(0f)]
		[Tooltip("Seconds spent fading to transparent at the end of the lifetime.")]
		public float fadeDurationSeconds = 20f;

		// Token: 0x0400723E RID: 29246
		public Color timerAccentColor = new Color(1f, 0.31f, 0.61f, 1f);

		// Token: 0x0400723F RID: 29247
		[Header("Selectable Designs")]
		public List<PencilBodyPaintCatalogSO.Option> options = new List<PencilBodyPaintCatalogSO.Option>();

		// Token: 0x02000E82 RID: 3714
		[Serializable]
		public sealed class Option
		{
			// Token: 0x1700123D RID: 4669
			// (get) Token: 0x06006F50 RID: 28496 RVA: 0x002CEBB4 File Offset: 0x002CCDB4
			public string DisplayName
			{
				get
				{
					if (!string.IsNullOrWhiteSpace(this.displayName))
					{
						return this.displayName;
					}
					if (this.decal != null && !string.IsNullOrWhiteSpace(this.decal.displayName))
					{
						return this.decal.displayName;
					}
					if (!(this.decal != null))
					{
						return "Body Paint";
					}
					return this.decal.name;
				}
			}

			// Token: 0x1700123E RID: 4670
			// (get) Token: 0x06006F51 RID: 28497 RVA: 0x002CEC20 File Offset: 0x002CCE20
			public Texture PreviewTexture
			{
				get
				{
					if (this.preview != null)
					{
						return this.preview.texture;
					}
					if (this.decal == null)
					{
						return null;
					}
					if (!(this.decal.bodyTexture != null))
					{
						return this.decal.headTexture;
					}
					return this.decal.bodyTexture;
				}
			}

			// Token: 0x04007240 RID: 29248
			public SkinDecal decal;

			// Token: 0x04007241 RID: 29249
			[Tooltip("Optional label override. Falls back to SkinDecal.displayName.")]
			public string displayName;

			// Token: 0x04007242 RID: 29250
			[Tooltip("In-engine portrait of Ciel wearing this design.")]
			public Sprite preview;
		}
	}
}
