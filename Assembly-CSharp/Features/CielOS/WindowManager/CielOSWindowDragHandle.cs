using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.CielOS.WindowManager
{
	// Token: 0x0200139E RID: 5022
	public class CielOSWindowDragHandle : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler
	{
		// Token: 0x06009B95 RID: 39829 RVA: 0x003F9333 File Offset: 0x003F7533
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

		// Token: 0x06009B96 RID: 39830 RVA: 0x003F9369 File Offset: 0x003F7569
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.window != null)
			{
				this.window.BringToFront();
			}
		}

		// Token: 0x06009B97 RID: 39831 RVA: 0x003F9384 File Offset: 0x003F7584
		public void OnDrag(PointerEventData eventData)
		{
			if (this.window == null)
			{
				return;
			}
			float num = ((this.canvas != null) ? this.canvas.scaleFactor : 1f);
			if (num <= 0f)
			{
				num = 1f;
			}
			this.window.ApplyMove(eventData.delta / num);
		}

		// Token: 0x0400A05A RID: 41050
		[Tooltip("Window this handle moves. If null, searched up the hierarchy.")]
		[SerializeField]
		private CielOSWindow window;

		// Token: 0x0400A05B RID: 41051
		[Tooltip("Canvas used to convert pointer delta to local UI units (handles canvas scaling).")]
		[SerializeField]
		private Canvas canvas;
	}
}
