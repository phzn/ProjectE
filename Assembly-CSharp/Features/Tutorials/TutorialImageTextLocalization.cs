using System;
using System.Collections.Generic;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Tutorials
{
	// Token: 0x02000BD9 RID: 3033
	public sealed class TutorialImageTextLocalization : MonoBehaviour
	{
		// Token: 0x06005758 RID: 22360 RVA: 0x00221368 File Offset: 0x0021F568
		public static void Bind(RawImage image)
		{
			if (image == null || image.texture == null)
			{
				return;
			}
			if (TutorialImageTextLocalization._definitions == null)
			{
				TutorialImageTextLocalization._definitions = new Dictionary<string, TutorialImageTextLocalization.ImageDefinition>(StringComparer.Ordinal);
				TextAsset textAsset = Resources.Load<TextAsset>("Localization/TutorialImageTextRegions");
				TutorialImageTextLocalization.Catalog catalog = ((textAsset != null) ? JsonUtility.FromJson<TutorialImageTextLocalization.Catalog>(textAsset.text) : null);
				if (((catalog != null) ? catalog.images : null) != null)
				{
					foreach (TutorialImageTextLocalization.ImageDefinition imageDefinition in catalog.images)
					{
						TutorialImageTextLocalization._definitions.Add(imageDefinition.textureName, imageDefinition);
					}
				}
			}
			TutorialImageTextLocalization.ImageDefinition imageDefinition2;
			if (!TutorialImageTextLocalization._definitions.TryGetValue(image.texture.name, out imageDefinition2))
			{
				return;
			}
			TutorialImageTextLocalization tutorialImageTextLocalization = image.GetComponent<TutorialImageTextLocalization>() ?? image.gameObject.AddComponent<TutorialImageTextLocalization>();
			tutorialImageTextLocalization._image = image;
			tutorialImageTextLocalization._definition = imageDefinition2;
			tutorialImageTextLocalization.Build();
		}

		// Token: 0x06005759 RID: 22361 RVA: 0x00221448 File Offset: 0x0021F648
		private void OnEnable()
		{
			GameLocalization.LocaleChanged += this.Refresh;
			this.Refresh();
		}

		// Token: 0x0600575A RID: 22362 RVA: 0x00221461 File Offset: 0x0021F661
		private void OnDisable()
		{
			GameLocalization.LocaleChanged -= this.Refresh;
		}

		// Token: 0x0600575B RID: 22363 RVA: 0x00221474 File Offset: 0x0021F674
		private void Build()
		{
			if (this._layer != null)
			{
				return;
			}
			this._layer = new GameObject("Localized screenshot labels", new Type[]
			{
				typeof(RectTransform),
				typeof(RectMask2D)
			}).GetComponent<RectTransform>();
			this._layer.SetParent(base.transform, false);
			this._layer.anchorMin = Vector2.zero;
			this._layer.anchorMax = Vector2.one;
			this._layer.offsetMin = (this._layer.offsetMax = Vector2.zero);
			if (this._definition.backdrops != null)
			{
				foreach (TutorialImageTextLocalization.Region region in this._definition.backdrops)
				{
					Image component = new GameObject("Localized text panel", new Type[]
					{
						typeof(RectTransform),
						typeof(Image)
					}).GetComponent<Image>();
					component.transform.SetParent(this._layer, false);
					component.raycastTarget = false;
					component.color = TutorialImageTextLocalization.ParseColor(region.background, new Color(0.1f, 0.08f, 0.13f, 1f));
					this._backdrops.Add(component.rectTransform);
				}
			}
			foreach (TutorialImageTextLocalization.Region region2 in this._definition.regions)
			{
				Image component2 = new GameObject("Authored text", new Type[]
				{
					typeof(RectTransform),
					typeof(Image)
				}).GetComponent<Image>();
				component2.transform.SetParent(this._layer, false);
				component2.raycastTarget = false;
				component2.color = TutorialImageTextLocalization.ParseColor(region2.background, new Color(0.1f, 0.08f, 0.13f, 1f));
				TextMeshProUGUI component3 = new GameObject("Translation", new Type[]
				{
					typeof(RectTransform),
					typeof(TextMeshProUGUI)
				}).GetComponent<TextMeshProUGUI>();
				component3.transform.SetParent(component2.transform, false);
				component3.rectTransform.anchorMin = Vector2.zero;
				component3.rectTransform.anchorMax = Vector2.one;
				component3.rectTransform.offsetMin = (component3.rectTransform.offsetMax = Vector2.zero);
				component3.color = TutorialImageTextLocalization.ParseColor(region2.foreground, Color.white);
				component3.alignment = ((region2.alignment == "center") ? TextAlignmentOptions.Center : TextAlignmentOptions.MidlineLeft);
				component3.fontStyle = (region2.bold ? FontStyles.Bold : FontStyles.Normal);
				component3.enableAutoSizing = true;
				component3.enableWordWrapping = true;
				component3.overflowMode = TextOverflowModes.Truncate;
				component3.raycastTarget = false;
				this._labels.Add(component3);
			}
			this.Layout();
			this.Refresh();
		}

		// Token: 0x0600575C RID: 22364 RVA: 0x00221784 File Offset: 0x0021F984
		private static Color ParseColor(string value, Color fallback)
		{
			Color color;
			if (!ColorUtility.TryParseHtmlString(value, out color))
			{
				return fallback;
			}
			return color;
		}

		// Token: 0x0600575D RID: 22365 RVA: 0x002217A0 File Offset: 0x0021F9A0
		private void LateUpdate()
		{
			if (this._image != null && (this._lastUv != this._image.uvRect || this._lastSize != this._image.rectTransform.rect.size))
			{
				this.Layout();
			}
		}

		// Token: 0x0600575E RID: 22366 RVA: 0x00221800 File Offset: 0x0021FA00
		private void Layout()
		{
			if (this._image == null || this._layer == null)
			{
				return;
			}
			this._lastUv = this._image.uvRect;
			this._lastSize = this._image.rectTransform.rect.size;
			if (this._lastUv.width <= 0f || this._lastUv.height <= 0f)
			{
				return;
			}
			float num = this._lastSize.y / (this._definition.height * this._lastUv.height);
			for (int i = 0; i < this._backdrops.Count; i++)
			{
				this.Position(this._backdrops[i], this._definition.backdrops[i]);
			}
			for (int j = 0; j < this._labels.Count; j++)
			{
				TutorialImageTextLocalization.Region region = this._definition.regions[j];
				RectTransform rectTransform = (RectTransform)this._labels[j].transform.parent;
				this.Position(rectTransform, region);
				this._labels[j].fontSize = (this._labels[j].fontSizeMax = Mathf.Max(1f, region.fontSize * num));
				this._labels[j].fontSizeMin = this._labels[j].fontSizeMax * 0.55f;
			}
		}

		// Token: 0x0600575F RID: 22367 RVA: 0x0022198C File Offset: 0x0021FB8C
		private void Position(RectTransform rect, TutorialImageTextLocalization.Region region)
		{
			float num = 1f - this._lastUv.y - this._lastUv.height;
			float num2 = (region.x / this._definition.width - this._lastUv.x) / this._lastUv.width;
			float num3 = (region.y / this._definition.height - num) / this._lastUv.height;
			float num4 = region.width / this._definition.width / this._lastUv.width;
			float num5 = region.height / this._definition.height / this._lastUv.height;
			rect.anchorMin = new Vector2(num2, 1f - num3 - num5);
			rect.anchorMax = new Vector2(num2 + num4, 1f - num3);
			rect.offsetMin = (rect.offsetMax = Vector2.zero);
			rect.gameObject.SetActive(num2 < 1f && num2 + num4 > 0f && num3 < 1f && num3 + num5 > 0f);
		}

		// Token: 0x06005760 RID: 22368 RVA: 0x00221AB8 File Offset: 0x0021FCB8
		private void Refresh()
		{
			if (this._layer == null)
			{
				return;
			}
			bool flag = GameLocalization.CurrentCode != "en";
			this._layer.gameObject.SetActive(flag);
			if (!flag)
			{
				return;
			}
			for (int i = 0; i < this._labels.Count; i++)
			{
				TextMeshProUGUI textMeshProUGUI = this._labels[i];
				textMeshProUGUI.font = GameLocalization.GetFont(TMP_Settings.defaultFontAsset);
				textMeshProUGUI.text = GameLocalization.GetSource("tutorial_image", this._definition.regions[i].english);
			}
		}

		// Token: 0x040057B7 RID: 22455
		private static Dictionary<string, TutorialImageTextLocalization.ImageDefinition> _definitions;

		// Token: 0x040057B8 RID: 22456
		private RawImage _image;

		// Token: 0x040057B9 RID: 22457
		private TutorialImageTextLocalization.ImageDefinition _definition;

		// Token: 0x040057BA RID: 22458
		private RectTransform _layer;

		// Token: 0x040057BB RID: 22459
		private readonly List<TextMeshProUGUI> _labels = new List<TextMeshProUGUI>();

		// Token: 0x040057BC RID: 22460
		private readonly List<RectTransform> _backdrops = new List<RectTransform>();

		// Token: 0x040057BD RID: 22461
		private Rect _lastUv;

		// Token: 0x040057BE RID: 22462
		private Vector2 _lastSize;

		// Token: 0x02000BDA RID: 3034
		[Serializable]
		public sealed class Region
		{
			// Token: 0x040057BF RID: 22463
			public string english;

			// Token: 0x040057C0 RID: 22464
			public string foreground;

			// Token: 0x040057C1 RID: 22465
			public string background;

			// Token: 0x040057C2 RID: 22466
			public string alignment;

			// Token: 0x040057C3 RID: 22467
			public float x;

			// Token: 0x040057C4 RID: 22468
			public float y;

			// Token: 0x040057C5 RID: 22469
			public float width;

			// Token: 0x040057C6 RID: 22470
			public float height;

			// Token: 0x040057C7 RID: 22471
			public float fontSize;

			// Token: 0x040057C8 RID: 22472
			public bool bold;
		}

		// Token: 0x02000BDB RID: 3035
		[Serializable]
		public sealed class ImageDefinition
		{
			// Token: 0x040057C9 RID: 22473
			public string textureName;

			// Token: 0x040057CA RID: 22474
			public float width;

			// Token: 0x040057CB RID: 22475
			public float height;

			// Token: 0x040057CC RID: 22476
			public TutorialImageTextLocalization.Region[] regions;

			// Token: 0x040057CD RID: 22477
			public TutorialImageTextLocalization.Region[] backdrops;
		}

		// Token: 0x02000BDC RID: 3036
		[Serializable]
		public sealed class Catalog
		{
			// Token: 0x040057CE RID: 22478
			public TutorialImageTextLocalization.ImageDefinition[] images;
		}
	}
}
