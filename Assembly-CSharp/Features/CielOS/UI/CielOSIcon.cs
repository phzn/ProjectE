using System;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CielOS.UI
{
	// Token: 0x020013BE RID: 5054
	[RequireComponent(typeof(Image))]
	public class CielOSIcon : MonoBehaviour
	{
		// Token: 0x170018D3 RID: 6355
		// (get) Token: 0x06009C4F RID: 40015 RVA: 0x003FE45E File Offset: 0x003FC65E
		// (set) Token: 0x06009C50 RID: 40016 RVA: 0x003FE466 File Offset: 0x003FC666
		public CielOSIconId Id
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
				this.Refresh();
			}
		}

		// Token: 0x06009C51 RID: 40017 RVA: 0x003FE475 File Offset: 0x003FC675
		private void OnEnable()
		{
			this.Refresh();
		}

		// Token: 0x06009C52 RID: 40018 RVA: 0x003FE480 File Offset: 0x003FC680
		private void Refresh()
		{
			if (this._image == null)
			{
				this._image = base.GetComponent<Image>();
			}
			if (this._image == null)
			{
				return;
			}
			this._image.type = Image.Type.Simple;
			this._image.preserveAspect = true;
			this._image.raycastTarget = false;
			string text = CielOSAppIcons.LogoResource(this.id);
			this._image.sprite = ((text != null) ? CielOSIconLibrary.GetAppLogoMaskSprite(text, this.id, this.size) : CielOSIconLibrary.GetSprite(this.id, this.size));
			Color trayIconTint = this.color;
			if (this.tintFromTheme && CielOSTheme.Active != null)
			{
				trayIconTint = CielOSTheme.Active.trayIconTint;
			}
			this._image.color = trayIconTint;
		}

		// Token: 0x06009C53 RID: 40019 RVA: 0x003FE54C File Offset: 0x003FC74C
		public static void Apply(Image img, CielOSIconId id, Color tint, int size = 64)
		{
			if (img == null)
			{
				return;
			}
			string text = CielOSAppIcons.LogoResource(id);
			img.sprite = ((text != null) ? CielOSIconLibrary.GetAppLogoMaskSprite(text, id, size) : CielOSIconLibrary.GetSprite(id, size));
			img.type = Image.Type.Simple;
			img.preserveAspect = true;
			img.color = tint;
		}

		// Token: 0x06009C54 RID: 40020 RVA: 0x003FE59C File Offset: 0x003FC79C
		public static void ApplyColored(Image img, CielOSIconId id, int size = 64)
		{
			if (img == null)
			{
				return;
			}
			string text = CielOSAppIcons.LogoResource(id);
			if (text != null)
			{
				img.sprite = CielOSIconLibrary.GetAppLogoMaskSprite(text, id, size);
				img.type = Image.Type.Simple;
				img.preserveAspect = true;
				Color color = ((CielOSTheme.Active != null) ? CielOSTheme.Active.trayIconTint : Color.white);
				img.color = color;
				return;
			}
			img.sprite = CielOSIconLibrary.GetColoredSprite(id, size);
			img.type = Image.Type.Simple;
			img.preserveAspect = true;
			img.color = Color.white;
		}

		// Token: 0x06009C55 RID: 40021 RVA: 0x003FE628 File Offset: 0x003FC828
		public static void ApplyBadge(Image img, CielOSIconId id, int size = 64)
		{
			if (img == null)
			{
				return;
			}
			string text = CielOSAppIcons.LogoResource(id);
			if (text != null)
			{
				img.sprite = CielOSIconLibrary.GetAppLogoSprite(text, id, size);
			}
			else if (CielOSIconLibrary.IsColoredAppIcon(id))
			{
				img.sprite = CielOSIconLibrary.GetColoredSprite(id, size);
			}
			else
			{
				img.sprite = CielOSIconLibrary.GetBadgedSprite(id, size);
			}
			img.type = Image.Type.Simple;
			img.preserveAspect = true;
			img.color = Color.white;
		}

		// Token: 0x0400A0CB RID: 41163
		[SerializeField]
		private CielOSIconId id = CielOSIconId.Folder;

		// Token: 0x0400A0CC RID: 41164
		[Tooltip("Texture size in px (the sprite is scaled by the RectTransform anyway).")]
		[SerializeField]
		private int size = 64;

		// Token: 0x0400A0CD RID: 41165
		[Tooltip("If true, tint comes from the theme (tray tint / accent); else use 'color'.")]
		[SerializeField]
		private bool tintFromTheme = true;

		// Token: 0x0400A0CE RID: 41166
		[SerializeField]
		private Color color = Color.white;

		// Token: 0x0400A0CF RID: 41167
		private Image _image;
	}
}
