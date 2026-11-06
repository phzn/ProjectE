using System;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Inventory.UI
{
	// Token: 0x02001154 RID: 4436
	internal sealed class InventoryContextBadge
	{
		// Token: 0x060086F6 RID: 34550 RVA: 0x003728CC File Offset: 0x00370ACC
		public InventoryContextBadge(Transform badge, string binding, Vector2 controllerOffset = default(Vector2))
		{
			this._badge = badge.gameObject;
			this._rect = badge as RectTransform;
			this._keyboardPosition = ((this._rect != null) ? this._rect.anchoredPosition : Vector2.zero);
			this._controllerOffset = controllerOffset;
			this._binding = binding;
			this._visible = this._badge.activeSelf;
			this._graphics = badge.GetComponentsInChildren<Graphic>(true);
			this._enabled = new bool[this._graphics.Length];
			for (int i = 0; i < this._graphics.Length; i++)
			{
				this._enabled[i] = this._graphics[i].enabled;
			}
		}

		// Token: 0x060086F7 RID: 34551 RVA: 0x0037298C File Offset: 0x00370B8C
		public void Refresh(bool controller)
		{
			if (this._badge == null)
			{
				return;
			}
			bool? controller2 = this._controller;
			if (!((controller2.GetValueOrDefault() == controller) & (controller2 != null)))
			{
				this._controller = new bool?(controller);
				if (this._rect != null)
				{
					this._rect.anchoredPosition = this._keyboardPosition + (controller ? this._controllerOffset : Vector2.zero);
				}
				this._badge.SetActive(this._visible && (!controller || this._binding != null));
				for (int i = 0; i < this._graphics.Length; i++)
				{
					if (this._graphics[i] != null)
					{
						this._graphics[i].enabled = !controller && this._enabled[i];
					}
				}
				if (controller && this._binding != null && this._glyph == null)
				{
					GameObject gameObject = new GameObject("ControllerInventoryContext", new Type[]
					{
						typeof(RectTransform),
						typeof(TextMeshProUGUI)
					});
					gameObject.layer = this._badge.layer;
					gameObject.transform.SetParent(this._badge.transform, false);
					RectTransform rectTransform = (RectTransform)gameObject.transform;
					rectTransform.anchorMin = Vector2.zero;
					rectTransform.anchorMax = Vector2.one;
					rectTransform.offsetMin = (rectTransform.offsetMax = Vector2.zero);
					this._glyph = gameObject.GetComponent<TMP_Text>();
					this._glyph.fontSize = 22f;
					this._glyph.enableAutoSizing = false;
					this._glyph.enableWordWrapping = false;
					this._glyph.overflowMode = TextOverflowModes.Overflow;
					this._glyph.alignment = TextAlignmentOptions.Center;
					this._glyph.raycastTarget = false;
					this._glyph.color = Color.white;
				}
				if (this._glyph != null)
				{
					this._glyph.gameObject.SetActive(controller);
				}
			}
			if (controller && this._glyph != null && this._glyphVersion != ControllerGlyphs.Version)
			{
				this._glyphVersion = ControllerGlyphs.Version;
				this._glyph.text = ControllerGlyphs.Format(this._binding);
			}
		}

		// Token: 0x040089A2 RID: 35234
		private readonly GameObject _badge;

		// Token: 0x040089A3 RID: 35235
		private readonly bool _visible;

		// Token: 0x040089A4 RID: 35236
		private readonly Graphic[] _graphics;

		// Token: 0x040089A5 RID: 35237
		private readonly bool[] _enabled;

		// Token: 0x040089A6 RID: 35238
		private readonly string _binding;

		// Token: 0x040089A7 RID: 35239
		private readonly RectTransform _rect;

		// Token: 0x040089A8 RID: 35240
		private readonly Vector2 _keyboardPosition;

		// Token: 0x040089A9 RID: 35241
		private readonly Vector2 _controllerOffset;

		// Token: 0x040089AA RID: 35242
		private TMP_Text _glyph;

		// Token: 0x040089AB RID: 35243
		private bool? _controller;

		// Token: 0x040089AC RID: 35244
		private int _glyphVersion = -1;
	}
}
