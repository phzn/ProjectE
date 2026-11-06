using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CompassNavigatorPro
{
	// Token: 0x02000280 RID: 640
	[AddComponentMenu("")]
	public class CompassIconEventHandler : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
	{
		// Token: 0x060010B4 RID: 4276 RVA: 0x0006F8B3 File Offset: 0x0006DAB3
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.compass == null)
			{
				return;
			}
			this.compass.BubbleEvent<CompassProPOI>(this.compass.OnPOIMiniMapIconMouseEnter, this.poi);
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x0006F8E0 File Offset: 0x0006DAE0
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.compass == null)
			{
				return;
			}
			this.compass.BubbleEvent<CompassProPOI>(this.compass.OnPOIMiniMapIconMouseExit, this.poi);
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x0006F90D File Offset: 0x0006DB0D
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.compass == null)
			{
				return;
			}
			this.compass.BubbleEvent<CompassProPOI, int>(this.compass.OnPOIMiniMapIconMouseClick, this.poi, (int)eventData.button);
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x0006F940 File Offset: 0x0006DB40
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.compass == null)
			{
				return;
			}
			this.compass.BubbleEvent<CompassProPOI, int>(this.compass.OnPOIMiniMapIconMouseDown, this.poi, (int)eventData.button);
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x0006F973 File Offset: 0x0006DB73
		public void OnPointerUp(PointerEventData eventData)
		{
			if (this.compass == null)
			{
				return;
			}
			this.compass.BubbleEvent<CompassProPOI, int>(this.compass.OnPOIMiniMapIconMouseUp, this.poi, (int)eventData.button);
		}

		// Token: 0x04001229 RID: 4649
		public CompassProPOI poi;

		// Token: 0x0400122A RID: 4650
		public CompassPro compass;
	}
}
