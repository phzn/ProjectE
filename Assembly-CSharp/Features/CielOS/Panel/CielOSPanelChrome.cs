using System;
using Features.CielOS.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CielOS.Panel
{
	// Token: 0x020013DB RID: 5083
	[RequireComponent(typeof(CielOSPanel))]
	public class CielOSPanelChrome : MonoBehaviour
	{
		// Token: 0x06009CFA RID: 40186 RVA: 0x00406975 File Offset: 0x00404B75
		private void Start()
		{
			this._panel = base.GetComponent<CielOSPanel>();
			this.Build();
		}

		// Token: 0x06009CFB RID: 40187 RVA: 0x0040698C File Offset: 0x00404B8C
		private void Build()
		{
			if (this._built)
			{
				return;
			}
			this._built = true;
			CielOSTheme active = CielOSTheme.Active;
			Color color = ((active != null) ? active.panelText : Color.white);
			Color color2 = ((active != null) ? active.panelAccent : new Color(0.66f, 0.38f, 0.79f, 1f));
			Color color3 = ((active != null) ? active.trayIconTint : new Color(0.86f, 0.8f, 0.92f, 1f));
			RectTransform rectTransform = (RectTransform)base.transform;
			Image component = base.GetComponent<Image>();
			if (component != null && active != null)
			{
				component.color = active.panelBackground;
			}
			Button button = this.MakeIconButton(rectTransform, "AppMenuButton", CielOSIconId.Menu, color3, 0f, 30f, 34f);
			GameObject gameObject = new GameObject("Logo", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject.transform.SetParent(rectTransform, false);
			RectTransform rectTransform2 = (RectTransform)gameObject.transform;
			CielOSPanelChrome.Anchor(rectTransform2, 0f, 0.5f);
			rectTransform2.sizeDelta = new Vector2(26f, 26f);
			rectTransform2.anchoredPosition = new Vector2(60f, 0f);
			CielOSIcon.Apply(gameObject.GetComponent<Image>(), CielOSIconId.DragonLogo, (active != null) ? active.logoPrimary : color2, 64);
			TMP_Text tmp_Text = this.MakeLabel(rectTransform, "Wordmark", "CielOS", 17f, color, TextAlignmentOptions.MidlineLeft);
			RectTransform rectTransform3 = (RectTransform)tmp_Text.transform;
			CielOSPanelChrome.Anchor(rectTransform3, 0f, 0.5f);
			rectTransform3.pivot = new Vector2(0f, 0.5f);
			rectTransform3.sizeDelta = new Vector2(120f, 30f);
			rectTransform3.anchoredPosition = new Vector2(84f, 0f);
			tmp_Text.fontStyle = FontStyles.Bold;
			if (this.workspaceSwitcher == null)
			{
				GameObject gameObject2 = new GameObject("Workspaces", new Type[] { typeof(RectTransform) });
				gameObject2.transform.SetParent(rectTransform, false);
				this.workspaceSwitcher = gameObject2.AddComponent<CielOSWorkspaceSwitcher>();
				RectTransform rectTransform4 = (RectTransform)gameObject2.transform;
				CielOSPanelChrome.Anchor(rectTransform4, 0f, 0.5f);
				rectTransform4.pivot = new Vector2(0f, 0.5f);
				rectTransform4.sizeDelta = new Vector2(180f, 30f);
				rectTransform4.anchoredPosition = new Vector2(220f, 0f);
			}
			this.workspaceSwitcher.Build(4);
			float num = -24f;
			Button button2 = this.MakeIconButton(rectTransform, "PowerButton", CielOSIconId.Power, color3, 1f, num, 34f);
			this.SetButtonIconColor(button2, (active != null) ? active.closeButton : color3);
			num -= 34f;
			GameObject gameObject3 = new GameObject("Avatar", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject3.transform.SetParent(rectTransform, false);
			RectTransform rectTransform5 = (RectTransform)gameObject3.transform;
			CielOSPanelChrome.Anchor(rectTransform5, 1f, 0.5f);
			rectTransform5.sizeDelta = new Vector2(26f, 26f);
			rectTransform5.anchoredPosition = new Vector2(num, 0f);
			Image component2 = gameObject3.GetComponent<Image>();
			component2.color = color2;
			CielOSTheme.Circle(component2);
			GameObject gameObject4 = new GameObject("Glyph", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject4.transform.SetParent(gameObject3.transform, false);
			RectTransform rectTransform6 = (RectTransform)gameObject4.transform;
			rectTransform6.anchorMin = Vector2.zero;
			rectTransform6.anchorMax = Vector2.one;
			rectTransform6.offsetMin = new Vector2(4f, 4f);
			rectTransform6.offsetMax = new Vector2(-4f, -4f);
			CielOSIcon.Apply(gameObject4.GetComponent<Image>(), CielOSIconId.Avatar, (active != null) ? active.logoAccent : Color.white, 64);
			num -= 42f;
			TMP_Text tmp_Text2 = this.MakeLabel(rectTransform, "Clock", "00:00", 15f, color, TextAlignmentOptions.MidlineRight);
			RectTransform rectTransform7 = (RectTransform)tmp_Text2.transform;
			CielOSPanelChrome.Anchor(rectTransform7, 1f, 0.5f);
			rectTransform7.pivot = new Vector2(1f, 0.5f);
			rectTransform7.sizeDelta = new Vector2(64f, 28f);
			rectTransform7.anchoredPosition = new Vector2(num, 0f);
			num -= 76f;
			this.MakeIcon(rectTransform, "Bell", CielOSIconId.Bell, color3, num);
			num -= 34f;
			Image volImg = this.MakeIcon(rectTransform, "Volume", CielOSIconId.Volume, color3, num);
			Button button3 = volImg.gameObject.AddComponent<Button>();
			volImg.raycastTarget = true;
			button3.onClick.AddListener(delegate
			{
				CielOSVolumePopup.Ensure(this.GetComponentInParent<Canvas>().transform).ToggleAt(volImg.rectTransform);
			});
			num -= 34f;
			this.MakeIcon(rectTransform, "Wifi", CielOSIconId.Wifi, color3, num);
			if (this._panel != null)
			{
				this._panel.BindRuntimeControls(tmp_Text2, button2, button);
			}
		}

		// Token: 0x06009CFC RID: 40188 RVA: 0x00406EFC File Offset: 0x004050FC
		private static void Anchor(RectTransform rt, float ax, float ay)
		{
			Vector2 vector = new Vector2(ax, ay);
			rt.anchorMax = vector;
			rt.anchorMin = vector;
			rt.pivot = new Vector2(0.5f, 0.5f);
		}

		// Token: 0x06009CFD RID: 40189 RVA: 0x00406F38 File Offset: 0x00405138
		private Button MakeIconButton(RectTransform parent, string name, CielOSIconId id, Color tint, float anchorX, float x, float hitSize)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(Button)
			});
			gameObject.transform.SetParent(parent, false);
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			CielOSPanelChrome.Anchor(rectTransform, anchorX, 0.5f);
			rectTransform.sizeDelta = new Vector2(hitSize, hitSize);
			rectTransform.anchoredPosition = new Vector2(x, 0f);
			gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
			Button component = gameObject.GetComponent<Button>();
			component.transition = Selectable.Transition.None;
			GameObject gameObject2 = new GameObject("Glyph", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform2 = (RectTransform)gameObject2.transform;
			rectTransform2.anchorMin = Vector2.zero;
			rectTransform2.anchorMax = Vector2.one;
			rectTransform2.offsetMin = new Vector2(5f, 5f);
			rectTransform2.offsetMax = new Vector2(-5f, -5f);
			CielOSIcon.Apply(gameObject2.GetComponent<Image>(), id, tint, 64);
			return component;
		}

		// Token: 0x06009CFE RID: 40190 RVA: 0x00407088 File Offset: 0x00405288
		private void SetButtonIconColor(Button btn, Color c)
		{
			if (btn == null)
			{
				return;
			}
			Transform transform = btn.transform.Find("Glyph");
			if (transform != null)
			{
				Image component = transform.GetComponent<Image>();
				if (component != null)
				{
					component.color = c;
				}
			}
		}

		// Token: 0x06009CFF RID: 40191 RVA: 0x004070D0 File Offset: 0x004052D0
		private Image MakeIcon(RectTransform parent, string name, CielOSIconId id, Color tint, float x)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject.transform.SetParent(parent, false);
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			CielOSPanelChrome.Anchor(rectTransform, 1f, 0.5f);
			rectTransform.sizeDelta = new Vector2(22f, 22f);
			rectTransform.anchoredPosition = new Vector2(x, 0f);
			Image component = gameObject.GetComponent<Image>();
			CielOSIcon.Apply(component, id, tint, 64);
			return component;
		}

		// Token: 0x06009D00 RID: 40192 RVA: 0x00407164 File Offset: 0x00405364
		private TMP_Text MakeLabel(RectTransform parent, string name, string content, float size, Color color, TextAlignmentOptions align)
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
			component.raycastTarget = false;
			component.enableWordWrapping = false;
			if (CielOSTheme.Active != null && CielOSTheme.Active.uiFont != null)
			{
				component.font = CielOSTheme.Active.uiFont;
			}
			return component;
		}

		// Token: 0x0400A17E RID: 41342
		[Tooltip("Optional: workspace switcher gets built here too. If null, created on this panel.")]
		[SerializeField]
		private CielOSWorkspaceSwitcher workspaceSwitcher;

		// Token: 0x0400A17F RID: 41343
		private CielOSPanel _panel;

		// Token: 0x0400A180 RID: 41344
		private bool _built;
	}
}
