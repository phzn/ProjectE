using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CompassNavigatorPro
{
	// Token: 0x020002A0 RID: 672
	[AddComponentMenu("")]
	public class CompassButtonHandler : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
	{
		// Token: 0x06001335 RID: 4917 RVA: 0x0007CC26 File Offset: 0x0007AE26
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.co != null)
			{
				base.StopCoroutine(this.co);
			}
			this.co = base.StartCoroutine(this.ExecuteHandler());
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x0007CC4E File Offset: 0x0007AE4E
		public void OnPointerUp(PointerEventData eventData)
		{
			if (this.co != null)
			{
				base.StopCoroutine(this.co);
			}
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x0007CC64 File Offset: 0x0007AE64
		private IEnumerator ExecuteHandler()
		{
			WaitForEndOfFrame w = new WaitForEndOfFrame();
			for (;;)
			{
				this.actionHandler();
				yield return w;
			}
			yield break;
		}

		// Token: 0x0400149D RID: 5277
		public UnityAction actionHandler;

		// Token: 0x0400149E RID: 5278
		private Coroutine co;
	}
}
