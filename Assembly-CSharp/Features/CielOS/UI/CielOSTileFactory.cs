using System;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CielOS.UI
{
	// Token: 0x020013D0 RID: 5072
	public static class CielOSTileFactory
	{
		// Token: 0x06009CA9 RID: 40105 RVA: 0x004039F0 File Offset: 0x00401BF0
		public static CielOSTileFactory.Tile BuildAppTile(Transform parent, string appId, string displayName, float iconSize = 40f, bool showLabel = false, bool addIndicator = false, bool colored = false, bool locked = false, bool fitLabel = false)
		{
			CielOSTheme active = CielOSTheme.Active;
			Color color = ((active != null) ? new Color(active.controlBackground.r * 0.7f, active.controlBackground.g * 0.7f, active.controlBackground.b * 0.7f, 1f) : new Color(0.13f, 0.1f, 0.17f, 1f));
			Color color2 = ((active != null) ? active.logoAccent : new Color(0.95f, 0.88f, 1f, 1f));
			GameObject gameObject = new GameObject(string.IsNullOrEmpty(displayName) ? appId : displayName, new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(Button),
				typeof(LayoutElement)
			});
			gameObject.transform.SetParent(parent, false);
			Image component = gameObject.GetComponent<Image>();
			component.color = color;
			CielOSTheme.Round(component);
			CielOSTileFactory.Tile tile = new CielOSTileFactory.Tile
			{
				Root = gameObject,
				Button = gameObject.GetComponent<Button>(),
				Background = component
			};
			Button button = tile.Button;
			button.transition = Selectable.Transition.ColorTint;
			button.targetGraphic = component;
			ColorBlock colors = button.colors;
			colors.normalColor = Color.white;
			colors.highlightedColor = ((active != null) ? CielOSTileFactory.Lighten(color, active.hover) : new Color(1.1f, 1.1f, 1.1f, 1f));
			colors.pressedColor = new Color(0.9f, 0.9f, 0.9f, 1f);
			colors.selectedColor = Color.white;
			colors.fadeDuration = 0.1f;
			button.colors = colors;
			GameObject gameObject2 = new GameObject("Icon", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform = (RectTransform)gameObject2.transform;
			Vector2 vector = new Vector2(0.5f, showLabel ? 0.62f : 0.5f);
			rectTransform.pivot = vector;
			rectTransform.anchorMin = (rectTransform.anchorMax = vector);
			rectTransform.anchoredPosition = Vector2.zero;
			rectTransform.sizeDelta = new Vector2(iconSize, iconSize);
			Image component2 = gameObject2.GetComponent<Image>();
			component2.raycastTarget = false;
			if (colored)
			{
				CielOSIcon.ApplyColored(component2, CielOSAppIcons.ForApp(appId), 64);
			}
			else
			{
				CielOSIcon.Apply(component2, CielOSAppIcons.ForApp(appId), color2, 64);
			}
			tile.Icon = component2;
			CielOSAppAccess.ApplyLock(component2, locked);
			if (showLabel)
			{
				GameObject gameObject3 = new GameObject("Label", new Type[]
				{
					typeof(RectTransform),
					typeof(TextMeshProUGUI)
				});
				gameObject3.transform.SetParent(gameObject.transform, false);
				RectTransform rectTransform2 = (RectTransform)gameObject3.transform;
				rectTransform2.anchorMin = new Vector2(0f, 0f);
				rectTransform2.anchorMax = new Vector2(1f, 0.34f);
				rectTransform2.offsetMin = new Vector2(2f, 2f);
				rectTransform2.offsetMax = new Vector2(-2f, 0f);
				TextMeshProUGUI component3 = gameObject3.GetComponent<TextMeshProUGUI>();
				component3.text = displayName;
				component3.fontSize = 12f;
				component3.alignment = TextAlignmentOptions.Center;
				component3.color = ((active != null) ? active.controlText : Color.white);
				component3.raycastTarget = false;
				component3.enableWordWrapping = fitLabel;
				if (fitLabel)
				{
					component3.enableAutoSizing = true;
					component3.fontSizeMin = 10f;
					component3.fontSizeMax = 12f;
				}
				component3.overflowMode = TextOverflowModes.Ellipsis;
				if (active != null && active.uiFont != null)
				{
					component3.font = active.uiFont;
				}
				LocalizedGameText.BindSource(component3, "cielos_shell", displayName, null).FitTranslationToRectangle(false);
			}
			if (addIndicator)
			{
				GameObject gameObject4 = new GameObject("ActiveDot", new Type[]
				{
					typeof(RectTransform),
					typeof(Image)
				});
				gameObject4.transform.SetParent(gameObject.transform, false);
				RectTransform rectTransform3 = (RectTransform)gameObject4.transform;
				vector = new Vector2(0.04f, 0.5f);
				rectTransform3.pivot = vector;
				rectTransform3.anchorMin = (rectTransform3.anchorMax = vector);
				rectTransform3.sizeDelta = new Vector2(6f, 22f);
				rectTransform3.anchoredPosition = new Vector2(2f, 0f);
				Image component4 = gameObject4.GetComponent<Image>();
				component4.color = ((active != null) ? active.dockActiveIndicator : new Color(0.78f, 0.5f, 0.92f, 1f));
				component4.raycastTarget = false;
				CielOSTheme.Round(component4);
				gameObject4.SetActive(false);
				tile.Indicator = component4;
			}
			CielOSTheme.AddBorder((RectTransform)gameObject.transform, null, 1f);
			CielOSSfx.Attach(tile.Button);
			return tile;
		}

		// Token: 0x06009CAA RID: 40106 RVA: 0x00403F20 File Offset: 0x00402120
		private static Color Lighten(Color baseColor, Color hover)
		{
			float a = hover.a;
			return new Color(Mathf.Lerp(baseColor.r, hover.r, a), Mathf.Lerp(baseColor.g, hover.g, a), Mathf.Lerp(baseColor.b, hover.b, a), 1f);
		}

		// Token: 0x020013D1 RID: 5073
		public class Tile
		{
			// Token: 0x0400A13C RID: 41276
			public GameObject Root;

			// Token: 0x0400A13D RID: 41277
			public Button Button;

			// Token: 0x0400A13E RID: 41278
			public Image Background;

			// Token: 0x0400A13F RID: 41279
			public Image Icon;

			// Token: 0x0400A140 RID: 41280
			public Image Indicator;
		}
	}
}
