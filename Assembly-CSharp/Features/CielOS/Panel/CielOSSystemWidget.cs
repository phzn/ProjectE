using System;
using Features.CielOS.UI;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CielOS.Panel
{
	// Token: 0x020013E1 RID: 5089
	public class CielOSSystemWidget : MonoBehaviour
	{
		// Token: 0x06009D17 RID: 40215 RVA: 0x00407F00 File Offset: 0x00406100
		private void OnEnable()
		{
			this._startTime = Time.realtimeSinceStartup;
			this._next = 0f;
			this.Build();
		}

		// Token: 0x06009D18 RID: 40216 RVA: 0x00407F20 File Offset: 0x00406120
		private void Build()
		{
			if (this._built)
			{
				return;
			}
			this._built = true;
			this._rect = (RectTransform)base.transform;
			CielOSTheme active = CielOSTheme.Active;
			Color color = ((active != null) ? active.panelText : Color.white);
			Color color2 = ((active != null) ? active.accent : new Color(0.66f, 0.38f, 0.79f, 1f));
			Image component = base.GetComponent<Image>();
			if (component != null && active != null)
			{
				component.color = active.windowBackground;
				CielOSTheme.Round(component);
				CielOSTheme.AddBorder((RectTransform)base.transform, null, 1f);
			}
			VerticalLayoutGroup verticalLayoutGroup = base.GetComponent<VerticalLayoutGroup>();
			if (verticalLayoutGroup == null)
			{
				verticalLayoutGroup = base.gameObject.AddComponent<VerticalLayoutGroup>();
			}
			verticalLayoutGroup.padding = new RectOffset(12, 12, 8, 8);
			verticalLayoutGroup.spacing = 5f;
			verticalLayoutGroup.childAlignment = TextAnchor.UpperLeft;
			verticalLayoutGroup.childControlWidth = true;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			GameObject gameObject = new GameObject("Header", new Type[]
			{
				typeof(RectTransform),
				typeof(LayoutElement)
			});
			gameObject.transform.SetParent(base.transform, false);
			gameObject.GetComponent<LayoutElement>().minHeight = 26f;
			gameObject.GetComponent<LayoutElement>().preferredHeight = 26f;
			GameObject gameObject2 = new GameObject("Logo", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform = (RectTransform)gameObject2.transform;
			rectTransform.anchorMin = new Vector2(0f, 0.5f);
			rectTransform.anchorMax = new Vector2(0f, 0.5f);
			rectTransform.pivot = new Vector2(0f, 0.5f);
			rectTransform.sizeDelta = new Vector2(22f, 22f);
			rectTransform.anchoredPosition = new Vector2(0f, 0f);
			CielOSIcon.Apply(gameObject2.GetComponent<Image>(), CielOSIconId.DragonLogo, (active != null) ? active.logoPrimary : color2, 64);
			this._header = this.MakeText(gameObject.transform, "Title", "CielOS System", 13f, color, TextAlignmentOptions.TopLeft, FontStyles.Bold);
			this._header.enableWordWrapping = true;
			RectTransform rectTransform2 = (RectTransform)this._header.transform;
			rectTransform2.anchorMin = new Vector2(0f, 0f);
			rectTransform2.anchorMax = new Vector2(1f, 1f);
			rectTransform2.offsetMin = new Vector2(28f, 0f);
			rectTransform2.offsetMax = new Vector2(0f, 0f);
			this._cpu = this.MakeBar("CPU");
			this._ram = this.MakeBar("RAM");
			this._swap = this.MakeBar("SWAP");
			this._disk = this.MakeBar("DISK");
			GameObject gameObject3 = new GameObject("Network", new Type[]
			{
				typeof(RectTransform),
				typeof(LayoutElement)
			});
			gameObject3.transform.SetParent(base.transform, false);
			gameObject3.GetComponent<LayoutElement>().minHeight = 40f;
			gameObject3.GetComponent<LayoutElement>().preferredHeight = 40f;
			this._netLabel = this.MakeText(gameObject3.transform, "NetLabel", "NET eth0", 11f, color, TextAlignmentOptions.MidlineLeft, FontStyles.Normal);
			this._netLabel.enableWordWrapping = true;
			RectTransform rectTransform3 = (RectTransform)this._netLabel.transform;
			rectTransform3.anchorMin = new Vector2(0f, 0.5f);
			rectTransform3.anchorMax = Vector2.one;
			rectTransform3.offsetMin = Vector2.zero;
			rectTransform3.offsetMax = Vector2.zero;
			GameObject gameObject4 = new GameObject("Spark", new Type[]
			{
				typeof(RectTransform),
				typeof(HorizontalLayoutGroup)
			});
			gameObject4.transform.SetParent(gameObject3.transform, false);
			RectTransform rectTransform4 = (RectTransform)gameObject4.transform;
			rectTransform4.anchorMin = Vector2.zero;
			rectTransform4.anchorMax = new Vector2(1f, 0.5f);
			rectTransform4.offsetMin = Vector2.zero;
			rectTransform4.offsetMax = Vector2.zero;
			HorizontalLayoutGroup component2 = gameObject4.GetComponent<HorizontalLayoutGroup>();
			component2.spacing = 2f;
			component2.childAlignment = TextAnchor.LowerCenter;
			component2.childControlWidth = true;
			component2.childControlHeight = false;
			component2.childForceExpandWidth = true;
			component2.childForceExpandHeight = false;
			this._spark = new Image[16];
			for (int i = 0; i < this._spark.Length; i++)
			{
				GameObject gameObject5 = new GameObject("b" + i.ToString(), new Type[]
				{
					typeof(RectTransform),
					typeof(Image),
					typeof(LayoutElement)
				});
				gameObject5.transform.SetParent(gameObject4.transform, false);
				gameObject5.GetComponent<LayoutElement>().flexibleWidth = 1f;
				Image component3 = gameObject5.GetComponent<Image>();
				component3.color = color2;
				component3.raycastTarget = false;
				this._spark[i] = component3;
				((RectTransform)gameObject5.transform).pivot = new Vector2(0.5f, 0f);
			}
		}

		// Token: 0x06009D19 RID: 40217 RVA: 0x004084B4 File Offset: 0x004066B4
		private CielOSBarMeter MakeBar(string label)
		{
			GameObject gameObject = new GameObject("Bar_" + label, new Type[] { typeof(RectTransform) });
			gameObject.transform.SetParent(base.transform, false);
			CielOSBarMeter cielOSBarMeter = gameObject.AddComponent<CielOSBarMeter>();
			cielOSBarMeter.Build(label);
			return cielOSBarMeter;
		}

		// Token: 0x06009D1A RID: 40218 RVA: 0x00408502 File Offset: 0x00406702
		private void Update()
		{
			if (!this._built)
			{
				return;
			}
			if (Time.unscaledTime < this._next)
			{
				return;
			}
			this._next = Time.unscaledTime + this.refreshInterval;
			this.Render();
		}

		// Token: 0x06009D1B RID: 40219 RVA: 0x00408534 File Offset: 0x00406734
		private void LateUpdate()
		{
			if (this._rect == null)
			{
				return;
			}
			RectTransform rectTransform = this._rect.parent as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			float num = rectTransform.rect.width - Mathf.Abs(this._rect.anchoredPosition.x) - 8f;
			float num2 = rectTransform.rect.height - Mathf.Abs(this._rect.anchoredPosition.y) - 8f;
			Vector2 vector = new Vector2(Mathf.Clamp(num, 200f, 280f), Mathf.Clamp(num2, 187f, 196f));
			if ((this._rect.sizeDelta - vector).sqrMagnitude > 0.25f)
			{
				this._rect.sizeDelta = vector;
			}
		}

		// Token: 0x06009D1C RID: 40220 RVA: 0x00408618 File Offset: 0x00406818
		private void Render()
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)(realtimeSinceStartup - this._startTime));
			string text = string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
			if (this._header != null)
			{
				this._header.text = GameLocalization.Format("cielos.shell.uptime", "CielOS 1.0   ·   up {0}", new object[] { text });
			}
			float num = 0.18f + 0.5f * (0.5f + 0.5f * Mathf.Sin(realtimeSinceStartup * 0.7f) * Mathf.Sin(realtimeSinceStartup * 0.23f + 1.3f));
			this.SetBar(this._cpu, num, string.Format("{0}%", Mathf.RoundToInt(num * 100f)));
			float num2 = (float)Mathf.Max(1024, SystemInfo.systemMemorySize) / 1024f;
			float num3 = 0.28f + 0.12f * (0.5f + 0.5f * Mathf.Sin(realtimeSinceStartup * 0.31f));
			this.SetBar(this._ram, num3, string.Format("{0:0.0}/{1:0.0}G", num3 * num2, num2));
			float num4 = 0.05f + 0.1f * (0.5f + 0.5f * Mathf.Sin(realtimeSinceStartup * 0.13f + 2f));
			this.SetBar(this._swap, num4, string.Format("{0}%", Mathf.RoundToInt(num4 * 100f)));
			float num5 = 0.62f;
			this.SetBar(this._disk, num5, string.Format("{0}%", Mathf.RoundToInt(num5 * 100f)));
			float num6 = 0.5f + 0.5f * Mathf.PerlinNoise(realtimeSinceStartup * 0.9f, 0.5f);
			int num7 = Mathf.RoundToInt(40f + num6 * 180f);
			int num8 = Mathf.RoundToInt(10f + num6 * 60f);
			if (this._netLabel != null)
			{
				this._netLabel.text = GameLocalization.Format("cielos.shell.network", "NET ▼{0} ▲{1} KiB/s", new object[] { num7, num8 });
			}
			this._netHistory[this._netCursor] = num6;
			this._netCursor = (this._netCursor + 1) % this._netHistory.Length;
			this.UpdateSparkline();
		}

		// Token: 0x06009D1D RID: 40221 RVA: 0x004088A0 File Offset: 0x00406AA0
		private void SetBar(CielOSBarMeter bar, float v, string text)
		{
			if (bar == null)
			{
				return;
			}
			bar.SetValue01(v);
			bar.SetValueText(text);
		}

		// Token: 0x06009D1E RID: 40222 RVA: 0x004088BC File Offset: 0x00406ABC
		private void UpdateSparkline()
		{
			if (this._spark == null)
			{
				return;
			}
			for (int i = 0; i < this._spark.Length; i++)
			{
				int num = (this._netCursor + i) % this._netHistory.Length;
				float num2 = Mathf.Clamp01(this._netHistory[num]);
				RectTransform rectTransform = (RectTransform)this._spark[i].transform;
				rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 4f + num2 * 14f);
				float num3 = 0.35f + 0.65f * num2;
				Color color = this._spark[i].color;
				color.a = num3;
				this._spark[i].color = color;
			}
		}

		// Token: 0x06009D1F RID: 40223 RVA: 0x00408974 File Offset: 0x00406B74
		private TMP_Text MakeText(Transform parent, string name, string content, float size, Color color, TextAlignmentOptions align, FontStyles style)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			gameObject.transform.SetParent(parent, false);
			TextMeshProUGUI component = gameObject.GetComponent<TextMeshProUGUI>();
			component.text = content;
			component.fontSize = size;
			component.color = color;
			component.alignment = align;
			component.fontStyle = style;
			component.raycastTarget = false;
			component.enableWordWrapping = false;
			component.enableAutoSizing = true;
			component.fontSizeMin = 9f;
			component.fontSizeMax = size;
			component.overflowMode = TextOverflowModes.Ellipsis;
			if (CielOSTheme.Active != null)
			{
				if (CielOSTheme.Active.monospaceFont != null)
				{
					component.font = CielOSTheme.Active.monospaceFont;
				}
				else if (CielOSTheme.Active.uiFont != null)
				{
					component.font = CielOSTheme.Active.uiFont;
				}
			}
			LocalizedGameText.BindFontOnly(component);
			return component;
		}

		// Token: 0x0400A196 RID: 41366
		[SerializeField]
		private float refreshInterval = 0.5f;

		// Token: 0x0400A197 RID: 41367
		private float _next;

		// Token: 0x0400A198 RID: 41368
		private float _startTime;

		// Token: 0x0400A199 RID: 41369
		private bool _built;

		// Token: 0x0400A19A RID: 41370
		private TMP_Text _header;

		// Token: 0x0400A19B RID: 41371
		private CielOSBarMeter _cpu;

		// Token: 0x0400A19C RID: 41372
		private CielOSBarMeter _ram;

		// Token: 0x0400A19D RID: 41373
		private CielOSBarMeter _swap;

		// Token: 0x0400A19E RID: 41374
		private CielOSBarMeter _disk;

		// Token: 0x0400A19F RID: 41375
		private TMP_Text _netLabel;

		// Token: 0x0400A1A0 RID: 41376
		private Image[] _spark;

		// Token: 0x0400A1A1 RID: 41377
		private readonly float[] _netHistory = new float[16];

		// Token: 0x0400A1A2 RID: 41378
		private int _netCursor;

		// Token: 0x0400A1A3 RID: 41379
		private RectTransform _rect;
	}
}
