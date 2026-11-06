using System;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CielOS
{
	// Token: 0x0200138B RID: 5003
	[RequireComponent(typeof(Image))]
	public class CielOSGradient : MonoBehaviour
	{
		// Token: 0x06009AE2 RID: 39650 RVA: 0x003F5E9C File Offset: 0x003F409C
		private void OnEnable()
		{
			if (this._image == null)
			{
				this._image = base.GetComponent<Image>();
			}
			if (this._image.sprite != null && this._image.sprite != this._sprite)
			{
				return;
			}
			this.Build();
		}

		// Token: 0x06009AE3 RID: 39651 RVA: 0x003F5EF8 File Offset: 0x003F40F8
		private void Build()
		{
			if (this.useTheme && CielOSTheme.Active != null)
			{
				Color accent = CielOSTheme.Active.accent;
				this.top = new Color(accent.r * 0.34f, accent.g * 0.28f, accent.b * 0.42f, 1f);
				this.bottom = CielOSTheme.Active.desktopBackground;
				this.bottom.a = 1f;
			}
			if (this._tex == null)
			{
				this._tex = new Texture2D(1, this.resolution, TextureFormat.RGBA32, false)
				{
					wrapMode = TextureWrapMode.Clamp
				};
			}
			for (int i = 0; i < this.resolution; i++)
			{
				float num = (float)i / (float)(this.resolution - 1);
				float num2 = num * num * (3f - 2f * num);
				this._tex.SetPixel(0, i, Color.Lerp(this.bottom, this.top, num2));
			}
			this._tex.Apply();
			if (this._sprite == null)
			{
				this._sprite = Sprite.Create(this._tex, new Rect(0f, 0f, 1f, (float)this.resolution), new Vector2(0.5f, 0.5f), 100f);
			}
			this._image.sprite = this._sprite;
			this._image.type = Image.Type.Simple;
			this._image.color = Color.white;
		}

		// Token: 0x06009AE4 RID: 39652 RVA: 0x003F6078 File Offset: 0x003F4278
		private void OnDestroy()
		{
			if (this._image != null && this._image.sprite == this._sprite)
			{
				this._image.sprite = null;
			}
			CielOSGradient.DestroyGeneratedObject(this._sprite);
			CielOSGradient.DestroyGeneratedObject(this._tex);
			this._sprite = null;
			this._tex = null;
		}

		// Token: 0x06009AE5 RID: 39653 RVA: 0x000A4FF8 File Offset: 0x000A31F8
		private static void DestroyGeneratedObject(Object value)
		{
			if (value == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(value);
				return;
			}
			Object.DestroyImmediate(value);
		}

		// Token: 0x04009FD8 RID: 40920
		[Tooltip("Top colour. If 'useTheme' is on, derived from the theme accent instead.")]
		[SerializeField]
		private Color top = new Color(0.2f, 0.1f, 0.28f, 1f);

		// Token: 0x04009FD9 RID: 40921
		[Tooltip("Bottom colour. If 'useTheme' is on, the theme desktop background.")]
		[SerializeField]
		private Color bottom = new Color(0.04f, 0.03f, 0.06f, 1f);

		// Token: 0x04009FDA RID: 40922
		[Tooltip("Pull gradient colours from CielOSTheme.Active (deep violet → near-black).")]
		[SerializeField]
		private bool useTheme = true;

		// Token: 0x04009FDB RID: 40923
		[SerializeField]
		private int resolution = 256;

		// Token: 0x04009FDC RID: 40924
		private Image _image;

		// Token: 0x04009FDD RID: 40925
		private Texture2D _tex;

		// Token: 0x04009FDE RID: 40926
		private Sprite _sprite;
	}
}
