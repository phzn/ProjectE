using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
	// Token: 0x02000004 RID: 4
	[AddComponentMenu("UI/Button", 30)]
	public class Button : Selectable, IPointerClickHandler, IEventSystemHandler, ISubmitHandler
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002157 File Offset: 0x00000357
		protected Button()
		{
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000F RID: 15 RVA: 0x0000216A File Offset: 0x0000036A
		// (set) Token: 0x06000010 RID: 16 RVA: 0x00002172 File Offset: 0x00000372
		public Button.ButtonClickedEvent onClick
		{
			get
			{
				return this.m_OnClick;
			}
			set
			{
				this.m_OnClick = value;
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000217B File Offset: 0x0000037B
		private void Press()
		{
			if (!this.IsActive() || !this.IsInteractable())
			{
				return;
			}
			UISystemProfilerApi.AddMarker("Button.onClick", this);
			this.m_OnClick.Invoke();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000021A4 File Offset: 0x000003A4
		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.Press();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000021B5 File Offset: 0x000003B5
		public virtual void OnSubmit(BaseEventData eventData)
		{
			this.Press();
			if (!this.IsActive() || !this.IsInteractable())
			{
				return;
			}
			this.DoStateTransition(Selectable.SelectionState.Pressed, false);
			base.StartCoroutine(this.OnFinishSubmit());
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000021E3 File Offset: 0x000003E3
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

		// Token: 0x0400000B RID: 11
		[FormerlySerializedAs("onClick")]
		[SerializeField]
		private Button.ButtonClickedEvent m_OnClick = new Button.ButtonClickedEvent();

		// Token: 0x02000079 RID: 121
		[Serializable]
		public class ButtonClickedEvent : UnityEvent
		{
		}
	}
}
