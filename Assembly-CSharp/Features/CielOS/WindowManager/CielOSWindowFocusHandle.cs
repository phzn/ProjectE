using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.CielOS.WindowManager
{
	// Token: 0x0200139F RID: 5023
	public class CielOSWindowFocusHandle : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
	{
		// Token: 0x06009B99 RID: 39833 RVA: 0x003F93E6 File Offset: 0x003F75E6
		private void Awake()
		{
			if (this.window == null)
			{
				this.window = base.GetComponentInParent<CielOSWindow>();
			}
		}

		// Token: 0x06009B9A RID: 39834 RVA: 0x003F9402 File Offset: 0x003F7602
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.window != null)
			{
				this.window.BringToFront();
			}
		}

		// Token: 0x0400A05C RID: 41052
		[SerializeField]
		private CielOSWindow window;
	}
}
