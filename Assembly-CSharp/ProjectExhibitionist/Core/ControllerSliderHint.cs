using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A3A RID: 2618
	[DisallowMultipleComponent]
	public sealed class ControllerSliderHint : MonoBehaviour
	{
		// Token: 0x17000C8D RID: 3213
		// (get) Token: 0x06004A51 RID: 19025 RVA: 0x001CCD81 File Offset: 0x001CAF81
		// (set) Token: 0x06004A52 RID: 19026 RVA: 0x001CCD89 File Offset: 0x001CAF89
		public Button ResetButton { get; private set; }

		// Token: 0x06004A53 RID: 19027 RVA: 0x001CCD92 File Offset: 0x001CAF92
		public static void Ensure(Slider slider)
		{
			if (slider != null && slider.GetComponent<ControllerSliderHint>() == null)
			{
				slider.gameObject.AddComponent<ControllerSliderHint>();
			}
		}

		// Token: 0x06004A54 RID: 19028 RVA: 0x001CCDB7 File Offset: 0x001CAFB7
		public static void BindReset(Slider slider, Button reset)
		{
			ControllerSliderHint.Ensure(slider);
			if (slider != null)
			{
				slider.GetComponent<ControllerSliderHint>().ResetButton = reset;
			}
		}

		// Token: 0x06004A55 RID: 19029 RVA: 0x001CCDD4 File Offset: 0x001CAFD4
		private void OnDisable()
		{
			if (this._label != null)
			{
				this._label.gameObject.SetActive(false);
			}
		}

		// Token: 0x06004A56 RID: 19030 RVA: 0x001CCDF8 File Offset: 0x001CAFF8
		private void LateUpdate()
		{
			bool flag = ControllerInput.IsActive && ControllerUINavigation.CurrentSelection == base.gameObject;
			if (this._label != null)
			{
				this._label.gameObject.SetActive(flag);
			}
			if (!flag)
			{
				return;
			}
			if (this._slider == null)
			{
				this._slider = base.GetComponent<Slider>();
			}
			if (this._label == null)
			{
				GameObject gameObject = new GameObject("Slider Keys", new Type[]
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
				this._label.fontSize = 18f;
				this._label.alignment = TextAlignmentOptions.Center;
				this._label.raycastTarget = false;
				this._label.enableWordWrapping = false;
				RectTransform rectTransform = this._label.rectTransform;
				RectTransform rectTransform2 = this._label.rectTransform;
				Vector2 vector = new Vector2(0.5f, 1f);
				rectTransform2.anchorMax = vector;
				rectTransform.anchorMin = vector;
				this._label.rectTransform.pivot = new Vector2(0.5f, 0f);
				this._label.rectTransform.anchoredPosition = new Vector2(0f, 2f);
				this._label.rectTransform.sizeDelta = new Vector2(100f, 22f);
			}
			string text = "LB / RB";
			if (ControllerUINavigation.FindSliderReset(this._slider) != null)
			{
				text += "  X";
			}
			if (this._version == ControllerGlyphs.Version && text == this._lastBinding)
			{
				return;
			}
			this._version = ControllerGlyphs.Version;
			this._lastBinding = text;
			ControllerGlyphs.Apply(this._label, text);
		}

		// Token: 0x04004C8A RID: 19594
		private TMP_Text _label;

		// Token: 0x04004C8B RID: 19595
		private Slider _slider;

		// Token: 0x04004C8C RID: 19596
		private string _lastBinding;

		// Token: 0x04004C8D RID: 19597
		private int _version = -1;
	}
}
