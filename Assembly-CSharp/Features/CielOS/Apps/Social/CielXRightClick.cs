using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.CielOS.Apps.Social
{
	// Token: 0x02001458 RID: 5208
	public class CielXRightClick : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x0600A0BA RID: 41146 RVA: 0x0042867D File Offset: 0x0042687D
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Right)
			{
				Action<Vector2> action = this.onRightClick;
				if (action == null)
				{
					return;
				}
				action(eventData.position);
			}
		}

		// Token: 0x0400A47A RID: 42106
		public Action<Vector2> onRightClick;
	}
}
