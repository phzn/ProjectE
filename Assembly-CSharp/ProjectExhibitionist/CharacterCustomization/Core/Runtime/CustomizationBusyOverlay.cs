using System;
using Features.Localization;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000AE9 RID: 2793
	[DisallowMultipleComponent]
	public sealed class CustomizationBusyOverlay : MonoBehaviour
	{
		// Token: 0x06004EFB RID: 20219 RVA: 0x001E9AC8 File Offset: 0x001E7CC8
		public static CustomizationBusyOverlay GetOrCreate(Transform host)
		{
			if (host == null)
			{
				return null;
			}
			CustomizationBusyOverlay componentInChildren = host.GetComponentInChildren<CustomizationBusyOverlay>(true);
			if (componentInChildren != null)
			{
				componentInChildren._presetAreaRect = CustomizationBusyOverlay.FindPresetAreaRect(host);
				return componentInChildren;
			}
			RectTransform rectTransform = CustomizationBusyOverlay.FindCanvasRect(host);
			if (rectTransform == null)
			{
				Debug.LogWarning("[CustomizationBusyOverlay] Nessun RectTransform Canvas trovato.", host);
				return null;
			}
			GameObject gameObject = new GameObject("CustomizationBusyOverlay", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image),
				typeof(CanvasGroup),
				typeof(Canvas),
				typeof(GraphicRaycaster),
				typeof(CustomizationBusyOverlay)
			});
			gameObject.transform.SetParent(rectTransform, false);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.one;
			component.offsetMin = Vector2.zero;
			component.offsetMax = Vector2.zero;
			component.SetAsLastSibling();
			Image component2 = gameObject.GetComponent<Image>();
			component2.color = new Color(0f, 0f, 0f, 0.001f);
			component2.raycastTarget = true;
			CanvasGroup component3 = gameObject.GetComponent<CanvasGroup>();
			component3.alpha = 0f;
			component3.interactable = true;
			component3.blocksRaycasts = true;
			Canvas component4 = gameObject.GetComponent<Canvas>();
			component4.overrideSorting = true;
			component4.sortingOrder = 32000;
			GameplayHudVisibility.RegisterCanvas(component4);
			GameObject gameObject2 = new GameObject("ToolbarStatus", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			RectTransform component5 = gameObject2.GetComponent<RectTransform>();
			RectTransform rectTransform2 = component5;
			RectTransform rectTransform3 = component5;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform3.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			component5.pivot = new Vector2(0.5f, 0.5f);
			component5.sizeDelta = new Vector2(360f, 58f);
			Image component6 = gameObject2.GetComponent<Image>();
			component6.color = new Color(0.13f, 0.055f, 0.19f, 0.96f);
			component6.raycastTarget = false;
			GameObject gameObject3 = new GameObject("Label", new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			gameObject3.transform.SetParent(gameObject2.transform, false);
			RectTransform component7 = gameObject3.GetComponent<RectTransform>();
			component7.anchorMin = Vector2.zero;
			component7.anchorMax = Vector2.one;
			component7.offsetMin = new Vector2(14f, 4f);
			component7.offsetMax = new Vector2(-14f, -4f);
			TMP_Text component8 = gameObject3.GetComponent<TMP_Text>();
			component8.text = "LOADING...";
			component8.fontSize = 20f;
			component8.fontStyle = FontStyles.Bold;
			component8.characterSpacing = 3f;
			component8.alignment = TextAlignmentOptions.Center;
			component8.color = new Color(1f, 0.84f, 1f, 0.96f);
			component8.raycastTarget = false;
			CustomizationBusyOverlay component9 = gameObject.GetComponent<CustomizationBusyOverlay>();
			component9._group = component3;
			component9._label = component8;
			LocalizedGameText localizedGameText = LocalizedGameText.BindSource(component8, "cielos_apps", "LOADING", (string value) => value + "...");
			if (localizedGameText != null)
			{
				localizedGameText.FitTranslationToRectangle(false);
			}
			component9._rootRect = component;
			component9._statusRect = component5;
			component9._presetAreaRect = CustomizationBusyOverlay.FindPresetAreaRect(host);
			component9.PositionInsidePresetArea();
			gameObject.SetActive(false);
			return component9;
		}

		// Token: 0x06004EFC RID: 20220 RVA: 0x001E9E70 File Offset: 0x001E8070
		public void Show(string message)
		{
			if (this._group == null)
			{
				this._group = base.GetComponent<CanvasGroup>();
			}
			base.gameObject.SetActive(true);
			base.transform.SetAsLastSibling();
			this.PositionInsidePresetArea();
			this._baseMessage = (string.IsNullOrWhiteSpace(message) ? "LOADING" : message.Trim().TrimEnd('.'));
			if (this._label != null)
			{
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this._label, "cielos_apps", this._baseMessage, (string value) => value + "...");
				if (localizedGameText != null)
				{
					localizedGameText.FitTranslationToRectangle(false);
				}
			}
			this._group.interactable = true;
			this._group.blocksRaycasts = true;
			this._group.alpha = 1f;
			this._targetAlpha = 1f;
			Canvas.ForceUpdateCanvases();
		}

		// Token: 0x06004EFD RID: 20221 RVA: 0x001E9F5D File Offset: 0x001E815D
		public void Hide()
		{
			this._targetAlpha = 0f;
			if (this._group != null)
			{
				this._group.interactable = false;
				this._group.blocksRaycasts = false;
			}
		}

		// Token: 0x06004EFE RID: 20222 RVA: 0x001E9F90 File Offset: 0x001E8190
		private void Update()
		{
			if (this._label != null && this._targetAlpha > 0f)
			{
				int dotCount = 1 + Mathf.FloorToInt(Time.unscaledTime * 3f) % 3;
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this._label, "cielos_apps", this._baseMessage, (string value) => value + new string('.', dotCount));
				if (localizedGameText != null)
				{
					localizedGameText.FitTranslationToRectangle(false);
				}
				this.PositionInsidePresetArea();
			}
			if (this._group == null)
			{
				return;
			}
			this._group.alpha = Mathf.MoveTowards(this._group.alpha, this._targetAlpha, 8f * Time.unscaledDeltaTime);
			if (this._targetAlpha <= 0f && this._group.alpha <= 0.001f)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x06004EFF RID: 20223 RVA: 0x001EA074 File Offset: 0x001E8274
		private void PositionInsidePresetArea()
		{
			if (this._rootRect == null || this._statusRect == null || this._presetAreaRect == null)
			{
				return;
			}
			Vector3 vector = this._presetAreaRect.TransformPoint(this._presetAreaRect.rect.center);
			this._statusRect.position = vector;
			Vector3 lossyScale = this._presetAreaRect.lossyScale;
			Vector3 lossyScale2 = this._rootRect.lossyScale;
			float num = this._presetAreaRect.rect.width * Mathf.Abs(lossyScale.x) / Mathf.Max(0.0001f, Mathf.Abs(lossyScale2.x));
			this._statusRect.sizeDelta = new Vector2(Mathf.Clamp(num * 0.62f, 280f, 420f), 58f);
		}

		// Token: 0x06004F00 RID: 20224 RVA: 0x001EA158 File Offset: 0x001E8358
		private static RectTransform FindCanvasRect(Transform host)
		{
			Canvas[] componentsInChildren = host.GetComponentsInChildren<Canvas>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != null && componentsInChildren[i].gameObject.activeInHierarchy)
				{
					RectTransform rectTransform = componentsInChildren[i].transform as RectTransform;
					if (rectTransform != null)
					{
						return rectTransform;
					}
				}
			}
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				if (componentsInChildren[j] != null)
				{
					RectTransform rectTransform2 = componentsInChildren[j].transform as RectTransform;
					if (rectTransform2 != null)
					{
						return rectTransform2;
					}
				}
			}
			return host as RectTransform;
		}

		// Token: 0x06004F01 RID: 20225 RVA: 0x001EA1E0 File Offset: 0x001E83E0
		private static RectTransform FindPresetAreaRect(Transform host)
		{
			RectTransform rectTransform = null;
			foreach (RectTransform rectTransform2 in host.GetComponentsInChildren<RectTransform>(true))
			{
				if (!(rectTransform2 == null) && rectTransform2.gameObject.activeInHierarchy)
				{
					if (rectTransform2.name == "MainPanel" && rectTransform2.parent != null && rectTransform2.parent.name == "Tools" && rectTransform2.parent.parent != null && rectTransform2.parent.parent.name == "Toolbar")
					{
						return rectTransform2;
					}
					if (rectTransform == null && rectTransform2.name == "Tools" && rectTransform2.parent != null && rectTransform2.parent.name == "Toolbar")
					{
						rectTransform = rectTransform2;
					}
				}
			}
			return rectTransform;
		}

		// Token: 0x0400502D RID: 20525
		private CanvasGroup _group;

		// Token: 0x0400502E RID: 20526
		private TMP_Text _label;

		// Token: 0x0400502F RID: 20527
		private RectTransform _rootRect;

		// Token: 0x04005030 RID: 20528
		private RectTransform _statusRect;

		// Token: 0x04005031 RID: 20529
		private RectTransform _presetAreaRect;

		// Token: 0x04005032 RID: 20530
		private string _baseMessage = "LOADING";

		// Token: 0x04005033 RID: 20531
		private float _targetAlpha;
	}
}
