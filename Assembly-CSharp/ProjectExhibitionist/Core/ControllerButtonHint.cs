using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A34 RID: 2612
	[DisallowMultipleComponent]
	public sealed class ControllerButtonHint : MonoBehaviour
	{
		// Token: 0x06004A36 RID: 18998 RVA: 0x001CBF80 File Offset: 0x001CA180
		public static void Bind(GameObject target, string binding)
		{
			if (target == null)
			{
				return;
			}
			ControllerButtonHint controllerButtonHint = target.GetComponent<ControllerButtonHint>() ?? target.AddComponent<ControllerButtonHint>();
			if (controllerButtonHint._binding == binding)
			{
				return;
			}
			controllerButtonHint._binding = binding;
			controllerButtonHint._version = -1;
		}

		// Token: 0x06004A37 RID: 18999 RVA: 0x001CBFC5 File Offset: 0x001CA1C5
		private void OnDisable()
		{
			this.Hide();
		}

		// Token: 0x06004A38 RID: 19000 RVA: 0x001CBFD0 File Offset: 0x001CA1D0
		private void Hide()
		{
			if (this._glyph != null)
			{
				this._glyph.gameObject.SetActive(false);
			}
			if (this._reservedSpace && this._caption != null)
			{
				this._caption.margin = this._originalMargin;
				this._caption.enableAutoSizing = this._originalAutoSize;
				this._caption.fontSizeMin = this._originalMin;
				this._caption.fontSizeMax = this._originalMax;
				this._caption.fontSize = this._originalFontSize;
			}
			if (this._hiddenIcon && this._icon != null)
			{
				this._icon.enabled = true;
			}
			this._hiddenIcon = false;
			this._reservedSpace = false;
		}

		// Token: 0x06004A39 RID: 19001 RVA: 0x001CC098 File Offset: 0x001CA298
		private void LateUpdate()
		{
			if (string.IsNullOrEmpty(this._binding) || !ControllerInput.IsActive || !ControllerUINavigation.OwnsInput || !ControllerUINavigation.IsFocusable(base.gameObject))
			{
				this.Hide();
				return;
			}
			if (this._glyph == null)
			{
				this.Build();
			}
			if (this._glyph == null)
			{
				return;
			}
			this._glyph.gameObject.SetActive(true);
			string binding = this._binding;
			if (this._version != ControllerGlyphs.Version || this._glyph.text != ControllerGlyphs.Format(binding))
			{
				this._version = ControllerGlyphs.Version;
				ControllerGlyphs.Apply(this._glyph, binding);
			}
			RectTransform rectTransform = (RectTransform)base.transform;
			float num = Mathf.Clamp((this._caption != null) ? this._caption.fontSize : ((this._legacyCaption != null) ? ((float)this._legacyCaption.fontSize) : (rectTransform.rect.height * 0.45f)), 10f, 22f);
			this._glyph.fontSize = num;
			this._glyph.rectTransform.sizeDelta = new Vector2(num * ((this._binding.Length > 2) ? 1.6f : 1.2f), num * 1.3f);
			Vector3 vector = new Vector3(rectTransform.rect.xMax - num * 0.6f - 4f, rectTransform.rect.yMin + num * 0.65f + 3f, 0f);
			if (this._icon != null)
			{
				if (this._icon.enabled)
				{
					this._icon.enabled = false;
					this._hiddenIcon = true;
				}
				vector = rectTransform.InverseTransformPoint(this._icon.rectTransform.TransformPoint(this._icon.rectTransform.rect.center));
			}
			else if (this._legacyCaption != null)
			{
				Rect rect = this._legacyCaption.rectTransform.rect;
				vector = rectTransform.InverseTransformPoint(this._legacyCaption.transform.TransformPoint(new Vector3(rect.center.x - this._legacyCaption.preferredWidth * 0.5f - num * 0.65f - 5f, rect.center.y, 0f)));
			}
			else if (this._caption != null && this._caption.gameObject.activeInHierarchy)
			{
				if (this._caption.havePropertiesChanged)
				{
					this._caption.ForceMeshUpdate(false, false);
				}
				Bounds textBounds = this._caption.textBounds;
				vector = rectTransform.InverseTransformPoint(this._caption.transform.TransformPoint(new Vector3(textBounds.min.x - num * 0.65f - 5f, textBounds.center.y, 0f)));
				if (vector.x - num * 0.55f < rectTransform.rect.xMin + 3f && !this._reservedSpace)
				{
					this._originalMargin = this._caption.margin;
					this._originalFontSize = this._caption.fontSize;
					this._originalAutoSize = this._caption.enableAutoSizing;
					this._originalMin = this._caption.fontSizeMin;
					this._originalMax = this._caption.fontSizeMax;
					Vector4 originalMargin = this._originalMargin;
					Vector3 vector2 = this._caption.rectTransform.InverseTransformPoint(rectTransform.TransformPoint(new Vector3(rectTransform.rect.xMin + num * 1.2f + 9f, 0f, 0f)));
					originalMargin.x = Mathf.Max(originalMargin.x, vector2.x - this._caption.rectTransform.rect.xMin);
					this._caption.margin = originalMargin;
					this._caption.enableAutoSizing = true;
					this._caption.fontSizeMax = this._originalFontSize;
					this._caption.fontSizeMin = Mathf.Min(this._originalFontSize, Mathf.Max(8f, this._originalFontSize * 0.78f));
					this._reservedSpace = true;
				}
				vector.x = Mathf.Max(rectTransform.rect.xMin + num * 0.6f + 4f, vector.x);
			}
			this._glyph.rectTransform.localPosition = vector;
			this._glyph.color = (ControllerUINavigation.IsUsable(base.gameObject) ? Color.white : new Color(1f, 1f, 1f, 0.5f));
		}

		// Token: 0x06004A3A RID: 19002 RVA: 0x001CC594 File Offset: 0x001CA794
		private void Build()
		{
			foreach (Image image in base.GetComponentsInChildren<Image>(false))
			{
				if (image.name == "Icon" && image.enabled && image.rectTransform.rect.width <= 40f)
				{
					Selectable componentInParent = image.GetComponentInParent<Selectable>();
					if (((componentInParent != null) ? componentInParent.gameObject : null) == base.gameObject)
					{
						this._icon = image;
						break;
					}
				}
			}
			foreach (TMP_Text tmp_Text in base.GetComponentsInChildren<TMP_Text>(false))
			{
				if (!(tmp_Text.GetComponentInParent<Selectable>() is Slider))
				{
					if (this._caption == null || tmp_Text.name == "Label" || tmp_Text.name == "Text" || tmp_Text.name == "Title")
					{
						this._caption = tmp_Text;
					}
					if (tmp_Text.name == "Label" || tmp_Text.name == "Title")
					{
						break;
					}
				}
			}
			if (this._caption == null)
			{
				this._icon = null;
			}
			if (this._caption == null)
			{
				this._legacyCaption = base.GetComponentInChildren<Text>();
			}
			GameObject gameObject = new GameObject("Controller Action Key", new Type[]
			{
				typeof(RectTransform),
				typeof(LayoutElement),
				typeof(TextMeshProUGUI)
			});
			gameObject.layer = base.gameObject.layer;
			gameObject.transform.SetParent(base.transform, false);
			gameObject.GetComponent<LayoutElement>().ignoreLayout = true;
			this._glyph = gameObject.GetComponent<TextMeshProUGUI>();
			this._glyph.font = ((this._caption != null) ? this._caption.font : TMP_Settings.defaultFontAsset);
			this._glyph.alignment = TextAlignmentOptions.Center;
			this._glyph.raycastTarget = false;
			this._glyph.enableWordWrapping = false;
			this._glyph.overflowMode = TextOverflowModes.Overflow;
		}

		// Token: 0x04004C71 RID: 19569
		private TextMeshProUGUI _glyph;

		// Token: 0x04004C72 RID: 19570
		private TMP_Text _caption;

		// Token: 0x04004C73 RID: 19571
		private Text _legacyCaption;

		// Token: 0x04004C74 RID: 19572
		private Vector4 _originalMargin;

		// Token: 0x04004C75 RID: 19573
		private float _originalFontSize;

		// Token: 0x04004C76 RID: 19574
		private float _originalMin;

		// Token: 0x04004C77 RID: 19575
		private float _originalMax;

		// Token: 0x04004C78 RID: 19576
		private bool _originalAutoSize;

		// Token: 0x04004C79 RID: 19577
		private Image _icon;

		// Token: 0x04004C7A RID: 19578
		private bool _hiddenIcon;

		// Token: 0x04004C7B RID: 19579
		private bool _reservedSpace;

		// Token: 0x04004C7C RID: 19580
		private string _binding;

		// Token: 0x04004C7D RID: 19581
		private int _version = -1;
	}
}
