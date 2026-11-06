using System;
using Features.MainMenu;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CensorBars
{
	// Token: 0x020014BB RID: 5307
	[DisallowMultipleComponent]
	public sealed class StreamerArtworkImage : MonoBehaviour
	{
		// Token: 0x0600A336 RID: 41782 RVA: 0x0043CB80 File Offset: 0x0043AD80
		public static void Bind(Image image, Sprite source)
		{
			if (image == null)
			{
				return;
			}
			StreamerArtworkImage streamerArtworkImage = image.GetComponent<StreamerArtworkImage>() ?? image.gameObject.AddComponent<StreamerArtworkImage>();
			streamerArtworkImage._image = image;
			streamerArtworkImage.original = source;
			streamerArtworkImage.Refresh();
		}

		// Token: 0x0600A337 RID: 41783 RVA: 0x0043CBB4 File Offset: 0x0043ADB4
		public static void Bind(RawImage image, Texture source)
		{
			if (image == null)
			{
				return;
			}
			StreamerArtworkImage streamerArtworkImage = image.GetComponent<StreamerArtworkImage>() ?? image.gameObject.AddComponent<StreamerArtworkImage>();
			streamerArtworkImage._raw = image;
			streamerArtworkImage.originalTexture = source;
			streamerArtworkImage.Refresh();
		}

		// Token: 0x0600A338 RID: 41784 RVA: 0x0043CBE8 File Offset: 0x0043ADE8
		public static void BindKnownImages(Transform root)
		{
			StreamerArtworkCatalog instance = StreamerArtworkCatalog.Instance;
			if (root == null || instance == null)
			{
				return;
			}
			foreach (Image image in root.GetComponentsInChildren<Image>(true))
			{
				Sprite sprite;
				if (image.GetComponent<StreamerArtworkImage>() == null && instance.TryGetSafe(image.sprite, out sprite))
				{
					StreamerArtworkImage.Bind(image, image.sprite);
				}
			}
			foreach (RawImage rawImage in root.GetComponentsInChildren<RawImage>(true))
			{
				Texture texture;
				if (rawImage.GetComponent<StreamerArtworkImage>() == null && instance.TryGetSafe(rawImage.texture, out texture))
				{
					StreamerArtworkImage.Bind(rawImage, rawImage.texture);
				}
			}
		}

		// Token: 0x0600A339 RID: 41785 RVA: 0x0043CCA0 File Offset: 0x0043AEA0
		private void OnEnable()
		{
			this._image = base.GetComponent<Image>();
			this._raw = base.GetComponent<RawImage>();
			GameSettingsService.SettingsChanged -= this.Refresh;
			GameSettingsService.SettingsChanged += this.Refresh;
			if (this.original != null || this.originalTexture != null)
			{
				this.Refresh();
			}
		}

		// Token: 0x0600A33A RID: 41786 RVA: 0x0043CD09 File Offset: 0x0043AF09
		private void OnDisable()
		{
			GameSettingsService.SettingsChanged -= this.Refresh;
		}

		// Token: 0x0600A33B RID: 41787 RVA: 0x0043CD1C File Offset: 0x0043AF1C
		private void Refresh()
		{
			StreamerArtworkCatalog instance = StreamerArtworkCatalog.Instance;
			if (this._image != null)
			{
				Sprite sprite = this.original;
				Sprite sprite2;
				if (GameSettingsService.StreamerMode && instance != null && instance.TryGetSafe(this.original, out sprite2))
				{
					sprite = sprite2;
				}
				this._image.sprite = sprite;
			}
			if (this._raw != null)
			{
				Texture texture = this.originalTexture;
				Texture texture2;
				if (GameSettingsService.StreamerMode && instance != null && instance.TryGetSafe(this.originalTexture, out texture2))
				{
					texture = texture2;
				}
				this._raw.texture = texture;
			}
		}

		// Token: 0x0400A6FE RID: 42750
		[SerializeField]
		private Sprite original;

		// Token: 0x0400A6FF RID: 42751
		[SerializeField]
		private Texture originalTexture;

		// Token: 0x0400A700 RID: 42752
		private Image _image;

		// Token: 0x0400A701 RID: 42753
		private RawImage _raw;
	}
}
