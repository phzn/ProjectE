using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000255 RID: 597
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[AddComponentMenu("Modern UI Pack/UI Manager/UI Manager (Custom Object)")]
	public class UIManagerCustom : MonoBehaviour
	{
		// Token: 0x06000FEF RID: 4079 RVA: 0x00069ECC File Offset: 0x000680CC
		private void Awake()
		{
			base.enabled = true;
			if (this.UIManagerAsset == null)
			{
				this.UIManagerAsset = Resources.Load<UIManager>("MUIP Manager");
			}
			if (!this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateElement();
				base.enabled = false;
			}
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x00069F18 File Offset: 0x00068118
		private void Update()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateElement();
			}
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x00069F3C File Offset: 0x0006813C
		public void UpdateElement()
		{
			if (this.objectType == UIManagerCustom.ObjectType.Image && this.imageObject == null)
			{
				this.imageObject = base.gameObject.GetComponent<Image>();
			}
			else if (this.objectType == UIManagerCustom.ObjectType.Text && this.textObject == null)
			{
				this.textObject = base.gameObject.GetComponent<TextMeshProUGUI>();
			}
			if (this.objectType == UIManagerCustom.ObjectType.Image && this.imageObject != null)
			{
				if (!this.keepAlphaValue)
				{
					if (this.colorType == UIManagerCustom.ColorType.Primary)
					{
						this.imageObject.color = this.UIManagerAsset.customObjPrimaryColor;
						return;
					}
					if (this.colorType == UIManagerCustom.ColorType.Secondary)
					{
						this.imageObject.color = this.UIManagerAsset.customObjSecondaryColor;
						return;
					}
				}
				else
				{
					if (this.colorType == UIManagerCustom.ColorType.Primary)
					{
						this.imageObject.color = new Color(this.UIManagerAsset.customObjPrimaryColor.r, this.UIManagerAsset.customObjPrimaryColor.g, this.UIManagerAsset.customObjPrimaryColor.b, this.imageObject.color.a);
						return;
					}
					if (this.colorType == UIManagerCustom.ColorType.Secondary)
					{
						this.imageObject.color = new Color(this.UIManagerAsset.customObjSecondaryColor.r, this.UIManagerAsset.customObjSecondaryColor.g, this.UIManagerAsset.customObjSecondaryColor.b, this.imageObject.color.a);
						return;
					}
				}
			}
			else if (this.objectType == UIManagerCustom.ObjectType.Text && this.textObject != null)
			{
				if (!this.useCustomColor)
				{
					if (!this.keepAlphaValue)
					{
						if (this.colorType == UIManagerCustom.ColorType.Primary)
						{
							this.textObject.color = this.UIManagerAsset.customObjPrimaryColor;
						}
						else if (this.colorType == UIManagerCustom.ColorType.Secondary)
						{
							this.textObject.color = this.UIManagerAsset.customObjSecondaryColor;
						}
					}
					else if (this.colorType == UIManagerCustom.ColorType.Primary)
					{
						this.textObject.color = new Color(this.UIManagerAsset.customObjPrimaryColor.r, this.UIManagerAsset.customObjPrimaryColor.g, this.UIManagerAsset.customObjPrimaryColor.b, this.textObject.color.a);
					}
					else if (this.colorType == UIManagerCustom.ColorType.Secondary)
					{
						this.textObject.color = new Color(this.UIManagerAsset.customObjSecondaryColor.r, this.UIManagerAsset.customObjSecondaryColor.g, this.UIManagerAsset.customObjSecondaryColor.b, this.textObject.color.a);
					}
				}
				if (!this.useCustomFont)
				{
					if (this.fontType == UIManagerCustom.FontType.Primary)
					{
						this.textObject.font = this.UIManagerAsset.customObjPrimaryFont;
						return;
					}
					if (this.fontType == UIManagerCustom.FontType.Secondary)
					{
						this.textObject.font = this.UIManagerAsset.customObjSecondaryFont;
					}
				}
			}
		}

		// Token: 0x04001157 RID: 4439
		[Header("Resources")]
		public UIManager UIManagerAsset;

		// Token: 0x04001158 RID: 4440
		[Header("Settings")]
		public UIManagerCustom.ObjectType objectType;

		// Token: 0x04001159 RID: 4441
		[Header("Color")]
		public UIManagerCustom.ColorType colorType;

		// Token: 0x0400115A RID: 4442
		public bool keepAlphaValue;

		// Token: 0x0400115B RID: 4443
		public bool useCustomColor;

		// Token: 0x0400115C RID: 4444
		[Header("Font")]
		public UIManagerCustom.FontType fontType;

		// Token: 0x0400115D RID: 4445
		public bool useCustomFont;

		// Token: 0x0400115E RID: 4446
		private Image imageObject;

		// Token: 0x0400115F RID: 4447
		private TextMeshProUGUI textObject;

		// Token: 0x02000256 RID: 598
		public enum ObjectType
		{
			// Token: 0x04001161 RID: 4449
			Text,
			// Token: 0x04001162 RID: 4450
			Image
		}

		// Token: 0x02000257 RID: 599
		public enum ColorType
		{
			// Token: 0x04001164 RID: 4452
			Primary,
			// Token: 0x04001165 RID: 4453
			Secondary
		}

		// Token: 0x02000258 RID: 600
		public enum FontType
		{
			// Token: 0x04001167 RID: 4455
			Primary,
			// Token: 0x04001168 RID: 4456
			Secondary
		}
	}
}
