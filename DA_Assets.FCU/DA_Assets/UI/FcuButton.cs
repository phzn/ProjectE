using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DA_Assets.UI
{
	// Token: 0x02000009 RID: 9
	[AddComponentMenu("UI/FcuButton", 30)]
	public class FcuButton : Selectable, IPointerClickHandler, IEventSystemHandler, ISubmitHandler
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002168 File Offset: 0x00000368
		// (set) Token: 0x06000015 RID: 21 RVA: 0x00002170 File Offset: 0x00000370
		public MaskableGraphic ButtonText
		{
			get
			{
				return this.m_buttonText;
			}
			set
			{
				this.m_buttonText = value;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002179 File Offset: 0x00000379
		// (set) Token: 0x06000017 RID: 23 RVA: 0x00002181 File Offset: 0x00000381
		public bool ChangeTextColor
		{
			get
			{
				return this.m_changeTextColor;
			}
			set
			{
				this.m_changeTextColor = value;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000018 RID: 24 RVA: 0x0000218A File Offset: 0x0000038A
		// (set) Token: 0x06000019 RID: 25 RVA: 0x00002192 File Offset: 0x00000392
		public Color TextDefaultColor
		{
			get
			{
				return this.m_textDefaultColor;
			}
			set
			{
				this.m_textDefaultColor = value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001A RID: 26 RVA: 0x0000219B File Offset: 0x0000039B
		// (set) Token: 0x0600001B RID: 27 RVA: 0x000021A3 File Offset: 0x000003A3
		public Color TextHoverColor
		{
			get
			{
				return this.m_textHoverColor;
			}
			set
			{
				this.m_textHoverColor = value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001C RID: 28 RVA: 0x000021AC File Offset: 0x000003AC
		// (set) Token: 0x0600001D RID: 29 RVA: 0x000021B4 File Offset: 0x000003B4
		public Color TextPressedColor
		{
			get
			{
				return this.m_textPressedColor;
			}
			set
			{
				this.m_textPressedColor = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000021BD File Offset: 0x000003BD
		// (set) Token: 0x0600001F RID: 31 RVA: 0x000021C5 File Offset: 0x000003C5
		public Color TextSelectedColor
		{
			get
			{
				return this.m_textSelectedColor;
			}
			set
			{
				this.m_textSelectedColor = value;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000021CE File Offset: 0x000003CE
		// (set) Token: 0x06000021 RID: 33 RVA: 0x000021D6 File Offset: 0x000003D6
		public Color TextDisabledColor
		{
			get
			{
				return this.m_textDisabledColor;
			}
			set
			{
				this.m_textDisabledColor = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000021DF File Offset: 0x000003DF
		// (set) Token: 0x06000023 RID: 35 RVA: 0x000021E7 File Offset: 0x000003E7
		public FcuButton.ButtonClickedEvent onClick
		{
			get
			{
				return this._onClick;
			}
			set
			{
				this._onClick = value;
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000021F0 File Offset: 0x000003F0
		private void Press()
		{
			if (!this.IsActive() || !this.IsInteractable())
			{
				return;
			}
			UISystemProfilerApi.AddMarker("Button.onClick", this);
			this._onClick.Invoke();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002219 File Offset: 0x00000419
		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.Press();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000222A File Offset: 0x0000042A
		public virtual void OnSubmit(BaseEventData eventData)
		{
			if (!this.IsActive() || !this.IsInteractable())
			{
				return;
			}
			this.Press();
			this.DoStateTransition(Selectable.SelectionState.Pressed, false);
			base.StartCoroutine(this.OnFinishSubmit());
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002258 File Offset: 0x00000458
		private IEnumerator OnFinishSubmit()
		{
			float fadeTime = base.colors.fadeDuration;
			float elapsedTime = 0f;
			while (elapsedTime < fadeTime)
			{
				elapsedTime += Time.unscaledDeltaTime;
				yield return null;
			}
			this.DoStateTransition(base.currentSelectionState, false);
			yield break;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002267 File Offset: 0x00000467
		public override void OnPointerDown(PointerEventData eventData)
		{
			base.OnPointerDown(eventData);
			if (base.interactable && this.m_buttonText != null && this.m_changeTextColor)
			{
				this.m_buttonText.color = this.m_textPressedColor;
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000229F File Offset: 0x0000049F
		public override void OnPointerUp(PointerEventData eventData)
		{
			base.OnPointerUp(eventData);
			if (base.interactable && this.m_buttonText != null && this.m_changeTextColor)
			{
				this.m_buttonText.color = this.m_textDefaultColor;
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000022D7 File Offset: 0x000004D7
		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
			if (base.interactable && this.m_buttonText != null && this.m_changeTextColor)
			{
				this.m_buttonText.color = this.m_textHoverColor;
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000230F File Offset: 0x0000050F
		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
			if (base.interactable && this.m_buttonText != null && this.m_changeTextColor)
			{
				this.m_buttonText.color = this.m_textDefaultColor;
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002347 File Offset: 0x00000547
		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			if (base.interactable && this.m_buttonText != null && this.m_changeTextColor)
			{
				this.m_buttonText.color = this.m_textSelectedColor;
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x0000237F File Offset: 0x0000057F
		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			if (base.interactable && this.m_buttonText != null && this.m_changeTextColor)
			{
				this.m_buttonText.color = this.m_textDefaultColor;
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000023B7 File Offset: 0x000005B7
		public override void Select()
		{
			base.Select();
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000023C0 File Offset: 0x000005C0
		public override bool IsInteractable()
		{
			bool flag = base.IsInteractable();
			if (this.m_buttonText == null)
			{
				return flag;
			}
			if (flag)
			{
				if (this.m_changeTextColor)
				{
					this.m_buttonText.color = this.m_textDefaultColor;
				}
			}
			else if (this.m_changeTextColor)
			{
				this.m_buttonText.color = this.m_textDisabledColor;
			}
			return flag;
		}

		// Token: 0x04000010 RID: 16
		[Space]
		[SerializeField]
		private MaskableGraphic m_buttonText;

		// Token: 0x04000011 RID: 17
		[Space]
		[SerializeField]
		private bool m_changeTextColor;

		// Token: 0x04000012 RID: 18
		[SerializeField]
		private Color m_textDefaultColor = Color.white;

		// Token: 0x04000013 RID: 19
		[SerializeField]
		private Color m_textHoverColor = new Color(0.9607843f, 0.9607843f, 0.9607843f, 1f);

		// Token: 0x04000014 RID: 20
		[SerializeField]
		private Color m_textPressedColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 1f);

		// Token: 0x04000015 RID: 21
		[SerializeField]
		private Color m_textSelectedColor = new Color(0.9607843f, 0.9607843f, 0.9607843f, 1f);

		// Token: 0x04000016 RID: 22
		[SerializeField]
		private Color m_textDisabledColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5019608f);

		// Token: 0x04000017 RID: 23
		[Space]
		[SerializeField]
		[FormerlySerializedAs("onClick")]
		private FcuButton.ButtonClickedEvent _onClick = new FcuButton.ButtonClickedEvent();

		// Token: 0x02000083 RID: 131
		[Serializable]
		public class ButtonClickedEvent : UnityEvent
		{
		}
	}
}
