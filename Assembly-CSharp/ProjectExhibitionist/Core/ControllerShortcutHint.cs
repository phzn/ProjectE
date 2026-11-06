using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A39 RID: 2617
	[DisallowMultipleComponent]
	public sealed class ControllerShortcutHint : MonoBehaviour
	{
		// Token: 0x06004A4D RID: 19021 RVA: 0x001CCB0F File Offset: 0x001CAD0F
		public static void Bind(Transform strip, string binding, bool hideForSlider = false)
		{
			if (strip == null)
			{
				return;
			}
			ControllerShortcutHint controllerShortcutHint = strip.GetComponent<ControllerShortcutHint>() ?? strip.gameObject.AddComponent<ControllerShortcutHint>();
			controllerShortcutHint._binding = binding;
			controllerShortcutHint._hideForSlider = hideForSlider;
			controllerShortcutHint._version = -1;
		}

		// Token: 0x06004A4E RID: 19022 RVA: 0x001CCB44 File Offset: 0x001CAD44
		private void OnDisable()
		{
			if (this._label != null)
			{
				this._label.gameObject.SetActive(false);
			}
		}

		// Token: 0x06004A4F RID: 19023 RVA: 0x001CCB68 File Offset: 0x001CAD68
		private void LateUpdate()
		{
			GameObject currentSelection = ControllerUINavigation.CurrentSelection;
			bool flag = ControllerInput.IsActive && ControllerUINavigation.OwnsInput && !string.IsNullOrEmpty(this._binding) && (!this._hideForSlider || !(currentSelection != null) || !(currentSelection.GetComponent<Slider>() != null));
			if (this._label != null)
			{
				this._label.gameObject.SetActive(flag);
			}
			if (!flag)
			{
				return;
			}
			if (this._label == null)
			{
				GameObject gameObject = new GameObject("Controller Strip Shortcut", new Type[]
				{
					typeof(RectTransform),
					typeof(LayoutElement),
					typeof(TextMeshProUGUI)
				});
				gameObject.layer = base.gameObject.layer;
				gameObject.transform.SetParent(base.transform, false);
				gameObject.GetComponent<LayoutElement>().ignoreLayout = true;
				this._label = gameObject.GetComponent<TextMeshProUGUI>();
				this._label.font = TMP_Settings.defaultFontAsset;
				this._label.fontSize = 20f;
				this._label.alignment = TextAlignmentOptions.Center;
				this._label.raycastTarget = false;
				this._label.enableWordWrapping = false;
				this._label.overflowMode = TextOverflowModes.Overflow;
				RectTransform rectTransform = this._label.rectTransform;
				RectTransform rectTransform2 = this._label.rectTransform;
				Vector2 vector = new Vector2(0.5f, 1f);
				rectTransform2.anchorMax = vector;
				rectTransform.anchorMin = vector;
				this._label.rectTransform.pivot = new Vector2(0.5f, 0f);
				this._label.rectTransform.anchoredPosition = new Vector2(0f, 4f);
				this._label.rectTransform.sizeDelta = new Vector2(160f, 24f);
			}
			if (this._version == ControllerGlyphs.Version)
			{
				return;
			}
			this._version = ControllerGlyphs.Version;
			ControllerGlyphs.Apply(this._label, this._binding);
		}

		// Token: 0x04004C85 RID: 19589
		private TMP_Text _label;

		// Token: 0x04004C86 RID: 19590
		private string _binding;

		// Token: 0x04004C87 RID: 19591
		private bool _hideForSlider;

		// Token: 0x04004C88 RID: 19592
		private int _version = -1;
	}
}
