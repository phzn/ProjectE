using System;
using UnityEngine;
using UnityEngine.UI;

namespace BoZo.CharacterCreator.MC.StyleSystem.UI
{
	// Token: 0x02000810 RID: 2064
	[RequireComponent(typeof(Button))]
	public sealed class MCPreviewClothingToggleUI : MonoBehaviour
	{
		// Token: 0x06003701 RID: 14081 RVA: 0x0013D848 File Offset: 0x0013BA48
		public void Bind(MCStyleCustomizationService service)
		{
			this.Unsubscribe();
			this._service = service;
			if (!this._initialized)
			{
				this._initialized = true;
				this._icon = MCPreviewClothingToggleUI.FindVisibleIcon(base.transform);
				Transform parent = base.transform.parent;
				this._hideClothesIcon = MCPreviewClothingToggleUI.GetIcon(parent, "ClothesOff") ?? ((this._icon != null) ? this._icon.sprite : null);
				this._showClothesIcon = MCPreviewClothingToggleUI.GetIcon(parent, "ClothesOn") ?? this._hideClothesIcon;
			}
			if (base.isActiveAndEnabled)
			{
				this.Subscribe();
			}
			this.Refresh();
		}

		// Token: 0x06003702 RID: 14082 RVA: 0x0013D8F0 File Offset: 0x0013BAF0
		private static Sprite GetIcon(Transform controls, string name)
		{
			Image image = MCPreviewClothingToggleUI.FindVisibleIcon((controls != null) ? controls.Find(name) : null);
			if (!(image != null))
			{
				return null;
			}
			return image.sprite;
		}

		// Token: 0x06003703 RID: 14083 RVA: 0x0013D928 File Offset: 0x0013BB28
		private static Image FindVisibleIcon(Transform root)
		{
			if (root == null)
			{
				return null;
			}
			Button component = root.GetComponent<Button>();
			if (component != null)
			{
				Image image = component.targetGraphic as Image;
				if (image != null && image.sprite != null)
				{
					return image;
				}
			}
			foreach (Image image2 in root.GetComponentsInChildren<Image>(true))
			{
				if (image2.sprite != null && image2.color.a > 0.01f)
				{
					return image2;
				}
			}
			return null;
		}

		// Token: 0x06003704 RID: 14084 RVA: 0x0013D9B0 File Offset: 0x0013BBB0
		private void OnEnable()
		{
			this.Subscribe();
			this.Refresh();
		}

		// Token: 0x06003705 RID: 14085 RVA: 0x0013D9BE File Offset: 0x0013BBBE
		private void OnDisable()
		{
			this.Unsubscribe();
		}

		// Token: 0x06003706 RID: 14086 RVA: 0x0013D9C6 File Offset: 0x0013BBC6
		private void Subscribe()
		{
			if (this._service == null)
			{
				return;
			}
			this._service.OnPreviewClothingVisibilityChanged -= this.Refresh;
			this._service.OnPreviewClothingVisibilityChanged += this.Refresh;
		}

		// Token: 0x06003707 RID: 14087 RVA: 0x0013DA05 File Offset: 0x0013BC05
		private void Unsubscribe()
		{
			if (this._service != null)
			{
				this._service.OnPreviewClothingVisibilityChanged -= this.Refresh;
			}
		}

		// Token: 0x06003708 RID: 14088 RVA: 0x0013DA2C File Offset: 0x0013BC2C
		private void Refresh()
		{
			if (this._icon == null)
			{
				return;
			}
			this._icon.overrideSprite = null;
			this._icon.sprite = ((this._service != null && this._service.IsPreviewClothingHidden) ? this._showClothesIcon : this._hideClothesIcon);
		}

		// Token: 0x0400372C RID: 14124
		private MCStyleCustomizationService _service;

		// Token: 0x0400372D RID: 14125
		private Image _icon;

		// Token: 0x0400372E RID: 14126
		private Sprite _hideClothesIcon;

		// Token: 0x0400372F RID: 14127
		private Sprite _showClothesIcon;

		// Token: 0x04003730 RID: 14128
		private bool _initialized;
	}
}
