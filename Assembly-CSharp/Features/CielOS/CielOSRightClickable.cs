using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.CielOS
{
	// Token: 0x0200138F RID: 5007
	public class CielOSRightClickable : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06009AFA RID: 39674 RVA: 0x003F65E4 File Offset: 0x003F47E4
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Right)
			{
				Action<Vector2> onRightClick = this.OnRightClick;
				if (onRightClick == null)
				{
					return;
				}
				onRightClick(eventData.position);
			}
		}

		// Token: 0x04009FED RID: 40941
		public Action<Vector2> OnRightClick;
	}
}
