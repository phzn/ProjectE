using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProjektSumperk
{
	// Token: 0x02000408 RID: 1032
	internal sealed class TimerEditableDigits : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IScrollHandler
	{
		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06001A37 RID: 6711 RVA: 0x000A6334 File Offset: 0x000A4534
		public bool IsEditing
		{
			get
			{
				return this._input != null && this._input.isFocused;
			}
		}

		// Token: 0x06001A38 RID: 6712 RVA: 0x000A6354 File Offset: 0x000A4554
		public void Configure(TimerUIController owner, TMP_Text label)
		{
			this._owner = owner;
			this._label = label;
			this._normalColor = label.color;
			label.raycastTarget = true;
			this._input = base.GetComponent<TMP_InputField>();
			if (this._input == null)
			{
				this._input = base.gameObject.AddComponent<TMP_InputField>();
			}
			this._input.textViewport = label.rectTransform;
			this._input.textComponent = label as TextMeshProUGUI;
			this._input.targetGraphic = null;
			this._input.transition = Selectable.Transition.None;
			this._input.lineType = TMP_InputField.LineType.SingleLine;
			this._input.contentType = TMP_InputField.ContentType.Custom;
			this._input.characterValidation = TMP_InputField.CharacterValidation.None;
			this._input.characterLimit = 8;
			this._input.customCaretColor = true;
			this._input.caretColor = Color.white;
			this._input.selectionColor = new Color(0.25f, 0.68f, 1f, 0.45f);
			this._input.onEndEdit.RemoveListener(new UnityAction<string>(this.ApplyText));
			this._input.onEndEdit.AddListener(new UnityAction<string>(this.ApplyText));
		}

		// Token: 0x06001A39 RID: 6713 RVA: 0x000A6490 File Offset: 0x000A4690
		public void SetEditable(bool editable)
		{
			if (this._input != null)
			{
				this._input.interactable = editable;
			}
		}

		// Token: 0x06001A3A RID: 6714 RVA: 0x000A64AC File Offset: 0x000A46AC
		public void SetTextWithoutNotify(string value)
		{
			if (this._input != null)
			{
				this._input.SetTextWithoutNotify(value);
			}
		}

		// Token: 0x06001A3B RID: 6715 RVA: 0x000A64C8 File Offset: 0x000A46C8
		public void Commit()
		{
			if (this._input == null)
			{
				return;
			}
			this.ApplyText(this._input.text);
			this._input.DeactivateInputField(false);
		}

		// Token: 0x06001A3C RID: 6716 RVA: 0x000A64F8 File Offset: 0x000A46F8
		public void OnPointerEnter(PointerEventData eventData)
		{
			this._hovered = true;
			if (this._input != null && this._input.interactable && this._label != null)
			{
				this._label.color = Color.Lerp(this._normalColor, Color.white, 0.28f);
			}
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x000A6555 File Offset: 0x000A4755
		public void OnPointerExit(PointerEventData eventData)
		{
			this._hovered = false;
			if (this._label != null)
			{
				this._label.color = this._normalColor;
			}
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x000A6580 File Offset: 0x000A4780
		public void OnScroll(PointerEventData eventData)
		{
			if (!this._hovered || this._owner == null || this._input == null || !this._input.interactable || Mathf.Approximately(eventData.scrollDelta.y, 0f))
			{
				return;
			}
			RectTransform rectTransform = this._label.rectTransform;
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out vector);
			float num = Mathf.InverseLerp(rectTransform.rect.xMin, rectTransform.rect.xMax, vector.x);
			float num2 = ((num < 0.33333334f) ? 3600f : ((num < 0.6666667f) ? 60f : 1f));
			this._owner.AdjustSeconds(Mathf.Sign(eventData.scrollDelta.y) * num2);
			this._input.SetTextWithoutNotify(this._label.text);
			eventData.Use();
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x000A6680 File Offset: 0x000A4880
		private void ApplyText(string value)
		{
			if (this._owner == null || this._label == null)
			{
				return;
			}
			if (!this._owner.TrySetDurationFromText(value))
			{
				this._owner.SetDurationSeconds(this._owner.CurrentTime);
			}
			this._input.SetTextWithoutNotify(this._label.text);
		}

		// Token: 0x04001B56 RID: 6998
		private TimerUIController _owner;

		// Token: 0x04001B57 RID: 6999
		private TMP_Text _label;

		// Token: 0x04001B58 RID: 7000
		private TMP_InputField _input;

		// Token: 0x04001B59 RID: 7001
		private Color _normalColor;

		// Token: 0x04001B5A RID: 7002
		private bool _hovered;
	}
}
