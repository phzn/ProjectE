using System;
using UnityEngine;
using UnityEngine.Events;

namespace MoreMountains.Tools
{
	// Token: 0x020000E0 RID: 224
	public class MMGameEventListener : MonoBehaviour, MMEventListener<MMGameEvent>, MMEventListenerBase
	{
		// Token: 0x060004CA RID: 1226 RVA: 0x00016700 File Offset: 0x00014900
		public void OnMMEvent(MMGameEvent gameEvent)
		{
			if (gameEvent.EventName == this.EventName)
			{
				UnityEvent onMMGameEvent = this.OnMMGameEvent;
				if (onMMGameEvent == null)
				{
					return;
				}
				onMMGameEvent.Invoke();
			}
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00016725 File Offset: 0x00014925
		protected virtual void OnEnable()
		{
			this.MMEventStartListening<MMGameEvent>();
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0001672D File Offset: 0x0001492D
		protected virtual void OnDisable()
		{
			this.MMEventStopListening<MMGameEvent>();
		}

		// Token: 0x04000638 RID: 1592
		[Header("MMGameEvent")]
		[Tooltip("the name of the event you want to listen for")]
		public string EventName = "Load";

		// Token: 0x04000639 RID: 1593
		[Tooltip("a UnityEvent hook you can use to call methods when the specified event gets triggered")]
		public UnityEvent OnMMGameEvent;
	}
}
