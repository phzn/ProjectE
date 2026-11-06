using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.CielOS.Apps.Dialogues
{
	// Token: 0x020014AB RID: 5291
	public class DialogueSliderDragNotifier : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
	{
		// Token: 0x0600A2E7 RID: 41703 RVA: 0x0043A462 File Offset: 0x00438662
		public void Init(Action onBegin, Action onEnd)
		{
			this._onBegin = onBegin;
			this._onEnd = onEnd;
		}

		// Token: 0x0600A2E8 RID: 41704 RVA: 0x0043A472 File Offset: 0x00438672
		private void Begin()
		{
			if (this._active)
			{
				return;
			}
			this._active = true;
			Action onBegin = this._onBegin;
			if (onBegin == null)
			{
				return;
			}
			onBegin();
		}

		// Token: 0x0600A2E9 RID: 41705 RVA: 0x0043A494 File Offset: 0x00438694
		private void End()
		{
			if (!this._active)
			{
				return;
			}
			this._active = false;
			Action onEnd = this._onEnd;
			if (onEnd == null)
			{
				return;
			}
			onEnd();
		}

		// Token: 0x0600A2EA RID: 41706 RVA: 0x0043A4B6 File Offset: 0x004386B6
		public void OnPointerDown(PointerEventData e)
		{
			this.Begin();
		}

		// Token: 0x0600A2EB RID: 41707 RVA: 0x0043A4B6 File Offset: 0x004386B6
		public void OnBeginDrag(PointerEventData e)
		{
			this.Begin();
		}

		// Token: 0x0600A2EC RID: 41708 RVA: 0x0043A4BE File Offset: 0x004386BE
		public void OnEndDrag(PointerEventData e)
		{
			this.End();
		}

		// Token: 0x0600A2ED RID: 41709 RVA: 0x0043A4BE File Offset: 0x004386BE
		public void OnPointerUp(PointerEventData e)
		{
			this.End();
		}

		// Token: 0x0400A69F RID: 42655
		private Action _onBegin;

		// Token: 0x0400A6A0 RID: 42656
		private Action _onEnd;

		// Token: 0x0400A6A1 RID: 42657
		private bool _active;
	}
}
