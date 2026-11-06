using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.CielOS
{
	// Token: 0x0200137C RID: 4988
	public class CielOSClickable : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, ISubmitHandler
	{
		// Token: 0x06009A81 RID: 39553 RVA: 0x003F337C File Offset: 0x003F157C
		public void OnSubmit(BaseEventData eventData)
		{
			Canvas componentInParent = base.GetComponentInParent<Canvas>();
			Vector2 vector = RectTransformUtility.WorldToScreenPoint((componentInParent != null && componentInParent.renderMode != RenderMode.ScreenSpaceOverlay) ? componentInParent.worldCamera : null, base.transform.position);
			if (this.OnDoubleClick != null)
			{
				this.OnDoubleClick(vector);
				return;
			}
			Action<Vector2> onClick = this.OnClick;
			if (onClick == null)
			{
				return;
			}
			onClick(vector);
		}

		// Token: 0x06009A82 RID: 39554 RVA: 0x003F33E4 File Offset: 0x003F15E4
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			if (eventData.clickCount >= 2)
			{
				Action<Vector2> onDoubleClick = this.OnDoubleClick;
				if (onDoubleClick == null)
				{
					return;
				}
				onDoubleClick(eventData.position);
				return;
			}
			else
			{
				Action<Vector2> onClick = this.OnClick;
				if (onClick == null)
				{
					return;
				}
				onClick(eventData.position);
				return;
			}
		}

		// Token: 0x04009F4E RID: 40782
		public Action<Vector2> OnClick;

		// Token: 0x04009F4F RID: 40783
		public Action<Vector2> OnDoubleClick;
	}
}
