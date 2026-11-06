using System;
using Michsky.MUIP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoZo.CharacterCreator.MC.StyleSystem.UI
{
	// Token: 0x02000814 RID: 2068
	[DisallowMultipleComponent]
	public sealed class MCStyleSurfaceSelectorLayout : MonoBehaviour
	{
		// Token: 0x06003733 RID: 14131 RVA: 0x0013ED64 File Offset: 0x0013CF64
		public static void Bind(HorizontalSelector value)
		{
			if (value == null)
			{
				return;
			}
			MCStyleSurfaceSelectorLayout mcstyleSurfaceSelectorLayout = value.GetComponent<MCStyleSurfaceSelectorLayout>() ?? value.gameObject.AddComponent<MCStyleSurfaceSelectorLayout>();
			mcstyleSurfaceSelectorLayout.selector = value;
			MCStyleSurfaceSelectorLayout.SetIdleVisibility(value.label, true);
			MCStyleSurfaceSelectorLayout.SetIdleVisibility(value.labelHelper, false);
			mcstyleSurfaceSelectorLayout.Apply();
		}

		// Token: 0x06003734 RID: 14132 RVA: 0x0013EDB4 File Offset: 0x0013CFB4
		private void LateUpdate()
		{
			this.Apply();
		}

		// Token: 0x06003735 RID: 14133 RVA: 0x0013EDBC File Offset: 0x0013CFBC
		private void Apply()
		{
			if (this.selector == null)
			{
				return;
			}
			MCStyleSurfaceSelectorLayout.Fit(this.selector.label, this.selector.contentLayout);
			MCStyleSurfaceSelectorLayout.Fit(this.selector.labelHelper, this.selector.contentLayoutHelper);
			if (this.selector.selectorAnimator == null || !this.selector.selectorAnimator.enabled)
			{
				MCStyleSurfaceSelectorLayout.SetIdleVisibility(this.selector.label, true);
				MCStyleSurfaceSelectorLayout.SetIdleVisibility(this.selector.labelHelper, false);
				return;
			}
			if (this.selector.label != null && !this.selector.label.enabled)
			{
				this.selector.label.enabled = true;
			}
			if (this.selector.labelHelper != null && !this.selector.labelHelper.enabled)
			{
				this.selector.labelHelper.enabled = true;
			}
		}

		// Token: 0x06003736 RID: 14134 RVA: 0x0013EEC4 File Offset: 0x0013D0C4
		private static void SetIdleVisibility(TextMeshProUGUI label, bool visible)
		{
			if (label == null)
			{
				return;
			}
			if (label.enabled != visible)
			{
				label.enabled = visible;
			}
			CanvasGroup component = label.transform.parent.GetComponent<CanvasGroup>();
			if (component != null && component.alpha != (visible ? 1f : 0f))
			{
				component.alpha = (visible ? 1f : 0f);
			}
		}

		// Token: 0x06003737 RID: 14135 RVA: 0x0013EF34 File Offset: 0x0013D134
		private static void Fit(TextMeshProUGUI label, HorizontalLayoutGroup layout)
		{
			if (label == null)
			{
				return;
			}
			if (layout != null && layout.enabled)
			{
				layout.enabled = false;
			}
			ContentSizeFitter component = label.GetComponent<ContentSizeFitter>();
			if (component != null && component.enabled)
			{
				component.enabled = false;
			}
			RectTransform rectTransform = label.transform.parent as RectTransform;
			if (rectTransform != null)
			{
				if (rectTransform.anchorMin != Vector2.zero)
				{
					rectTransform.anchorMin = Vector2.zero;
				}
				if (rectTransform.anchorMax != Vector2.one)
				{
					rectTransform.anchorMax = Vector2.one;
				}
				Vector2 sizeDelta = rectTransform.sizeDelta;
				if (sizeDelta.y != 0f)
				{
					rectTransform.sizeDelta = new Vector2(sizeDelta.x, 0f);
				}
				Vector2 anchoredPosition = rectTransform.anchoredPosition;
				if (anchoredPosition.y != 0f)
				{
					rectTransform.anchoredPosition = new Vector2(anchoredPosition.x, 0f);
				}
			}
			RectTransform rectTransform2 = label.rectTransform;
			if (rectTransform2.anchorMin != Vector2.zero)
			{
				rectTransform2.anchorMin = Vector2.zero;
			}
			if (rectTransform2.anchorMax != Vector2.one)
			{
				rectTransform2.anchorMax = Vector2.one;
			}
			if (rectTransform2.pivot != new Vector2(0.5f, 0.5f))
			{
				rectTransform2.pivot = new Vector2(0.5f, 0.5f);
			}
			if (rectTransform2.offsetMin != new Vector2(44f, 3f))
			{
				rectTransform2.offsetMin = new Vector2(44f, 3f);
			}
			if (rectTransform2.offsetMax != new Vector2(-44f, -3f))
			{
				rectTransform2.offsetMax = new Vector2(-44f, -3f);
			}
			label.enableWordWrapping = false;
			label.enableAutoSizing = true;
			label.fontSizeMin = 9f;
			label.fontSizeMax = 16f;
			label.alignment = TextAlignmentOptions.Center;
			label.overflowMode = TextOverflowModes.Ellipsis;
			label.raycastTarget = false;
		}

		// Token: 0x04003754 RID: 14164
		private HorizontalSelector selector;
	}
}
