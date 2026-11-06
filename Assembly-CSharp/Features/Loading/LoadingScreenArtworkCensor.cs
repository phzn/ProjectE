using System;
using Features.Localization;
using Features.MainMenu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Loading
{
	// Token: 0x020010D6 RID: 4310
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Image))]
	public sealed class LoadingScreenArtworkCensor : MonoBehaviour
	{
		// Token: 0x06008294 RID: 33428 RVA: 0x00355114 File Offset: 0x00353314
		public static void Bind(Image image, Sprite original, TMP_FontAsset font = null)
		{
			if (image == null)
			{
				return;
			}
			LoadingScreenArtworkCensor component = image.GetComponent<LoadingScreenArtworkCensor>();
			if (component != null)
			{
				component.SetSource(original, font);
			}
		}

		// Token: 0x06008295 RID: 33429 RVA: 0x00355143 File Offset: 0x00353343
		public void SetSource(Sprite original, TMP_FontAsset font = null)
		{
			this.source = original;
			if (font != null)
			{
				this._font = font;
			}
			this.Refresh();
		}

		// Token: 0x06008296 RID: 33430 RVA: 0x00355162 File Offset: 0x00353362
		private void OnEnable()
		{
			GameSettingsService.SettingsChanged -= this.Refresh;
			GameSettingsService.SettingsChanged += this.Refresh;
			this.Refresh();
		}

		// Token: 0x06008297 RID: 33431 RVA: 0x0035518C File Offset: 0x0035338C
		private void OnDisable()
		{
			GameSettingsService.SettingsChanged -= this.Refresh;
			this.SetVisibleCount(0);
		}

		// Token: 0x06008298 RID: 33432 RVA: 0x003551A6 File Offset: 0x003533A6
		private void OnDestroy()
		{
			GameSettingsService.SettingsChanged -= this.Refresh;
		}

		// Token: 0x06008299 RID: 33433 RVA: 0x003551BC File Offset: 0x003533BC
		private void Refresh()
		{
			if (this._image == null)
			{
				this._image = base.GetComponent<Image>();
			}
			this._regions = Array.Empty<Rect>();
			if (this.source != null && this.entries != null)
			{
				for (int i = 0; i < this.entries.Length; i++)
				{
					if (this.entries[i].original == this.source)
					{
						this._regions = this.entries[i].regions ?? Array.Empty<Rect>();
						break;
					}
				}
			}
			int num = ((base.isActiveAndEnabled && GameSettingsService.StreamerMode) ? Mathf.Min(this._regions.Length, this._bars.Length) : 0);
			if (num > 0)
			{
				this.EnsureBars();
			}
			this.RefreshLayout();
			this.SetVisibleCount(num);
		}

		// Token: 0x0600829A RID: 33434 RVA: 0x00355298 File Offset: 0x00353498
		private void EnsureBars()
		{
			if (this._font == null)
			{
				this._font = TMP_Settings.defaultFontAsset;
			}
			for (int i = 0; i < this._bars.Length; i++)
			{
				if (this._bars[i] == null)
				{
					GameObject gameObject = new GameObject("CensorRegion" + (i + 1).ToString(), new Type[]
					{
						typeof(RectTransform),
						typeof(Image)
					});
					gameObject.SetActive(false);
					gameObject.layer = base.gameObject.layer;
					gameObject.transform.SetParent(base.transform, false);
					this._bars[i] = gameObject.GetComponent<Image>();
					this._bars[i].color = Color.black;
					this._bars[i].raycastTarget = false;
					GameObject gameObject2 = new GameObject("CENSORED", new Type[]
					{
						typeof(RectTransform),
						typeof(TextMeshProUGUI)
					});
					gameObject2.layer = base.gameObject.layer;
					gameObject2.transform.SetParent(gameObject.transform, false);
					TextMeshProUGUI component = gameObject2.GetComponent<TextMeshProUGUI>();
					this._labels[i] = component;
					component.color = Color.white;
					component.fontStyle = FontStyles.Bold;
					component.alignment = TextAlignmentOptions.Center;
					component.enableAutoSizing = true;
					component.fontSizeMin = 8f;
					component.fontSizeMax = 48f;
					component.fontSize = 48f;
					component.enableWordWrapping = false;
					component.raycastTarget = false;
					component.margin = new Vector4(4f, 0f, 4f, 0f);
					component.rectTransform.anchorMin = Vector2.zero;
					component.rectTransform.anchorMax = Vector2.one;
					component.rectTransform.offsetMin = Vector2.zero;
					component.rectTransform.offsetMax = Vector2.zero;
				}
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this._labels[i], "gameplay_prompts", "CENSORED", null);
				if (this._font != null)
				{
					localizedGameText.SetSourceFont(this._font);
				}
				localizedGameText.FitTranslationToRectangle(true);
			}
		}

		// Token: 0x0600829B RID: 33435 RVA: 0x003554C8 File Offset: 0x003536C8
		private void SetVisibleCount(int count)
		{
			for (int i = 0; i < this._bars.Length; i++)
			{
				if (this._bars[i] != null && this._bars[i].gameObject.activeSelf != i < count)
				{
					this._bars[i].gameObject.SetActive(i < count);
				}
			}
		}

		// Token: 0x0600829C RID: 33436 RVA: 0x00355526 File Offset: 0x00353726
		private void OnRectTransformDimensionsChange()
		{
			this.RefreshLayout();
		}

		// Token: 0x0600829D RID: 33437 RVA: 0x00355530 File Offset: 0x00353730
		private void RefreshLayout()
		{
			if (this._image == null || this.source == null)
			{
				return;
			}
			Vector2 one = Vector2.one;
			Vector2 zero = Vector2.zero;
			Rect rect = this._image.rectTransform.rect;
			Rect rect2 = this.source.rect;
			if (this._image.preserveAspect && rect.width > 0f && rect.height > 0f && rect2.width > 0f && rect2.height > 0f)
			{
				float num = Mathf.Min(rect.width / rect2.width, rect.height / rect2.height);
				one = new Vector2(rect2.width * num / rect.width, rect2.height * num / rect.height);
				Vector2 pivot = this._image.rectTransform.pivot;
				zero = new Vector2((1f - one.x) * pivot.x, (1f - one.y) * pivot.y);
			}
			int num2 = 0;
			while (num2 < this._bars.Length && num2 < this._regions.Length)
			{
				if (!(this._bars[num2] == null))
				{
					Rect rect3 = this._regions[num2];
					RectTransform rectTransform = this._bars[num2].rectTransform;
					rectTransform.anchorMin = new Vector2(zero.x + rect3.xMin * one.x, zero.y + rect3.yMin * one.y);
					rectTransform.anchorMax = new Vector2(zero.x + rect3.xMax * one.x, zero.y + rect3.yMax * one.y);
					rectTransform.offsetMin = Vector2.zero;
					rectTransform.offsetMax = Vector2.zero;
				}
				num2++;
			}
		}

		// Token: 0x040085C5 RID: 34245
		[SerializeField]
		private Sprite source;

		// Token: 0x040085C6 RID: 34246
		public LoadingScreenArtworkCensor.Entry[] entries = Array.Empty<LoadingScreenArtworkCensor.Entry>();

		// Token: 0x040085C7 RID: 34247
		private readonly Image[] _bars = new Image[2];

		// Token: 0x040085C8 RID: 34248
		private readonly TMP_Text[] _labels = new TMP_Text[2];

		// Token: 0x040085C9 RID: 34249
		private Image _image;

		// Token: 0x040085CA RID: 34250
		private TMP_FontAsset _font;

		// Token: 0x040085CB RID: 34251
		private Rect[] _regions = Array.Empty<Rect>();

		// Token: 0x020010D7 RID: 4311
		[Serializable]
		public struct Entry
		{
			// Token: 0x040085CC RID: 34252
			public Sprite original;

			// Token: 0x040085CD RID: 34253
			[Tooltip("Regions normalized to the original sprite, with the origin at its bottom-left.")]
			public Rect[] regions;
		}
	}
}
