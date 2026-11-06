using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MoreMountains.Tools
{
	// Token: 0x0200004F RID: 79
	public class MMOnPointer : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerEnterHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler
	{
		// Token: 0x060001BD RID: 445 RVA: 0x00008B54 File Offset: 0x00006D54
		public void OnPointerEnter(PointerEventData eventData)
		{
			UnityEvent pointerEnter = this.PointerEnter;
			if (pointerEnter == null)
			{
				return;
			}
			pointerEnter.Invoke();
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00008B66 File Offset: 0x00006D66
		public void OnPointerExit(PointerEventData eventData)
		{
			UnityEvent pointerExit = this.PointerExit;
			if (pointerExit == null)
			{
				return;
			}
			pointerExit.Invoke();
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00008B78 File Offset: 0x00006D78
		public void OnPointerDown(PointerEventData eventData)
		{
			UnityEvent pointerDown = this.PointerDown;
			if (pointerDown == null)
			{
				return;
			}
			pointerDown.Invoke();
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00008B8A File Offset: 0x00006D8A
		public void OnPointerUp(PointerEventData eventData)
		{
			UnityEvent pointerUp = this.PointerUp;
			if (pointerUp == null)
			{
				return;
			}
			pointerUp.Invoke();
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00008B9C File Offset: 0x00006D9C
		public void OnPointerClick(PointerEventData eventData)
		{
			UnityEvent pointerClick = this.PointerClick;
			if (pointerClick == null)
			{
				return;
			}
			pointerClick.Invoke();
		}

		// Token: 0x0400025F RID: 607
		[Header("Pointer movement")]
		[Tooltip("an event to trigger when the pointer enters the associated game object")]
		public UnityEvent PointerEnter;

		// Token: 0x04000260 RID: 608
		[Tooltip("an event to trigger when the pointer exits the associated game object")]
		public UnityEvent PointerExit;

		// Token: 0x04000261 RID: 609
		[Header("Clicks")]
		[Tooltip("an event to trigger when the pointer is pressed down on the associated game object")]
		public UnityEvent PointerDown;

		// Token: 0x04000262 RID: 610
		[Tooltip("an event to trigger when the pointer is pressed up on the associated game object")]
		public UnityEvent PointerUp;

		// Token: 0x04000263 RID: 611
		[Tooltip("an event to trigger when the pointer is clicked on the associated game object")]
		public UnityEvent PointerClick;
	}
}
