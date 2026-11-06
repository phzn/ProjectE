using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.CielOS.Apps
{
	// Token: 0x020013FC RID: 5116
	public class CielOSPaintSurfaceProxy : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler, IPointerUpHandler
	{
		// Token: 0x06009DF9 RID: 40441 RVA: 0x0040F7F0 File Offset: 0x0040D9F0
		public void OnPointerDown(PointerEventData e)
		{
			CielOSPaintApp cielOSPaintApp = this.app;
			if (cielOSPaintApp == null)
			{
				return;
			}
			cielOSPaintApp.OnPointerDown(e);
		}

		// Token: 0x06009DFA RID: 40442 RVA: 0x0040F803 File Offset: 0x0040DA03
		public void OnDrag(PointerEventData e)
		{
			CielOSPaintApp cielOSPaintApp = this.app;
			if (cielOSPaintApp == null)
			{
				return;
			}
			cielOSPaintApp.OnDrag(e);
		}

		// Token: 0x06009DFB RID: 40443 RVA: 0x0040F816 File Offset: 0x0040DA16
		public void OnPointerUp(PointerEventData e)
		{
			CielOSPaintApp cielOSPaintApp = this.app;
			if (cielOSPaintApp == null)
			{
				return;
			}
			cielOSPaintApp.OnPointerUp(e);
		}

		// Token: 0x0400A243 RID: 41539
		public CielOSPaintApp app;
	}
}
