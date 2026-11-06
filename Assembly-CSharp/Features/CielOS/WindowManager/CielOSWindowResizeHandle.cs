using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.CielOS.WindowManager
{
	// Token: 0x020013A2 RID: 5026
	public class CielOSWindowResizeHandle : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler
	{
		// Token: 0x06009BB1 RID: 39857 RVA: 0x003F9DCB File Offset: 0x003F7FCB
		private void Awake()
		{
			if (this.window == null)
			{
				this.window = base.GetComponentInParent<CielOSWindow>();
			}
			if (this.canvas == null)
			{
				this.canvas = base.GetComponentInParent<Canvas>();
			}
		}

		// Token: 0x06009BB2 RID: 39858 RVA: 0x003F9E01 File Offset: 0x003F8001
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.window != null && this.window.IsResizable)
			{
				this.window.BringToFront();
			}
		}

		// Token: 0x06009BB3 RID: 39859 RVA: 0x003F9E2C File Offset: 0x003F802C
		public void OnDrag(PointerEventData eventData)
		{
			if (this.window == null || !this.window.IsResizable)
			{
				return;
			}
			float num = ((this.canvas != null) ? this.canvas.scaleFactor : 1f);
			if (num <= 0f)
			{
				num = 1f;
			}
			this.window.ApplySizeDelta(eventData.delta / num);
		}

		// Token: 0x0400A06C RID: 41068
		[SerializeField]
		private CielOSWindow window;

		// Token: 0x0400A06D RID: 41069
		[SerializeField]
		private Canvas canvas;
	}
}
